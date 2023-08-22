using AuthService.EventBus;
using AuthService.Repositories.Abstractions;
using JwtAuthLibrary;
using MediatR;
using MessagingHelper.Events;

namespace AuthService.UseCases.Commands.EditUser;

public class EditUserCommandHandler : IRequestHandler<EditUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IHttpContextAccessor _contextAccessor;

    public EditUserCommandHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IHttpContextAccessor contextAccessor
        )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        string jwtToken = SharedAuthHandler.GetJwtTokenFromHttpContext(_contextAccessor.HttpContext);
        int userId = SharedAuthHandler.GetIdClaimIfJwtTokenValid(jwtToken);
        var user = await _userRepository.GetUserByUserId(userId);
        if (user == null) throw new Exception("User not found!");
        if(user.Email != request.Email)
        {
            user.MailVerified = false;
        }
        user.Email = request.Email;
        user.Username = request.Username;
        if(request.Password != null)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.MailVerified = false;
        }
        user.Name = request.Name;
        await _unitOfWork.SaveChangesAsync();
        await _eventBus.PublishAsync<UserEditedEvent>(new UserEditedEvent
        {
            Email = user.Email,
            OriginalUserId = user.Id,
            UserName = user.Username
        });
    }
}
