using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaBooker.Shared;
using AkkaBooker.Shared.Actors;


namespace AkkaBooker
{
    class Program
    {
        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("akkabooker");

            var actorRef = actorSystem.ActorOf(Props.Create(() => new TheatreActor(SeatBookedEventHandler)), "theatre");

            actorRef.Tell(new OpenBooking(20, 15));
            
            do
            {
                //Thread.Sleep(500);
                //Console.Write("Book: ");
                //var readLine = Console.ReadLine();

                //if (readLine == "x")
                //{
                //    actorSystem.Shutdown();
                //    actorSystem.AwaitTermination();
                //    break;
                //}
                //var strings = readLine.Split(',');

                //var row = int.Parse(strings[0]);
                //var seat = int.Parse(strings[1]);

                //Console.WriteLine("Trying to book row {0} seat {1}", row, seat);

                //actorRef.Tell(new BookSeat(row, seat));
            } while (true);
        }

        private static void SeatBookedEventHandler(object sender, IEnumerable<Seat> e)
        {
            ConsoleWriter.PrintSeatLayout(e);
        }
    }
}
