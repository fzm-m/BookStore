using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookNook.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly BookNookDbContext _DbContext;

        public CartController(BookNookDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Cart> carts = _DbContext.Carts.ToList();
                foreach (Cart cart in carts)
                {
                    Item item = _DbContext.Items.Where(i => i.Id == cart.ItemId).FirstOrDefault();
                    cart.ImagePath = item.ImagePath;
                    cart.Name = item.Name;
                    cart.Description = item.Description;
                    cart.Price = item.Price;

                    if (item.PromotionId != 0)
                    {
                        Promotion promotion = _DbContext.Promotions.Where(i => i.Id == item.PromotionId).FirstOrDefault();
                        if (promotion != null)
                        {
                            decimal price = item.Price * promotion.DiscountPercentage;
                            cart.DiscountedPrice = Math.Round(price, 2);
                        }
                    }
                }
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cart cart)
        {
            try
            {
                // Fetch the item to check stock availability
                Item item = _DbContext.Items.FirstOrDefault(i => i.Id == cart.ItemId);

                if (item == null)
                {
                    return NotFound("Item not found.");
                }

                // Check if stock is sufficient
                Cart existingCart = _DbContext.Carts
                    .FirstOrDefault(c => c.UserId == cart.UserId && c.ItemId == cart.ItemId);

                int existingCartQuantity = existingCart != null ? existingCart.ItemNumber : 0;

                if (item.Stock < existingCartQuantity + cart.ItemNumber)
                {
                    return BadRequest($"Insufficient stock. Only {item.Stock - existingCartQuantity} left.");
                }

                // Add to cart or update quantity
                if (existingCart == null)
                {
                    cart.DateAdded = DateTime.Now;
                    _DbContext.Carts.Add(cart);
                }
                else
                {
                    existingCart.ItemNumber += cart.ItemNumber;
                }

                await _DbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Cart bean = _DbContext.Carts.Where(i => i.Id == id).FirstOrDefault();
                if (bean == null)
                {
                    return NotFound();
                }
                _DbContext.Remove(bean);
                await _DbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

