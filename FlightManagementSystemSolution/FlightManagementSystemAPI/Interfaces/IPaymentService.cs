using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IPaymentService
    {
        public Task<ReturnPaymentDTO> AddPayment(PaymentDTO paymentDTO);
    }
}
