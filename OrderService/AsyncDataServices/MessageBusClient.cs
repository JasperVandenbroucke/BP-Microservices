using System.Text;
using System.Text.Json;
using OrderService.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private IChannel? _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            _factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQHost"],
                Port = int.Parse(_config["RabbitMQPort"])
            };
        }

        public async Task InitializeAsync()
        {
            try
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                await _channel.ExchangeDeclareAsync(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to message bus: {ex.Message}");
            }
        }

        public async Task PublishNewOrder(OrderPublishedDto orderPublishedDto)
        {
            var message = JsonSerializer.Serialize(orderPublishedDto);

            if (_channel == null || _connection == null)
            {
                throw new InvalidOperationException("RabbitMQ connection is not initialized");
            }

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending something...");
                // Put the OrderPublishedDto onto the MessageBus
                await SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection is closed, not sending");
            }
        }

        private async Task SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(
                exchange: "trigger",
                routingKey: "",
                body: body
            );
            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            System.Console.WriteLine("--> MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.CloseAsync();
                _connection.CloseAsync();
            }
        }

        private async Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
            await Task.CompletedTask;
        }
    }
}