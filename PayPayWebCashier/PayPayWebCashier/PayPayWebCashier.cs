using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PayPayWebCashier.Entities;
using PayPayWebCashier.Models;

namespace PayPayWebCashier
{
    public class PayPayWebCashier: IPayPayWebCashier
    {

        protected const string BASE_URL = "https://api.paypay.ne.jp";
        protected const string DELIMITER = "\n";
        protected readonly string ApiKey;
        protected readonly string MerchantId;
        protected readonly byte[] ApiSecret;
        protected readonly string Uri;
        protected static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="merchantId"></param>
        /// <param name="uri"></param>
        protected PayPayWebCashier(string apiKey, string apiSecret, string merchantId, string uri)
        {
            this.ApiKey = apiKey;
            this.ApiSecret = Encoding.UTF8.GetBytes(apiSecret);
            this.Uri = uri;
            this.MerchantId = merchantId;
        }

        /// <summary>
        /// Singleton
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="merchantId"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static PayPayWebCashier Create(string apiKey, string apiSecret, string merchantId, string uri = BASE_URL)
        {
            return new PayPayWebCashier(apiKey, apiSecret, merchantId, uri);
        }

        public virtual async Task<CreateCodeResult> CreateCodeAsync(string merchantPaymentId, int amount, string redirectUrl, Dictionary<string, string> metaData, string codeType = "ORDER_QR", string currency = "JPY")
        {
            var requestOpject = new PayPayEntity
            {
                MerchantPaymentId = merchantPaymentId,
                Amount = new PayPayAmount
                {
                    Amount = amount,
                    Currency = currency,
                },
                Metadata = metaData,
                CodeType = codeType,
                RedirectUrl = redirectUrl,
            };

            var json = JsonConvert.SerializeObject(requestOpject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var contentType = content.Headers.GetValues("Content-Type").First();

            DateTimeOffset timestamp = DateTimeOffset.UtcNow;
            long epoc = timestamp.ToUnixTimeSeconds();
            var headerAuthorizationObject = this.GetHeaderAuthorizationObject(json, contentType, "/v2/codes", "POST", merchantPaymentId, epoc);
            var authorization = new AuthenticationHeaderValue("hmac", headerAuthorizationObject);

            // Request CreateCode
            var result = await SendAsync(HttpMethod.Post, $"{this.Uri}/v2/codes", authorization, content, this.MerchantId);

            var responseContent = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CreateCodeResult>(responseContent);

            return response;
        }

        public virtual async Task<RefundResult> RefundPaymentAsync(string merchantRefundId, string paymentId, int amount, string reason, string nonce, string currency = "JPY")
        {
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;
            long epoc = timestamp.ToUnixTimeSeconds();

            RefundRequestModel requestOpject = new RefundRequestModel
            {
                MerchantRefundId = merchantRefundId,
                Amount = new PayPayAmount
                {
                    Amount = amount,
                    Currency = currency,
                },
                PaymentId = paymentId,
                RequestedAt = epoc,
                Reason = reason,
            };

            string json = JsonConvert.SerializeObject(requestOpject);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            string contentType = content.Headers.GetValues("Content-Type").First();
            string path = "/v2/refunds";
            string method = "POST";

            var headerAuthorizationObject = this.GetHeaderAuthorizationObject(json, contentType, path, method, nonce, epoc);
            var authorization = new AuthenticationHeaderValue("hmac", headerAuthorizationObject);

            var result = await SendAsync(HttpMethod.Post, $"{this.Uri}{path}", authorization, content, this.MerchantId);

            var responseContent = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RefundResult>(responseContent);

            return response;
        }

        /// <summary>
        /// SHA256(MD5)
        /// https://www.paypay.ne.jp/opa/doc/jp/v1.0/webcashier#section/Authentication
        /// </summary>
        /// <returns></returns>
        public string GetHeaderAuthorizationObject(string body, string contentType, string path, string method, string nonce, long epoc)
        {
            var authHeader = this.GetAuthorizationObject(this.ApiKey, this.ApiSecret, body, contentType, path, method, nonce, epoc);

            return authHeader;
        }

        /// <summary>
        /// Send Request
        /// </summary>
        /// <param name="method"></param>
        /// <param name="requestUri"></param>
        /// <param name="authenticationHeaderValue"></param>
        /// <param name="content"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> SendAsync(HttpMethod method, string requestUri, AuthenticationHeaderValue authenticationHeaderValue, HttpContent content, string merchantId)
        {
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Authorization = authenticationHeaderValue;
            request.Headers.Add("X-ASSUME-MERCHANT", merchantId);

            request.Content = content;
            var response = await _client.SendAsync(request).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// SHA256(MD5)
        /// https://www.paypay.ne.jp/opa/doc/jp/v1.0/webcashier#section/Authentication
        /// </summary>
        /// <returns></returns>
        private string GetAuthorizationObject(string apiKey, byte[] apiSecret, string body, string contentType, string path, string method, string nonce, long epoc)
        {
            var hash = this.PayPayMD5(body, contentType);
            var macData = this.PayPaySha256(hash, apiSecret, contentType, path, method, nonce, epoc.ToString());

            string headerHash = "empty";
            if (string.IsNullOrEmpty(hash) == false)
            {
                headerHash = hash;
            }

            var authHeader = $"OPA-Auth:{apiKey}:{macData}:{nonce}:{epoc}:{headerHash}";

            return authHeader;
        }


        private string PayPayMD5(string body, string contentType)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            try
            {
                string value = $"{contentType}{body}";

                // 第１HASH MD5
                var byteBody = System.Text.Encoding.UTF8.GetBytes(body);
                var byteContentType = System.Text.Encoding.UTF8.GetBytes(contentType);

                var byteValue = byteBody.Concat(byteContentType).ToArray();


                byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
                byte[] bs = md5.ComputeHash(data);
                string result = Convert.ToBase64String(bs);

                return result;
            }
            catch
            {
            }
            finally
            {
                md5.Clear();
            }

            return string.Empty;
        }

        /// <summary>
        /// ①secretで sha256HMAC を init
        /// ②指定した文字列の組み合わせをハッシュ化
        /// ③ ②の値を base64 に変換して完了
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="contentType"></param>
        /// <param name="path"></param>
        /// <param name="method"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        private string PayPaySha256(string hash, byte[] apiSecret, string contentType, string path, string method, string nonce, string timestamp)
        {
            string value = this.Sha256Hash(hash, apiSecret, contentType, path, method, nonce, timestamp);
            return value;
        }

        /// <summary>
        /// ①secretで sha256HMAC を init
        /// ②指定した文字列の組み合わせをハッシュ化
        /// ③ ②の値を base64 に変換して完了
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="contentType"></param>
        /// <param name="path"></param>
        /// <param name="method"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        private string Sha256Hash(string hash, byte[] apiSecret, string contentType, string path, string method, string nonce, string timestamp)
        {
            string value = $"{path}{DELIMITER}{method}{DELIMITER}{nonce}{DELIMITER}{timestamp}{DELIMITER}{contentType}{DELIMITER}{hash}";

            var secretHash = new HMACSHA256(apiSecret);
            var byteValue = Encoding.UTF8.GetBytes(value);
            var byteResult = secretHash.ComputeHash(byteValue);
            string result = Convert.ToBase64String(byteResult);

            return result;
        }
    }
}
