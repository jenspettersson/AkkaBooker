using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;

namespace AkkaBooker.Shared.Actors
{
    public class TheatreActor : TypedActor, 
        IHandle<OpenBooking>,
        IHandle<SeatBooked>, 
        IHandle<BookSeat>
    {
        private readonly EventHandler<IEnumerable<Seat>> _statusChanged;


        IDictionary<string, IActorRef> _seatActors = new Dictionary<string, IActorRef>(); 
        private IList<Seat> _allSeats = new List<Seat>(); 
        public TheatreActor(EventHandler<IEnumerable<Seat>> statusChanged)
        {
            _statusChanged = statusChanged;
        }

        private static string CreateSeatId(int row, int seat)
        {
            return string.Format("{0}-{1}", row, seat);
        }

        public void Handle(BookSeat message)
        {
            _seatActors[CreateSeatId(message.Row, message.Seat)].Forward(message);
        }

        public void Handle(SeatBooked message)
        {
            var seat = _allSeats.First(x => x.Row == message.Row && x.Number == message.Seat);
            seat.MarkAsBooked();
            
            _statusChanged(this, _allSeats);
        }

        public void Handle(OpenBooking message)
        {
            for (var row = 1; row <= message.Rows; row++)
            {
                for (var seat = 1; seat <= message.Seats; seat++)
                {
                    var actorRef = Context.ActorOf(Props.Create(() => new SeatActor(row, seat)));
                    _seatActors[CreateSeatId(row, seat)] = actorRef;

                    _allSeats.Add(new Seat(row, seat));
                }
            }

            _statusChanged(this, _allSeats);
        }
    }

    public class OpenBooking
    {
        public int Rows { get; private set; }
        public int Seats { get; private set; }

        public OpenBooking(int rows, int seats)
        {
            Rows = rows;
            Seats = seats;
        }
    }

    public class SeatActor : ReceiveActor
    {
        private readonly int _row;
        private readonly int _seat;
        private bool _isBooked;

        public SeatActor(int row, int seat)
        {
            _row = row;
            _seat = seat;

            Bookable();
        }

        private void Bookable()
        {
            Receive<BookSeat>(m =>
            {
                _isBooked = true;
                Context.Parent.Tell(new SeatBooked(_row, _seat));
                Sender.Tell(new SeatBooked(_row, _seat));
                Become(Booked);
            }, seat => seat.Row == _row && seat.Seat == _seat);
        }

        private void Booked()
        {
            Receive<BookSeat>(m =>
            {
                Sender.Tell(new BookingFailed(_row, _seat));
            });
        }
    }
}