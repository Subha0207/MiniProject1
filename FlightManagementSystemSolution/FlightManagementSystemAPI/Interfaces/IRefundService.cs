using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IRefundService
    {
        /// <summary>
        /// Used to add refund request by user
        /// </summary>
        /// <param name="refundDTO"></param>
        /// <returns></returns>
        public Task<ReturnRefundDTO> AddRefund(RefundDTO refundDTO);
    
        /// <summary>
        /// Used to get refund Status of themselves by user
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        public  Task<ReturnRefundDTO> GetRefund(int refundId);
       /// <summary>
       /// Used to update Refund status by admin (approve/reject)
       /// </summary>
       /// <param name="RefundReturnDTO"></param>
       /// <returns></returns>
       public Task<ReturnRefundDTO> UpdateRefund(ReturnRefundDTO RefundReturnDTO);
     
     
    }
}
