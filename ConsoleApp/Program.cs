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
        private static string currentOrderId;
        private static List<string> orders = new List<string>();
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
                            currentOrderId = "";

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
                                o.OrderId = currentOrderId;
                                o.CustomerId = customerId;
                                o.DateSent = DateTime.Now;
                                o.Products = products;
                            });

                            products = new List<Product>();
                            orders.Add(currentOrderId);
                            currentOrderId = "";

                            Console.WriteLine("Order Submitted");

                            break;
                        case "AddProduct":
                            var commands = new List<ICommand>();

                            if(!products.Any() && String.IsNullOrEmpty(currentOrderId))
                            {
                                currentOrderId = Guid.NewGuid().ToString();
                                bus.Send<StartOrder>(o =>
                                {
                                    o.OrderId = currentOrderId;
                                    o.CustomerId = customerId;
                                    o.DateSent = DateTime.Now;
                                });
                            }

                            products.Add(new Product { Name = pieces[1] });

                            bus.Send<AddProductToOrder>(o =>
                            {
                                o.OrderId = currentOrderId;
                                o.CustomerId = customerId;
                                o.ProductId = pieces[1];
                                o.DateSent = DateTime.Now;
                            });

                            Console.WriteLine("Added product: "  + pieces[1]);

                            break;
                        case "RemoveProduct":

                            if (!products.Any(p => p.Name == pieces[1]))
                            {
                                Console.WriteLine("There are no products added that match: " + pieces[1]);
                                break;
                            }

                            products.RemoveAll(p => p.Name == pieces[1]);

                            bus.Send<RemoveProductFromOrder>(o =>
                            {
                                o.OrderId = currentOrderId;
                                o.CustomerId = customerId;
                                o.ProductId = pieces[1];
                                o.DateSent = DateTime.Now;
                            });

                            Console.WriteLine("Removed product: " + pieces[1]);

                            break;
                        case "CancelOrder":
                            bus.Send<CancelOrder>(o =>
                            {
                                o.OrderId = orders[orders.Count - 1];
                                o.DateSent = DateTime.Now;
                            });

                            products = new List<Product>();

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

        public static void ClearCurrentOrder()
        {
            products = new List<Product>();
            currentOrderId = "";
        }

        static void PrintInstructions()
        {
            Console.WriteLine("Here are the list of actions you can run:");
            Console.WriteLine("Login:[customerId]");
            Console.WriteLine("AddProduct:[product]");
            Console.WriteLine("RemoveProduct:[product]");
            Console.WriteLine("SubmitOrder (must have products)");
            Console.WriteLine("CancelOrder");
            Console.WriteLine("---------------------------------");
            Console.Write((customerId ?? "NotLoggedIn") + "> ");
        }
    }
}
