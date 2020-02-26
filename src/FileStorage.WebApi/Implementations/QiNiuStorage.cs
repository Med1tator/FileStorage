using FileStorage.WebApi.Models;
using FileStorage.WebApi.Settings;
using FileStorage.WebApi.Utilities;
using Newtonsoft.Json;
using Qiniu.Common;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using System;
using System.IO;
using System.Threading.Tasks;
using FileStorage.WebApi.Abstracts;

namespace FileStorage.WebApi.Implementations
{
    public class QiNiuStorage : Abstracts.FileStorage
    {
        private readonly QiNiuSettings qiNiuSettings;

        public QiNiuStorage(QiNiuSettings qiNiuSettings)
        {
            this.qiNiuSettings = qiNiuSettings;
        }

        public async override Task<UploadFileResult> UploadFileAsync(Stream stream, string fileName = null, string savePath = null)
        {
            try
            {
                Config.AutoZone(qiNiuSettings.AK, qiNiuSettings.DefaultBucket, false);
                var mac = new Mac(qiNiuSettings.AK, qiNiuSettings.SK);

                var putPolicy = new PutPolicy
                {
                    Scope = qiNiuSettings.DefaultBucket
                };
                putPolicy.SetExpires(3600);

                var jstr = putPolicy.ToJsonString();
                var token = Auth.CreateUploadToken(mac, jstr);

                var result = await UploadAsync(stream, fileName, token);
                Console.WriteLine(result);


                if (result.Code == (int)Qiniu.Http.HttpCode.OK)
                {
                    var qiNiuUploadFileResult = JsonConvert.DeserializeObject<QiNiuUploadFileResult>(result.Text);

                    var fileUrl = $"{qiNiuSettings.FileUrlPrefix.TrimEnd('/')}/{qiNiuUploadFileResult.Key}";
                    return new UploadFileResult { Status = 1, Message = "File upload successfully.", FileName = fileName, FileUrl = fileUrl };
                }

                return new UploadFileResult { Status = 0, Message = "File upload failed.", FileName = fileName };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UploadFileAsync ERROR:{ex.Message}");
                return new UploadFileResult { Status = 0, Message = $"File upload failed. Error Message:{ex.Message}", FileName = fileName };
            }
        }

        public async Task<Qiniu.Http.HttpResult> UploadAsync(Stream stream, string fileName, string token, string savePath = null)
        {
            UploadManager um = new UploadManager();

            var dayPath = DateTime.Today.ToString("yyyy/MM/dd");
            var saveKey = string.IsNullOrWhiteSpace(savePath)
                ? $"{dayPath}/{fileName}"
                : $"{savePath}/{fileName}";

            var result = await um.UploadStreamAsync(stream, saveKey, token);
            if (result.Code == (int)Qiniu.Http.HttpCode.FILE_EXISTS)
            {
                saveKey = string.IsNullOrWhiteSpace(savePath)
                    ? $"{dayPath}/{StringUtilities.GenerateString(16, containsUpperCase: false, containsSpecialChar: false)}-{fileName}"
                    : $"{savePath}/{StringUtilities.GenerateString(16, containsUpperCase: false, containsSpecialChar: false)}-{fileName}";
                result = await UploadAsync(stream, saveKey, token);
            }
            return result;
        }

        public override UploadFileResult UploadFile(Stream stream, string fileName = null, string savePath = null)
        {
            return UploadFileAsync(stream, fileName, savePath).Result;
        }
    }
}
