using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoCores.Domain.Entities;
public class CmykColor
{
    public double Cyan { get; set; }
    public double Magenta { get; set; }
    public double Yellow { get; set; }
    public double Key { get; set; }

    public CmykColor(double cyan, double magenta, double yellow, double key)
    {
        Cyan = cyan;
        Magenta = magenta;
        Yellow = yellow;
        Key = key;
    }

    public static CmykColor CalculateCmyk(RgbColor rgb)
    {
        if (rgb.Red == 0 && rgb.Green == 0 && rgb.Blue == 0)
        return new CmykColor(0, 0 ,0, 1);
        

        double ConvertedRed = rgb.Red / 255.0;
        double ConvertedGreen = rgb.Green / 255.0;
        double ConvertedBlue = rgb.Blue / 255.0;
        double Key = 1 - Math.Max(ConvertedRed, Math.Max(ConvertedGreen, ConvertedBlue));
        return new CmykColor
        (
            Math.Round((1 - ConvertedRed - Key) / (1 - Key), 4),
            Math.Round((1 - ConvertedGreen - Key) / (1 - Key), 4),
            Math.Round((1 - ConvertedBlue - Key) / (1 - Key), 4),
            Key   
        );
    }
}