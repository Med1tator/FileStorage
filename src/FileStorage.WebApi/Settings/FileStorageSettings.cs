namespace FileStorage.WebApi.Settings
{
    public class FileStorageSettings
    {
        public QiNiuSettings QiNiuSettings { get; set; }
        public AliSettings AliSettings { get; set; }
        public string FileStorageType { get; set; }
    }
}
