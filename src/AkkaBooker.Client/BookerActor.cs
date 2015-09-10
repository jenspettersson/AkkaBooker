using System;
using Akka.Actor;
using AkkaBooker.Shared;

namespace AkkaBooker.Client
{
    public class BookerActor : TypedActor, IHandle<CreateBooking>, IHandle<SeatBooked>, IHandle<BookingFailed>
    {
        private readonly EventHandler<BookingResult> _bookingStatus;
        private ActorSelection _bookingActor;

        public BookerActor(EventHandler<BookingResult> bookingStatus)
        {
            _bookingActor = Context.ActorSelection("akka.tcp://akkabooker@localhost:8099/user/theatre");
            _bookingStatus = bookingStatus;
        }

        public void Handle(CreateBooking message)
        {
            _bookingActor.Tell(new BookSeat(message.Row, message.Seat));
        }

        public void Handle(SeatBooked message)
        {
            _bookingStatus(this, new BookingResult(message.Row, message.Seat, "success"));
        }

        public void Handle(BookingFailed message)
        {
            _bookingStatus(this, new BookingResult(message.Row, message.Seat, "failed"));
        }
    }

   
}