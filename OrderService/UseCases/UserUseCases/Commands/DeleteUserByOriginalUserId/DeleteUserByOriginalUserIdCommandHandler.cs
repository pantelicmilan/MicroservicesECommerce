using MediatR;
using OrderService.Repositories.Abstractions;

namespace OrderService.UseCases.UserUseCases.Commands.DeleteUserByOriginalUserId;

public class DeleteUserByOriginalUserIdCommandHandler : IRequestHandler<DeleteUserByOriginalUserIdCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserByOriginalUserIdCommandHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteUserByOriginalUserIdCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetUserByOriginalUserId(request.OriginalUserId);
        if (user == null) throw new Exception("User not found");
        _userRepository.DeleteUser(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
