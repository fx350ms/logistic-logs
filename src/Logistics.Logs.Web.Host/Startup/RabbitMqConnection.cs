using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace Logistics.Logs.Web.Host.Startup
{
    public class RabbitMqConnection : IRabbitMqConnection, IDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;

        public RabbitMqConnection(string host, string user, string pass)
        {
            _factory = new ConnectionFactory()
            {
                HostName = host,
                UserName = user,
                Password = pass
            };
        }

        public IConnection GetConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _factory.CreateConnectionAsync().Result;
            }
            return _connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }

    public interface IRabbitMqConnection
    {
        IConnection GetConnection();
    }
}
