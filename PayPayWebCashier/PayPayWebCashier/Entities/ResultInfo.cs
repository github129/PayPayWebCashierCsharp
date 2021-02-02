using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayPayWebCashier.Entities
{
    public class ResultInfo
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("codeId")]
        public string CodeId { get; set; }
    }
}
