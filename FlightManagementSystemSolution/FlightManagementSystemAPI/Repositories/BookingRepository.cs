using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;

namespace FlightManagementSystemAPI.Repositories
{
    public class BookingRepository : IRepository<int, Booking>
    {
        public Task<Booking> Add(Booking item)
        {
            throw new NotImplementedException();
        }

        public Task<Booking> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public Task<Booking> Get(int key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Booking> Update(Booking item)
        {
            throw new NotImplementedException();
        }
    }
}
