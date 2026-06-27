using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Session1
{
    // Applies role requirements to BooksController actions without editing BooksController (TASK 4.5).
    public class BooksAuthorizationConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (!string.Equals(controller.ControllerName, "Books", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            foreach (var action in controller.Actions)
            {
                var roles = action.ActionName is "Create" or "Update" or "Delete"
                    ? new[] { "Admin" }
                    : new[] { "Reader", "Admin" };

                action.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .RequireRole(roles)
                    .Build()));
            }
        }
    }
}
