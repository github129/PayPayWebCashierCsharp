using Newtonsoft.Json;
using PayPayWebCashier.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayPayWebCashier.Models
{
    public class RefundResult
    {
        [JsonProperty("resultInfo")]
        public ResultInfo Result { get; set; }

        [JsonProperty("data")]
        public ResultData Data { get; set; }

        public class ResultData : RefundRequestModel
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("acceptedAt")]
            public long AcceptedAt { get; set; }

            [JsonProperty("expiryDate")]
            public long ExpiryDate { get; set; }

            [JsonProperty("assumeMerchant")]
            public long AssumeMerchant { get; set; }
        }
    }
}
