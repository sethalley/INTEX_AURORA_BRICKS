using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace INTEX_AURORA_BRICKS.Models
{
    [Table("Orders")] // Specify the table name
    public class Order
    {
        [Key]
        [Column("transaction_ID")] // Specify the column name
        public int TransactionId { get; set; }

        [ForeignKey("Customer")]

        [Column("customer_ID")] // Specify the column name
        public short CustomerId { get; set; }

        [Column("date")] // Specify the column name
        public DateTime Date { get; set; }


        [Display(Name = "Day of Week")]
        [Column("day_of_week")] // Specify the column name
        public string? DayOfWeek { get; set; }

        [Column("time")] // Specify the column name
        public int? Time { get; set; }

        [Display(Name = "Entry Mode")]
        [Column("entry_mode")] // Specify the column name
        public string? EntryMode { get; set; }

        [Column("amount")] // Specify the column name
        public int? Amount { get; set; }

        [Display(Name = "Type of Transaction")]
        [Column("type_of_transaction")] // Specify the column name
        public string? TypeOfTransaction { get; set; }

        [Display(Name = "Country of Transaction")]
        [Column("country_of_transaction")] // Specify the column name
        public string? CountryOfTransaction { get; set; }

        [Display(Name = "Shipping Address")]
        [Column("shipping_address")] // Specify the column name
        public string? ShippingAddress { get; set; }

        [Column("bank")] // Specify the column name
        public string? Bank { get; set; }

        [Display(Name = "Type of Card")]
        [Column("type_of_card")] // Specify the column name
        public string? TypeOfCard { get; set; }

        [Column("fraud")] // Specify the column name
        public int? Fraud { get; set; }

        // Navigation property
        //public virtual HistoricCustomer HistoricCustomers { get; set; }
    }
}
