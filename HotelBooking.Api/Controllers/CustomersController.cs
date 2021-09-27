using Hangfire;
using HotelBooking.Api.Helpers;
using HotelBooking.Api.Models;
using HotelBooking.MongoDBClient.Services;
using HotelBooking.Api.States;
using HotelBooking.MongoDBClient.Entities;
using HotelBooking.MongoDBClient.Helpers;
using HotelBooking.MongoDBClient.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Postal;
using Stateless;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IHangFireOpRepository _hangFireOpRepository;
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;


        //private readonly StateMachine<RoomState, Trigger> _roomMachine;
        //private readonly StateMachine<ClientState, Trigger> _clientMachine;
        //// The TriggerWithParameters object is used when a trigger requires a payload.
        //private readonly StateMachine<RoomState, Trigger>.TriggerWithParameters<BookingDetailsDVM> _roomTrigger;
        //private readonly StateMachine<ClientState, Trigger>.TriggerWithParameters<BookingDVM> _bookingTrigger;

        public CustomersController(IHangFireOpRepository hangFireOpRepository, ICustomerService customerService, IBookingService bookingService)
        {
            _hangFireOpRepository = hangFireOpRepository;
            _customerService = customerService;
            _bookingService = bookingService;

            // Instantiate a new state machine in the Open state
            //_roomMachine = new StateMachine<RoomState, Trigger>(RoomState.Free);
            //_clientMachine = new StateMachine<ClientState, Trigger>(ClientState.BookedRoom);

            //// Instantiate a new trigger with a parameter. 
            //_bookingTrigger = _clientMachine.SetTriggerParameters<BookingDVM>(Trigger.BookRoom);
            //_roomTrigger = _roomMachine.SetTriggerParameters<BookingDetailsDVM>(Trigger.ReserveRoom);

        }
        #region Customer CRUD
        // GET: api/<CustomersController>
        [HttpGet]
        public IActionResult Get()
        {
            var customers = _customerService.GetAllCustomers().Select(m => new CustomerDVM
            {
                Address = m.Address,
                BirthDate = m.BirthDate,
                CreatedAt = m.CreatedAt,
                Email = m.Email,
                Gender = m.Gender,
                Id = m.Id.ToString(),
                Name = m.Name,
                Phone = m.Phone,
                Pictures = m.Pictures,
            });
            return Ok(customers);
        }

        // GET api/<CustomersController>/5
        [HttpGet]
        [Route("GetCustomer/{id}")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            var d = await _customerService.GetByIdAsync(id);
            return Ok(d);
        }

        // POST api/<CustomersController>
        [HttpPost]
        [Route("AddCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] Customer value)
        {
            var cus = await _customerService.AddCustomer(value);
            return Ok(cus);
        }
        [HttpPost]
        [Route("UpdateCustomer")]
        public IActionResult UpdateCustomer([FromBody] Customer value)
        {
            _customerService.AddCustomer(value);
            return Ok(true);
        }

        // DELETE api/<CustomersController>/5
        [Route("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            await _customerService.DeleteCustomer(id);
            return Ok(true);
        }
        #endregion

        #region Customer Booking

        /// <summary>
        /// Book a  Room Action
        /// </summary>
        /// <param name="value">The value of the booking and booking details model</param>
        [HttpPost]
        [Route("BookRoom")]
        public async Task<IActionResult> BookRoom([FromBody] BookingDVM value)
        {
            // Configure the Open state

            //// Configure the Assigned state
            //_clientMachine.Configure(ClientState.Registered)
            //        .OnEntryFrom(_bookingTrigger.Trigger, ProcessBooking(value))  // This is where the TriggerWithParameters is used. Note that the TriggerWithParameters object is used, not something from the enum
            //        .PermitReentry(Trigger.Assign)
            //        .Permit(Trigger.Close, State.Closed)
            //        .Permit(Trigger.Defer, State.Deferred)
            //        .OnExit(OnDeassigned);

            //// Configure the Assigned state
            //_roomMachine.Configure(RoomState.Free)
            //    .OnEntryFrom(_roomTrigger, ProcessBooking())  // This is where the TriggerWithParameters is used. Note that the TriggerWithParameters object is used, not something from the enum
            //    .PermitReentry(Trigger.Assign)
            //    .Permit(Trigger.Close, State.Closed)
            //    .Permit(Trigger.Defer, State.Deferred)
            //    .OnExit(OnDeassigned);

            var check = await ProcessBooking(value);
            if (check)
                RunBackgroundJobs(value);

            return Ok(value);
        }
        #endregion

        private async Task<bool> ProcessBooking(BookingDVM booking)
        {
            bool value = false;
            var book = new Booking
            {
                CustomerId = booking.CustomerId,
                Discount = booking.Discount,
                Status = BookingStatus.BookedRoom,
                TotalCost = booking.TotalCost
            };

            var bd = new List<BookingDetails>();
            if (booking.BookingDetails.Count() > 0)
            {
                bd = booking.BookingDetails.Select(m => new BookingDetails
                {
                    ItemId = m.ItemId,
                    ServiceType = BookServiceType.Room
                }
                ).ToList();
            }
            var result = await _bookingService.AddBooking(book, bd);
            if (result != null)
                value = true;
            return value;
        }

        public void RunBackgroundJobs(BookingDVM d)
        {

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es-ES");
            BackgroundJob.Enqueue(() => SendEmail(d));
        }

        [AutomaticRetry(Attempts = 20)]
        [LogFailure]
        public void SendEmail(BookingDVM b)
        {
            var currentCultureName = Thread.CurrentThread.CurrentCulture.Name;
            if (currentCultureName != "es-ES")
            {
                throw new InvalidOperationException(String.Format("Current culture is {0}", currentCultureName));
            }
            var cust =  _customerService.GetByIdAsync(b.CustomerId).Result;
            var file = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplate\\" + "BookingMsg" + ".cshtml");
            string body = System.IO.File.ReadAllText(file);

            body = body.Replace("@ViewBag.CustomerName", cust.Name);
            body = body.Replace("@ViewBag.CYear", DateTime.Now.Year.ToString());
            body = body.ToString();
            string subject = "Room Successfully Booked : Infusync";

            var msg = EmailSender.BuildEmailTemplate(subject, body, cust.Email);
        }
    }
}
