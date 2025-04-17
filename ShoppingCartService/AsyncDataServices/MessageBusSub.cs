using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShoppingCartService.EventProcessing;

namespace ShoppingCartService.AsyncDataServices
{
    public class MessageBusSub
    {
        private readonly IConfiguration _config;
        private readonly IEventProcessor _eventProcessor;
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IChannel _channel;
        private QueueDeclareOk _queue;
        private string _queueName;

        public MessageBusSub(IConfiguration config, IEventProcessor eventProcessor)
        {
            _config = config;
            _eventProcessor = eventProcessor;
            _factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQHost"],
                Port = int.Parse(_config["RabbitMQPort"])
            };
        }

        public async Task InitializeRabbitMQAsync()
        {
            try
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                await _channel.ExchangeDeclareAsync(exchange: "trigger", type: ExchangeType.Fanout);

                _queue = await _channel.QueueDeclareAsync();
                _queueName = _queue.QueueName;
                await _channel.QueueBindAsync(queue: _queueName, exchange: "trigger", routingKey: "");

                Console.WriteLine("--> Listening to the Message Bus...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to message bus: {ex.Message}");
            }

            WaitForEvents();
        }

        private async void WaitForEvents()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += (model, ea) =>
            {
                Console.WriteLine("--> Event Received!");
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _eventProcessor.ProcessEvent(message);
                return Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
        }
    }
}