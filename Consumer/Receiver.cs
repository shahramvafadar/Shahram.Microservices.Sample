using RabbitMQ.Client;
using System.Text;
using System;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public class Receiver
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

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                    await Task.Yield();
                };

                await channel.BasicConsumeAsync(queue: "BasicTest",
                                     autoAck: true,
                                     consumer: consumer);
                Console.WriteLine(" [*] Waiting for messages.");
                Console.ReadLine();
            }
        }
    }
}