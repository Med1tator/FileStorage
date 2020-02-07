namespace FileStorage.WebApi.Models
{
    public class UploadFileResult
    {
        public string FileName { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string FileUrl { get; set; }
    }
}
