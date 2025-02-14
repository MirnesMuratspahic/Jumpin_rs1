namespace Jumpin.Models.DTO
{
    public class dtoRequestAcceptOrDecline
    {
        public int RequestId { get; set; }
        public int Decision {  get; set; }
        public string RequestType { get; set; } = string.Empty;
    }
}
