using MediatR;
using OrderService.Repositories.Abstractions;

namespace OrderService.UseCases.UserUseCases.Commands.EditUser;

public class EditUserCommandHandler : IRequestHandler<EditUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetUserByOriginalUserId(request.OriginalUserId);
        if (user == null) throw new Exception("User not found!");
        user.Email = request.Email;
        user.Username = request.UserName;
        await _unitOfWork.SaveChangesAsync();
    }
}
