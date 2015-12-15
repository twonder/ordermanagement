using NServiceBus;
using OrderEntry.Commands;
using OrderProcessing.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static string customerId;
        private static List<Product> products = new List<Product>();

        static void Main(string[] args)
        {
            Console.Title = "Console Order Entry";

            var configuration = new BusConfiguration();
            configuration.EndpointName("OrderManagement.ConsoleApp");

            // using json instead of XML
            configuration.UseSerialization<JsonSerializer>();

            configuration.UsePersistence<InMemoryPersistence>();

            // specify what the commands and events can be recognized by
            configuration.Conventions().DefiningCommandsAs(e => e.Namespace != null & e.Namespace.EndsWith("Commands"));
            configuration.Conventions().DefiningEventsAs(e => e.Namespace != null & e.Namespace.EndsWith("Events"));

            var startableBus = Bus.Create(configuration);
            using (var bus = startableBus.Start())
            {
                RunApplication(bus);
            }
        }

        static void RunApplication(IBus bus)
        {
            PrintInstructions();

            var line = "";
            while (line != null)
            {
                line = Console.ReadLine();

                try
                {
                    var pieces = line.Split(':');
                    var actionEntered = pieces[0];

                    switch (actionEntered)
                    {
                        case "Login":
                            customerId = pieces[1];
                            products = new List<Product>();

                            Console.WriteLine("===> Logged in");

                            break;
                        case "SubmitOrder":

                            if (!products.Any())
                            {
                                Console.WriteLine("You must first add products before submitting the order");
                                break;
                            }

                            bus.Send<SubmitOrder>(o =>
                            {
                                o.CustomerId = customerId;
                                o.DateSent = DateTime.Now;
                                o.Products = products;
                            });
                            products = new List<Product>();

                            Console.WriteLine("Order Submitted");

                            break;
                        case "AddProduct":
                            products.Add(new Product { Name = pieces[1] });

                            Console.WriteLine("Added product: "  + pieces[1]);

                            break;
                        case "CancelOrder":
                            bus.Send<CancelOrder>(o =>
                            {
                                o.OrderId = pieces[1];
                                o.DateSent = DateTime.Now;
                            });

                            Console.WriteLine("Cancelled Order");
                            break;
                        default:
                            Console.WriteLine("===> Unrecognized Action");
                            continue;
                            break;
                    }
                }
                catch (Exception e)
                {
                    continue;
                }

                Console.WriteLine("---------------------------------");
                Console.Write((customerId ?? "NotLoggedIn") + "> ");
            }
        }

        static void PrintInstructions()
        {
            Console.WriteLine("Here are the list of actions you can run:");
            Console.WriteLine("Login:[customerId]");
            Console.WriteLine("AddProduct:[product]");
            Console.WriteLine("SubmitOrder (must have products)");
            Console.WriteLine("CancelOrder:[orderId]");
            Console.WriteLine("---------------------------------");
            Console.Write((customerId ?? "NotLoggedIn") + "> ");
        }
    }
}
