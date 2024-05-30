using FlightManagementSystemAPI.Exceptions.CancellationExceptions;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.RefundExceptions;
using FlightManagementSystemAPI.Exceptions.UserExceptions;
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
            { CancellationReason=cancellation.Reason,
                RefundId = savedRefund.RefundId,
                RefundStatus=savedRefund.RefundStatus,
                
            };
            return returnRefundDTO;
        }
        private async Task<ReturnRefundDTO> MapRefundToReturnRefundDTO(Refund refund)
        {
            var cancellation = await _cancellationRepository.Get(refund.CancellationId);
            return new ReturnRefundDTO
            {
                RefundId = refund.RefundId,
                RefundStatus = refund.RefundStatus,
                CancellationReason = cancellation.Reason
            };
        }

     


        public async Task<ReturnRefundDTO> GetRefund(int refundId)
        {
            var refund = await _refundRepository.Get(refundId);
            if (refund == null)
            {
                throw new RefundException("No refund found with the given ID");
            }

            var cancellation = await _cancellationRepository.Get(refund.CancellationId);

            var returnRefundDTO = new ReturnRefundDTO
            { CancellationReason=cancellation.Reason,
                RefundId = refund.RefundId,
                RefundStatus = refund.RefundStatus,
            
            };
            return returnRefundDTO;
        }

        public async Task<ReturnRefundDTO> UpdateRefund(ReturnRefundDTO refundReturnDTO)
        {
            try
            {
                // Convert the DTO to a Refund object
                Refund refund = new Refund
                {    CancellationId=1,
                    RefundId = refundReturnDTO.RefundId,
                    RefundStatus = refundReturnDTO.RefundStatus
                };

      
                // Call the Update method
                await _refundRepository.Update(refund);

                // Re-query the database to get the updated refund
                Refund updatedRefund = await _refundRepository.Get(refund.RefundId);


                // Convert the updated Refund object back to a DTO
                ReturnRefundDTO updatedRefundDTO = new ReturnRefundDTO
                {
                    RefundId = updatedRefund.RefundId,
                    RefundStatus = updatedRefund.RefundStatus
                };

                return updatedRefundDTO;
            }
            catch (Exception ex)
            {
               
                throw new RefundException("Error while updating refund", ex);
            }
        }

    }
}

