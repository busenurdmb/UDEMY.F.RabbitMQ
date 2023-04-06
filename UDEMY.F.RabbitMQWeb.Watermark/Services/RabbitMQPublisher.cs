using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace UDEMY.F.RabbitMQWeb.Watermark.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitmqClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitmqClientService)
        {
            _rabbitmqClientService = rabbitmqClientService;
        }

        public void Publish(productImageCreatedEvent productImageCreatedEvent) {
            var channel = _rabbitmqClientService.Connect();
            var bodyString=JsonSerializer.Serialize(productImageCreatedEvent);

            var bodybyte=Encoding.UTF8.GetBytes(bodyString);

            var properties = channel.CreateBasicProperties();
            properties.Persistent= true;

            channel.BasicPublish( RabbitMQClientService.ExchangeName, RabbitMQClientService.RoutingWatermark, basicProperties: properties, body: bodybyte);
                
                
                }
    }
}
