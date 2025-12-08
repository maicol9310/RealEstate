using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Application.Commands;
using RealEstate.Application.Interfaces;
using RealEstate.SharedKernel;
using RealEstate.Contracts.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstate.Application.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<AuthDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(IUnitOfWork uow, IConfiguration config, ILogger<LoginHandler> logger)
        {
            _uow = uow;
            _config = config;
            _logger = logger;
        }

        public async Task<Result<AuthDto>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByUsernameAsync(request.Username, ct);

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),

                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                ),

                Audience = _config["Jwt:Audience"],
                Issuer = _config["Jwt:Issuer"]
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return Result<AuthDto>.Success(new AuthDto
            {
                Username = user.Username,
                PasswordHash = "*****",
                Token = handler.WriteToken(token)
            });
        }

    }
}
