using Aliyun.OSS;
using Aliyun.OSS.Util;
using FileStorage.WebApi.Models;
using FileStorage.WebApi.Settings;
using FileStorage.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.WebApi.Implementations
{
    public class AliStorage : Abstracts.FileStorage
    {
        private readonly AliSettings aliSettings;

        public AliStorage(AliSettings aliSettings)
        {
            this.aliSettings = aliSettings;
        }

        public override UploadFileResult UploadFile(Stream stream, string fileName = null, string savePath = null)
        {
            return UploadFileAsync(stream, fileName, savePath).Result;
        }

        public async override Task<UploadFileResult> UploadFileAsync(Stream stream, string fileName = null, string savePath = null)
        {
            var client = new OssClient(aliSettings.BucketEndpoint, aliSettings.AK, aliSettings.SK);
            try
            {
                var dayPath = DateTime.Today.ToString("yyyy/MM/dd");
                var saveKey = string.IsNullOrWhiteSpace(savePath)
                        ? $"{dayPath}/{fileName}"
                        : $"{savePath}/{fileName}";

                if (client.DoesObjectExist(aliSettings.DefaultBucket, saveKey))
                {
                    var uploadObjectMd5 = OssUtils.ComputeContentMd5(stream, stream.Length);
                    var existObjectMd5 = client.GetObjectMetadata(aliSettings.DefaultBucket, saveKey).ContentMd5;
                    if (uploadObjectMd5 != existObjectMd5)
                    {
                        saveKey = string.IsNullOrWhiteSpace(savePath)
                            ? $"{dayPath}/{StringUtilities.GenerateString(16, containsUpperCase: false, containsSpecialChar: false)}-{fileName}" 
                            : $"{savePath}/{StringUtilities.GenerateString(16, containsUpperCase: false, containsSpecialChar: false)}-{fileName}";
                    }
                }
                var uploadResult = client.PutObject(aliSettings.DefaultBucket, saveKey, stream);

                if (uploadResult.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var fileUrl = $"{aliSettings.FileUrlPrefix.TrimEnd('/')}/{saveKey}";
                    return new UploadFileResult { Status = 1, Message = $"File upload successfully.", FileName = fileName, FileUrl = fileUrl };
                }

                return new UploadFileResult { Status = 0, Message = "File upload failed.", FileName = fileName };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UploadFileAsync ERROR:{ex.Message}");
                return new UploadFileResult { Status = 0, Message = $"File upload failed. Error Message:{ex.Message}", FileName = fileName };
            }
        }
    }
}
