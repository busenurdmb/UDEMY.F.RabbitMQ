// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;
using UDEMY.F.RabbitMQ.publisher.Enum;

var connectionFactory = new ConnectionFactory();

connectionFactory.Uri = new Uri("amqps://twnvxtkd:EjPB8j_UQ8vcf7b7ELv8VvMK6ZRnj6i-@shark.rmq.cloudamqp.com/twnvxtkd ");



using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();
//durable->true fiziksel olarak kaydedilsisın resart attığımda kaybolmasın
channel.ExchangeDeclare(exchange: "logs-topic", durable: true, type: ExchangeType.Topic);
//channel.QueueDeclare(queue: "helloo-queue", durable: true, exclusive: false, autoDelete: false);

//Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
//{
//    Random rnd=new Random();
//    LogNames log1 = (LogNames)rnd.Next(1, 5);
//    LogNames log2 = (LogNames)rnd.Next(1, 5);
//    LogNames log3 = (LogNames)rnd.Next(1, 5);

//    var routkey = $"{log1}.{log2}.{log3}";
//    //var queueName = $"direct-queue-{x}";
//    //exclusive->false olursa başka channellardan bağlanabilrim
//    //autoDelete->otamatik silinmesin 
//    //channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
//    //channel.QueueBind(queue: queueName, "logs-direct",routkey,null);
//});

Random rnd = new Random();
//50 tane mesaj oluşturmak için döngüye alıyoruz
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    //LogNames log = (LogNames)new Random().Next(1, 5);


    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    var routkey = $"{log1}.{log2}.{log3}";


    string message = $"log-type:{log1}-{log2}-{log3}";
    //var routkey=$"route-{log}";
    var bytemesage = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish("logs-topic", routkey, basicProperties: null, bytemesage);

    Console.WriteLine($"Mesaj Gönderilmiştir{message}");
});





Console.ReadLine();




