using FileStorage.WebApi.Abstracts;
using FileStorage.WebApi.Settings;

namespace FileStorage.WebApi.Implementations
{
    public class QiNiuStorageFactory : IFileStorageFactory
    {
        private readonly QiNiuSettings qiNiuSettings;

        public QiNiuStorageFactory(QiNiuSettings qiNiuSettings)
        {
            this.qiNiuSettings = qiNiuSettings;
        }

        public Abstracts.FileStorage CreateFileStorage()
        {
            return new QiNiuStorage(qiNiuSettings);
        }
    }
}
