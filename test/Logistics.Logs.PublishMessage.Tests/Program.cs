using MassTransit;

namespace Logistics.Logs.PublishMessage.Tests
{

    public class MessagePublisher
    {
        private readonly IBusControl _busControl;
        private const string RabbitMqHost = "localhost"; // Thay đổi nếu RabbitMQ của bạn ở host khác
        private const string QueueName = "entity-audit-log-queue";

        public MessagePublisher()
        {
            _busControl = ConfigureBus();
        }

        private IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                // Cấu hình Host RabbitMQ
                cfg.Host(RabbitMqHost, "/", h =>
                {
                    h.Username("guest"); // User mặc định
                    h.Password("guest"); // Pass mặc định
                });
            });
        }

        /// <summary>
        /// Kết nối đến RabbitMQ và gửi tin nhắn EntityAuditLogMessage đến Queue "repro-service".
        /// </summary>
        public async Task PublishMessage(EntityAuditLogDto message)
        {
            Console.WriteLine("Starting MassTransit Bus...");
            await _busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);

            try
            {
                // Lấy Send Endpoint cho Queue cụ thể
                var endpointUri = new Uri($"rabbitmq://{RabbitMqHost}/{QueueName}");
                var sendEndpoint = await _busControl.GetSendEndpoint(endpointUri);

                // Gửi tin nhắn
                Console.WriteLine($"Sending EntityAuditLogMessage to Queue: {QueueName}...");

                await sendEndpoint.Send(message);

                Console.WriteLine($"[SUCCESS] Message Sent:");
                Console.WriteLine($"  EntityId: {message.EntityId}");
                Console.WriteLine($"  EntityType: {message.EntityType}");
                Console.WriteLine($"  Method: {message.MethodName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] An error occurred while sending message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[INNER EXCEPTION] {ex.InnerException.Message}");
                }
            }
            finally
            {
                // Dừng Bus
                Console.WriteLine("Stopping MassTransit Bus...");
                await _busControl.StopAsync();
            }
        }
    }

    // -----------------------------------------------------------
    // 3. Logic thực thi chính
    // -----------------------------------------------------------
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("MassTransit RabbitMQ EntityAuditLog Publisher Console App");
            Console.WriteLine("-------------------------------------------------------");

            var publisher = new MessagePublisher();

            // **Tạo tin nhắn EntityAuditLogMessage với dữ liệu mẫu**
            var message = new EntityAuditLogDto
            {
                Id = 0,
                EntityId = Guid.NewGuid().ToString(),
                TenantId = 1,
                ServiceName = "TestService",
                MethodName = "Create",
                EntityType = "Order",
                Data = "Sample data created at " + DateTime.Now.ToString("o"),
                CreatationTime = DateTime.Now,
                UserId = 999,
                UserName = "Tester"
                
            };

            // Gửi tin nhắn
            await publisher.PublishMessage(message);

            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    public class EntityAuditLogDto
    {
        public int Id { get; set; }
        public string EntityId { get; set; }
        public int? TenantId { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public string EntityType { get; set; }
        public string Data { get; set; }
        public DateTime CreatationTime { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
    }
}
