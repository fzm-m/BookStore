using BookNook.Client.Pages.User.UserHome;
using BookNook.Data;
using BookNook.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace BookNook.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly BookNookDbContext _DbContext;
        public OrderController(BookNookDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] BookNook.Client.Models.PlaceOrderModel placeOrderModel)
        {
            try
            {
                // Create new payment and shipment records
                Payment payment = new Payment()
                {
                    Id = Guid.NewGuid().ToString()
                };
                Shipment shipment = new Shipment()
                {
                    Id = Guid.NewGuid().ToString()
                };

                // Create new order
                Order order = new Order();
                order.Id = Guid.NewGuid().ToString();
                order.UserId = placeOrderModel.UserId;
                order.RecipientName = placeOrderModel.RecipientName;
                order.RecipientPhone = placeOrderModel.RecipientPhone;
                order.RecipientAddress = placeOrderModel.RecipientAddress;
                order.TotalPrice = placeOrderModel.TotalPrice;
                order.OrderDate = DateTime.Now;
                order.PaymentId = payment.Id;
                order.ShipmentId = shipment.Id;

                foreach (BookNook.Client.Models.OrderDetails bean in placeOrderModel.orderDetailsList)
                {
                    // Fetch the item from the database
                    var item = _DbContext.Items.FirstOrDefault(i => i.Id == bean.ItemId);
                    if (item == null)
                    {
                        return NotFound($"Item with ID {bean.ItemId} not found.");
                    }

                    // Check if stock is sufficient
                    if (item.Stock < bean.Quantity)
                    {
                        return BadRequest($"Insufficient stock for item '{item.Name}'. Only {item.Stock} left.");
                    }

                    // Reduce the stock
                    item.Stock -= bean.Quantity;

                    // Create order details
                    OrderDetails details = new OrderDetails
                    {
                        ItemId = bean.ItemId,
                        Quantity = bean.Quantity,
                        TotalPrice = bean.TotalPrice,
                        OrderId = order.Id
                    };
                    _DbContext.OrderDetails.Add(details);
                }

                // Save payment details
                payment.PaymentMethod = placeOrderModel.PaymentMethod;
                payment.PaymentAmount = placeOrderModel.PaymentAmount;
                payment.PaymentDate = DateTime.Now;
                payment.OrderId = order.Id;

                // Save shipment details
                shipment.TrackingNumber = DateTime.Now.ToString().Substring(0, 10) + Guid.NewGuid().ToString().Substring(0, 5);
                shipment.Status = Client.Models.ShipmentStatusModel.Waiting;
                shipment.ShipmentDate = DateTime.Now;
                shipment.OrderId = order.Id;

                // Add order, payment, and shipment to the database
                _DbContext.Orders.Add(order);
                _DbContext.Payments.Add(payment);
                _DbContext.Shipments.Add(shipment);

                // Save changes to update stock and add order
                await _DbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("getTotalCount")]
        public IActionResult getTotalCount(string? userName, string? shipmentStatus)
        {
            try { 
            int totalCount = 0;
            var queryable = _DbContext.Orders
                .GroupJoin(
                _DbContext.Users,
                order => order.UserId,
                user => user.Id,
                (order, userGroup) => new { Order = order, Users = userGroup })
                .SelectMany(
                    x => x.Users.DefaultIfEmpty(),
                     (x, user) => new { Order = x.Order, User = user })
                    .GroupJoin(
                    _DbContext.Shipments,
                    combined => combined.Order.Id,
                    shipment => shipment.OrderId,
                    (combined, shipmentGroup) => new { combined.Order, combined.User, Shipments = shipmentGroup })
               .SelectMany(
                    x => x.Shipments.DefaultIfEmpty(),
                    (x, shipment) => new Domain.Order
                    {
                        Id = x.Order.Id,
                        UserId = x.User.Id,
                        RecipientName = x.Order.RecipientName,
                        RecipientPhone = x.Order.RecipientPhone,
                        RecipientAddress = x.Order.RecipientAddress,
                        TotalPrice = x.Order.TotalPrice,
                        OrderDate = x.Order.OrderDate,
                        PaymentId = x.Order.PaymentId,
                        UserName = x.User.UserName,
                        ShipmentStatus= shipment.Status
                    });


            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(shipmentStatus) && shipmentStatus != "0")
            {
                totalCount = queryable.Where(i =>  i.UserName == userName & i.ShipmentStatus == shipmentStatus).Count();
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                totalCount = queryable.Where(i => i.UserName == userName).Count();
            }
            else if (!string.IsNullOrEmpty(shipmentStatus) && shipmentStatus != "0")
            {
                totalCount = queryable.Where(i => i.ShipmentStatus == shipmentStatus).Count();
            }
            else
            {
                totalCount = queryable.Count();
            }
            return Ok(totalCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetItems(string? userName, string? shipmentStatus, int page = 1, int pageSize = 5)
        {
            try { 
            var queryable = _DbContext.Orders
               .GroupJoin(
               _DbContext.Users,
               order => order.UserId,
               user => user.Id,
               (order, userGroup) => new { Order = order, Users = userGroup })
               .SelectMany(
                   x => x.Users.DefaultIfEmpty(),
                    (x, user) => new { Order = x.Order, User = user })
                   .GroupJoin(
                   _DbContext.Shipments,
                   combined => combined.Order.Id,
                   shipment => shipment.OrderId,
                   (combined, shipmentGroup) => new { combined.Order, combined.User, Shipments = shipmentGroup })
              .SelectMany(
                   x => x.Shipments.DefaultIfEmpty(),
                   (x, shipment) => new Order
                   {
                       Id = x.Order.Id,
                       UserId = x.User.Id,
                       RecipientName = x.Order.RecipientName,
                       RecipientPhone = x.Order.RecipientPhone,
                       RecipientAddress = x.Order.RecipientAddress,
                       TotalPrice = x.Order.TotalPrice,
                       OrderDate = x.Order.OrderDate,
                       PaymentId = x.Order.PaymentId,
                       UserName = x.User.UserName,
                       ShipmentStatus = shipment.Status,
                       ShipmentId=shipment.Id
                   });
            List<Order> items = new List<Order>();

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(shipmentStatus) && shipmentStatus != "0")
            {
                items = queryable.Where(i =>  i.UserName == userName & i.ShipmentStatus == shipmentStatus).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                items = queryable.Where(i =>  i.UserName == userName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else if (!string.IsNullOrEmpty(shipmentStatus) && shipmentStatus != "0")
            {
                items = queryable.Where(i =>  i.ShipmentStatus == shipmentStatus).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                items = queryable.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getItemById")]
        public IActionResult getItemById(string id)
        {
            try { 
            BookNook.Client.Models.PlaceOrderModel placeOrderModel = new BookNook.Client.Models.PlaceOrderModel();
            Order order = _DbContext.Orders.Where(i => i.Id == id).FirstOrDefault();
            placeOrderModel.UserId = order.UserId;
            placeOrderModel.RecipientName = order.RecipientName;
            placeOrderModel.RecipientPhone = order.RecipientPhone;
            placeOrderModel.RecipientAddress = order.RecipientAddress;
            placeOrderModel.TotalPrice = order.TotalPrice;
            placeOrderModel.OrderDate = order.OrderDate;

            BookNookUser bookNookUser = _DbContext.Users.Where(i => i.Id == order.UserId).FirstOrDefault();
            placeOrderModel.UserName = bookNookUser.UserName;

            Payment payment = _DbContext.Payments.Where(i => i.Id == order.PaymentId).FirstOrDefault();
            placeOrderModel.PaymentMethod = payment.PaymentMethod;
            placeOrderModel.PaymentAmount = payment.PaymentAmount;

            Shipment shipment = _DbContext.Shipments.Where(i => i.Id == order.ShipmentId).FirstOrDefault();
            placeOrderModel.TrackingNumber = shipment.TrackingNumber;
            placeOrderModel.Status = shipment.Status;
            placeOrderModel.ShipmentDate = shipment.ShipmentDate;

            List<OrderDetails> orderDetailsList = _DbContext.OrderDetails.Where(d => d.OrderId == order.Id).ToList();
            placeOrderModel.orderDetailsList = new List<Client.Models.OrderDetails>();
            foreach (OrderDetails details in orderDetailsList)
            {
                BookNook.Client.Models.OrderDetails bean = new Client.Models.OrderDetails();
                bean.Id = details.Id;
                bean.ItemId = details.ItemId;
                bean.Quantity = details.Quantity;
                bean.TotalPrice = details.TotalPrice;
                bean.OrderId = details.OrderId;

                Item item = _DbContext.Items.Where(i => i.Id == bean.ItemId).FirstOrDefault();
                bean.ItemName = item.Name;
                bean.image = item.ImagePath;

                placeOrderModel.orderDetailsList.Add(bean);
            }
           

            return Ok(placeOrderModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("EditShipment/{id}")]
        public async Task<IActionResult> EditShipment(string id, [FromBody] string shipmentStatus)
        {
            try { 
            Shipment bean = _DbContext.Shipments.Where(i => i.Id == id).FirstOrDefault();
            if (bean == null)
            {
                return NotFound();
            }

            bean.Status = shipmentStatus;
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
