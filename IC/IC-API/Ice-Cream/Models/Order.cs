using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Ice_Cream.Models
{
     [Table("Order")]
     public class Order
     {
          public int Id { get; set; }
          public string? Email { get; set; }
          public string? ContactDetails { get; set; }
          public string? Address { get; set; }
          public int? PaymentInfoId { get; set; }
          public virtual PaymentInfo PaymentInfo { get; set; }
          public float? OrderPrice { get; set; }
          public DateTime? CreatedAt { get; set; }
          public int? SubscriptionId { get; set; } // FK to Subscription
          public virtual Subscription Subscription { get; set; }


          public int? BookId { get; set; }
          public virtual Book Book { get; set; }
          

          // [NotMapped]
          // public float TotalPrice
          // {
          //      get
          //      {
          //           float total = 0;

          //           if (Books != null)
          //           {
          //                foreach (var book in Books)
          //                {
          //                     total += book.Price ?? 0;
          //                }
          //           }

          //           if (Subscriptions != null)
          //           {
          //                foreach (var subscription in Subscriptions)
          //                {
          //                     if (float.TryParse(subscription.SubPrice, out float subPrice))
          //                     {
          //                          total += subPrice;
          //                     }
          //                }
          //           }

          //           return total;
          //      }
          // }

     }
}