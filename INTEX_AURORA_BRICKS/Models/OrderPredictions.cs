using Microsoft.AspNetCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX_AURORA_BRICKS.Models
{
    public class OrderPredictions
    {
        [Key]
        public int PredictionId { get; set; }
        
        [ForeignKey("Orders")]
        public int TransactionId { get; set; }
        public Order Orders { get; set; }
        public string Prediction { get; set; }
    }
}
