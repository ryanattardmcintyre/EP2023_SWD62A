using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; } 

        public double Price { get; set; }   

        public int Stock { get; set; }

        public string Image { get; set; }

       
        [ForeignKey(nameof(Category))] 
        public int CategoryFK { get; set; } //foreign key property
        public Category Category { get; set; } //navigational property

    }
}
