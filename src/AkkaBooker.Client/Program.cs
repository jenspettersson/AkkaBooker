using System;
using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.Event;
using AkkaBooker.Shared;

namespace AkkaBooker.Client
{
    public class BookingResult
    {
        public int Row { get; private set; }
        public int Seat { get; private set; }
        public string Message { get; private set; }

        public BookingResult(int row, int seat, string message)
        {
            Row = row;
            Seat = seat;
            Message = message;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("akkabookerclient");
            var actorRef = system.ActorOf(Props.Create(() => new BookerActor(BookingStatusReceived)), "tst");

            var rowRandom = Enumerable.Range(1, 20).ToList();
            var seatRandom = Enumerable.Range(1, 15).ToList();
            do
            {
                Console.Write("Book: ");
                var readLine = Console.ReadLine();

                if (readLine == "x")
                {
                    system.Shutdown();
                    system.AwaitTermination();
                    break;
                }

                if (readLine == "rnd")
                {
                    for (var i = 0; i < 100; i++)
                    {
                        var rndRow = rowRandom.OrderBy(x => Guid.NewGuid()).First();
                        var rndSeat = seatRandom.OrderBy(x => Guid.NewGuid()).First();
                        Console.WriteLine("Trying to book row {0} seat {1}", rndRow, rndSeat);

                        actorRef.Tell(new CreateBooking(rndRow, rndSeat));
                        Thread.Sleep(100);
                    }
                    continue;
                }
                var strings = readLine.Split(',');

                var row = int.Parse(strings[0]);
                var seat = int.Parse(strings[1]);

                Console.WriteLine("Trying to book row {0} seat {1}", row, seat);

                actorRef.Tell(new CreateBooking(row, seat));
            } while (true);
        }

        private static void BookingStatusReceived(object sender, BookingResult e)
        {
            if(e.Message == "success")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\rBooked row {0} seat {1}", e.Row, e.Seat);
            }
            else if (e.Message == "failed")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\rBooking for row {0} seat {1} failed!", e.Row, e.Seat);
            }

            Console.ResetColor();
        }
    }
}
