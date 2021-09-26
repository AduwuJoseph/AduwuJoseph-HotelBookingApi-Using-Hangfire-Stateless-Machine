using HotelBooking.MongoDBClient.Entities;
using HotelBooking.MongoDBClient.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.MongoDBClient.Services
{
    public interface IBookingService
    {
        #region Booking
        IEnumerable<Booking> GetAllBookings();
        Task<Booking> GetByIdAsync(string id);
        Task AddBooking(Booking b);
        Task DeleteBooking(string id);
        Task UpdateBooking(Booking r);
        IEnumerable<Booking> GetBookingByStatus(string status);
        IEnumerable<Booking> GetBookingByCustomerId(string id);
        IEnumerable<Booking> GetBookingByCost(decimal cost);
        #endregion

        #region Booking Details

        #endregion
    }
    public class BookingService : IBookingService
    {
        private readonly IHotelBookingMongoRepository<Booking> _bookingRepository;

        public BookingService(IHotelBookingMongoRepository<Booking> bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task AddBooking(Booking b)
        {
            try
            {
                await _bookingRepository.InsertOneAsync(b);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Booking> GetBookingByStatus( string status)
        {
            var rooms = _bookingRepository.FilterBy(
                filter => filter.Status == status
            );
            return rooms;
        }
        public IEnumerable<Booking> GetBookingByCustomerId(string id)
        {
            var rooms = _bookingRepository.FilterBy(
                filter => filter.CustomerId == id
            );
            return rooms;
        }
        public IEnumerable<Booking> GetAllBookings()
        {
            return _bookingRepository.AsQueryable(); ;
        }

        public async Task<Booking> GetByIdAsync(string id)
        {
            var room = await _bookingRepository.FindByIdAsync(id);
            return room;
        }

        public async Task DeleteBooking(string id)
        {
            await _bookingRepository.DeleteByIdAsync(id);
        }

        public IEnumerable<Booking> GetBookingByCost(decimal cost)
        {
            var rooms = _bookingRepository.FilterBy(
                   filter => filter.TotalCost == cost
               );
            return rooms;
        }

        public async Task UpdateBooking(Booking r)
        {
            await _bookingRepository.ReplaceOneAsync(r);
        }
    }
}
