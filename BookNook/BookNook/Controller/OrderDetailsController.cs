using System.Linq;
using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookNook.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly BookNookDbContext _DbContext;
        public OrderDetailsController(BookNookDbContext DbContext)
        {
            _DbContext = DbContext;
        }


        [HttpGet]
        public IActionResult GetItems(string userId)
        {
            try { 
            List<string> orderIdList = _DbContext.Orders.Where(i => i.UserId == userId).Select(i=>i.Id).ToList();
            List<OrderDetails>  items = _DbContext.OrderDetails.Where(i => orderIdList.Contains(i.OrderId)).ToList();


            foreach (OrderDetails bean in items)
            {
                Item item = _DbContext.Items.Where(i => i.Id == bean.ItemId).FirstOrDefault();
                bean.image = item.ImagePath;
                bean.ItemName = item.Name;
                bean.Description = item.Description;
                bean.Price = item.Price;

                Review review = _DbContext.Reviews.Where(i => i.UserId == userId&i.OrderId== bean.OrderId&i.ItemId==bean.ItemId).FirstOrDefault();
                if (review!=null)
                {
                    bean.Comment = review.Comment;
                }
                
            }
            return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
