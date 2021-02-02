# PayPayWebCashierCsharp

## What is this?

So you are a developer and want to start accepting payments using PayPay.
PayPayWebCashierCsharp makes it easy to implement PayPay's WebCashier in C#.

## How to use

### Create a Code

```
{
    string apikey = "YOUR API KEY";
    string apiSecret = "YOUR API KEY SECRET";
    string merchantId = "YOUR MERCHANT ID";
    var paypayweb = PayPayWebCashier.PayPayWebCashier.Create(apikey, apiSecret, merchantId);
    
    string merchantPaymentId = "";
    int amount = 100;
    string redirectUrl = "";
    Dictionary<string, string> metaData = new Dictionary<string, string>();

    var result = await paypayweb.CreateCodeAsync(merchantPaymentId, amount, redirectUrl, metaData);

    // Payment URL
    Console.WriteLine(result.Data.Url);
}

```

### Refund


### Details


