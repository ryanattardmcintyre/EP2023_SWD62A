using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class ListProductViewModel
    {
        public Guid Id { get; set; } //31F61C7D-61EE-484B-9B39-4FB5BBC2D0B5

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public string? Image { get; set; }
        public string CategoryName { get; set; }

    }
}
