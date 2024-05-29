namespace FlightManagementSystemAPI.Model.DTOs
{
    public class ReturnRefundDTO
    {

        public int RefundId { get; set; }
        public string RefundStatus { get; set; } = "Initiated";
    }
}
