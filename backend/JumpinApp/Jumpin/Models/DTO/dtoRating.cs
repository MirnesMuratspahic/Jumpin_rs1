namespace Jumpin.Models.DTO
{
    public class dtoRating
    {
        public string UserWritingEmail { get; set; }
        public string UsersRatingEmail {  get; set; } 
        public float Review {  get; set; }
        public string Comment { get; set; } = string.Empty ;
    }
}
