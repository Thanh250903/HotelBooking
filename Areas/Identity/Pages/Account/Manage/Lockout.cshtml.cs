using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace HotelApp.Areas.Identity.Pages.Account.Manage
{
    [AllowAnonymous]
    public class LockOutModel : PageModel
    {
        public void OnGet()
        {
        }
    }

}
