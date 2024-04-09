namespace INTEX_AURORA_BRICKS.Models
{
    internal class ProductsViewModel
    {
        public List<Products> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}