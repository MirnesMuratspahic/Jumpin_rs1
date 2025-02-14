using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jumpin.Models
{
    public class CodeStatus
    {
        [Key]public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool ErrorStatus { get; set; } = false;
        public DateTime DateTime { get; set; }
        public User User { get; set; } 


        public CodeStatus() { }
        public CodeStatus(string name, bool status, User user) 
        {
            Name = name;
            ErrorStatus = status;
            DateTime = DateTime.Now;
            User = user;
        }
    }
}
