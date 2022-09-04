using Assessment.Logic.Dtos.ImageDtos;

namespace Assessment.Logic.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
