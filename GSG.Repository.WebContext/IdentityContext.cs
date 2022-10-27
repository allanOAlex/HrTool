using Microsoft.AspNetCore.Http;

namespace GSG.Repository.WebContext;

public class IdentityContext : IIdentityContext
{
    public string UserName { get; }
    public string Name { get; }
    public string LastName { get; }
    public string Email { get; }

    public IdentityContext(IHttpContextAccessor httpContextAccessor)
    {
        LastName = httpContextAccessor.HttpContext.User.Claims.First(row =>
            row.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;

        Name = httpContextAccessor.HttpContext.User.Claims.First(row =>
            row.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
        
        UserName = httpContextAccessor.HttpContext.User.Claims.First(row =>
            row.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
        
        Email = httpContextAccessor.HttpContext.User.Claims.First(row =>
            row.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn").Value;
    }
}