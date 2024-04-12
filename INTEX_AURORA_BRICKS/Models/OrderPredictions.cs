using Microsoft.AspNetCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX_AURORA_BRICKS.Models
{
    public class OrderPredictions
    {
        //[Key]
        //public int prediction_ID { get; set; }
        //[Required]
        //public int transaction_ID { get; set; }

        //[ForeignKey("transaction_ID")]
        public Order Orders { get; set; }
        public string prediction { get; set; }
    }
}
