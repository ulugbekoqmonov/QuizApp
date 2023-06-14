using Application.DataTransferObjects.User;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings;

public class MapProfiles:Profile
{
	public MapProfiles()
	{
		CreateMap<CreateUserDto,User>().ReverseMap();
	}
}
