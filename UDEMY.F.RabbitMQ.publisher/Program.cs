// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;
using UDEMY.F.RabbitMQ.publisher.Enum;

var connectionFactory = new ConnectionFactory();

connectionFactory.Uri = new Uri("amqps://twnvxtkd:EjPB8j_UQ8vcf7b7ELv8VvMK6ZRnj6i-@shark.rmq.cloudamqp.com/twnvxtkd ");



using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();
//durable->true fiziksel olarak kaydedilsisın resart attığımda kaybolmasın
channel.ExchangeDeclare(exchange: "logs-direct", durable: true, type: ExchangeType.Direct);
//channel.QueueDeclare(queue: "helloo-queue", durable: true, exclusive: false, autoDelete: false);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    var routkey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    //exclusive->false olursa başka channellardan bağlanabilrim
    //autoDelete->otamatik silinmesin 
    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
    channel.QueueBind(queue: queueName, "logs-direct",routkey,null);
});


//50 tane mesaj oluşturmak için döngüye alıyoruz
Enumerable.Range(1,50).ToList().ForEach(x =>
{
    LogNames log = (LogNames)new Random().Next(1, 5);
string message = $"log-type:{log}";

var bytemesage = Encoding.UTF8.GetBytes(message);

    var routkey=$"route-{log}";

channel.BasicPublish("logs-direct",routkey, basicProperties: null, bytemesage);

Console.WriteLine($"Mesaj Gönderilmiştir{message}");
});





Console.ReadLine();




