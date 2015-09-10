namespace AkkaBooker.Shared
{
    public class Seat
    {
        public int Row { get; private set; }
        public int Number { get; private set; }
        public bool IsBooked { get; private set; }

        public Seat(int row, int number)
        {
            Row = row;
            Number = number;
        }

        public void MarkAsBooked()
        {
            IsBooked = true;
        }

        protected bool Equals(Seat other)
        {
            return Row == other.Row && Number == other.Number;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Row;
                hashCode = (hashCode * 397) ^ Number;
                return hashCode;
            }
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}