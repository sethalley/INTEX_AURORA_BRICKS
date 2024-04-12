using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace INTEX_AURORA_BRICKS.Models
{
    public class Order
    {
        [Key]
        public int transaction_ID { get; set; }

        [ForeignKey("Customer")]
        public short customer_ID { get; set; }

        public DateTime date { get; set; }

        [Display(Name = "Day of Week")]
        public string? day_of_week { get; set; }

        public int? time { get; set; }

        [Display(Name = "Entry Mode")]
        public string? entry_mode { get; set; }

        public int? amount { get; set; }

        [Display(Name = "Type of Transaction")]
        public string? type_of_transaction { get; set; }

        [Display(Name = "Country of Transaction")]
        public string? country_of_transaction { get; set; }

        [Display(Name = "Shipping Address")]
        public string? shipping_address { get; set; }

        public string? bank { get; set; }

        [Display(Name = "Type of Card")]
        public string? type_of_card { get; set; }

        public int? fraud { get; set; }

        // Navigation property
        public virtual Customer Customer { get; set; }
    }
}
