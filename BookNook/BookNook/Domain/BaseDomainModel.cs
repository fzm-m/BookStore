using System.ComponentModel.DataAnnotations;

namespace BookNook.Domain
{
    public abstract class BaseDomainModel
    {
        [Key]
        public int Id { get; set; }
    }
}
