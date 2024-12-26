using Ice_Cream.DTO;

namespace Ice_Cream.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? ContactDetails { get; set; }
        public string? Address { get; set; }
        public int? PaymentInfoId { get; set; }
        public int? SubscriptionId { get; set; }
        public int? BookId { get; set; }
        public float? OrderPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<PaymentDTO>? PaymentInfos { get; set; }
        public List<SubscriptionDTO>? Subscriptions { get; set; }
        public List<BookDTO>? Books { get; set; }
    }
}