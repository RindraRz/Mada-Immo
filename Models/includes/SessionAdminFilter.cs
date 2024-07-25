

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mada_immo.Models.includes;
public class SessionAdminFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var id = context.HttpContext.Session.GetInt32("idAdmin");

        if (id == null)
        {
            context.Result = new RedirectToActionResult("AdminLog", "Home", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}


