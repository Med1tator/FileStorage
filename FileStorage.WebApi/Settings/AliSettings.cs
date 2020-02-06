using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.WebApi.Settings
{
    public class AliSettings
    {
        public string AK { get; set; }
        public string SK { get; set; }
        public string BucketEndpoint { get; set; }
        public string DefaultBucket { get; set; }
        public string FileUrlPrefix { get; set; }
    }
}
