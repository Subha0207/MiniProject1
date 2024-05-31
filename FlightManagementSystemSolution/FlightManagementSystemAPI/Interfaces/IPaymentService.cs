using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Used to add payment by user after booking
        /// </summary>
        /// <param name="paymentDTO"></param>
        /// <returns></returns>
        public Task<ReturnPaymentDTO> AddPayment(PaymentDTO paymentDTO);
    }
}
