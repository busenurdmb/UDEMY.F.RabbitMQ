// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

var connectionFactory = new ConnectionFactory()
{
    Uri = new Uri("amqps://twnvxtkd:EjPB8j_UQ8vcf7b7ELv8VvMK6ZRnj6i-@shark.rmq.cloudamqp.com/twnvxtkd ")

};

var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();
//durable->true fiziksel olarak kaydedilsisın resart attığımda kaybolmasın
channel.ExchangeDeclare(exchange: "logs-fanout", durable: true, type: ExchangeType.Fanout);
//channel.QueueDeclare(queue: "helloo-queue", durable: true, exclusive: false, autoDelete: false);

//50 tane mesaj oluşturmak için döngüye alıyoruz
Enumerable.Range(1,50).ToList().ForEach(x =>
{
string message = $"log{x}";

var bytemesage = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("logs-fanout","", basicProperties: null, bytemesage);

Console.WriteLine($"Mesaj Gönderilmiştir{message}");
});





Console.ReadLine();




