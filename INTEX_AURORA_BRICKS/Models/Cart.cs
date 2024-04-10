using System.ComponentModel.DataAnnotations;

namespace INTEX_AURORA_BRICKS.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public void AddItem(Products p, int quantity)
        {
            CartLine? line = Lines
                .Where(x => x.Products.product_ID == p.product_ID)
                .FirstOrDefault();

            // Has item been added to cart
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Products = p,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Products p) => Lines.RemoveAll(x => x.Products.product_ID == p.product_ID);

        public void UpdateQuantity(Products product, int quantity)
        {
            CartLine line = Lines.FirstOrDefault(x => x.Products.product_ID == product.product_ID);

            if (line != null)
            {
                line.Quantity = quantity;
            }
        }

        public void Clear() => Lines.Clear();

        /// CHECK THIS LINE "Products p" may or may not be necessary 
        public decimal CalculateTotal() => (decimal)Lines.Sum(x => x.Products.price * x.Quantity);
    
    public class CartLine
        {
            public int CartLineId { get; set; }
            public Products Products { get; set; }
            public int Quantity { get; set; }
        }
    }
}
