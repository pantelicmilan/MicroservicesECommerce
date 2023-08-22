namespace MessagingHelper.Events;

public class VerficationCodeEvent
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string VerificationCode { get; set; }
}
