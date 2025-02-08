
using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookNook.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly BookNookDbContext _DbContext;
        public ItemController(BookNookDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        [HttpGet("getTotalCount")]
        public IActionResult getTotalCount(string? searchItemName, int searchItemTypeId)
        {
            try
            {
                int totalCount = 0;
                if (searchItemTypeId != 0 && !string.IsNullOrEmpty(searchItemName))
                {
                    totalCount = _DbContext.Items.Where(i => i.IsDelete != 1 & i.ItemTypeId == searchItemTypeId & i.Name == searchItemName).Count();
                }
                else if (searchItemTypeId != 0)
                {
                    totalCount = _DbContext.Items.Where(i => i.IsDelete != 1 & i.ItemTypeId == searchItemTypeId).Count();
                }
                else if (!string.IsNullOrEmpty(searchItemName))
                {
                    totalCount = _DbContext.Items.Where(i => i.IsDelete != 1 & i.Name == searchItemName).Count();
                }
                else
                {
                    totalCount = _DbContext.Items.Where(i => i.IsDelete != 1).Count();
                }
                return Ok(totalCount);
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
          
        }
        [HttpGet]
        public IActionResult GetItems(int searchItemTypeId, string? searchItemName, int page = 1, int pageSize = 5)
        {
            try { 
            List<Item> items = new List<Item>();
            if (searchItemTypeId != 0 && !string.IsNullOrEmpty(searchItemName))
            {
                items = _DbContext.Items.Where(i => i.IsDelete != 1 & i.ItemTypeId == searchItemTypeId & i.Name == searchItemName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else if (searchItemTypeId != 0)
            {
                items = _DbContext.Items.Where(i => i.IsDelete != 1 & i.ItemTypeId == searchItemTypeId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else if (!string.IsNullOrEmpty(searchItemName))
            {
                items = _DbContext.Items.Where(i => i.IsDelete != 1 & i.Name == searchItemName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                items = _DbContext.Items.Where(i => i.IsDelete != 1 ).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            foreach (Item bean in items)
            {
                ItemType itemType = _DbContext.ItemTypes.Where(i => i.Id == bean.ItemTypeId).FirstOrDefault();
                bean.ItemTypeName = itemType.Name;
                Promotion  promotion= _DbContext.Promotions.Where(i => i.Id == bean.PromotionId).FirstOrDefault();
                if (promotion!=null)
                {
                    bean.PromotionCode = promotion.PromoCode;
                }
                
            }
            return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetItemList")]
        public IActionResult GetItemList(int searchItemTypeId, string? searchItemName)
        {
            try { 
            List<Item> items = new List<Item>();
            if (searchItemTypeId != 0 && !string.IsNullOrEmpty(searchItemName))
            {
                items = _DbContext.Items.Where(i => i.IsDelete != 1 & i.ItemTypeId == searchItemTypeId & i.Name == searchItemName).ToList();
            }
            else if (searchItemTypeId != 0)
            {
                items = _DbContext.Items.Where(i => i.IsDelete != 1 & i.ItemTypeId == searchItemTypeId).ToList();
            }
            else if (!string.IsNullOrEmpty(searchItemName))
            {
                items = _DbContext.Items.Where(i => i.IsDelete != 1 & i.Name == searchItemName).ToList();
            }
            else
            {
                items = _DbContext.Items.Where(i => i.IsDelete != 1).ToList();
            }

            foreach (Item bean in items)
            {
                ItemType itemType = _DbContext.ItemTypes.Where(i => i.Id == bean.ItemTypeId).FirstOrDefault();
                bean.ItemTypeName = itemType.Name;
                if (bean.PromotionId!=0)
                {
                Promotion promotion = _DbContext.Promotions.Where(i => i.Id == bean.PromotionId).FirstOrDefault();
                if (promotion != null)
                {
                    bean.PromotionCode = promotion.PromoCode;

                    decimal price=bean.Price*promotion.DiscountPercentage;
                        bean.DiscountedPrice=Math.Round(price,2);    
                }
                }
                
            }
            return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getItemById")]
        public IActionResult getItemById(int id)
        {
            try { 
            Item item = _DbContext.Items.Where(i => i.Id == id).FirstOrDefault();

            ItemType itemType = _DbContext.ItemTypes.Where(i => i.Id == item.ItemTypeId).FirstOrDefault();
            item.ItemTypeName = itemType.Name;
            if (item.PromotionId != 0)
            {
                Promotion promotion = _DbContext.Promotions.Where(p => p.Id == item.PromotionId).FirstOrDefault();
                decimal price = item.Price * promotion.DiscountPercentage;
                item.DiscountedPrice = Math.Round(price, 2);
            }

            return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetHotSellingProducts")]
        public IActionResult GetHotSellingProducts()
        {
            try { 
            List<Item> items = new List<Item>();
                items = _DbContext.Items.Where(i => i.IsDelete != 1).Take(9).ToList();
            foreach (Item item in items)
            {
                ItemType itemType = _DbContext.ItemTypes.Where(i => i.Id == item.ItemTypeId).FirstOrDefault();
                item.ItemTypeName = itemType.Name;
                if (item.PromotionId!=0)
                {
                  Promotion promotion=  _DbContext.Promotions.Where(p => p.Id == item.PromotionId).FirstOrDefault();
                    decimal price = item.Price * promotion.DiscountPercentage;
                    item.DiscountedPrice = Math.Round(price, 2);
                }
            }
            return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetPromotionalProducts")]
        public IActionResult GetPromotionalProducts()
        {
            try { 
            List<Item> items = new List<Item>();
            items = _DbContext.Items.Where(i => i.IsDelete != 1&i.PromotionId!=0).ToList();
            foreach (Item item in items)
            {
                ItemType itemType = _DbContext.ItemTypes.Where(i => i.Id == item.ItemTypeId).FirstOrDefault();
                item.ItemTypeName = itemType.Name;
              
                    Promotion promotion = _DbContext.Promotions.Where(p => p.Id == item.PromotionId).FirstOrDefault();
                    decimal price = item.Price * promotion.DiscountPercentage;
                    item.DiscountedPrice =Math.Round(price,2);
                
            }
            return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Item item)
        {
            try { 
            Item bean = _DbContext.Items.Where(i => i.Id == id).FirstOrDefault();
            if (bean == null)
            {
                return NotFound();
            }

            bean.Name = item.Name;
            bean.Description = item.Description;
            bean.Price = item.Price;
            bean.Stock = item.Stock;
            bean.ItemTypeId = item.ItemTypeId;

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
            Item bean = _DbContext.Items.Where(i => i.Id == id).FirstOrDefault();
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
          [HttpPost("CreateProduct")]
          public async Task<IActionResult> CreateProduct([FromBody] BookNook.Client.Models.Item item)
        {
            try { 
              Item bean = new Item();
              bean.Name = item.Name;
              bean.Description = item.Description;
              bean.Price = item.Price;
              bean.Stock = item.Stock;
              bean.ItemTypeId = item.ItemTypeId;
              bean.IsDelete = 0;
            _DbContext.Items.Add(bean);

              await _DbContext.SaveChangesAsync();
              return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ImageUpload")]
        public async Task<IActionResult> ImageUpload()
        {
            try{
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    
                    var filePath = Path.Combine("wwwroot/images", file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
           
        }
        [HttpPut("SetImgPathById/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] string imgPath)
        {
            try { 
            Item bean = _DbContext.Items.Where(i => i.Id == id).FirstOrDefault();
            if (bean == null)
            {
                return NotFound();
            }

            bean.ImagePath = imgPath;
            await _DbContext.SaveChangesAsync();
            return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("SelectPromotion/{id}")]
        public async Task<IActionResult> SelectPromotion(int id, [FromBody] int promotionId)
        {
            try { 
            Item bean = _DbContext.Items.Where(i => i.Id == id).FirstOrDefault();
            if (bean == null)
            {
                return NotFound();
            }

            bean.PromotionId = promotionId;
            await _DbContext.SaveChangesAsync();
            return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletePromotion/{id}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            try { 
            Item bean = _DbContext.Items.Where(i => i.Id == id).FirstOrDefault();
            if (bean == null)
            {
                return NotFound();
            }
            bean.PromotionId = 0;
            await _DbContext.SaveChangesAsync();
            return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private class ImgPathBean
        {
            public int Id { get; set; }
            public string ImgPath { get; set; }
        }
    }
}
