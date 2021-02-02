using PayPayWebCashier.Entities;
using PayPayWebCashier.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayPayWebCashier
{
    interface IPayPayWebCashier
    {
        /// <summary>
        /// Create a Code
        /// 支払いのためのコードを作成します。作成したコードの有効期限は「expiryDate」に設定されます。
        /// </summary>
        /// <param name="merchantPaymentId"></param>
        /// <param name="amount"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="metaData"></param>
        /// <param name="codeType"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        Task<CreateCodeResult> CreateCodeAsync(string merchantPaymentId, int amount, string redirectUrl, Dictionary<string, string> metaData, string codeType = "ORDER_QR", string currency = "JPY");

        /// <summary>
        /// Refund a payment
        /// 返金を行います。
        /// </summary>
        /// <param name="merchantRefundId"></param>
        /// <param name="paymentId"></param>
        /// <param name="amount"></param>
        /// <param name="reason"></param>
        /// <param name="nonce"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        string GetHeaderAuthorizationObject(string body, string contentType, string path, string method, string transactionId, long epoc);
    }
}
