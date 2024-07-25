

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mada_immo.Models.includes;
public class SessionPropFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var id = context.HttpContext.Session.GetInt32("idProp");

        if (id == null)
        {
            context.Result = new RedirectToActionResult("Prop", "Home", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}


