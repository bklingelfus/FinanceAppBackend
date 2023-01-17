using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceAppBackend.Contexts;
using FinanceAppBackend.Models;
using FinanceAppBackend.Dtos;
using FinanceAppBackend.Helpers;

namespace FinanceAppBackend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;
        private readonly PsqlQuery _psqlQuery;

        public AuthController(IUserRepository repository, PsqlQuery psqlQuery, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
            _psqlQuery = psqlQuery;
        }
        
        [HttpPost("user/register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
            
            return Created("Success", _repository.Create(user));
        }
        
        // = * = * = User Routes = * = * =
        [HttpPost("user/login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _repository.GetByEmail(dto.Email);

            if (user == null) return BadRequest(new {message = "Invalid Credentials"});

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new {message = "Invalid Password"});
            }

            var jwt = _jwtService.generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions 
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "Success"
            });
        }
        
        [HttpGet("user/info")]
        public IActionResult User()
        {
            try 
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _repository.GetById(userId);

                return Ok(user);
            } catch (Exception e)
            {
                return Unauthorized();
            }
        }

        [HttpPost("user/logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "Logged Out"
            });
        }

        [HttpDelete("user/delete")]
        public IActionResult Delete()
        {
            try 
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _repository.GetById(userId);

                Response.Cookies.Delete("jwt");
                _repository.Delete(user);
                
                return Ok("Success");
                
            } catch (Exception e)
            {
                return Unauthorized();
            }
        }
        
        [HttpPut("user/edit")]
        public IActionResult Update(EditDto dto)
        {
            try 
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _repository.GetById(userId);

                if (user.Email != dto.Email) return Unauthorized();

                var editedUser = new User {
                    Name = dto.Name,
                    Email = dto.Email,
                    ProfileImage = dto.ProfileImage,
                    DarkMode = dto.DarkMode,
                    Notifications = dto.Notifications,
                    Balance = dto.Balance,
                };
                
                _repository.Update(editedUser);
                
                return Ok("Success");
                
            } catch (Exception e)
            {
                return Unauthorized();
            }
        }
        
        [HttpGet("user/verifyEmail")]
        public IActionResult Check(EmailNotTakenDto dto)
        {
            var user = _repository.GetByEmail(dto.Email);

            if (user == null) return Ok(new{taken = false});

            return Ok(new{taken = true});
        }
        
        // = * = * = Stocks Routes = * = * =
        [HttpGet("stocks/single")]
        public IActionResult Single(StockDto dto)
        {
            return Ok(new {message = _psqlQuery.GetStock(dto.Symbol)});
        }

        [HttpPost("stocks/update")]
        public IActionResult Update(List<StockDto> stockList)
        {
            return Ok(new {message = _psqlQuery.Update(stockList)});
        }

        // = * = * = Order Routes = * = * =
        [HttpPost("order/insert")]
        public IActionResult Insert(Order order)
        {
            var newOrder = new Order {
                Type = order.Type,
                Quantity = order.Quantity,
                Price = order.Price,
                User = order.User,
                Asset = order.Asset
            };

            _repository.Execute(newOrder);
            return Ok(new {message = "Success"});
        }

        [HttpGet("order/fetch")]
        public IActionResult Fetch()
        {
            try 
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _repository.GetById(userId);

                return Ok(new {message = _psqlQuery.Fetch(user.Id)});
            } catch (Exception e)
            {
                return Unauthorized();
            }
        }

        // = * = * = Query Routes = * = * =
        [HttpGet("search/stocks")]
        public IActionResult Search(SearchDto search)
        {
            return Ok(new {message = _psqlQuery.Search(search.text)});
        }
        
    }
}