using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jumpin.Models
{
    public class UserRating
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UserWritingId")]
        public User UserWriting { get; set; }
        public int UserWritingId { get; set; } 

        [Required]
        [ForeignKey("UsersRatingId")]
        public User UsersRating { get; set; }
        public int UsersRatingId { get; set; }

        [Required]
        public Rating Rating { get; set; }
    }
}
