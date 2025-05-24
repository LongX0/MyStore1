using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
