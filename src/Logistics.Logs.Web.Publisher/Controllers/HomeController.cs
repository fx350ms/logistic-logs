using Logistics.Logs.Web.Publisher.Models;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public IActionResult Index(string price = "")
        {
            var data = new
            {
                productNameVi = "Giầy",
                productNameCn = "鞋子",
                trackingNumber = "773393223969882",
                orderId = 0,
                packageNumber = "BT0603695",
                bagNumber = (string)null, // Phải chỉ định kiểu cho các giá trị null
                shippingPartnerId = (int?)null,
                shippingLineId = 2,
                shippingLineString = "TMĐT",
                shippingLineShortString = "TMDT",
                productGroupTypeId = 1,
                categoryId = (int?)null,
                quantity = 3,
                price = 355500.00, 
                isPriceUpdate = false,
                weightUpdateReason = (string)null,
                priceCN = price,
                priceStr = "355.500 ₫",
                totalFee = 32550.00,
                totalFeeStr = "32.550 ₫",
                length = (double?)null,
                lengthString = "",
                // ... (Thêm các thuộc tính còn lại) ...
                customerName = "KPV",
                isDeleted = false,
                id = 194193
            };
            var entityauditLogDto = new AuditLogs.Dto.EntityAuditLogDto
            {
                EntityId = "194193",
                TenantId = 1,
                ServiceName = "pbt",
                MethodName = "Create",
                EntityType = "Package",
                Data = JsonConvert.SerializeObject(data),
                UserId = 42,
                UserName = "TestUser",
                Title = "Cập nhật giá tiền"
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
