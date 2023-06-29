using API_Project.Exceptions;
using API_Project.Interface;
using API_Project.Models;
using API_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Collections.Generic;

namespace API_Project.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
       
        
        public UserController(IUserService userService)
        {
            _userService = userService;
           
        }


        // for User Registration
        [HttpPost("signUp")]
        public IActionResult Register(User user)
        {
            bool flag;
            try
            {
                flag = _userService.Register(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            if (flag)
            {
                return Ok("Registation successful");
            }
            return StatusCode(500, "Server Error");
        }

        // For User Login
      
        [HttpPost("login")]

        public IActionResult Login([FromQuery] string email, [FromQuery] string password)
        {

            string JwtToken;
            try
            {

                JwtToken = _userService.Login(email, password);
                
            }
            catch(UserNotFound e)
            {
                return NotFound(e.Message);//400
            }
            
            catch(IncorrectCredential)
            {
                return StatusCode(403,"Incorrect Username or Password");
            }
            catch(Exception)
            {
                return StatusCode(500, "Something Went wrong");
            }
           
            return Ok("Jwt Token " + JwtToken);
        }

        //for to get the password
        [Authorize(Roles = "user")]
        [HttpPost("forgetpassword")]
        public IActionResult ForgetPassword([FromQuery] string email)
        {
            string password;
            try
            {
                password = _userService.ForgetPassword(email);
            }
            catch (UserNotFound e)
            {
                return BadRequest(e.Message);
            }
            catch (IncorrectCredential e)
            {
                return Unauthorized(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something wrong");
            }
            return Ok(" Your password is :" + password);
        }
       
       //For view the Pizza Menu
        [Authorize(Roles ="user")]
        [HttpGet("ViewMenu")]
        public IActionResult ViewMenu()
        {
            return Ok(_userService.ViewMenu());

         }
           
         

             
        
        //for to create the order
        [Authorize(Roles ="user")]
        [HttpPost("createOrder")]

        public IActionResult CreateOrder([FromBody] OrderPizza order)
        {
            string id;
            if(order == null)
            {
                return StatusCode(404,"request body is incorrect");
            }
            
            try
            {
                id= _userService.CreateOrder(order);
            }
            catch(MenuNotFound e)
            {
                return BadRequest("Currently Not Avalialble");
            }
            catch(Exception e){
                return BadRequest(e.Message);
            }
            return Ok("Your Order Id is :"+id);
        }

        // for track the order status
        [Authorize(Roles ="user")]
        [HttpGet("trackOrder")]
        public IActionResult TrackOrder([FromQuery] string orderId)
        {
            List<OrderDetails> order = new List<OrderDetails>();
            try
            {
                order = (List<OrderDetails>)_userService.TrackOrder(orderId);

            }catch(Exception e)
            {
                return StatusCode(404,"Not Found");
            }
            return Ok(order);
        }

        // for view OrderHistory

        [Authorize(Roles ="user")]
        [HttpGet("viewOrderHistory")]
        public IActionResult ViewOrderHistory(string orderId)
        {


            List<OrderDetails> order = new List<OrderDetails>();
            try
            {
                order = (List<OrderDetails>)_userService.ViewOrderHistory(orderId);

            }
            catch (Exception e)
            {
                return StatusCode(404, "Not Found");
            }
            return Ok(order);
            
        }


    }
}
