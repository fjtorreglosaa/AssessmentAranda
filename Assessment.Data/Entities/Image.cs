namespace Assessment.Data.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string ImageType { get; set; }
        public Product Product { get; set; }
    }
}
