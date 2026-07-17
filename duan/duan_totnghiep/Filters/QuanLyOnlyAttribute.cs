using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace duan_totnghiep.Filters
{
    public class QuanLyOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("VaiTro");

            if (role != "Admin")
            {
                context.Result = new RedirectToActionResult(
                    "AccessDenied",
                    "Home",
                    null);

                return;
            }

            base.OnActionExecuting(context);
        }
    }
}