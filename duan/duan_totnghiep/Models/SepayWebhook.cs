using Microsoft.AspNetCore.Mvc;

namespace duan_totnghiep.Models
{
    public class SepayWebhook
    {
        public int id { get; set; }

        public string gateway { get; set; }

        public string transactionDate { get; set; }

        public string accountNumber { get; set; }

        public string content { get; set; }

        public decimal transferAmount { get; set; }

        public decimal accumulated { get; set; }

        public string code { get; set; }

        public string referenceCode { get; set; }

        public string description { get; set; }
    }
}
