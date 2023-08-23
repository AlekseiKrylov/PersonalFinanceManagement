using AutoMapper;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Repositories;
using PersonalFinanceManagement.Domain.Interfaces.Services;

namespace PersonalFinanceManagement.BLL.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        protected virtual UserDTO GetDTO(User item) => _mapper.Map<UserDTO>(item);

        public async Task<bool> ExistByEmailAsync(string email, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null, empty, or contain only whitespace.");

            return await _userRepository.ExistByEmailAsync(email, cancel).ConfigureAwait(false);
        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null, empty, or contain only whitespace.");

            return await _userRepository.GetByEmailAsync(email, cancel).ConfigureAwait(false);
        }

        public async Task<User> GetUserByVerificationTokenAsync(string verificationToken, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(verificationToken))
                throw new ArgumentException("Verification token cannot be null, empty, or contain only whitespace.");

            return await _userRepository.GetByVerificationTokenAsync(verificationToken, cancel).ConfigureAwait(false);
        }

        public async Task<UserDTO> RegisterUserAsync(string email, string password, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(string.IsNullOrWhiteSpace(email)
                    ? $"{nameof(email)} cannot be null, empty, or contain only whitespace."
                    : $"{nameof(password)} cannot be null, empty, or contain only whitespace.");

            if (await ExistByEmailAsync(email, cancel).ConfigureAwait(false))
                throw new InvalidOperationException($"The email is already in use.");

            string hashedPassword = HashPassword(password);

            var user = new User
            {
                Email = email,
                PasswordHash = hashedPassword,
            };

            await _userRepository.AddAsync(user, cancel).ConfigureAwait(false);
            return GetDTO(user);
        }

        public async Task<bool> VerifyUserAsync(string verificationToken, CancellationToken cancel)
        {
            if (await GetUserByVerificationTokenAsync(verificationToken, cancel).ConfigureAwait(false) is not User user)
                return false;

            user.VerifiedAt = DateTime.Now;
            await _userRepository.UpdateAsync(user, cancel).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email, CancellationToken cancel)
        {
            if (await GetUserByEmailAsync(email, cancel).ConfigureAwait(false) is not User user)
                return false;

            user.ResetPasswordToken = Guid.NewGuid().ToString();
            user.ResetTokenExpires = DateTime.Now.AddHours(8);
            await _userRepository.UpdateAsync(user, cancel).ConfigureAwait(false);

            return true;
        }

        public async Task<bool?> ResetPasswordAsync(string email, string password, string resetToken, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(resetToken))
                throw new ArgumentException(string.IsNullOrWhiteSpace(email)
                    ? $"{nameof(email)} cannot be null, empty, or contain only whitespace."
                    : string.IsNullOrWhiteSpace(password)
                        ? $"{nameof(password)} cannot be null, empty, or contain only whitespace."
                        : $"{nameof(resetToken)} cannot be null, empty, or contain only whitespace.");

            if (await GetUserByEmailAsync(email, cancel).ConfigureAwait(false) is not User user)
                return null;

            if (user.ResetPasswordToken != resetToken || user.ResetTokenExpires < DateTime.Now)
                return false;

            user.PasswordHash = HashPassword(password);
            user.ResetPasswordToken = null;
            user.ResetTokenExpires = null;

            await _userRepository.UpdateAsync(user, cancel).ConfigureAwait(false);
            return true;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }
    }
}
