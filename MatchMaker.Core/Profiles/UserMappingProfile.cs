using Mapster;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Profiles;

public class UserMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserDTO, User>()
            .Map(dest => dest.CreatedDate, src => DateTime.UtcNow)
            .Map(dest => dest.UserRole, src => "Guest");

        config.NewConfig<UpdateUserDTO, User>()
                .Map(dest => dest.ID, src => src.UserId)
                .Map(dest => dest.PasswordHash, src => src.PasswordHash)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.UserRole, src => src.UserRole);

        config.NewConfig<User, UserDTO>()
                .Map(dest => dest.UserId, src => src.ID)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.CreatedDate, src => src.CreatedDate)
                .Map(dest => dest.UserRole, src => src.UserRole);
    }
}
