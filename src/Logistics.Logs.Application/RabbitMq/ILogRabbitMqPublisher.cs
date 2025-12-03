using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Logs.RabbitMq
{
    public interface ILogRabbitMqPublisher
    {
        Task PublishAsync(LogMessageDto message);
    }

    public class LogRabbitMqConsumer : PeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _config;
        private IConnection _connection;
        private IModel _channel;

        public LogRabbitMqConsumer(
            AbpTimer timer,
            IConfiguration config)
            : base(timer)
        {
            Timer.Period = 1000; // 1 giây
            _config = config;

            InitRabbit();
        }

        private async Task InitRabbit()
        {
            var rabbit = _config.GetSection("App:RabbitMQ");

            var factory = new ConnectionFactory()
            {
                HostName = rabbit["HostName"],
                UserName = rabbit["UserName"],
                Password = rabbit["Password"],
                Port = rabbit.GetValue<int>("Port"),
                VirtualHost = rabbit["VirtualHost"],
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: rabbit["ExchangeName"],
                type: "direct",
                durable: true
            );

            _channel.QueueDeclare(
                queue: rabbit["QueueName"],
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            _channel.QueueBind(
                queue: rabbit["QueueName"],
                exchange: rabbit["ExchangeName"],
                routingKey: rabbit["RoutingKey"]
            );
        }

        protected override void DoWork()
        {
            var rabbit = _config.GetSection("App:RabbitMQ");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var log = JsonSerializer.Deserialize<LogMessageDto>(body);

                Logger.Info($"Receive Log: {log.Service} - {log.Level} - {log.Message}");

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(
                queue: rabbit["QueueName"],
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
