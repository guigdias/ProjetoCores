using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoCores.Domain.Entities;

public class RgbColor
{
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }

    public RgbColor(int red, int green, int blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public static RgbColor ConvertHexToRgb(string hex)
    {
        hex = hex.Replace("#", ""); // substituir # por um espaço em branco

        return new RgbColor
        (
           Convert.ToInt32(hex.Substring(0, 2), 16),
           Convert.ToInt32(hex.Substring(2, 2), 16),
           Convert.ToInt32(hex.Substring(4, 2), 16)
        );

    }
}
