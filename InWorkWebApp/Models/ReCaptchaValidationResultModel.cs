using Newtonsoft.Json;
using System.Collections.Generic;

namespace InWorkWebApp.Models
{
    public class ReCaptchaValidationResultModel
    {
        public bool Success { get; set; }

        public string HostName { get; set; }

        [JsonProperty("challenge_ts")]
        public string TimeStamp { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}