

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mada_immo.Models.includes;
public class SessionClientFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var id = context.HttpContext.Session.GetInt32("idClient");

        if (id == null)
        {
            context.Result = new RedirectToActionResult("Clients", "Home", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}


