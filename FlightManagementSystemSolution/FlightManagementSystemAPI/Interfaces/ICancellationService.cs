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
        /// <summary>
        /// Used to get Cancellation by Cancellation ID
        /// </summary>
        /// <param name="CancellationId"></param>
        /// <returns></returns>
        public Task<ReturnCancellationDTO> GetCancellationById(int cancellationId);
      /// <summary>
      /// Used to delete cancellation by admin
      /// </summary>
      /// <param name="bookingId"></param>
      /// <returns></returns>
      public Task<ReturnCancellationDTO> DeleteCancellationById(int cancellationId);

    }
}
