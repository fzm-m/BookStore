
using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public IActionResult getTotalCount(string? searchItemName, int searchItemTypeId, string? searchAuthorName)
        {
            try
            {
                var query = _DbContext.Items.Where(i => i.IsDelete != 1);

                if (searchItemTypeId != 0)
                    query = query.Where(i => i.ItemTypeId == searchItemTypeId);

                if (!string.IsNullOrEmpty(searchItemName))
                    query = query.Where(i => i.Name.Contains(searchItemName));

                if (!string.IsNullOrEmpty(searchAuthorName))
                    query = query.Where(i => i.Author.Contains(searchAuthorName));

                var totalCount = query.Count();
                return Ok(totalCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetItems(
           int searchItemTypeId,
           string? searchItemName,
           string? searchAuthorName,
           string? sortByReleaseDate = "none",
           int page = 1,
           int pageSize = 5
        )
        {
            try
            {
                var query = _DbContext.Items.Where(i => i.IsDelete != 1);

                if (!string.IsNullOrEmpty(searchItemName))
                    query = query.Where(i => i.Name.Contains(searchItemName));

                if (searchItemTypeId != 0)
                    query = query.Where(i => i.ItemTypeId == searchItemTypeId);

                if (!string.IsNullOrEmpty(searchAuthorName))
                    query = query.Where(i => i.Author.Contains(searchAuthorName));

                if (sortByReleaseDate == "asc")
                    query = query.OrderBy(i => i.ReleaseDate);
                else if (sortByReleaseDate == "desc")
                    query = query.OrderByDescending(i => i.ReleaseDate);

                var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                foreach (var item in items)
                {
                    var itemType = _DbContext.ItemTypes.FirstOrDefault(it => it.Id == item.ItemTypeId);
                    item.ItemTypeName = itemType?.Name;

                    // 🛠️ Ensure Discounted Price is Calculated
                    if (item.PromotionId != 0)
                    {
                        var promotion = _DbContext.Promotions.FirstOrDefault(p => p.Id == item.PromotionId);
                        if (promotion != null)
                        {
                            decimal price = item.Price * promotion.DiscountPercentage; // Keep your existing logic
                            item.DiscountedPrice = Math.Round(price, 2);
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

        [HttpGet("GetSimilarItems/{itemId}")]
        public IActionResult GetSimilarItems(int itemId)
        {
            try
            {
                // Find the current item
                var currentItem = _DbContext.Items.FirstOrDefault(i => i.Id == itemId && i.IsDelete != 1);
                if (currentItem == null)
                {
                    return NotFound("Item not found");
                }

                // Find other items from the same category (excluding the current item)
                var similarItems = _DbContext.Items
                    .Where(i => i.ItemTypeId == currentItem.ItemTypeId && i.Id != itemId && i.IsDelete != 1)
                    .Take(6) // Limit the number of recommendations
                    .ToList();

                // Attach item type names and promotions
                foreach (var item in similarItems)
                {
                    var itemType = _DbContext.ItemTypes.FirstOrDefault(it => it.Id == item.ItemTypeId);
                    item.ItemTypeName = itemType?.Name;

                    // Apply promotion price if applicable
                    if (item.PromotionId != 0)
                    {
                        var promotion = _DbContext.Promotions.FirstOrDefault(p => p.Id == item.PromotionId);
                        if (promotion != null)
                        {
                            decimal price = item.Price * promotion.DiscountPercentage;
                            item.DiscountedPrice = Math.Round(price, 2);
                        }
                    }
                }

                return Ok(similarItems);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpGet("GetItemList")]
        public IActionResult GetItemList(int searchItemTypeId, string? searchItemName, string? author, bool sortByReleaseDate = false)
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
                items = _DbContext.Items.Where(i => i.IsDelete != 1 && i.Name.Contains(searchItemName)).ToList();
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
            try
            {
                // Fetch the item from the database
                var item = _DbContext.Items.FirstOrDefault(i => i.Id == id && i.IsDelete != 1);

                // If item is not found, return 404
                if (item == null)
                {
                    return NotFound($"Item with ID {id} not found.");
                }

                // Fetch associated ItemType
                var itemType = _DbContext.ItemTypes.FirstOrDefault(it => it.Id == item.ItemTypeId);
                item.ItemTypeName = itemType?.Name ?? "Unknown Type";

                // If there is a promotion, calculate the discounted price
                if (item.PromotionId != 0)
                {
                    var promotion = _DbContext.Promotions.FirstOrDefault(p => p.Id == item.PromotionId);
                    if (promotion != null)
                    {
                        decimal price = item.Price * (1 - promotion.DiscountPercentage / 100);
                        item.DiscountedPrice = Math.Round(price, 2);
                    }
                }

                // Return the item
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
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

        [HttpPut("updateItem/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Item item)
        {
            try
            {
                Console.WriteLine($"Received Data: {JsonSerializer.Serialize(item)}"); // Debugging log
                Item bean = _DbContext.Items.FirstOrDefault(i => i.Id == id);
                if (bean == null)
                {
                    return NotFound();
                }

                bean.Name = item.Name;
                bean.Description = item.Description;
                bean.Price = item.Price;
                bean.Stock = item.Stock;
                bean.ItemTypeId = item.ItemTypeId;
                bean.Author = item.Author;
                bean.ReleaseDate = item.ReleaseDate;

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
            try
            {
                Item bean = new Item();
                bean.Name = item.Name;
                bean.Description = item.Description;
                bean.Price = item.Price;
                bean.Stock = item.Stock;
                bean.ItemTypeId = item.ItemTypeId;
                bean.IsDelete = 0;
                bean.Author = item.Author; 
                bean.ReleaseDate = item.ReleaseDate; 

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
