using Application.DataTransferObjects.User;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings;

public class MapProfiles:Profile
{
	public MapProfiles()
	{
		CreateMap<CreateUserDto, User>().ReverseMap()
            .ForMember(u => u.ConfirmPassword, op => op.Ignore());

		CreateMap<GetAllUsersDto,User>().ReverseMap();

		CreateMap<UpdateUserDto, User>().ReverseMap()
			.ForMember(u => u.ConfirmPassword, op => op.Ignore());

		CreateMap<GetByIdUserDto, User>().ReverseMap();

	}
}
