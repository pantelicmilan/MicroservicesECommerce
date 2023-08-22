using AuthService.Entities;
using AuthService.Exceptions;
using AuthService.Repositories.Abstractions;
using FluentValidation;
using MediatR;
using AuthService.EventBus;
using MessagingHelper.Events;

namespace AuthService.Users.Commands.CreateUser;

internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly IEventBus _eventBus;

    public CreateUserCommandHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork, 
        IValidator<CreateUserCommand> validator,
        IEventBus eventBus
        )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _eventBus = eventBus;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        { 
            throw new InputValidationException("Error with validation");
        }

        var user = new User 
        {
            Email = request.Email, 
            Name = request.Name, 
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password), 
            Username = request.Username 
        };

        await _userRepository.CreateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        await _eventBus.PublishAsync<UserCreatedEvent>(new UserCreatedEvent
        {
            Email = user.Email,
            OriginalUserId = user.Id,
            Username = user.Username
        });
    }
}
