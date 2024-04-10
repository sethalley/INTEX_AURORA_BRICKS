using System.ComponentModel.DataAnnotations;

namespace INTEX_AURORA_BRICKS.Models
{
    public class ItemBasedRecommendations
    {
        [Key]
        public byte Product_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Product_Name { get; set; }

        public byte Recommendation_ID_1 { get; set; }

        [StringLength(100)]
        public string? Recommendation_Name_1 { get; set; }

        public byte? Recommendation_ID_2 { get; set; }

        [StringLength(100)]
        public string? Recommendation_Name_2 { get; set; }

        public byte? Recommendation_ID_3 { get; set; }

        [StringLength(100)]
        public string? Recommendation_Name_3 { get; set; }

        public byte? Recommendation_ID_4 { get; set; }

        [StringLength(100)]
        public string? Recommendation_Name_4 { get; set; }

        public byte? Recommendation_ID_5 { get; set; }

        [StringLength(100)]
        public string? Recommendation_Name_5 { get; set; }
    }
}