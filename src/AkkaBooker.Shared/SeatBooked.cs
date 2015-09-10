namespace AkkaBooker.Shared
{
    public class SeatBooked
    {
        public int Row { get; private set; }
        public int Seat { get; private set; }

        public SeatBooked(int row, int seat)
        {
            Row = row;
            Seat = seat;
        }
    }

    public class BookingFailed : SeatBooked
    {
        public BookingFailed(int row, int seat) : base(row, seat)
        {
        }
    }
}