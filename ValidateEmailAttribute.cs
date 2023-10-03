using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using VanitiProject.Models;

/// <summary>
/// An action filter attribute for validating the format of an email address.
/// </summary>
public class UsernameValidationAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Validates the format of the email.
    /// </summary>
    /// <param name="actionContext">The context of the HTTP action being executed.</param>
    public override void OnActionExecuting(HttpActionContext actionContext)
    {
        try
        {
            // Determine if the action arguments have the rating element
            if (actionContext.ActionArguments.TryGetValue("rating", out object ratingObj))
            {
                if (ratingObj is Rating rating)
                {
                    string userName = rating.UserName;

                    // Validate the username using a re
                    if (!IsValidEmail(userName))
                    {
                        actionContext.Response = actionContext.Request.CreateErrorResponse(
                            HttpStatusCode.BadRequest,
                            "Invalid email address format.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }

        base.OnActionExecuting(actionContext);
    }

    /// <summary>
    /// Validates whether a string follows a valid email address format.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>true if the email address is valid.</returns>
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        const string regexPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
        return Regex.IsMatch(email, regexPattern);
    }
}
