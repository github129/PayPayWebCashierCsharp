using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayPayWebCashier.Models
{
    public class RefundRequestModel
    {
        [JsonProperty("merchantRefundId")]
        public string MerchantRefundId { get; set; }

        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }

        [JsonProperty("amount")]
        public Entities.PayPayAmount Amount { get; set; }

        [JsonProperty("requestedAt")]
        public long RequestedAt { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
