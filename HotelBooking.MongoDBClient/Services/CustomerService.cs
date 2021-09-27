using HotelBooking.MongoDBClient.Entities;
using HotelBooking.MongoDBClient.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.MongoDBClient.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAllCustomers();
        Task<Customer> GetByIdAsync(string id);
        Task<Customer> AddCustomer(Customer c);
        Task DeleteCustomer(string id);
        Task UpdateCustomer(Customer c);
        IEnumerable<Customer> GetCustomerByEmail(string email);
        Customer GetCustomerByEmailToSignle(string email);
        IEnumerable<Customer> GetCustomerByGender(string gender);
    }
    public class CustomerService : ICustomerService
    {
        private readonly IHotelBookingMongoRepository<Customer> _customerRepository;

        public CustomerService(IHotelBookingMongoRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> AddCustomer(Customer c)
        {
            try
            {
                await _customerRepository.InsertOneAsync(c);
                return c;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Customer> GetCustomerByEmail(string email)
        {
            var c = _customerRepository.FilterBy(
                filter => filter.Email == email
            );
            return c;
        }
        public Customer GetCustomerByEmailToSignle(string email)
        {
            var c = _customerRepository.FindOne(
                filter => filter.Email == email
            );
            return c;
        }
        public IEnumerable<Customer> GetCustomerByGender(string g)
        {
            var c = _customerRepository.FilterBy(
                filter => filter.Gender == g
            );
            return c;
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customerRepository.AsQueryable(); ;
        }

        public async Task<Customer> GetByIdAsync(string id)
        {
            var room = await _customerRepository.FindByIdAsync(id);
            return room;
        }

        public async Task DeleteCustomer(string id)
        {
            await _customerRepository.DeleteByIdAsync(id);
        }

        public async Task UpdateCustomer(Customer r)
        {
            await _customerRepository.ReplaceOneAsync(r);
        }
    }
}
