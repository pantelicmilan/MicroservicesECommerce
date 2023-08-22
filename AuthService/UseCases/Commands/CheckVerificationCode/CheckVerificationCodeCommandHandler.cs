using AuthService.Services.Abstractions;
using MediatR;
using AuthService.Services;
using AuthService.Repositories.Abstractions;
using JwtAuthLibrary;

namespace AuthService.Users.Commands.CheckVerificationCode;

public class CheckVerificationCodeCommandHandler : IRequestHandler<CheckVerificationCodeCommand>
{
    private readonly IMemoryCachingHandler _cachingHandler;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckVerificationCodeCommandHandler(
        IMemoryCachingHandler cachingHandler, 
        IHttpContextAccessor contextAccessor,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _cachingHandler = cachingHandler;
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CheckVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null) throw new Exception("Can not catch HttpContext");
        var userId = SharedAuthHandler.GetClaimValueByClaimKey(httpContext, "id");
        var user = await _userRepository.GetUserByUserId(Convert.ToInt32(userId));
        if (user == null) throw new Exception("User with this id not found!");
        if (user.MailVerified == true) throw new Exception("User already verirfied");
        var verificationCodeValue = _cachingHandler.GetCache<string>(CacheType.UserVerificationCode, userId);
        if (verificationCodeValue == null) throw new Exception("You not start verification or verification code expired, resend code");
        if (request.VerificationCode != verificationCodeValue) throw new Exception("Verification code not matching");
        _cachingHandler.DeleteCache(CacheType.UserVerificationCode, userId);
        user.MailVerified = true;
        _userRepository.EditUser(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
