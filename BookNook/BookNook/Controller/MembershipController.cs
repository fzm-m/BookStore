using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookNook.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController : ControllerBase
    {
        private readonly BookNookDbContext _DbContext;
        public MembershipController(BookNookDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpPost("Become")]
        public async Task<IActionResult> Become([FromBody] string UserId)
        {
            try
            {
                List<MembershipLevel> list = _DbContext.MembershipLevels.OrderByDescending(m => m.DiscountRate).ToList();
                if (list != null && list.Count > 0)
                {
                    Membership membership = new Membership();
                    membership.UserId = UserId;
                    membership.MembershipLevelId = list[0].Id;
                    membership.DateCreated = DateTime.Now;
                    _DbContext.Memberships.Add(membership);
                    await _DbContext.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return Unauthorized("No membership level information, please contact the administrator");
                }
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
           
        }

        [HttpGet("UpgradeMembership")]
        public async Task<IActionResult> UpgradeMembership(string userId)
        {
            try
            {
                BookNook.Client.Models.MembershipModel membershipModel = new BookNook.Client.Models.MembershipModel();
                membershipModel.levelMes = $"no";
                Domain.Membership membership = _DbContext.Memberships.Where(m => m.UserId == userId).FirstOrDefault();
                int levelId = 0;
                string level = "";
                if (membership != null)
                {
                    List<Domain.Order> orders = _DbContext.Orders.Where(o => o.UserId == userId).ToList();

                    List<Domain.MembershipLevel> membershipLevels = _DbContext.MembershipLevels.OrderByDescending(m => m.DiscountRate).ToList();
                    if (orders.Count > 0 & membershipLevels.Count > 0)
                    {
                        decimal totalPrice = 0;
                        foreach (var bean in orders)
                        {
                            totalPrice += bean.TotalPrice;
                        }

                        for (int i = 0; i < membershipLevels.Count; i++)
                        {
                            if (totalPrice > 1000 * (i + 1))
                            {
                                if ((i + 1) < membershipLevels.Count)
                                {
                                    levelId = membershipLevels[i + 1].Id;
                                    level = membershipLevels[i + 1].MembershipLevelName;
                                }

                            }
                        }
                        if (levelId != 0 & membership.MembershipLevelId != levelId)
                        {
                            membership.MembershipLevelId = levelId;
                            await _DbContext.SaveChangesAsync();
                            membershipModel.levelMes = $"Your membership has been upgraded:{level}";
                            return Ok(membershipModel);
                        }

                    }

                    return Ok(membershipModel);
                }
                else
                {
                    return Ok(membershipModel);
                }
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
           
        }
    }
}
