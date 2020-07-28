namespace Infrastructure.Data
{
    public class ProductSeedModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public double Price { get; set; }
        public double NetPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductBrandId { get; set; }

    }
}