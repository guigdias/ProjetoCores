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
        opt => opt.MapFrom(src => $"#{src.Rgb.Red:X2}{src.Rgb.Green:X2}{src.Rgb.Blue:X2}"))
        
        .ForMember(dest => dest.Red,
        opt => opt.MapFrom(src => src.Rgb.Red))
        .ForMember(dest => dest.Green,
        opt => opt.MapFrom(src => src.Rgb.Green))
        .ForMember(dest => dest.Blue,
        opt => opt.MapFrom(src => src.Rgb.Blue))

        .ForMember(dest => dest.Cyan,
        opt => opt.MapFrom(src => src.Cmyk.Cyan))
        .ForMember(dest => dest.Magenta,
        opt => opt.MapFrom(src => src.Cmyk.Magenta))
        .ForMember(dest => dest.Yellow,
        opt => opt.MapFrom(src => src.Cmyk.Yellow))
        .ForMember(dest => dest.Key,
        opt => opt.MapFrom(src => src.Cmyk.Key));
    }
}