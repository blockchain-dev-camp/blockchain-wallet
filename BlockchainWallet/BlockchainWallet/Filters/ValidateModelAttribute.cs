namespace Invoicer.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private string _view;

        public ValidateModelAttribute()
        {
            
        }

        public ValidateModelAttribute(string view)
        {
            _view = view;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                var result = new ViewResult();
                if (!string.IsNullOrEmpty(this._view))
                {
                    result.ViewName = _view;
                }

                context.Result = result;
            }
        }
    }
}
