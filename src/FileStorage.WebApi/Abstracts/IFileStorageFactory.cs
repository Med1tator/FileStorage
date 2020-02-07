namespace FileStorage.WebApi.Abstracts
{
    public interface IFileStorageFactory
    {
        public abstract FileStorage CreateFileStorage();
    }
}
