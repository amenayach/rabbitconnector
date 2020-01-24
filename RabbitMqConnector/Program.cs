using System;
using System.Threading.Tasks;

namespace RabbitMqConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please enter the host(s) comma seperated:");
                var hosts = Console.ReadLine().Split(',');

                Console.WriteLine("Please enter the username:");
                var username = Console.ReadLine();

                Console.WriteLine("Please enter the password:");
                var password = Console.ReadLine();

                Console.WriteLine("Please enter the VirtualHost:");
                var vhost = Console.ReadLine();

                Console.WriteLine("Please enter the port:");
                int.TryParse(Console.ReadLine(), out int port);

                var rabbitService = new RabbitMqService(hosts, username, password, vhost, port);

                rabbitService.Run();

                Console.WriteLine("Hit enter to exit");
                Console.ReadLine();

                rabbitService.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
        }
    }
}
