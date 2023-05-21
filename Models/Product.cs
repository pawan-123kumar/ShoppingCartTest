using System.ComponentModel.DataAnnotations;

namespace ShoppingCartTest.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string Catagory { get; set; }
        [Required]

        public string? Description { get; set; }
        [Required]

        public int Price { get; set; }
        [Required]
        public int Quantity { get; set;}


    }
}
