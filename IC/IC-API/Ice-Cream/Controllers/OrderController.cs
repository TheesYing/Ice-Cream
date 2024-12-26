using Microsoft.AspNetCore.Mvc;
using Ice_Cream.Models;
using Ice_Cream.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ice_Cream.DB;
using Ice_Cream.DTO;

namespace Ice_Cream.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // Get All Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            // Eager load PaymentInfo, Subscription, and Books
            var orders = await _context.Orders
                .Include(o => o.PaymentInfo)        // Include PaymentInfo
                .Include(o => o.Subscription)       // Include Subscription
                .Include(o => o.Book)              // Include Books
                .ToListAsync();

            var ordersDTO = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                Email = order.Email,
                ContactDetails = order.ContactDetails,
                Address = order.Address,
                PaymentInfoId = order.PaymentInfoId,
                SubscriptionId = order.SubscriptionId,
                BookId = order.BookId,
                OrderPrice = order.OrderPrice,
                CreatedAt = order.CreatedAt,

                // If PaymentInfo is null, return an empty list
                PaymentInfos = order.PaymentInfo == null ? new List<PaymentDTO>() : new List<PaymentDTO>
        {
            new PaymentDTO
            {
                Id = order.PaymentInfo.Id,
                AccountId = order.PaymentInfo.AccountId,
                Amount = order.PaymentInfo.Amount,
                Currency = order.PaymentInfo.Currency,
                Status = order.PaymentInfo.Status,
                TransactionId = order.PaymentInfo.TransactionId,
                CreatedAt = order.PaymentInfo.CreatedAt
            }
        },

                // If Subscription is null, return an empty list
                Subscriptions = order.Subscription == null ? new List<SubscriptionDTO>() : new List<SubscriptionDTO>
        {
            new SubscriptionDTO
            {
                Id = order.Subscription.Id,
                SubType = order.Subscription.SubType,
                SubDescription = order.Subscription.SubDescription,
                SubPrice = order.Subscription.SubPrice
            }
        },

                // Handle Books as a list, even if one book
                Books = order.Book == null ? new List<BookDTO>() : new List<BookDTO>
        {
            new BookDTO
            {
                    Id = order.Id,
                    BookName = order.Book.BookName,
                    BookDescription = order.Book.BookDescription,
                    BookDate = order.Book.BookDate,
                    BookImage = order.Book.BookImage
                }
            }
            });
            return Ok(ordersDTO);
        }
    

        // Get Order By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            var order = await _context.Orders
    .Include(o => o.PaymentInfo)        // Include PaymentInfo
    .Include(o => o.Subscription)      // Include Subscription
    .Include(o => o.Book)             // Include Books (assuming it's a collection)
    .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();

            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                Email = order.Email,
                ContactDetails = order.ContactDetails,
                Address = order.Address,
                PaymentInfoId = order.PaymentInfoId,
                SubscriptionId = order.SubscriptionId,
                BookId = order.BookId,
                OrderPrice = order.OrderPrice,
                CreatedAt = order.CreatedAt,
                PaymentInfos = order.PaymentInfo == null ? new List<PaymentDTO>() : new List<PaymentDTO>
        {
            new PaymentDTO
            {
                Id = order.PaymentInfo.Id,
                AccountId = order.PaymentInfo.AccountId,
                Amount = order.PaymentInfo.Amount,
                Currency = order.PaymentInfo.Currency,
                Status = order.PaymentInfo.Status,
                TransactionId = order.PaymentInfo.TransactionId,
                CreatedAt = order.PaymentInfo.CreatedAt
            }
        },

                // Assigning list of Subscriptions (even if it's one object, we'll treat it as a list)
                Subscriptions = order.Subscription == null ? new List<SubscriptionDTO>() : new List<SubscriptionDTO>
        {
            new SubscriptionDTO
            {
                Id = order.Subscription.Id,
                SubType = order.Subscription.SubType,
                SubDescription = order.Subscription.SubDescription,
                SubPrice = order.Subscription.SubPrice
            }
        },

                // Assigning list of Books (Books can have multiple, hence list)
                Books = order.Book == null ? new List<BookDTO>() : new List<BookDTO>
        {
            new BookDTO
            {
                    Id = order.Id,
                    BookName = order.Book.BookName,
                    BookDescription = order.Book.BookDescription,
                    BookDate = order.Book.BookDate,
                    BookImage = order.Book.BookImage
                }
            }
            };
            return Ok(orderDTO);
        }

        // Create New Order
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var newOrder = new Order
            {
                Email = orderDTO.Email,
                ContactDetails = orderDTO.ContactDetails,
                Address = orderDTO.Address,
                SubscriptionId = orderDTO.SubscriptionId,
                BookId = orderDTO.BookId,
                PaymentInfoId = orderDTO.PaymentInfoId,
                OrderPrice = orderDTO.OrderPrice,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, orderDTO);
        }

        // Update Existing Order
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDTO orderDTO)
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null) return NotFound();

            existingOrder.Email = orderDTO.Email;
            existingOrder.ContactDetails = orderDTO.ContactDetails;
            existingOrder.Address = orderDTO.Address;
            existingOrder.SubscriptionId = orderDTO.SubscriptionId;
            existingOrder.BookId = orderDTO.BookId;
            existingOrder.PaymentInfoId = orderDTO.PaymentInfoId;
            existingOrder.OrderPrice = orderDTO.OrderPrice;
            existingOrder.CreatedAt = orderDTO.CreatedAt;

            _context.Entry(existingOrder).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete Order
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
