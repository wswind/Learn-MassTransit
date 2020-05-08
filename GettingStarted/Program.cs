using System;
using System.Threading.Tasks;
using MassTransit;

namespace GettingStarted
{
  public class Message
{ 
    public string Text { get; set; }
}

public class Program
{
    public static async Task Main()
    {
        var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
        {
                sbc.Host("rabbitmq://192.168.56.10",host =>
                {
                    host.Username("rabbit");
                    host.Password("rabbit");
                }
            );
           

            sbc.ReceiveEndpoint("test_queue", ep =>
            {
                ep.Handler<Message>(context =>
                {
                    return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
                });
            });
        });

        await bus.StartAsync(); // This is important!
        
        await bus.Publish(new Message{Text = "Hi"});
        
        Console.WriteLine("Press any key to exit");
        await Task.Run(() => Console.ReadKey());
        
        await bus.StopAsync();
    }
}
}
