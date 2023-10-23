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

        public Product()
        {
            //Id = Guid.NewGuid();    
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//this will only work (incremenent the value by 1) for an int
        public Guid Id { get; set; } //31F61C7D-61EE-484B-9B39-4FB5BBC2D0B5

        [Required]
        public string Name { get; set; }
        public string Description { get; set; } 

        public double Price { get; set; }   

        public int Stock { get; set; }

        public string? Image { get; set; }

       
        [ForeignKey(nameof(Category))] 
        public int CategoryFK { get; set; } //foreign key property
        public virtual Category Category { get; set; } //navigational property

        public double WholesalePrice { get; set; }
        public string? Supplier { get; set; }

    }
}
