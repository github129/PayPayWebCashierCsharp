using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayPayWebCashier.Entities
{
    public class PayPayEntity
    {
        [JsonProperty("merchantPaymentId")]
        public string MerchantPaymentId { get; set; }

        [JsonProperty("amount")]
        public PayPayAmount Amount { get; set; }

        [JsonProperty("orderDescription")]
        public string OrderDescription { get; set; }

        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        [JsonProperty("codeType")]
        public string CodeType { get; set; }

        [JsonProperty("storeInfo")]
        public string StoreInfo { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonProperty("assumeMerchant")]
        public long AssumeMerchant { get; set; }

        [JsonProperty("redirectType")]
        public string RedirectType { get; set; } = "WEB_LINK";

        [JsonProperty("isAuthorization")]
        public bool IsAuthorization { get; set; } = false;
    }

    public class PayPayAmount
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
