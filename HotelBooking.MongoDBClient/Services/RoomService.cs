using HotelBooking.MongoDBClient.Entities;
using HotelBooking.MongoDBClient.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.MongoDBClient.Services
{
    public interface IRoomService
    {
        IEnumerable<Room> GetAllRooms();
        Task<Room> GetByIdAsync(string id);
        Task AddRoom(Room room);
        Task DeleteRoom(string id);
        Task UpdateRoom(Room r);
        IEnumerable<Room> GetRoomByStatus(string status);
        IEnumerable<Room> GetRoomByCost(decimal cost);
    }
    public class RoomService: IRoomService
    {
        private readonly IHotelBookingMongoRepository<Room> _roomRepository;

        public RoomService(IHotelBookingMongoRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task AddRoom(Room room)
        {
            try
            {
                await _roomRepository.InsertOneAsync(room);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Room> GetRoomByStatus( string status)
        {
            var rooms = _roomRepository.FilterBy(
                filter => filter.Status == status
            );
            return rooms;
        }
        public IEnumerable<Room> GetAllRooms()
        {
            return _roomRepository.AsQueryable(); ;
        }

        public async Task<Room> GetByIdAsync(string id)
        {
            var room = await _roomRepository.FindByIdAsync(id);
            return room;
        }

        public async Task DeleteRoom(string id)
        {
            await _roomRepository.DeleteByIdAsync(id);
        }

        public IEnumerable<Room> GetRoomByCost(decimal cost)
        {
            var rooms = _roomRepository.FilterBy(
                   filter => filter.Cost == cost
               );
            return rooms;
        }

        public async Task UpdateRoom(Room r)
        {
            await _roomRepository.ReplaceOneAsync(r);
        }
    }
}
