using MediatR;
using RealEstate.SharedKernel;
using RealEstate.Contracts.DTOs;

namespace RealEstate.Application.Commands
{
    public record LoginCommand(string Username, string Password) : IRequest<Result<AuthDto>>;
}
