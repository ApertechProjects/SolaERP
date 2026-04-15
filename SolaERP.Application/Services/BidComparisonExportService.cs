using AutoMapper;
using Microsoft.AspNetCore.Http;
using NPOI.OOXML.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.UnitOfWork;
using NPOI.SS.Util;
using QRCoder;
using SkiaSharp;
using System.Globalization;

namespace SolaERP.Persistence.Services;

public class BidComparisonExportService : IBidComparisonExportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public BidComparisonExportService(
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task GetCardExportByRfqMainIdAsync(
    int rfqMainId,
    HttpResponse response,
    List<BidComparisonBidHeaderDto> bids,
    List<BidComparisonRFQDetailsDto> rfqDetails,
    BidComparisonHeaderDto bcc,
    List<string> requestDepartmentCodes,
    List<BidComparisonApprovedUsersApprovalInformationDto> approvedUsers,
    List<string> requestNumbers)
{
    IWorkbook workbook = new XSSFWorkbook();
    try
    {
        var sheet = workbook.CreateSheet("BCC");

        sheet.ProtectSheet("STJ15u{^Mx8Qo,!!OVSgF");

        var colorMap = new DefaultIndexedColorMap();
        
        var darkBlueColor = new XSSFColor(new SKColor(28, 61, 110), colorMap);
        var redColor = new XSSFColor(new SKColor(244, 67, 54), colorMap);
        var greenColor = new XSSFColor(new SKColor(118, 219, 73), colorMap);
        
        var boldStyle = CreateStyle(workbook, true, 0, false, null, null, null);
        var centeredBoldBigStyle = CreateStyle(workbook, true, 1, false, null, null, 30);
        var centeredBorderStyle = CreateStyle(workbook, false, 1, true, null, null, null);
        var centeredBoldBorderStyle = CreateStyle(workbook, true, 1, true, null, null, null);
        var centeredBoldBorderRedStyle = CreateStyle(workbook, true, 1, true, redColor, null, null);
        var centeredBoldBorderRedFontStyle = CreateStyle(workbook, true, 1, true, null, IndexedColors.Red.Index, null);
        var centeredBoldBorderGreenStyle = CreateStyle(workbook, true, 1, true, greenColor, null, null);
        var centeredBoldBorderGreenFontStyle = CreateStyle(workbook, true, 1, true, null, IndexedColors.Green.Index, null);
        var centeredBoldBorderPaleBlueStyle = CreateStyle(workbook, true, 1, true, darkBlueColor, IndexedColors.White.Index, null);

        var rightBoldBorderStyle = CreateStyle(workbook, true, 2, true, null, null, null);

        int lastRow;

        PrepareColumnWidth(sheet);

        // await SetLogoToB2Async(workbook, sheet, logoLink);

        PrepareRow1(sheet, centeredBoldBigStyle);

        PrepareRow2(sheet, centeredBoldBorderStyle, bcc, rightBoldBorderStyle);

        PrepareRow3(sheet, centeredBoldBorderStyle, centeredBoldBorderPaleBlueStyle, bcc, requestNumbers);

        PrepareRow4(sheet);

        PrepareRow5(sheet, centeredBoldBorderStyle, centeredBoldBorderPaleBlueStyle, bcc, requestDepartmentCodes);

        PrepareRow6(sheet);

        PrepareRow78(sheet, centeredBoldBorderPaleBlueStyle, bids);

        lastRow = PrepareRequestItems(
            sheet,
            centeredBoldBorderStyle,
            centeredBoldBorderRedFontStyle,
            bids,
            rfqDetails,
            workbook,
            centeredBoldBorderGreenFontStyle);

        lastRow = PrepareBidDetail(sheet, lastRow, centeredBorderStyle, centeredBoldBorderPaleBlueStyle, bids);

        lastRow = PrepareRowEmpty(sheet, lastRow);

        lastRow = PrepareRfxComment(sheet, lastRow, bcc, centeredBoldBorderStyle, centeredBoldBorderPaleBlueStyle);

        lastRow = PrepareWinners(sheet, lastRow, bids, centeredBoldBorderStyle, centeredBoldBorderPaleBlueStyle);

        lastRow = PrepareRowEmpty(sheet, lastRow);

        lastRow = PrepareApprovedUsers(sheet, lastRow, approvedUsers, centeredBoldBorderStyle, centeredBoldBorderPaleBlueStyle);

        lastRow = PrepareRowEmpty(sheet, lastRow);
        lastRow = PrepareRowEmpty(sheet, lastRow);

        // lastRow = PrepareRowUnder(sheet, lastRow, centeredBorderStyle, centeredBoldBorderPaleBlueStyle);

        // if (bcc.approveStatusId != null && bcc.approveStatusId == ApproveStatuses.APPROVED.GetId())
        if (bcc.approveStatusId != null && bcc.approveStatusId == 3)
        {
            await GenerateQrCodeAsync(workbook, sheet, rfqMainId);
        }

        var fallbackFilename = "Muqayise_cedveli.xlsx";
        var encodedFilename = Uri.EscapeDataString("Müqayisə_cədvəli.xlsx");

        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        response.Headers["Content-Disposition"] =
            $"attachment; filename=\"{fallbackFilename}\"; filename*=UTF-8''{encodedFilename}";

        await using var ms = new MemoryStream();
        workbook.Write(ms, leaveOpen: true);
        ms.Position = 0;

        await ms.CopyToAsync(response.Body);
        await response.Body.FlushAsync();
    }
    finally
    {
        workbook.Close();
    }
}
    
    
    private ICellStyle CreateStyle(
        IWorkbook workbook,
        bool isBold,
        int textPosition,
        bool isBorder,
        XSSFColor backgroundColor,
        short? fontColor,
        int? fontSize)
    {
        var style = workbook.CreateCellStyle();
        style.WrapText = true;

        var font = workbook.CreateFont();

        fontSize ??= 14;
        font.FontHeightInPoints = (short)fontSize;

        if (isBold)
            font.IsBold = true;

        if (fontColor.HasValue)
            font.Color = fontColor.Value;

        style.SetFont(font);

        // Alignment
        if (textPosition == 1)
        {
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
        }
        else if (textPosition == 2)
        {
            style.Alignment = HorizontalAlignment.Right;
        }

        // Borders
        if (isBorder)
        {
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
        }

        // Background color (only works with XSSFCellStyle)
        if (backgroundColor != null && style is XSSFCellStyle xssfStyle)
        {
            xssfStyle.SetFillForegroundColor(backgroundColor);
            xssfStyle.FillPattern = FillPattern.SolidForeground;
        }

        return style;
    }
    
    private void PrepareColumnWidth(ISheet sheet)
    {
        var widths = new Dictionary<int, int>
        {
            {0, 1000}, {1, 4000}, {2, 6000}, {3, 9000},
            {5, 4000}, {6, 5600}, {7, 5600}, {8, 5600},
            {9, 6000}, {10, 3000}, {11, 5600}, {12, 5600},
            {13, 5600}, {14, 6000}, {15, 3000}, {16, 5600},
            {17, 5600}, {18, 5600}, {19, 6000}, {20, 3000},
            {21, 5600}, {22, 5600}, {23, 5600}, {24, 6000},
            {25, 3000}, {26, 5600}, {27, 5600}, {28, 5600},
            {29, 6000}, {30, 3000}
        };

        foreach (var kv in widths)
        {
            sheet.SetColumnWidth(kv.Key, kv.Value);
        }
    }
    
    private void PrepareRow1(ISheet sheet, ICellStyle centeredBoldBigStyle)
    {
        var row1 = sheet.CreateRow(0);
        row1.HeightInPoints = 100;

        var cellC1 = row1.CreateCell(2);
        cellC1.SetCellValue("Müqayisə cədvəli");
        cellC1.CellStyle = centeredBoldBigStyle;

        // Merge cells from C1 to L1
        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 2, 11));
    }

    private void PrepareRow2(
        ISheet sheet,
        ICellStyle centeredBoldBorderStyle,
        BidComparisonHeaderDto bcc,
        ICellStyle rightBoldBorderStyle)
    {
        var row2 = sheet.CreateRow(1);

        var cellB2 = row2.CreateCell(1);
        cellB2.SetCellValue("ADY-TD-SŞ-FR1-00");
        cellB2.CellStyle = centeredBoldBorderStyle;

        // Merge cells from B2 to F2
        sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 5));

        for (int col = 2; col <= 5; col++)
        {
            var cell = row2.CreateCell(col);
            cell.CellStyle = centeredBoldBorderStyle;
        }

        var createdAt = bcc.CreatedDate == null
            ? null
            : bcc.CreatedDate.Value.ToString("yyyy-MM-dd");

        if ("apertech".Equals("ady", StringComparison.OrdinalIgnoreCase))
        {
            createdAt = "2024-09-06";
        }

        var cellG2 = row2.CreateCell(6);
        cellG2.SetCellValue("Hazırlanma tarixi: " + createdAt);
        cellG2.CellStyle = rightBoldBorderStyle;

        // Merge cells from G2 to L2
        sheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 11));

        for (int col = 7; col <= 11; col++)
        {
            var cell = row2.CreateCell(col);
            cell.CellStyle = centeredBoldBorderStyle;
        }
    }
    
    private void PrepareRow3(
        ISheet sheet,
        ICellStyle centeredBoldBorderStyle,
        ICellStyle centeredBoldBorderPaleBlueStyle,
        BidComparisonHeaderDto bcc,
        List<string> requestNumbers)
    {
        var row3 = sheet.CreateRow(2);
        row3.HeightInPoints = 60;

        var cellB3 = row3.CreateCell(1);
        cellB3.SetCellValue("Tələb nömrəsi: ");
        cellB3.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellC3 = row3.CreateCell(2);
        cellC3.CellStyle = centeredBoldBorderStyle;

        // Merge cells from B3 to C3
        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 2));

        var cellD3 = row3.CreateCell(3);
        cellD3.SetCellValue(string.Join(",", requestNumbers));
        cellD3.CellStyle = centeredBoldBorderStyle;

        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 5));

        var cellG3 = row3.CreateCell(6);
        cellG3.SetCellValue("Sifariş tələb tarixi: ");
        cellG3.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellH3 = row3.CreateCell(7);
        cellH3.SetCellValue(bcc.RFQDate.ToString());
        cellH3.CellStyle = centeredBoldBorderStyle;

        var cellJ3 = row3.CreateCell(9);
        cellJ3.SetCellValue("İcraçı: ");
        cellJ3.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellK3 = row3.CreateCell(10);
        cellK3.SetCellValue(bcc.BuyerName);
        cellK3.CellStyle = centeredBoldBorderStyle;

        var cellL3 = row3.CreateCell(11);
        cellL3.CellStyle = centeredBoldBorderStyle;

        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 10, 11));
    }

    private void PrepareRow4(ISheet sheet)
    {
        var row4 = sheet.CreateRow(3);
        row4.HeightInPoints = 5;
        sheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 11));
    }
    
    private void PrepareRow5(
        ISheet sheet,
        ICellStyle centeredBoldBorderStyle,
        ICellStyle centeredBoldBorderPaleBlueStyle,
        BidComparisonHeaderDto bcc,
        List<string> requestDepartmentCodes)
    {
        var row5 = sheet.CreateRow(4);
        row5.HeightInPoints = 60;

        var cellB5 = row5.CreateCell(1);
        cellB5.SetCellValue("Müqayisə cədvəlin № ");
        cellB5.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellC5 = row5.CreateCell(2);
        cellC5.CellStyle = centeredBoldBorderStyle;

        // Merge cells from B5 to C5
        sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 2));

        var cellD5 = row5.CreateCell(3);
        cellD5.SetCellValue(bcc.ComparisonNo);
        cellD5.CellStyle = centeredBoldBorderStyle;

        // If you want the value area to span D5:F5, create E5/F5 too
        var cellE5 = row5.CreateCell(4);
        cellE5.CellStyle = centeredBoldBorderStyle;

        var cellF5 = row5.CreateCell(5);
        cellF5.CellStyle = centeredBoldBorderStyle;

        sheet.AddMergedRegion(new CellRangeAddress(4, 4, 3, 5));

        var cellG5 = row5.CreateCell(6);
        cellG5.SetCellValue("Sifarisci struktur qurum");
        cellG5.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellH5 = row5.CreateCell(7);
        cellH5.SetCellValue(string.Join(",", requestDepartmentCodes));
        cellH5.CellStyle = centeredBoldBorderStyle;

        var cellJ5 = row5.CreateCell(9);
        cellJ5.SetCellValue("Satınalma Methodu");
        cellJ5.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellK5 = row5.CreateCell(10);
        cellK5.SetCellValue(bcc.ProcurementMethodName);
        cellK5.CellStyle = centeredBoldBorderStyle;

        var cellL5 = row5.CreateCell(11);
        cellL5.CellStyle = centeredBoldBorderStyle;

        sheet.AddMergedRegion(new CellRangeAddress(4, 4, 10, 11));
    }

    private void PrepareRow6(ISheet sheet)
    {
        var row6 = sheet.CreateRow(5);
        row6.HeightInPoints = 9;
        sheet.AddMergedRegion(new CellRangeAddress(5, 5, 0, 11));
    }
    
    private void PrepareRow78(
    ISheet sheet,
    ICellStyle centeredBoldBorderPaleBlueStyle,
    List<BidComparisonBidHeaderDto> bids)
{
    var row7 = sheet.CreateRow(6);
    row7.HeightInPoints = 12;

    var row8 = sheet.CreateRow(7);
    row8.HeightInPoints = 26;

    var cellB7 = row7.CreateCell(1);
    cellB7.SetCellValue("№");
    cellB7.CellStyle = centeredBoldBorderPaleBlueStyle;

    row8.CreateCell(1).CellStyle = centeredBoldBorderPaleBlueStyle;
    sheet.AddMergedRegion(new CellRangeAddress(6, 7, 1, 1));

    var cellC7 = row7.CreateCell(2);
    cellC7.SetCellValue("Malın, xidmətin adı və təsviri");
    cellC7.CellStyle = centeredBoldBorderPaleBlueStyle;

    row8.CreateCell(2).CellStyle = centeredBoldBorderPaleBlueStyle;
    sheet.AddMergedRegion(new CellRangeAddress(6, 7, 2, 2));

    var cellD7 = row7.CreateCell(3);
    cellD7.SetCellValue("Texniki xüsusiyyətlər");
    cellD7.CellStyle = centeredBoldBorderPaleBlueStyle;

    row8.CreateCell(3).CellStyle = centeredBoldBorderPaleBlueStyle;
    sheet.AddMergedRegion(new CellRangeAddress(6, 7, 3, 3));

    var cellE7 = row7.CreateCell(4);
    cellE7.SetCellValue("Ölçü vahidi");
    cellE7.CellStyle = centeredBoldBorderPaleBlueStyle;

    row8.CreateCell(4).CellStyle = centeredBoldBorderPaleBlueStyle;
    sheet.AddMergedRegion(new CellRangeAddress(6, 7, 4, 4));

    var cellF7 = row7.CreateCell(5);
    cellF7.SetCellValue("Miqdar");
    cellF7.CellStyle = centeredBoldBorderPaleBlueStyle;

    row8.CreateCell(5).CellStyle = centeredBoldBorderPaleBlueStyle;
    sheet.AddMergedRegion(new CellRangeAddress(6, 7, 5, 5));

    int startColumn = 6;

    foreach (var bid in bids)
    {
        int vendorStartColumn = startColumn;

        var vendorHeaderCell = row7.CreateCell(vendorStartColumn);
        vendorHeaderCell.SetCellValue(bid.vendorName);
        vendorHeaderCell.CellStyle = centeredBoldBorderPaleBlueStyle;

        for (int col = vendorStartColumn + 1; col <= vendorStartColumn + 4; col++)
        {
            row7.CreateCell(col).CellStyle = centeredBoldBorderPaleBlueStyle;
        }

        sheet.AddMergedRegion(new CellRangeAddress(6, 6, vendorStartColumn, vendorStartColumn + 4));

        var cellG8 = row8.CreateCell(startColumn++);
        cellG8.SetCellValue("Vahidinin qiyməti");
        cellG8.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellH8 = row8.CreateCell(startColumn++);
        cellH8.SetCellValue("Miqdar");
        cellH8.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellI8 = row8.CreateCell(startColumn++);
        cellI8.SetCellValue("Cəmi");
        cellI8.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellJ8 = row8.CreateCell(startColumn++);
        cellJ8.SetCellValue("Alternativ");
        cellJ8.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellK8 = row8.CreateCell(startColumn++);
        cellK8.SetCellValue("Status");
        cellK8.CellStyle = centeredBoldBorderPaleBlueStyle;
    }
}
    
    private int PrepareRequestItems(
    ISheet sheet,
    ICellStyle centeredBoldBorderStyle,
    ICellStyle centeredBoldBorderRedStyle,
    List<BidComparisonBidHeaderDto> bids,
    List<BidComparisonRFQDetailsDto> rfqDetails,
    IWorkbook workbook,
    ICellStyle centeredBoldBorderGreenStyle)
{
    int startRow = 8;

    foreach (var rfqDetail in rfqDetails)
    {
        bool haveRejectedBidDetail = false;

        foreach (var bid in bids)
        {
            var rejected = bid.BidDetails?
                .Where(x =>
                    x.RFQDetailId == rfqDetail.RfqDetailId &&
                    x.ApproveStatusId == 4)
                .ToList();

            if (rejected != null && rejected.Any())
            {
                haveRejectedBidDetail = true;
                break;
            }
        }

        var itemStyle = centeredBoldBorderStyle;

        // ---------- Dynamic height ----------
        int rfqDetailDescriptionCount = 0;

        if (!string.IsNullOrEmpty(rfqDetail.RfqDetailDescription))
            rfqDetailDescriptionCount = rfqDetail.RfqDetailDescription.Length;

        if (!string.IsNullOrEmpty(rfqDetail.ItemName) &&
            rfqDetail.ItemName.Length > rfqDetailDescriptionCount)
            rfqDetailDescriptionCount = rfqDetail.ItemName.Length;

        foreach (var bid in bids)
        {
            var bidDetail = bid.BidDetails?
                .FirstOrDefault(x => x.RFQDetailId == rfqDetail.RfqDetailId);

            if (bidDetail == null)
                continue;

            if (!string.IsNullOrEmpty(bidDetail.AlternativeDescription) &&
                bidDetail.AlternativeDescription.Length > rfqDetailDescriptionCount)
            {
                rfqDetailDescriptionCount = bidDetail.AlternativeDescription.Length;
            }
        }

        int rowHeight = 120;

        if (rfqDetailDescriptionCount > 82 && rfqDetailDescriptionCount <= 118)
            rowHeight = 160;
        else if (rfqDetailDescriptionCount <= 150)
            rowHeight = 190;
        else if (rfqDetailDescriptionCount <= 220)
            rowHeight = 230;
        else if (rfqDetailDescriptionCount <= 320)
            rowHeight = 290;
        else if (rfqDetailDescriptionCount <= 420)
            rowHeight = 390;
        else if (rfqDetailDescriptionCount > 420)
            rowHeight = 500;

        // ---------- Row ----------
        var row = sheet.CreateRow(startRow);
        row.HeightInPoints = rowHeight;
        startRow++;

        var cellB = row.CreateCell(1);
        cellB.SetCellValue(rfqDetail.LineNumber?.ToString());
        cellB.CellStyle = itemStyle;

        var cellC = row.CreateCell(2);
        cellC.SetCellValue(rfqDetail.ItemName);
        cellC.CellStyle = itemStyle;

        var cellD = row.CreateCell(3);
        cellD.SetCellValue(rfqDetail.RfqDetailDescription);
        cellD.CellStyle = itemStyle;

        var cellE = row.CreateCell(4);
        cellE.SetCellValue(rfqDetail.UomName);
        cellE.CellStyle = itemStyle;

        var cellF = row.CreateCell(5);
        cellF.SetCellValue(
            rfqDetail.Quantity != null
                ? Math.Round(rfqDetail.Quantity, 2).ToString()
                : "-"
        );
        cellF.CellStyle = itemStyle;

        int startVendorColumn = 6;

        foreach (var bid in bids)
        {
            var bidDetail = bid.BidDetails?
                .FirstOrDefault(x => x.RFQDetailId == rfqDetail.RfqDetailId);

            if (bidDetail == null)
                continue;

            // bool isWon = bidDetail.ApproveStatusId == ApproveStatuses.WON.GetId();
            bool isWon = bidDetail.ApproveStatusId == 1;

            string unitPrice = bidDetail.UnitPrice == null
                ? "-"
                : bidDetail.UnitPrice
                    .ToString("0.###"); // removes trailing zeros

            string totalPrice = bidDetail.TotalAmount == null
                ? "-"
                : Math.Round(bidDetail.TotalAmount, 2).ToString();

            var cellG = row.CreateCell(startVendorColumn++);
            cellG.SetCellValue($"{unitPrice} {bid.currencyKey}");
            cellG.CellStyle = centeredBoldBorderStyle;

            var cellH = row.CreateCell(startVendorColumn++);
            cellH.SetCellValue(
                $"{Math.Round(bidDetail.Quantity ?? 0, 2)} {bidDetail.PUOMName}"
            );
            cellH.CellStyle = centeredBoldBorderStyle;

            var cellI = row.CreateCell(startVendorColumn++);
            cellI.SetCellValue($"{totalPrice} {bid.currencyKey}");
            cellI.CellStyle = centeredBoldBorderStyle;

            var cellJ = row.CreateCell(startVendorColumn++);
            cellJ.SetCellValue(
                bidDetail.AlternativeItem == true
                    ? bidDetail.AlternativeDescription
                    : "-"
            );
            cellJ.CellStyle = centeredBoldBorderStyle;

            string statusText = "";
            var statusStyle = centeredBoldBorderStyle;

            if (isWon)
            {
                statusText = "✓";
                statusStyle = centeredBoldBorderGreenStyle;
            }
            else if (haveRejectedBidDetail)
            {
                statusText = "✘";
                statusStyle = centeredBoldBorderRedStyle;
            }

            var cellK = row.CreateCell(startVendorColumn++);
            cellK.SetCellValue(statusText);
            cellK.CellStyle = statusStyle;
        }
    }

    return startRow;
}

    private int PrepareBidDetail(
        ISheet sheet,
        int lastRow,
        ICellStyle centeredBorderStyle,
        ICellStyle centeredBoldBorderPaleBlueStyle,
        List<BidComparisonBidHeaderDto> bids)
    {
        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Cəmi (ilkin məzənnə ilə)",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => ScaleToTwoDecimalString(bid.total));

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Endirimli məbləğ",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => ScaleToTwoDecimalString(bid.discountedAmount));

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Endirim",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => ScaleToTwoDecimalString(bid.discount));

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Çatdırılma vaxtı",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => bid.deliveryTime ?? string.Empty);

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Çatdırılma şərtləri",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => bid.deliveryTermName ?? string.Empty);

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Bank qarantiyası",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => ToYesNoValue(bid.hasBankGuarantee));

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Ödəniş şərtləri",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => bid.paymentTermName ?? string.Empty);

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Zəmanət",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => bid.warranty ?? string.Empty);

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Vergi dəyəri",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => ScaleToTwoDecimalString(bid.taxValue));

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Yekun (AZN)",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => ScaleToTwoDecimalString(bid.grantTotalAZN));

        lastRow = CreateBidDetailRow(
            sheet,
            lastRow,
            "Əlavə məlumatlar",
            bids,
            centeredBorderStyle,
            centeredBoldBorderPaleBlueStyle,
            bid => bid.operatorComment ?? string.Empty);

        return lastRow;
    }

    private static int CreateBidDetailRow(
        ISheet sheet,
        int lastRow,
        string label,
        List<BidComparisonBidHeaderDto> bids,
        ICellStyle centeredBorderStyle,
        ICellStyle centeredBoldBorderPaleBlueStyle,
        Func<BidComparisonBidHeaderDto, string> valueSelector)
    {
        var row = sheet.CreateRow(lastRow);
        row.HeightInPoints = 15;

        var labelCell = row.CreateCell(1);
        labelCell.SetCellValue(label);
        labelCell.CellStyle = centeredBoldBorderPaleBlueStyle;

        for (var col = 2; col <= 5; col++)
        {
            row.CreateCell(col).CellStyle = centeredBorderStyle;
        }

        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 1, 5));

        var startVendorColumn = 6;
        foreach (var bid in bids)
        {
            var cell = row.CreateCell(startVendorColumn);
            cell.SetCellValue(valueSelector(bid) ?? string.Empty);
            cell.CellStyle = centeredBorderStyle;

            row.CreateCell(startVendorColumn + 1).CellStyle = centeredBorderStyle;
            row.CreateCell(startVendorColumn + 2).CellStyle = centeredBorderStyle;
            row.CreateCell(startVendorColumn + 3).CellStyle = centeredBorderStyle;
            row.CreateCell(startVendorColumn + 4).CellStyle = centeredBorderStyle;

            sheet.AddMergedRegion(
                new CellRangeAddress(lastRow, lastRow, startVendorColumn, startVendorColumn + 4));

            startVendorColumn += 5;
        }

        return lastRow + 1;
    }

    private static string ScaleToTwoDecimalString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed) ||
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed))
        {
            var rounded = Math.Round(parsed, 2, MidpointRounding.AwayFromZero);
            return rounded.ToString("0.00", CultureInfo.InvariantCulture);
        }

        return value;
    }

    private static string ScaleToTwoDecimalString(decimal value)
    {
        var rounded = Math.Round(value, 2, MidpointRounding.AwayFromZero);
        return rounded.ToString("0.00", CultureInfo.InvariantCulture);
    }

    private static string ToYesNoValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "Xeyr";

        if (bool.TryParse(value, out var boolValue))
            return boolValue ? "Bəli" : "Xeyr";

        return value == "1" ? "Bəli" : "Xeyr";
    }

    private int PrepareWinners(
        ISheet sheet,
        int lastRow,
        List<BidComparisonBidHeaderDto> bids,
        ICellStyle centeredBorderStyle,
        ICellStyle centeredBoldBorderPaleBlueStyle)
    {
        var winnersBuilder = new System.Text.StringBuilder();
        var haveAdded = false;

        foreach (var bid in bids)
        {
            var bidDetails = bid.BidDetails?
                // .Where(x => x.ApproveStatusId == ApproveStatuses.WON.GetId())
                .Where(x => x.ApproveStatusId == 5)
                .ToList();

            if (bidDetails == null || !bidDetails.Any())
                continue;

            if (haveAdded)
                winnersBuilder.Append(", ");

            winnersBuilder.Append(bid.vendorName);
            haveAdded = true;
        }

        var winners = winnersBuilder.ToString();

        var rowDiscount = sheet.CreateRow(lastRow);
        rowDiscount.HeightInPoints = 25;

        var cellDiscount = rowDiscount.CreateCell(1);
        cellDiscount.SetCellValue("Udan(lar)");
        cellDiscount.CellStyle = centeredBoldBorderPaleBlueStyle;

        for (var col = 2; col <= 5; col++)
        {
            rowDiscount.CreateCell(col).CellStyle = centeredBorderStyle;
        }

        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 1, 5));

        var cell = rowDiscount.CreateCell(6);
        cell.SetCellValue(winners);
        cell.CellStyle = centeredBorderStyle;

        rowDiscount.CreateCell(7).CellStyle = centeredBorderStyle;
        rowDiscount.CreateCell(8).CellStyle = centeredBorderStyle;

        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 6, 8));

        lastRow++;
        return lastRow;
    }

    private int PrepareApprovedUsers(
        ISheet sheet,
        int lastRow,
        List<BidComparisonApprovedUsersApprovalInformationDto> approvedUsers,
        ICellStyle centeredBorderStyle,
        ICellStyle centeredBoldBorderPaleBlueStyle)
    {
        var rowApproveText = sheet.CreateRow(lastRow);
        rowApproveText.HeightInPoints = 25;

        var cellApproveText = rowApproveText.CreateCell(1);
        cellApproveText.SetCellValue("Təsdiq etdilər");
        cellApproveText.CellStyle = centeredBoldBorderPaleBlueStyle;

        for (var col = 2; col <= 11; col++)
        {
            rowApproveText.CreateCell(col).CellStyle = centeredBorderStyle;
        }

        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 1, 11));
        lastRow++;

        var rowHeadings = sheet.CreateRow(lastRow);
        rowHeadings.HeightInPoints = 15;

        var cellNumber = rowHeadings.CreateCell(1);
        cellNumber.SetCellValue("№");
        cellNumber.CellStyle = centeredBoldBorderPaleBlueStyle;

        var cellUser = rowHeadings.CreateCell(2);
        cellUser.SetCellValue("Ad və Soyad");
        cellUser.CellStyle = centeredBoldBorderPaleBlueStyle;
        rowHeadings.CreateCell(3).CellStyle = centeredBoldBorderPaleBlueStyle;
        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 2, 3));

        var cellDate = rowHeadings.CreateCell(4);
        cellDate.SetCellValue("Tarix");
        cellDate.CellStyle = centeredBoldBorderPaleBlueStyle;
        rowHeadings.CreateCell(5).CellStyle = centeredBoldBorderPaleBlueStyle;
        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 4, 5));

        var cellComment = rowHeadings.CreateCell(6);
        cellComment.SetCellValue("Qeyd");
        cellComment.CellStyle = centeredBoldBorderPaleBlueStyle;
        for (var col = 7; col <= 11; col++)
        {
            rowHeadings.CreateCell(col).CellStyle = centeredBoldBorderPaleBlueStyle;
        }
        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 6, 11));

        lastRow++;
        var dataNumber = 1;

        foreach (var approvedUser in approvedUsers)
        {
            var rowData = sheet.CreateRow(lastRow);
            rowData.HeightInPoints = 15;

            var cellNumberData = rowData.CreateCell(1);
            cellNumberData.SetCellValue(dataNumber++);
            cellNumberData.CellStyle = centeredBorderStyle;

            var cellUserData = rowData.CreateCell(2);
            cellUserData.SetCellValue(approvedUser.fullName ?? string.Empty);
            cellUserData.CellStyle = centeredBorderStyle;

            rowData.CreateCell(3).CellStyle = centeredBorderStyle;
            sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 2, 3));

            var cellDateData = rowData.CreateCell(4);
            cellDateData.SetCellValue(approvedUser.approveDate?.ToString() ?? string.Empty);
            cellDateData.CellStyle = centeredBorderStyle;

            rowData.CreateCell(5).CellStyle = centeredBorderStyle;
            sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 4, 5));

            var cellCommentData = rowData.CreateCell(6);
            cellCommentData.SetCellValue(approvedUser.comment ?? string.Empty);
            cellCommentData.CellStyle = centeredBorderStyle;

            for (var col = 7; col <= 11; col++)
            {
                rowData.CreateCell(col).CellStyle = centeredBorderStyle;
            }

            sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 6, 11));

            lastRow++;
        }

        return lastRow;
    }

    private Task GenerateQrCodeAsync(IWorkbook workbook, ISheet sheet, int rfqMainId)
    {
        var baseUrl = Environment.GetEnvironmentVariable("Mail__ServerUrlUI")
                      ?? Environment.GetEnvironmentVariable("MAIL__SERVERURLUI")
                      ?? string.Empty;
        baseUrl = baseUrl.TrimEnd('/');

        var qrText = $"{baseUrl}/procurement/bid-comparison/card/{rfqMainId}";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        var qrBytes = qrCode.GetGraphic(20);

        var pictureIdx = workbook.AddPicture(qrBytes, PictureType.PNG);
        var drawing = sheet.CreateDrawingPatriarch();
        var helper = workbook.GetCreationHelper();
        var anchor = helper.CreateClientAnchor();

        anchor.Col1 = 12;
        anchor.Row1 = 0;
        anchor.Col2 = 13;
        anchor.Row2 = 1;
        anchor.Dx1 = 0;
        anchor.Dy1 = 0;

        var picture = drawing.CreatePicture(anchor, pictureIdx);
        picture.Resize(1.0);

        var printSetup = sheet.PrintSetup;
        printSetup.Landscape = true;
        sheet.FitToPage = true;
        sheet.Autobreaks = true;
        printSetup.FitWidth = 1;
        printSetup.FitHeight = 1;

        return Task.CompletedTask;
    }

    private int PrepareRowEmpty(ISheet sheet, int lastRow)
    {
        var row = sheet.CreateRow(lastRow);
        row.HeightInPoints = 9;
        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 0, 11));

        lastRow++;
        return lastRow;
    }

    private int PrepareRfxComment(
        ISheet sheet,
        int lastRow,
        BidComparisonHeaderDto bcc,
        ICellStyle centeredBorderStyle,
        ICellStyle centeredBoldBorderPaleBlueStyle)
    {
        var rfxComment = bcc.RFQComment;

        var rowDiscount = sheet.CreateRow(lastRow);
        rowDiscount.HeightInPoints = 25;

        var cellDiscount = rowDiscount.CreateCell(1);
        cellDiscount.SetCellValue("RFX şərh");
        cellDiscount.CellStyle = centeredBoldBorderPaleBlueStyle;

        for (var col = 2; col <= 5; col++)
        {
            rowDiscount.CreateCell(col).CellStyle = centeredBorderStyle;
        }

        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 1, 5));

        var cell = rowDiscount.CreateCell(6);
        cell.SetCellValue(rfxComment ?? string.Empty);
        cell.CellStyle = centeredBorderStyle;

        rowDiscount.CreateCell(7).CellStyle = centeredBorderStyle;
        rowDiscount.CreateCell(8).CellStyle = centeredBorderStyle;

        sheet.AddMergedRegion(new CellRangeAddress(lastRow, lastRow, 6, 8));

        lastRow++;
        return lastRow;
    }
}
