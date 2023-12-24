using ECommerce.Interface;
using ECommerce.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _IOrderService;
        public OrderController(IOrderService orderService)
        {
            _IOrderService = orderService;
        }

        [HttpPost]
        [Route("Recent")]
        public IActionResult GetRecentCustomerOrder(RequestVM request)
        {
            ReturnVM ret = _IOrderService.GetRecentCustomerOrder(request.CustomerId, request.Email);
            if(ret.IsSuccess)
            {
                return Ok(ret);
            }
            else
            {
                return BadRequest(ret);
            }
        }
    }
}
