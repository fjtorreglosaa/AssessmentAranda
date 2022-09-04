namespace Assessment.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? ImageId { get; set; }
        public virtual Image? Image { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
