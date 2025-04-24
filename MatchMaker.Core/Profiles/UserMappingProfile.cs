using Mapster;
using MatchMaker.Domain.DTOs.Users;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Profiles;

public class UserMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserDTO, User>()
            .Map(dest => dest.PasswordHash, src => src.Password)
            .Map(dest => dest.CreatedDate, src => DateTime.UtcNow)
            .Map(dest => dest.UserRole, src => "Guest");

        config.NewConfig<UpdateUserDTO, User>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.PasswordHash, src => src.PasswordHash)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.UserRole, src => src.UserRole);

        config.NewConfig<User, UpdateUserDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.UserRole, src => src.UserRole)
            .IgnoreNonMapped(true);


        config.NewConfig<User, UserDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.CreatedDate, src => src.CreatedDate)
            .Map(dest => dest.UserRole, src => src.UserRole)
            .IgnoreNonMapped(true);

        config.NewConfig<User, AuthenticatedUserDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.UserRole, src => src.UserRole)
            .IgnoreNonMapped(true);
    }
}
