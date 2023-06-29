using API_Project.Exceptions;
using API_Project.Interface;
using API_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        public ManagerController(IManagerService managerService)
        {
            this._managerService = managerService;
        }

        //for manager Login
        [HttpPost("login")]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {
            string JwtToken;

            try
            {
                JwtToken = _managerService.ManagerLogin(username, password);
            }
            catch (UserNotFound e)
            {
                return NotFound(e.Message);//400
            }

            catch (IncorrectCredential)
            {
                return BadRequest("Incorrect Username or Password");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something Went wrong");
            }

            return Ok("Jwt Token " + JwtToken);
        }

        // for View all order 
        [Authorize(Roles = "manager")]
        [HttpGet("viewAllOrder")]
        public IActionResult ViewAllOrder()
        {
            return Ok(_managerService.ViewAllOrder());
        }

        //used for manage the order 

        [Authorize(Roles ="manager")]
        [HttpGet("manageOrder")]
        public IActionResult ManageOrder([FromQuery] string order_Id, [FromQuery] string order_Status)
        {
            try
            {
                _managerService.ManageOrder(order_Id, order_Status);
            }
            catch(OrderNotFound e){
                return NotFound(e.Message);
            }
            return Ok("Order Status update");
        }
      

        // to get the order details
        [Authorize(Roles ="manager")]
        [HttpGet("orderDetails")]
        public IActionResult OrderDetails( string order_Id)
        {
            OrderDetails details;
            try
            {
                details = _managerService.OrderDetails(order_Id);
            }
            catch(OrderNotFound e)
            {
                return NotFound(e.Message);
            }
            catch(NullPointerException e)
            {
                return StatusCode(404, "Not Found");
            }
            return Ok(details);
        }

    }
}
