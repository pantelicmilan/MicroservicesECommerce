using AuthService.EventBus;
using AuthService.Repositories.Abstractions;
using JwtAuthLibrary;
using MediatR;
using MessagingHelper.Events;

namespace AuthService.UseCases.Commands.DeleteUserById;

public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IHttpContextAccessor _contextAccessor;

    public DeleteUserByIdCommandHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork, 
        IEventBus eventBus,
        IHttpContextAccessor contextAccessor)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        var token = SharedAuthHandler.GetJwtTokenFromHttpContext(_contextAccessor.HttpContext);
        int userId = SharedAuthHandler.GetIdClaimIfJwtTokenValid(token);
        var user = await _userRepository.GetUserByUserId(userId);
        if (user == null) throw new Exception("User not found");
        _userRepository.DeleteUser(user);
        await _unitOfWork.SaveChangesAsync();
        await _eventBus.PublishAsync<UserDeletedEvent>(new UserDeletedEvent { 
            OriginalUserId = user.Id 
        });
    }
}
