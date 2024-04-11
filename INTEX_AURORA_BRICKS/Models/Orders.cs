using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace INTEX_AURORA_BRICKS.Models
{
    public class Order
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public DateTime? Date { get; set; }

        [Display(Name = "Day of Week")]
        public string? DayOfWeek { get; set; }

        public int? Time { get; set; }

        [Display(Name = "Entry Mode")]
        public string? EntryMode { get; set; }

        public int? Amount { get; set; }

        [Display(Name = "Type of Transaction")]
        public string? TypeOfTransaction { get; set; }

        [Display(Name = "Country of Transaction")]
        public string? CountryOfTransaction { get; set; }

        [Display(Name = "Shipping Address")]
        public string? ShippingAddress { get; set; }

        public string? Bank { get; set; }

        [Display(Name = "Type of Card")]
        public string? TypeOfCard { get; set; }

        public bool? Fraud { get; set; }

        // Navigation property
        public virtual Customers Customer { get; set; }
    }
}
