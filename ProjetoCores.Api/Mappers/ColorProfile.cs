using AutoMapper;
using ProjetoCores.Domain.Entities;
using ProjetoCores.Api.DTOs;

namespace ProjetoCores.Api.Mappers;

public class ColorProfile : Profile
{
    public ColorProfile()
    {
        CreateMap<Color, ColorResponseDto>()
            .ForMember(dest => dest.Hex,
                opt => opt.MapFrom(src => $"#{src.Rgb.Red:X2}{src.Rgb.Green:X2}{src.Rgb.Blue:X2}"));
    }
}