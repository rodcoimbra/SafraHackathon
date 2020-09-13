using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Report_Generator_V1.Model.TransactionData
{
    public class Amount
    {
        [JsonProperty("amount")]
        public string _Amount;

        [JsonProperty("currency")]
        public string Currency;
    }

    public class BankTransactionCode
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("subCode")]
        public string SubCode;
    }

    public class ProprietaryBankTransactionCode
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("issuer")]
        public string Issuer;
    }

    public class Amount2
    {
        [JsonProperty("amount")]
        public string Amount;

        [JsonProperty("currency")]
        public string Currency;
    }

    public class Balance
    {
        [JsonProperty("amount")]
        public Amount2 Amount;

        [JsonProperty("creditDebitIndicator")]
        public string CreditDebitIndicator;

        [JsonProperty("type")]
        public string Type;
    }

    public class Transaction
    {
        [JsonProperty("accountId")]
        public string AccountId;

        [JsonProperty("transactionId")]
        public string TransactionId;

        [JsonProperty("amount")]
        public Amount Amount;

        [JsonProperty("creditDebitIndicator")]
        public string CreditDebitIndicator;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("bookingDateTime")]
        public DateTime BookingDateTime;

        [JsonProperty("valueDateTime")]
        public DateTime ValueDateTime;

        [JsonProperty("transactionInformation")]
        public string TransactionInformation;

        [JsonProperty("bankTransactionCode")]
        public BankTransactionCode BankTransactionCode;

        [JsonProperty("proprietaryBankTransactionCode")]
        public ProprietaryBankTransactionCode ProprietaryBankTransactionCode;

        [JsonProperty("balance")]
        public Balance Balance;
    }

    public class Data
    {
        [JsonProperty("transaction")]
        public List<Transaction> Transaction;
    }

    public class Links
    {
        [JsonProperty("self")]
        public string Self;
    }

    public class Root
    {
        [JsonProperty("data")]
        public Data Data;

        [JsonProperty("links")]
        public Links Links;
    }






}
