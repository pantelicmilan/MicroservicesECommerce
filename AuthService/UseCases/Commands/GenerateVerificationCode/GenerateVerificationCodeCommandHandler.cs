using AuthService.EventBus;
using AuthService.Repositories.Abstractions;
using AuthService.Services;
using AuthService.Services.Abstractions;
using MediatR;
using MessagingHelper.Events;
using JwtAuthLibrary;
namespace AuthService.Users.Commands.SendVerificationCode;

public class GenerateVerificationCodeCommandHandler : IRequestHandler<GenerateVerificationCodeCommand>
{
    private readonly IMemoryCachingHandler _cachingHandler;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IEventBus _eventBus;

    public GenerateVerificationCodeCommandHandler(
        IMemoryCachingHandler cachingHandler,
        IHttpContextAccessor contextAccessor,
        IUserRepository userRepository,
        IEventBus eventBus)
    {
        _cachingHandler = cachingHandler;
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(GenerateVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var token = SharedAuthHandler.GetJwtTokenFromHttpContext(_contextAccessor.HttpContext);
        int userId = SharedAuthHandler.GetIdClaimIfJwtTokenValid(token);
        var user = await _userRepository.GetUserByUserId(userId);
        if (user.MailVerified == true)
        {
            throw new Exception("You are already verified");
        }
        Guid guid = Guid.NewGuid();
        string hexString = guid.ToString("N");
        string finalVerificationCode = hexString.Substring(0, 6);

        _cachingHandler.SetCache<string>(CacheType.UserVerificationCode, finalVerificationCode, user.Id.ToString());

        await _eventBus.PublishAsync<VerficationCodeEvent>(
            new VerficationCodeEvent { 
                Email= user.Email, 
                Username = user.Username, 
                VerificationCode = finalVerificationCode
            }
            );

        Console.WriteLine("code string:");
        Console.WriteLine(finalVerificationCode);
    }
}
