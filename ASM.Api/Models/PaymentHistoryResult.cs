using ASM.Data.Entities;
using ASM.Data.Enums;
using System;

namespace ASM.Api.Models
{
    public class PaymentHistoryResult
    {
        public PaymentHistoryResult() { }
        public PaymentHistoryResult(PaymentHistory history) 
        {
            Id = history.Id;
            CreatedDate = history.CreatedDate;
            Price = history.Price;
            UserPaymentId = history.UserPaymentId;
            Status = history.Status;
            Plan = history.SubscriptionPlan?.Name;
            SellerId = history.SellerId;
        }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public string? MetaData { get; set; }
        public Guid? UserPaymentId { get; set; }
        public PaymentStatus? Status { get; set; }
        public string Plan { get; set; }

        public Guid SellerId { get; set; }
    }
}
