using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookNook.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipLevelController : ControllerBase
    {
        private readonly BookNookDbContext _DbContext;
        public MembershipLevelController(BookNookDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        
        [HttpGet]
        public IActionResult Get()
        {
            try { 
            List<MembershipLevel> membershipLevels = _DbContext.MembershipLevels.Where(i => i.IsDelete != 1).ToList();

            return Ok(membershipLevels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("getMembershipLevelById")]
        public IActionResult getMembershipLevelById(int id)
        {
            try { 
            MembershipLevel membershipLevel = _DbContext.MembershipLevels.Where(i => i.Id == id).FirstOrDefault();
            return Ok(membershipLevel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MembershipLevel membershipLevel)
        {
            try { 
            MembershipLevel bean = _DbContext.MembershipLevels.Where(i => i.Id == id).FirstOrDefault();
            if (bean == null)
            {
                return NotFound();
            }
            bean.MembershipLevelName = membershipLevel.MembershipLevelName;
            bean.DiscountRate = membershipLevel.DiscountRate;

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
            MembershipLevel bean = _DbContext.MembershipLevels.Where(i => i.Id == id).FirstOrDefault();
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
        public async Task<IActionResult> Create([FromBody] MembershipLevel membershipLevel)
        {
            try { 
            MembershipLevel bean = new MembershipLevel();
            bean.MembershipLevelName = membershipLevel.MembershipLevelName;
            bean.DiscountRate = membershipLevel.DiscountRate;
            bean.IsDelete = 0;
            _DbContext.MembershipLevels.Add(bean);

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
