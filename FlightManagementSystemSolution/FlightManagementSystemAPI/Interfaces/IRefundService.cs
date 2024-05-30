using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface IRefundService
    {
        public Task<ReturnRefundDTO> AddRefund(RefundDTO refundDTO);
        public  Task<ReturnRefundDTO> GetRefund(int refundId);
        public Task<ReturnRefundDTO> UpdateRefund(ReturnRefundDTO RefundReturnDTO);
     
     
    }
}
