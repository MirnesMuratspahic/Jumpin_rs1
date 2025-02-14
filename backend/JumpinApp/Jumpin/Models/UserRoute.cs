using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Jumpin.Models
{
    public class UserRoute
    {
        [JsonIgnore] [Key]public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public TheRoute Route { get; set; }

    }
}
