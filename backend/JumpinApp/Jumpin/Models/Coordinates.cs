using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jumpin.Models
{
    public class Coordinates
    {
        [Key]public int Id { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
