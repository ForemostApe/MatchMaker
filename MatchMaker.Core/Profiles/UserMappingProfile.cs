using Mapster;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Profiles;

public class UserMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserDTO, UserEntity>()
            .Map(dest => dest.CreatedDate, src => DateTime.UtcNow)
            .Map(dest => dest.Role, src => "Guest");

        config.NewConfig<UserEntity, UserDTO>()
                .Map(dest => dest.UserId, src => src.ID)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.CreatedDate, src => src.CreatedDate)
                .Map(dest => dest.Role, src => src.Role);
    }
}
