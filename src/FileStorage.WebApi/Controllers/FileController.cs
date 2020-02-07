using FileStorage.WebApi.Implementations;
using FileStorage.WebApi.Models;
using FileStorage.WebApi.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileStorageSettings fileStorageSettings;

        public FileController(FileStorageSettings fileStorageSettings)
        {
            this.fileStorageSettings = fileStorageSettings;
        }

        /// <summary>
        /// 上传文件流，返回存储地址
        /// </summary>
        /// <param name="imageBuffer"></param>
        /// <returns></returns>
        //public async Task<IActionResult> Upload(byte[] buffer, string fileName)
        [HttpPost]
        [Route("upload")]
        //[Consumes("application/json", "multipart/form-data")]
        public async Task<IActionResult> UploadAsync([FromForm]IFormCollection data)
        {
            var result = new List<UploadFileResult>();
            var files = data.Files;
            if (files != null && files.Any())
            {
                //var fileStorageFactory = fileStorageSettings.FileStorageType switch
                //{
                //    FileStorageType.QiNiu => new QiNiuStorageFactory(fileStorageSettings.QiNiuSettings),
                //    FileStorageType.Ali => new AliStorageFactory(fileStorageSettings.AliSetting),
                //    _ => new QiNiuStorageFactory(fileStorageSettings.QiNiuSettings)
                //};

                Abstracts.IFileStorageFactory fileStorageFactory;
                switch (fileStorageSettings.FileStorageType)
                {
                    case FileStorageType.QiNiu:
                        fileStorageFactory = new QiNiuStorageFactory(fileStorageSettings.QiNiuSettings);
                        break;
                    case FileStorageType.Ali:
                        fileStorageFactory = new AliStorageFactory(fileStorageSettings.AliSettings);
                        break;
                    default:
                        fileStorageFactory = new QiNiuStorageFactory(fileStorageSettings.QiNiuSettings);
                        break;
                }

                var fileStorage = fileStorageFactory.CreateFileStorage();

                foreach (var file in files)
                {
                    using var fileStream = file.OpenReadStream();
                    var uploadEntry = await fileStorage.UploadFileAsync(fileStream, file.FileName);
                    result.Add(uploadEntry);
                }
            }

            return new JsonResult(result);
        }
    }
}