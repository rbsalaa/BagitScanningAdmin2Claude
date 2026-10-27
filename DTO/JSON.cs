namespace SelfKOrderWeb.DTO
{
    public class OrderResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public List<TotalLine> totals { get; set; }
        public DateTime dateReceivedByPos { get; set; }
        public string comments { get; set; }
        public string referenceId { get; set; }
        public string source { get; set; }
        public string orderName { get; set; }
        public Fulfillment fulfillment { get; set; }
        public List<OrderLine> orderLines { get; set; }
        public List<Payment> payments { get; set; }
        public long checkId { get; set; }
        public int checkNo { get; set; }
        public long tableId { get; set; }
        public int orderNumber { get; set; }
        public string afmQRCode { get; set; }
        public bool autoClose { get; set; }
    }

    public class TotalLine
    {
        public string lineId { get; set; }
        public decimal value { get; set; }
    }

    public class Fulfillment
    {
        public string notes { get; set; }
        public string type { get; set; }
    }

    public class ProductId
    {
        public string value { get; set; }
    }

    public class OrderLine
    {
        public string lineId { get; set; }
        public long? alohaEntryId { get; set; }
        public string description { get; set; }
        public string parentLineId { get; set; }
        public ProductId productId { get; set; }
        public decimal unitPrice { get; set; }
        public string fulfillmentResult { get; set; }
        public bool overridePrice { get; set; }
    }

    public class Payment
    {
        public long alohaPaymentId { get; set; }
        public int tenderID { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }
        public string maskedPAN { get; set; }
        public string authCode { get; set; }
    }

}
