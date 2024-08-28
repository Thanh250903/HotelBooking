using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Areas.SystemManager.Controllers
{
    // SystemManager sẽ có LogController: Quản lý và xem các Log của hệ thống => Method: ViewLogs, ViewLogDetail, SearchLog, DeleteLog    
    // SystemManager sẽ có BackupController: Quản lý và sao lưu dữ liệu của hệ thống
    public class SystemManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
