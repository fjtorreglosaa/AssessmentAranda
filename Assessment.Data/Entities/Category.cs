using Microsoft.AspNetCore.Identity;

namespace Assessment.Data.Entities
{
    public class Category
    {
        public Category()
        {
            Products = new List<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
