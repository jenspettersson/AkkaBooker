using System;
using System.Collections.Generic;
using System.Linq;

namespace AkkaBooker.Shared
{
    public static class ConsoleWriter
    {
        public static void PrintSeatLayout(IEnumerable<Seat> e)
        {
            Console.Clear();
            var seats = e.GroupBy(x => x.Number);
            Console.Write("\t");
            foreach (var seat in seats)
            {
                if (seat.Key < 10)
                    Console.Write(" " + seat.Key + "  ");
                else
                {
                    Console.Write(" " + seat.Key + " ");
                }
            }
            Console.Write("\n");
            var rows = e.GroupBy(x => x.Row);
            foreach (var row in rows)
            {
                Console.Write("R {0}:\t", row.Key);
                foreach (var seat in row)
                {
                    string booked = seat.IsBooked ? "X" : " ";
                    Console.Write("[" + booked + "] ");
                }
                Console.Write("\n");
            }
        }
    }
}