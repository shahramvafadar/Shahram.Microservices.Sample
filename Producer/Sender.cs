using RabbitMQ.Client;
using System.Text;
using System;
using System.Threading.Tasks;

namespace Producer
{
    public class Sender
    {
        public static async Task Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            await using (var connection = await factory.CreateConnectionAsync())
            await using (var channel = await connection.CreateChannelAsync())
            {
                await channel.QueueDeclareAsync(queue: "BasicTest",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: "BasicTest",
                    body: body
                );

                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit the Sender App.");
            Console.ReadLine();
        }
    }
}