using Newtonsoft.Json;
using PayPayWebCashier.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayPayWebCashier.Models
{
    public class CreateCodeResult
    {
        [JsonProperty("resultInfo")]
        public ResultInfo Result { get; set; }

        [JsonProperty("data")]
        public ResultData Data { get; set; }

        public class ResultData : PayPayEntity
        {
            [JsonProperty("codeId")]
            public string CodeId { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("expiryDate")]
            public long ExpiryDate { get; set; }
        }
    }
}
