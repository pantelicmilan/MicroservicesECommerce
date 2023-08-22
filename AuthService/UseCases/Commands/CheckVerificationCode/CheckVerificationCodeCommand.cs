using MediatR;

namespace AuthService.Users.Commands.CheckVerificationCode;

public class CheckVerificationCodeCommand : IRequest
{
    public string VerificationCode { get; set; }
}
