using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranslateAPIgoogle.entities;
using TranslateAPIgoogle.Helper;
using TranslateAPIgoogle.Interface;
using TranslateAPIgoogle.Services;

namespace TranslateAPIgoogle.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService orderService;
        public OrderController() { orderService = new OrderService(); }

        [HttpPost]
        public async Task<IActionResult> PostUser(User User)
        {
            var res = await orderService.OrderTranslate(User);
            return Ok(res);
        }
        [HttpGet]
        public IActionResult LayHoaDon([FromQuery] int? userID,
                                       [FromQuery] Pagination pagination = null)
        {
            var query = orderService.LayHoaDon(userID);
            var addres = PageResult<User>.ToPageResult(pagination, query).AsEnumerable();
            pagination.TotalCount = query.Count();
            var res = new PageResult<User>(pagination, addres);
            return Ok(res);
        }
    }
}
