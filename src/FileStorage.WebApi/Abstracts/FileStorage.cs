using FileStorage.WebApi.Models;
using System.IO;
using System.Threading.Tasks;

namespace FileStorage.WebApi.Abstracts
{
    public abstract class FileStorage
    {
        public abstract UploadFileResult UploadFile(Stream stream, string fileName = null);

        public abstract Task<UploadFileResult> UploadFileAsync(Stream stream, string fileName = null);
    }
}
