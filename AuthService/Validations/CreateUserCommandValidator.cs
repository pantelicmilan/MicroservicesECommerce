using AuthService.Repositories.Abstractions;
using AuthService.Users.Commands.CreateUser;
using FluentValidation;

namespace AuthService.Validations;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(c => c.Email).EmailAddress().WithMessage("Invalid email.");
        RuleFor(c => c).MustAsync(async (user, _) =>
        {
            bool isUsernameAndEmailValid = await userRepository.IsUsernameAndEmailUniqueAsync(user.Username, user.Email);
            return isUsernameAndEmailValid;
        }).WithMessage("Email or username already exist");
        RuleFor(c => c).Must((user, _) => 
        {
            const int minUsernameLen = 5;
            const int maxUsernameLen = 40;
            const int minPasswordLen = 7;
            const int maxPasswordLen = 40;
            const int minNameLen = 5;
            const int maxNameLen = 40;

            var isUsernameLengthValid = user.Username.Length > minNameLen && user.Username.Length < maxNameLen ;
            var isPasswordLengthValid = user.Password.Length > minPasswordLen && user.Password.Length < maxPasswordLen;
            var isNameLengthValid = user.Name.Length > minNameLen && user.Name.Length < maxNameLen;

            if( isUsernameLengthValid && isPasswordLengthValid && isNameLengthValid)
            {
                return true;
            }
            return false;

        }).WithMessage("Invalid parameters length");
    }
}
