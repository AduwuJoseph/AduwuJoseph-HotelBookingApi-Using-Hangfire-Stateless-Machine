
namespace HotelBooking.Api.States
{
    public enum Trigger
    {
        Register,
        SendEmail,
        ReserveRoom,
        BookRoom,
        Checkin,
        Checkout
    }

    public enum ClientState
    {
        Registered,
        BookedRoom,
        CheckIn,
        CheckOut
    }

    public enum RoomState
    {
        Free,
        Occupied,
        Reserved,
        Cleaned,
        Unavailable
    }
}