using System.ComponentModel.DataAnnotations;

namespace BookNook.Client.Models
{
    public class ItemType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public int IsDelete { get; set; }
 
    }
}
