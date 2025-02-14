using System.ComponentModel.DataAnnotations;

namespace Jumpin.Models
{
    public class RouteRequest : Request
    {
        [Required]
        public UserRoute UserRoute { get; set; }

    }
}
