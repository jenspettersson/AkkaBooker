namespace AkkaBooker.Client
{
    public class CreateBooking
    {
        public int Row { get; private set; }
        public int Seat { get; private set; }

        public CreateBooking(int row, int seat)
        {
            Row = row;
            Seat = seat;
        }
    }
}