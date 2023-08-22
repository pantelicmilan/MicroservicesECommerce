using MediatR;
using OrderService.Repositories.Abstractions;
using OrderService.Entities;

namespace OrderService.UseCases.UserUseCases.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork, 
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _userRepository.CreateUser(new User { 
            Email=request.Email ,
            OriginalUserId=request.OriginalUserId, 
            Username=request.Username 
        });
        await _unitOfWork.SaveChangesAsync();
    }
}
