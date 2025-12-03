using Logistics.Logs.Web.Publisher.Models;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Logistics.Logs.Web.Publisher.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBus _bus;
        public HomeController(ILogger<HomeController> logger,
             IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public IActionResult Index()
        {
            var entityauditLogDto = new AuditLogs.Dto.EntityAuditLogDto
            {
                EntityId = "123",
                TenantId = 1,
                ServiceName = "TestService",
                MethodName = "Create",
                EntityType = "TestEntity",
                Data = "{ \"Name\": \"Test\" }",
                CreatationTime = DateTime.Now,
                UserId = 42,
                UserName = "TestUser"
            };
            _bus.Publish(entityauditLogDto);
            return Ok();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
