using System.Security.Claims;
using System.Text;
using new_backend.Exceptions;

namespace new_backend.Services;

public interface IAuthManager
{
    long GetUserId();
}

public class AuthManager : IAuthManager
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public AuthManager(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public long GetUserId()
    {
        var nameClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (nameClaim == null)
        {
            throw new NotFoundException("Could not find name identifier, a.k.a \"sub\" in claims.");
        }

        return GetUserIdNumberFromAuthName(nameClaim.Value);
    }

    private long GetUserIdNumberFromAuthName(string authName)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < authName.Length; i++)
        {
            if (char.IsDigit(authName[i]))
            {
                sb.Append(authName[i]);
            }
        }

        return long.Parse(sb.ToString().Substring(0, Math.Min(10, sb.Length)));
    }
}