using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface ICancellationService
    {
        public Task<ReturnCancellationDTO> AddCancellation(CancellationDTO cancellationDTO);

        public Task<List<ReturnCancellationDTO>> GetAllCancellations();
    }
}
