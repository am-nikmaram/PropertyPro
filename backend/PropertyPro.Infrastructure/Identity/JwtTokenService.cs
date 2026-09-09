//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;

//namespace PropertyPro.Infrastructure.Identity;

//public class JwtTokenService
//{
//    private readonly JwtOptions _options;

//    public JwtTokenService(IOptions<JwtOptions> options)
//    {
//        _options = options.Value;
//    }

//    public string CreateToken(
//        ApplicationUser user,
//        IReadOnlyList<string> roles)
//    {
//        var claims = new List<Claim>
//        {
//            new(JwtRegisteredClaimNames.Sub, user.Id),
//            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
//            new(ClaimTypes.NameIdentifier, user.Id),
//            new(ClaimTypes.Email, user.Email ?? string.Empty),
//            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
//        };

//        claims.AddRange(
//            roles.Select(role =>
//                new Claim(ClaimTypes.Role, role)));

//        var key = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(_options.SecretKey));

//        var credentials = new SigningCredentials(
//            key,
//            SecurityAlgorithms.HmacSha256);

//        var expiration = DateTime.UtcNow.AddMinutes(
//            _options.ExpirationMinutes);

//        var token = new JwtSecurityToken(
//            issuer: _options.Issuer,
//            audience: _options.Audience,
//            claims: claims,
//            expires: expiration,
//            signingCredentials: credentials);

//        return new JwtSecurityTokenHandler()
//            .WriteToken(token);
//    }
//}