using Application.Products;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings;

public class MapProfiles:Profile
{
	public MapProfiles()
	{
		CreateMap<CreateProductCommand,Product>().ReverseMap();
	}
}
