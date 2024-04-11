namespace INTEX_AURORA_BRICKS.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Products
    {
        [Key]
        public short product_ID { get; set; }

        public string? name { get; set; }

        public int? year { get; set; }

        public int? num_parts { get; set; }

        public int? price { get; set; }

        public string? img_link { get; set; }

        public string? primary_color { get; set; }

        public string? secondary_color { get; set; }

        public string? description { get; set; }

        public string? category { get; set; }
    }

}
