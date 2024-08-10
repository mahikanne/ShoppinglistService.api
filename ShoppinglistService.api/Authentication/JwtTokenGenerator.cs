using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppinglistService.api.Authentication
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username, string[] roles,ref DateTime date)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            date = DateTime.Now.AddMinutes(30);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("tokentype", "JWT"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            //var roleClaims = new List<Claim>();

            //foreach (var role in roles)
            //{
            //    roleClaims.Add(new Claim(ClaimTypes.Role, role));
            //}

            //claims = claims.Concat(roleClaims).ToArray();

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: date,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //public ClaimsPrincipal ValidateJwtToken(string token, string secretKey)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(secretKey);

        //    var ClaimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(key),
        //        ValidateIssuer = false,
        //        ValidateAudience = false,
        //        // Set ClockSkew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        //        ClockSkew = TimeSpan.Zero
        //    }, out SecurityToken validatedToken);


        //    return ClaimsPrincipal;
        //}
    }
}
