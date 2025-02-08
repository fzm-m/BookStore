
using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookNook.Controller
{
     [ApiController]
    [Route("api/[controller]")]
    public class ItemTypeController : ControllerBase
    {
        private readonly BookNookDbContext _DbContext;
        public ItemTypeController(BookNookDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        [HttpGet("getTotalCount")]
        public IActionResult getTotalCount()
        {
            try
            {
                int totalCount = _DbContext.ItemTypes.Where(i => i.IsDelete != 1).Count();
                return Ok(totalCount);
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
           
        }
        [HttpGet]
        public IActionResult GetItemTypes(int page = 1, int pageSize = 5)
        {
            try
            {
                List<ItemType> itemTypes = _DbContext.ItemTypes.Where(i => i.IsDelete != 1).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Ok(itemTypes);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
          
        }
        [HttpGet("getItemTypeList")]
        public IActionResult getItemTypeList()
        {
            try
            {
                List<ItemType> itemTypes = _DbContext.ItemTypes.Where(i => i.IsDelete != 1).ToList();

            return Ok(itemTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("getItemTypeById")]
        public IActionResult getItemTypeById(int id)
        {
            try
            {
                ItemType itemType = _DbContext.ItemTypes.Where(i=>i.Id==id).FirstOrDefault();
            return Ok(itemType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ItemType itemType)
        {
            try
            {
                ItemType bean = _DbContext.ItemTypes.Where(i=>i.Id==id).FirstOrDefault();
            if (bean == null)
            {
                return NotFound();
            }

            bean.Name = itemType.Name;
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
            try
            {
                ItemType bean = _DbContext.ItemTypes.Where(i => i.Id == id).FirstOrDefault();
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
        public async Task<IActionResult> CreateProduct([FromBody] ItemType itemType)
        {
            try
            {
                ItemType bean=new ItemType();
            bean.Name=itemType.Name;
            bean.IsDelete = 0;
            _DbContext.ItemTypes.Add(bean);

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
