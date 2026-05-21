using PRS.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PRS.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _http;

    public AuthService(AppDbContext db, IHttpContextAccessor http)
    {
        _db = db;
        _http = http;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.UserUsername == username);

        if (user == null) return false;

        if (!BCrypt.Net.BCrypt.Verify(password, user.UserPassword))
            return false;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserUsername),
            new Claim("UserPslId", user.UserPslId?.ToString() ?? string.Empty)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        //var http = _http.HttpContext;
        //if (http == null) return false;

        //await http.SignInAsync(
        //    CookieAuthenticationDefaults.AuthenticationScheme,
        //    principal);

        return true;
    }

    public async Task LogoutAsync()
    {
        await _http.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}