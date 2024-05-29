using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;

namespace FlightManagementSystemAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Flight> _flightRepository;
        private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepository<int, SubRoute> _subRouteRepository;

        public BookingService(IRepository<int, Flight> flightRepository, IRepository<int, FlightRoute> routeRepository, IRepository<int, SubRoute> subRouteRepository, IRepository<int, Booking> bookingRepository)
        {
            _flightRepository = flightRepository;
            _routeRepository = routeRepository;
            _bookingRepository = bookingRepository;
            _subRouteRepository = subRouteRepository;
        }
        public Task<ReturnBookingDTO> AddBooking(BookingDTO bookingDTO)
        {
            throw new NotImplementedException();
        }
    }
}
