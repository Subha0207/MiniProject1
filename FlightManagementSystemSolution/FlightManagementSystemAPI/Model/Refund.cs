namespace FlightManagementSystemAPI.Model
{
    public class Refund
    {
        public int RefundId { get; set; }
        public string RefundStatus { get; set; } = "Initiated";
        
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

    }
}
