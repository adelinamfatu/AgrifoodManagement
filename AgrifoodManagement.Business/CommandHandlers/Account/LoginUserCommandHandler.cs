using AgrifoodManagement.Business.Commands.Account;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgrifoodManagement.Business.CommandHandlers.Account
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public LoginUserCommandHandler(IApplicationDbContext context,
                                       IPasswordHasher<User> passwordHasher,
                                       IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<Result<LoginResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user == null)
                return Result<LoginResponseDto>.Failure("Invalid credentials");

            // Verify the password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
                return Result<LoginResponseDto>.Failure("Invalid credentials");

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var loginResponse = new LoginResponseDto
            {
                Token = tokenString,
                Expiration = tokenDescriptor.Expires.Value,
                UserType = user.UserType
            };

            return Result<LoginResponseDto>.Success(loginResponse);
        }
    }
}
