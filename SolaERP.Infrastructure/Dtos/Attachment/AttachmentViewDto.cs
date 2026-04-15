namespace SolaERP.Application.Dtos.Attachment;

public class AttachmentViewDto
{
    public int Id { get; set; }
    public int? LineNumber { get; set; }
    public string SourceId { get; set; }
    public string FileName { get; set; }
    public string FileLink { get; set; }
    public string PreviewLink { get; set; }
    public string ExtensionType { get; set; }
    public string Size { get; set; }
    public string SourceTypeId { get; set; }
    public string Comment { get; set; }

}