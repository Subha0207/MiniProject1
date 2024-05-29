using FlightManagementSystemAPI.Exceptions.CancellationExceptions;
using FlightManagementSystemAPI.Exceptions.RefundExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;

namespace FlightManagementSystemAPI.Services
{
    public class RefundService : IRefundService
    {
        private readonly IRepository<int, Cancellation> _cancellationRepository;
        private readonly IRepository<int, Refund> _refundRepository;
        private readonly IRepository<int, Booking> _bookingRepository;

        public RefundService(IRepository<int, Cancellation> cancellationRepository,  IRepository<int, Refund> refundRepository)
        {
            _cancellationRepository = cancellationRepository;
            _refundRepository = refundRepository;


        }
        public async Task<ReturnRefundDTO> AddRefund(RefundDTO refundDTO)
        {
            var cancellation = await _cancellationRepository.Get(refundDTO.CancellationId);
            if (cancellation == null)
            {
                throw new RefundException("No cancellation is present with the given ID");
            }
           
            var newRefund = new Refund
            {
                CancellationId=refundDTO.CancellationId,
                 
            };
            var savedRefund = await _refundRepository.Add(newRefund);

         
            var returnRefundDTO = new ReturnRefundDTO
            {
                RefundId = savedRefund.RefundId,
                RefundStatus=savedRefund.RefundStatus,
                
            };
            return returnRefundDTO;
        }

        public async Task<ReturnRefundDTO> GetRefund(int refundId)
        {
            var refund = await _refundRepository.Get(refundId);
            if (refund == null)
            {
                throw new RefundException("No refund found with the given ID");
            }

            var returnRefundDTO = new ReturnRefundDTO
            {
                RefundId = refund.RefundId,
                RefundStatus = refund.RefundStatus,
            
            };
            return returnRefundDTO;
        }

    }
}

