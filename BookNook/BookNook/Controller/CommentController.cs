using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookNook.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly BookNookDbContext _DbContext;
        public CommentController(BookNookDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BookNook.Client.Models.Review review)
        {
            try
            {
                Review bean = new Review();
                bean.ReviewDate = DateTime.Now;
                bean.Rating = review.Rating;
                bean.Comment = review.Comment;
                bean.UserId = review.UserId;
                bean.OrderId = review.OrderId;
                bean.ItemId = review.ItemId;

                _DbContext.Reviews.Add(bean);

                await _DbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
          
        }

        [HttpGet]
        public IActionResult GetItems(string userId)
        {
            try
            {
                List<string> orderIdList = _DbContext.Orders.Where(i => i.UserId == userId).Select(i => i.Id).ToList();
                List<OrderDetails> items = _DbContext.OrderDetails.Where(i => orderIdList.Contains(i.OrderId)).ToList();


                foreach (OrderDetails bean in items)
                {
                    Item item = _DbContext.Items.Where(i => i.Id == bean.ItemId).FirstOrDefault();
                    bean.image = item.ImagePath;
                    bean.ItemName = item.Name;
                    bean.Description = item.Description;
                    bean.Price = item.Price;

                    Review review = _DbContext.Reviews.Where(i => i.UserId == userId & i.OrderId == bean.OrderId & i.ItemId == bean.ItemId).FirstOrDefault();
                    if (review != null)
                    {
                        bean.Comment = review.Comment;
                    }

                }
                return Ok(items);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet("GetReview")]
        public IActionResult GetReview()
        {
            try
            {
                List<Review> items = _DbContext.Reviews.ToList();


                foreach (Review bean in items)
                {
                    Item item = _DbContext.Items.Where(i => i.Id == bean.ItemId).FirstOrDefault();
                    bean.image = item.ImagePath;
                    bean.ItemName = item.Name;
                    bean.Description = item.Description;
                    bean.Price = item.Price;
                }
                return Ok(items);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
           
        }
    }
}
