using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayPayWebCashier.Entities
{
    public class RevertEntity
    {
        [JsonProperty("acceptedAt")]
        public long AcceptedAt { get; set; }

        [JsonProperty("merchantRevertId")]
        public string MerchantRevertId { get; set; }

        [JsonProperty("requestedAt")]
        public long RequestedAt { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
