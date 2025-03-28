namespace Eventify.DTO
{
    public class UtenteConRuoliDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Ruoli { get; set; }
    }
}