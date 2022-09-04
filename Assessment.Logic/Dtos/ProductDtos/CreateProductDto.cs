using Assessment.Logic.Dtos.ImageDtos;

namespace Assessment.Logic.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public CreateImageDto? Image { get; set; }
        public int CategoryId { get; set; }
    }
}
