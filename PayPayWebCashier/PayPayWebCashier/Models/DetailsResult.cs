using Newtonsoft.Json;
using PayPayWebCashier.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayPayWebCashier.Models
{
    public class DetailsResult
    {
        [JsonProperty("resultInfo")]
        public ResultInfo Result { get; set; }

        [JsonProperty("data")]
        public ResultData Data { get; set; }

        public class ResultData
        {
            [JsonProperty("paymentId")]
            public string PaymentId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("acceptedAt")]
            public long AcceptedAt { get; set; }

            [JsonProperty("refunds")]
            public RefundRequestModel Refunds { get; set; }

            [JsonProperty("revert")]
            public RevertEntity Revert { get; set; }

            [JsonProperty("merchantPaymentId")]
            public string MerchantPaymentId { get; set; }

            [JsonProperty("amount")]
            public PayPayAmount Amount { get; set; }

            [JsonProperty("requestedAt")]
            public long RequestedAt { get; set; }

            [JsonProperty("canceledAt")]
            public long CanceledAt { get; set; }

            [JsonProperty("storeId")]
            public string StoreId { get; set; }

            [JsonProperty("terminalId")]
            public string TerminalId { get; set; }

            [JsonProperty("orderReceiptNumber")]
            public string OrderReceiptNumber { get; set; }

            [JsonProperty("orderDescription")]
            public string OrderDescription { get; set; }

            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }
        }
    }
}
