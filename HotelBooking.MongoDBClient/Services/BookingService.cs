using HotelBooking.MongoDBClient.Entities;
using HotelBooking.MongoDBClient.Helpers;
using HotelBooking.MongoDBClient.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.MongoDBClient.Services
{
    public interface IBookingService
    {
        #region Booking
        IEnumerable<Booking> GetAllBookings();
        Task<Booking> GetByIdAsync(string id);
        Task<Booking> AddBooking(Booking b);
        Task<Booking> AddBooking(Booking b, BookingDetails bd);
        Task<Booking> AddBooking(Booking b, List<BookingDetails> bd);
        Task DeleteBooking(string id);
        Task UpdateBooking(Booking r);
        IEnumerable<Booking> GetBookingByStatus(string status);
        IEnumerable<Booking> GetBookingByCustomerId(string id);
        IEnumerable<Booking> GetBookingByCost(decimal cost);
        IEnumerable<Booking> GetBookingByDateBooked(DateTime d);
        IEnumerable<Booking> GetBookingByCheckIn(DateTime d);
        IEnumerable<Booking> GetBookingByCheckOut(DateTime d);
        int CheckIns();
        int CheckOuts();
        decimal Revenue();
        Task<bool> GuestCheckout(string bookingId, DateTime? date);
        Task<bool> GuestCheckIn(string bookingId, DateTime? date);
        #endregion

        #region Booking Details
        IEnumerable<BookingDetails> GetAllBookingDetails();
        Task<BookingDetails> GetBookingDetailsByIdAsync(string id);
        Task<BookingDetails> AddBookingDetails(BookingDetails b);
        Task DeleteBookingDetails(string id);
        Task UpdateBookingDetails(BookingDetails r);
        IEnumerable<BookingDetails> GetBookingDetailsByType(string status);
        IEnumerable<BookingDetails> GetBookingDetailsByItemId(string id);
        IEnumerable<BookingDetails> GetBookingDetailsByBookingId(string id);
        IEnumerable<BookingDetails> GetBookingDetailsByBookingIdAndType(string id, string type);

        #endregion
    }
    public class BookingService : IBookingService
    {
        private readonly IHotelBookingMongoRepository<Booking> _bookingRepository;
        private readonly IHotelBookingMongoRepository<BookingDetails> _bookingDetailsRepository;
        private readonly IHotelBookingMongoRepository<Room> _roomRepository;

        public BookingService(IHotelBookingMongoRepository<Booking> bookingRepository, 
            IHotelBookingMongoRepository<BookingDetails> bookingDetailsRepository,
            IHotelBookingMongoRepository<Room> roomRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingDetailsRepository = bookingDetailsRepository;
            _roomRepository = roomRepository;
        }

        #region Booking
        public async Task<Booking> AddBooking(Booking b, List<BookingDetails> bd)
        {
            try
            {
                decimal totalcost = 0;
                await _bookingRepository.InsertOneAsync(b);
                if (b.Id != null)
                {
                    var bdn = new List<BookingDetails>();
                    foreach (var item in bd)
                    {
                        // Adding Rooms to Booking  Details
                        item.BookingId = b.Id.ToString();
                        bdn.Add(item);

                        var cd = _roomRepository.FindById(item.ItemId);
                        //Updating Room Status
                        cd.Status = BookingStatus.Reserved;
                        await _roomRepository.ReplaceOneAsync(cd);

                        // getting total cost of rooms
                        totalcost += cd.Cost;
                    }
                    //Saving Booding details
                    await _bookingDetailsRepository.InsertManyAsync(bdn);

                    //updating booking total cost
                    b.TotalCost = totalcost;
                    await _bookingRepository.ReplaceOneAsync(b);
                }
                return b;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public async Task<Booking> AddBooking(Booking b, BookingDetails bd)
        {
            try
            {
                decimal totalcost = 0;
                await _bookingRepository.InsertOneAsync(b);
                if (b.Id != null)
                { 
                    // Adding Rooms to Booking  Details
                    bd.BookingId = b.Id.ToString();
                    //Saving Booding details
                    await _bookingDetailsRepository.InsertOneAsync(bd);

                    var cd = _roomRepository.FindById(bd.ItemId);
                    //Updating Room Status
                    cd.Status = BookingStatus.Reserved;
                    await _roomRepository.ReplaceOneAsync(cd);

                    // getting total cost of rooms
                    totalcost += cd.Cost;

                    //updating booking total cost
                    b.TotalCost = totalcost;
                    await _bookingRepository.ReplaceOneAsync(b);
                }
                return b;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Booking> AddBooking(Booking b)
        {
            try
            {
                await _bookingRepository.InsertOneAsync(b);
                return b;
            }
            catch (Exception e)
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
            return _bookingRepository.AsQueryable(); 
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

        public IEnumerable<Booking> GetBookingByDateBooked(DateTime d)
        {
            var rooms = _bookingRepository.FilterBy(
                   filter => filter.CreatedAt == d
               );
            return rooms;
        }
        public IEnumerable<Booking> GetBookingByCheckIn(DateTime d)
        {
            var rooms = _bookingRepository.FilterBy(
                   filter => filter.CheckInDate == d
               );
            return rooms;
        }
        public IEnumerable<Booking> GetBookingByCheckOut(DateTime d)
        {
            var rooms = _bookingRepository.FilterBy(
                   filter => filter.CheckOutDate == d
               );
            return rooms;
        }
        public async Task UpdateBooking(Booking r)
        {
            await _bookingRepository.ReplaceOneAsync(r);
        }
        public int CheckIns()
        {
            var rooms = _bookingRepository.FilterBy(
                filter => filter.Status == BookingStatus.CheckIn
            );
            return rooms.Count();
        }

        public int CheckOuts()
        {
            var rooms = _bookingRepository.FilterBy(
                filter => filter.Status == BookingStatus.CheckOut
            );
            return rooms.Count();
        }

        public decimal Revenue()
        {
            var rev = GetBookingByStatus(BookingStatus.CheckOut).Sum(m => m.TotalCost);
            return rev;
        }
        public async Task<bool> GuestCheckout(string bookingId, DateTime? date)
        {
            try
            {
                var book = await GetByIdAsync(bookingId);
                if(book != null)
                {
                    // Update Booking Status
                    book.CheckOutDate = date != null ? date : DateTime.Now;
                    book.Status = BookingStatus.CheckOut;
                    await UpdateBooking(book);

                    // Update Room Status
                    var bk = GetBookingDetailsByBookingIdAndType(bookingId, BookServiceType.Room);
                    foreach(var item in bk)
                    {
                        var room = _roomRepository.FindById(item.ItemId);
                        if(room != null)
                        {
                            room.Status = BookingStatus.Free;
                            await _roomRepository.ReplaceOneAsync(room);
                        }
                    }

                    return true;
                }
                return false;
            }catch(Exception e)
            {
                return false;
            }
        }
        public async Task<bool> GuestCheckIn(string bookingId, DateTime? date)
        {
            try
            {
                var book = await GetByIdAsync(bookingId);
                if (book != null)
                {
                    // Update Booking Status
                    book.CheckInDate = date != null ? date : DateTime.Now;
                    book.Status = BookingStatus.CheckIn;
                    await UpdateBooking(book);

                    // Update Room Status
                    var bk = GetBookingDetailsByBookingIdAndType(bookingId, BookServiceType.Room);
                    foreach (var item in bk)
                    {
                        var room = _roomRepository.FindById(item.ItemId);
                        if (room != null)
                        {
                            room.Status = BookingStatus.Occupied;
                            await _roomRepository.ReplaceOneAsync(room);
                        }
                    }

                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region Booking Details
        public IEnumerable<BookingDetails> GetAllBookingDetails()
        {
            return _bookingDetailsRepository.AsQueryable();
        }

        public async Task<BookingDetails> GetBookingDetailsByIdAsync(string id)
        {
            var room = await _bookingDetailsRepository.FindByIdAsync(id);
            return room;
        }

        public async Task<BookingDetails> AddBookingDetails(BookingDetails b)
        {
            try
            {
                await _bookingDetailsRepository.InsertOneAsync(b);
                return b;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteBookingDetails(string id)
        {
            await _bookingRepository.DeleteByIdAsync(id);
        }

        public async Task UpdateBookingDetails(BookingDetails r)
        {
            await _bookingDetailsRepository.ReplaceOneAsync(r);
        }

        public IEnumerable<BookingDetails> GetBookingDetailsByType(string status)
        {
            var rooms = _bookingDetailsRepository.FilterBy(
                filter => filter.ServiceType == status
            );
            return rooms;
        }

        public IEnumerable<BookingDetails> GetBookingDetailsByItemId(string id)
        {
            var rooms = _bookingDetailsRepository.FilterBy(
                filter => filter.ItemId == id
            );
            return rooms;
        }
        public IEnumerable<BookingDetails> GetBookingDetailsByBookingId(string id)
        {
            var rooms = _bookingDetailsRepository.FilterBy(
                filter => filter.BookingId == id
            );
            return rooms;
        }
        public IEnumerable<BookingDetails> GetBookingDetailsByBookingIdAndType(string id, string type)
        {
            var rooms = _bookingDetailsRepository.FilterBy(
                filter => filter.BookingId == id && filter.ServiceType == type
            );
            return rooms;
        }
        #endregion
    }
}
