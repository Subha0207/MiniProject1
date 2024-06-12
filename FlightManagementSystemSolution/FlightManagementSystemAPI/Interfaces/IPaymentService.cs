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
        /// <summary>
        /// Get Payment by Payment ID by user
        /// </summary>
        /// <param name="PaymentId"></param>
        /// <returns></returns>
        public Task<PaymentDetailsDTO> GetPayment(int PaymentId);
        /// <summary>
        /// GetAll the Payments done
        /// </summary>
        /// <returns></returns>
        public Task<List<PaymentDetailsDTO>> GetAllPayments();
        /// <summary>
        /// Delete payment by payment ID
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public Task<ReturnPaymentDTO> DeletePaymentById(int paymentId);





    }
}
