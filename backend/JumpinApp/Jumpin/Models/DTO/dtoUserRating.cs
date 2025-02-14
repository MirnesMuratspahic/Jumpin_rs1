namespace Jumpin.Models.DTO
{
    public class dtoUserRating
    {
        public int Id { get; set; } 
        public string UserWritingEmail { get; set; } = string.Empty;
        public float RatingReview { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
    }
}
