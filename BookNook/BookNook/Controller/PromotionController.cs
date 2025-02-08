using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookNook.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        private readonly BookNookDbContext _DbContext;
        public PromotionController(BookNookDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpGet("getTotalCount")]
        public IActionResult getTotalCount()
        {
            try { 
            int totalCount = _DbContext.Promotions.Where(i => i.IsDelete != 1).Count();
            return Ok(totalCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult Get(int page = 1, int pageSize = 5)
        {
            try { 
            List<Promotion> promotions = _DbContext.Promotions.Where(i => i.IsDelete != 1).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(promotions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPromotionList")]
        public IActionResult getPromotionList()
        {
            try { 
            List<Promotion> promotions = _DbContext.Promotions.Where(i => i.IsDelete != 1).ToList();

            return Ok(promotions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getPromotionById")]
        public IActionResult getPromotionById(int id)
        {
            try { 
            Promotion promotion = _DbContext.Promotions.Where(i => i.Id == id).FirstOrDefault();
            return Ok(promotion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Promotion promotion)
        {
            try { 
            Promotion bean = _DbContext.Promotions.Where(i => i.Id == id).FirstOrDefault();
            if (bean == null)
            {
                return NotFound();
            }

            bean.PromoCode = promotion.PromoCode;
            bean.Description = promotion.Description;
            bean.DiscountPercentage = promotion.DiscountPercentage;

            await _DbContext.SaveChangesAsync();
            return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try { 
            Promotion bean = _DbContext.Promotions.Where(i => i.Id == id).FirstOrDefault();
            if (bean == null)
            {
                return NotFound();
            }
            bean.IsDelete = 1;
            await _DbContext.SaveChangesAsync();
            return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Promotion promotion)
        {
            try { 
            Promotion bean = new Promotion();

            bean.PromoCode = promotion.PromoCode;
            bean.Description = promotion.Description;
            bean.DiscountPercentage = promotion.DiscountPercentage;
            bean.IsDelete = 0;
            _DbContext.Promotions.Add(bean);

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
