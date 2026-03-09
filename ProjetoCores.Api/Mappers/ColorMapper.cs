using ProjetoCores.Api.DTOs;
using ProjetoCores.Domain.Entities;

namespace ProjetoCores.Api.Mappers;

public class ColorMapper
{
    public static ColorResponseDto ToResponseDto(Color color)
    {
        return new ColorResponseDto
        {
           Id = color.Id!,
           Name = color.Name,
           Red = color.Red,
           Green = color.Green,
           Blue = color.Blue,
           Cyan = color.Cyan,
           Magenta = color.Magenta,
           Yellow = color.Yellow,
           Key = color.Key,
           Hex = ConvertRgbToHex(color.Red, color.Green, color.Blue)
        };
    }
    private static string ConvertRgbToHex(int r, int g, int b)
    {
        return $"#{r:X2}{g:X2}{b:X2}";
    }
}
