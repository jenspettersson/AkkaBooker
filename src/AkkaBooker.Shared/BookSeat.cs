namespace AkkaBooker.Shared
{
    public class BookSeat
    {
        public int Row { get; private set; }
        public int Seat { get; private set; }

        public BookSeat(int row, int seat)
        {
            Row = row;
            Seat = seat;
        }
    }
}