using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace INTEX_AURORA_BRICKS.Models
{
    public class LineItem
    {
        [Key]
        public int LineItemId { get; set; }

        [ForeignKey("Order")]
        public int? TransactionId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public int? Rating { get; set; }

        // Navigation property
        public virtual Order Order { get; set; }
    }
}
