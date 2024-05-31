using FlightManagementSystemAPI.Model.DTOs;

namespace FlightManagementSystemAPI.Interfaces
{
    public interface ICancellationService
    {
       /// <summary>
       /// Used to add Cancellation for the booking by the user
       /// </summary>
       /// <param name="cancellationDTO"></param>
       /// <returns></returns>
       public Task<ReturnCancellationDTO> AddCancellation(CancellationDTO cancellationDTO);

       /// <summary>
       /// Used to get all the cancellations by admin
       /// </summary>
       /// <returns></returns>
       public Task<List<ReturnCancellationDTO>> GetAllCancellations();
    }
}
