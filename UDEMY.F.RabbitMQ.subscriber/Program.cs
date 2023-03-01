// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;

var connectionFactory = new ConnectionFactory()
{
    Uri = new Uri("amqps://twnvxtkd:EjPB8j_UQ8vcf7b7ELv8VvMK6ZRnj6i-@shark.rmq.cloudamqp.com/twnvxtkd ")

};

var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();

//channel.QueueDeclare(queue: "hello-queue", durable: false, exclusive: false, autoDelete: false);

//eğer bunu silersek subscriber ayağa kalktığında böyle bir kuyruk yoksa hata alırsınız 
//silmesseniz publisher bu kuyruğu oluşturmassa  subscriber bu kuruğu oluşturur ve uygulamada herhangibi bir hata almassınız 
//eğer pıblisherın bu kuyruğu gerçekten oluşturduğundan eminseniz kuyruğu silebilirsiniz 

//kaç kaç göndereceğiz 
//prefetchSize->boyut ver diyor o deyince bana herhangi bir boyutttaki mesajı gönderebilirsin
//prefetchCount->kaç kaç mesajlar gelsin her bir subscriber a 1 er 1 er gelsin
//global olsunmu ->
//channel.BasicQos(0,5,false) tek bir seferde her subscriber 5 5 gönderir 
//channel.BasicQos(0,5,true ) kaç tane subscriber varsa tek seferde tüm subscriberları toplam değeri 5 olarak gönderir 3 birine  2 birine
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);


var consumer = new EventingBasicConsumer(channel);
//autoAck->true verirsek rabbitmq mesaj verdiğinde hemen siliyordu
//autoAck->False verirsek hemen silme ben seni haberdar edicem diyorum
channel.BasicConsume("helloo-queue", autoAck: false, consumer: consumer);


consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var messsage = Encoding.UTF8.GetString(e.Body.ToArray());

    Thread.Sleep(1000);

    Console.WriteLine("Gelen Mesaj:" + messsage);

    //BasicAck()methodum var bu methoda diyorimki sen ilgili mesajı artık silebilirsin (DeliveryTag) bana ulaştırılan tagı rabbitmqye gönderiyorum bu rabbitmq hangi tagla bu emsajı ulaştırmışsa ilgili mesajı bulup kuyruktan siliyor,
    //multiple değeri var->true dersek memory işlenmiş ama rabbitmqye gitmemiş başka mesajlarda varsa onun bilgilerini rabiitmqye haberdar eder biz tek mesaj işliyoruz false dedik sadce ilgili mesajın durumunu bilidr diyoruz
    //eğe hata alırsak bu mesajı göndermeyiz rabbitmq haber vemreyiz rabbitmq da haber edilmeyen mesajları belli süre sonra tekrar başka bir subscriber a gönderir
    channel.BasicAck(e.DeliveryTag,multiple:false);
};


Console.ReadLine();
