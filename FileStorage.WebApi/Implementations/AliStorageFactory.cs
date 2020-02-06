using FileStorage.WebApi.Abstracts;
using FileStorage.WebApi.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.WebApi.Implementations
{
    public class AliStorageFactory : IFileStorageFactory
    {
        private readonly AliSettings aliSettings;

        public AliStorageFactory(AliSettings aliSettings)
        {
            this.aliSettings = aliSettings;
        }

        public Abstracts.FileStorage CreateFileStorage()
        {
            return new AliStorage(aliSettings);
        }
    }
}
