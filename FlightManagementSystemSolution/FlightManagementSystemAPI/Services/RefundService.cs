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
        private readonly ILogger<RefundService> _logger;
        private readonly IRepository<int, Booking> _bookingRepository;

        public RefundService(IRepository<int, Cancellation> cancellationRepository, IRepository<int, Refund> refundRepository, ILogger<RefundService> logger)
        {
            _cancellationRepository = cancellationRepository;
            _refundRepository = refundRepository;
            _logger = logger;
        }

        public async Task<ReturnRefundDTO> AddRefund(RefundDTO refundDTO)
        {
            _logger.LogInformation($"Adding a new refund for cancellation ID {refundDTO.CancellationId}...");
            var cancellation = await _cancellationRepository.Get(refundDTO.CancellationId);
            if (cancellation == null)
            {
                _logger.LogError($"No cancellation found with the given ID {refundDTO.CancellationId}.");
                throw new RefundException("No cancellation is present with the given ID");
            }

            var newRefund = new Refund
            {
                CancellationId = refundDTO.CancellationId,
            };
            var savedRefund = await _refundRepository.Add(newRefund);

            _logger.LogInformation($"Successfully added a new refund with ID {savedRefund.RefundId} for cancellation ID {refundDTO.CancellationId}.");

            var returnRefundDTO = new ReturnRefundDTO
            {
                CancellationReason = cancellation.Reason,
                RefundId = savedRefund.RefundId,
                RefundStatus = savedRefund.RefundStatus,
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


        public async Task<List<ReturnRefundDTO>> GetAllPendingRefunds()
        {
            _logger.LogInformation("Getting all pending refunds...");
            var allRefunds = await _refundRepository.GetAll(); // Assuming you have a method to get all refunds

            var initiatedRefunds = new List<ReturnRefundDTO>();

            foreach (var refund in allRefunds)
            {
                if (refund.RefundStatus == "Initiated")
                {
                    var returnRefundDTO = new ReturnRefundDTO
                    {
                        CancellationReason = refund.CancellationId != null
                            ? (await _cancellationRepository.Get(refund.CancellationId))?.Reason
                            : null,
                        RefundId = refund.RefundId,
                        RefundStatus = refund.RefundStatus
                    };

                    initiatedRefunds.Add(returnRefundDTO);
                }
            }

            _logger.LogInformation($"Found {initiatedRefunds.Count} pending refunds.");
            return initiatedRefunds;
        }

        public async Task<ReturnRefundDTO> GetRefund(int refundId)
        {
            _logger.LogInformation($"Getting refund with ID {refundId}...");
            var refund = await _refundRepository.Get(refundId);
            if (refund == null)
            {
                _logger.LogError($"No refund found with the given ID {refundId}.");
                throw new RefundException("No refund found with the given ID");
            }

            var cancellation = await _cancellationRepository.Get(refund.CancellationId);

            var returnRefundDTO = new ReturnRefundDTO
            {
                CancellationReason = cancellation.Reason,
                RefundId = refund.RefundId,
                RefundStatus = refund.RefundStatus
            };

            _logger.LogInformation($"Successfully got refund with ID {refundId}.");
            return returnRefundDTO;
        }

        public async Task<ReturnRefundDTO> UpdateRefund(UpdateRefundDTO updateRefundDTO)
        {
            try
            {
                _logger.LogInformation($"Updating refund with ID {updateRefundDTO.RefundId}...");
                var refund1 = await _refundRepository.Get(updateRefundDTO.RefundId);

                // Convert the UpdateRefundDTO to a Refund object
                Refund refund = new Refund
                {
                    CancellationId = refund1.CancellationId,
                    RefundId = updateRefundDTO.RefundId,
                    RefundStatus = updateRefundDTO.RefundStatus
                };

                // Call the Update method
                await _refundRepository.Update(refund);

                // Re-query the database to get the updated refund
                Refund updatedRefund = await _refundRepository.Get(refund.RefundId);
                var cancellation = await _cancellationRepository.Get(refund1.CancellationId);
                // Convert the updated Refund object back to a ReturnRefundDTO
                ReturnRefundDTO updatedRefundDTO = new ReturnRefundDTO
                {
                    RefundId = updatedRefund.RefundId,
                    RefundStatus = updatedRefund.RefundStatus,
                    CancellationReason = cancellation.Reason
                };

                _logger.LogInformation($"Successfully updated refund with ID {updateRefundDTO.RefundId}.");
                return updatedRefundDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating the refund with ID {updateRefundDTO.RefundId}.");
                throw new RefundException("Error while updating refund", ex);
            }
        }

        public Task<ReturnRefundDTO> DeleteRefundById(int refundId)
        {
            throw new NotImplementedException();
        }
    }
}

