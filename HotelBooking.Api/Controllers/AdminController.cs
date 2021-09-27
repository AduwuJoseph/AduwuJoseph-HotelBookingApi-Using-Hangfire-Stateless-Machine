using Hangfire;
using HotelBooking.Api.Helpers;
using HotelBooking.Api.Models;
using HotelBooking.Api.States;
using HotelBooking.MongoDBClient.Entities;
using HotelBooking.MongoDBClient.Helpers;
using HotelBooking.MongoDBClient.Repositories;
using HotelBooking.MongoDBClient.Services;
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
    public class AdminController : ControllerBase
    {
        private readonly IHangFireOpRepository _hangFireOpRepository;
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;
        private readonly IHotelServiceService _hotelService;
        private readonly IRoomService _roomService;
        public AdminController(IHangFireOpRepository hangFireOpRepository, 
            ICustomerService customerService, 
            IBookingService bookingService,
            IHotelServiceService hotelService,
            IRoomService roomService)
        {
            _hangFireOpRepository = hangFireOpRepository;
            _customerService = customerService;
            _bookingService = bookingService;
            _roomService = roomService;
            _hotelService = hotelService;
        }
        #region Room CRUD
        [HttpGet]
        [Route("GetRooms")]
        public IActionResult GetRooms()
        {
            var r = _roomService.GetAllRooms().Select(m => new RoomDVM
            {
                Id = m.Id,
                Name = m.Name,
                Pictures = m.Pictures,
                Description = m.Description,
                Cost = m.Cost,
                Status = m.Status,
                CreatedAt = m.CreatedAt,
            });
            return Ok(r);
        }

        [HttpGet]
        [Route("GetRoom/{id}")]
        public async Task<IActionResult> GetRoom(string id)
        {
            var d = await _roomService.GetByIdAsync(id);
            return Ok(d);
        }

        [HttpPost]
        [Route("AddRoom")]
        public async Task<IActionResult> AddRoom([FromBody] Room value)
        {
            var cus = await _roomService.AddRoom(value);
            return Ok(cus);
        }
        [HttpPost]
        [Route("UpdateRoom")]
        public IActionResult UpdateRoom([FromBody] Room value)
        {
            _roomService.UpdateRoom(value);
            return Ok(true);
        }

        [Route("DeleteRoom/{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            await _roomService.DeleteRoom(id);
            return Ok(true);
        }
        #endregion

        #region Hotel Service CRUD
        [HttpGet]
        [Route("GetServices")]
        public IActionResult GetServices()
        {
            var r = _hotelService.GetAllHotelServices().Select(m => new HotelServiceDVM
            {
                Id = m.Id.ToString(),
                Name = m.Name,
                Pictures = m.Pictures,
                Description = m.Description,
                Cost = m.Cost,
                Status = m.Status,
                CreatedAt = m.CreatedAt,
            });
            return Ok(r);
        }

        [HttpGet]
        [Route("GetHotelService/{id}")]
        public async Task<IActionResult> GetHotelService(string id)
        {
            var d = await _hotelService.GetByIdAsync(id);
            return Ok(d);
        }

        [HttpPost]
        [Route("AddService")]
        public async Task<IActionResult> AddService([FromBody] HotelService value)
        {
             var cus = await _hotelService.AddHotelService(value);
            return Ok(cus);
        }
        [HttpPost]
        [Route("UpdateService")]
        public IActionResult UpdateService([FromBody] HotelService value)
        {
            _hotelService.UpdateHotelService(value);
            return Ok(true);
        }

        [Route("DeleteService/{id}")]
        public async Task<IActionResult> DeleteService(string id)
        {
            await _hotelService.DeleteHotelService(id);
            return Ok(true);
        }
        #endregion

        #region Customer Booking Operations

        [HttpPost]
        [Route("GuestCheckIn")]
        public async Task<IActionResult> GuestCheckIn([FromBody] string bookingId, DateTime? value)
        {
            var check = await _bookingService.GuestCheckIn(bookingId, value);            
            return Ok(check);
        }
        [HttpPost]
        [Route("GuestCheckOut")]
        public async Task<IActionResult> GuestCheckOut([FromBody] string bookingId, DateTime? value)
        {
            var check = await _bookingService.GuestCheckout(bookingId, value);
            if (check)
            {
                var b = await _bookingService.GetByIdAsync(bookingId);
                if (b != null)
                    RunBackgroundJobs(b);
            }

            return Ok(value);
        }
        #endregion

        public void RunBackgroundJobs(Booking d)
        {

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es-ES");
            BackgroundJob.Enqueue(() => SendEmail(d));
        }

        [AutomaticRetry(Attempts = 20)]
        [LogFailure]
        public void SendEmail(Booking b)
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
