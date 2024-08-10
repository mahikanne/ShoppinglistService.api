using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppinglistService.api.Authentication;
using ShoppinglistService.api.Data;
using ShoppinglistService.api.Model;


using System;
using System.Reflection;

namespace ShoppinglistService.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly DapperDbContext _context;
        public AuthController(JwtTokenGenerator jwtTokenGenerator, DapperDbContext context)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserRegistration model)
        {
            // Here you would normally validate the user credentials against a database
            // For this example, let's assume the user is valid
            DateTime date = new DateTime();
            AuthResponseData authResponseData = new AuthResponseData();

            var SqlQuery = @"SELECT * FROM [dbo].[UserRegistration] WHERE EMAIL = @Email";

            using var connection = _context.CreateConnection();

            var result = connection.QueryFirstOrDefault<UserRegistration>(SqlQuery, model);

            if (result != null)
            {
                authResponseData.email = model.Email;
                authResponseData.localId = Convert.ToString(result.UserId);
                if (model.Email == result.Email)
                {
                    if (model.Password == result.Password)
                    {
                        var roles = new[] { "Admin" };

                        var token = _jwtTokenGenerator.GenerateToken(model.Email, roles, ref date);

                        authResponseData.idToken = token;

                        //ClaimsPrincipal clmprinciple = _jwtTokenGenerator.ValidateJwtToken(token, "kannegantimahendrababu");

                        //if (clmprinciple != null)
                        //{
                        //    Claim emailClaim = clmprinciple.FindFirst("tokentype");
                        //    string email = emailClaim?.Value;
                        //}

                        authResponseData.expiresIn = date.ToString();
                        authResponseData.message = "LOGIN_SUCCESSFULL";

                        return Ok(authResponseData);
                    }
                    else
                    {
                        authResponseData.message = "PASSWORD_NOT_MATCHED";
                        return Ok(authResponseData);
                    }
                }
            }
            else
            {
                authResponseData.message = "EMAIL_NOT_MATCHED";
                return Ok(authResponseData);
            }


            return Unauthorized();
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration([FromBody] UserRegistration model)
        {
            // Here you would normally validate the user credentials against a database
            // For this example, let's assume the user is valid
            AuthResponseData authResponseData = null;
            if (ModelState.IsValid)
            {
                authResponseData = new AuthResponseData();
                if (IsEmailExists(model))
                {
                    authResponseData.message = "USER_EXISTS";
                    return Ok(authResponseData);
                }
                authResponseData.email = model.Email;

                var sql = @"INSERT INTO [dbo].[UserRegistration](Email,[Password]) output inserted.UserId Values(@Email,@Password)";
                using var connection = _context.CreateConnection();

                int uId = (int)await connection.ExecuteScalarAsync(sql, model);
                authResponseData.localId = uId.ToString();  

                authResponseData.message = "REGISTERED";
                return Ok(authResponseData);
            }



            return Unauthorized();
        }

        [NonAction]
        public bool IsEmailExists(UserRegistration model)
        {
            var SqlQuery = @"SELECT * FROM [dbo].[UserRegistration] WHERE EMAIL = @Email";

            using var connection = _context.CreateConnection();

            var result = connection.QueryFirstOrDefault<UserRegistration>(SqlQuery, model);


            return (result != null);
        }
    }
}
