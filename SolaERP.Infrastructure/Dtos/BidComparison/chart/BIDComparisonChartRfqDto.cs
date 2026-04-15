using SolaERP.Application.Dtos.RFQ;

namespace SolaERP.Application.Dtos.BidComparison;

public class BIDComparisonChartRfqDto
{
    public List<BidComparisonRFQDetailsDto> RfqDetails { get; set; }
    public List<RFQRequestInformationDto> rfqRequestInformations { get; set; }
        
}