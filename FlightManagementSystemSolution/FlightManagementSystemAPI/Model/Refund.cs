namespace FlightManagementSystemAPI.Model
{
    public class Refund
    {
        public int RefundId { get; set; }
        public string RefundStatus { get; set; } = "Initiated";

        public int CancellationId { get; set; }
        public Cancellation Cancellation { get; set; }

    }
}
