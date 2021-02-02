using System;
using System.Text;
using Xunit;

namespace PayPayWebCashierTest
{
    public class PayPayWebCashierTest
    {
        [Fact]
        public void GetHeaderAuthorizationObject_HashLogicTest_001()
        {
            long epoc = 1579843452;
            var apikey = "APIKeyGenerated";
            var apiSecret = "APIKeySecretGenerated";
            var body = "{\"sampleRequestBodyKey1\":\"sampleRequestBodyValue1\",\"sampleRequestBodyKey2\":\"sampleRequestBodyValue2\"}";
            var contentType = "application/json;charset=UTF-8;";
            var path = "/v2/codes";
            var method = "POST";
            var merchantId = "acd028";

            var paypayweb = PayPayWebCashier.PayPayWebCashier.Create(apikey, apiSecret, merchantId);

            var authHeader = paypayweb.GetHeaderAuthorizationObject(body, contentType, path, method, merchantId, epoc);

            Assert.Equal("OPA-Auth:APIKeyGenerated:NW1jKIMnzR7tEhMWtcJcaef+nFVBt7jjAGcVuxHhchc=:acd028:1579843452:1j0FnY4flNp5CtIKa7x9MQ==", authHeader);
        }
    }
}
