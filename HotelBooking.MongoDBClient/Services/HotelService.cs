using HotelBooking.MongoDBClient.Entities;
using HotelBooking.MongoDBClient.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.MongoDBClient.Services
{
    public interface IHotelServiceService
    {
        IEnumerable<HotelService> GetAllHotelServices();
        Task<HotelService> GetByIdAsync(string id);
        Task AddHotelService(HotelService hs);
        Task DeleteHotelService(string id);
        Task UpdateHotelService(HotelService r);
        IEnumerable<HotelService> GetHotelServiceByStatus(string status);
        IEnumerable<HotelService> GetHotelServiceByCost(decimal cost);
    }
    public class HotelServiceService: IHotelServiceService
    {
        private readonly IHotelBookingMongoRepository<HotelService> _hotelServiceRepository;

        public HotelServiceService(IHotelBookingMongoRepository<HotelService> hotelServiceRepository)
        {
            _hotelServiceRepository = hotelServiceRepository;
        }

        public async Task AddHotelService(HotelService hs)
        {
            try
            {
                await _hotelServiceRepository.InsertOneAsync(hs);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<HotelService> GetHotelServiceByStatus( string status)
        {
            var rooms = _hotelServiceRepository.FilterBy(
                filter => filter.Status == status
            );
            return rooms;
        }
        public IEnumerable<HotelService> GetAllHotelServices()
        {
            return _hotelServiceRepository.AsQueryable(); ;
        }

        public async Task<HotelService> GetByIdAsync(string id)
        {
            var room = await _hotelServiceRepository.FindByIdAsync(id);
            return room;
        }

        public async Task DeleteHotelService(string id)
        {
            await _hotelServiceRepository.DeleteByIdAsync(id);
        }

        public IEnumerable<HotelService> GetHotelServiceByCost(decimal cost)
        {
            var rooms = _hotelServiceRepository.FilterBy(
                   filter => filter.Cost == cost
               );
            return rooms;
        }

        public async Task UpdateHotelService(HotelService r)
        {
            await _hotelServiceRepository.ReplaceOneAsync(r);
        }
    }
}
