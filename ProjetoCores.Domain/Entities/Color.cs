using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjetoCores.Domain.Entities;
namespace ProjetoCores.Domain.Entities;

public class Color
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; private set; }
    public string Name { get; private set; }
    public RgbColor Rgb { get; private set; }
    public CmykColor Cmyk { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsMerged { get; private set; }
    public List<string> SourceColorsId { get; private set; } = new();

    // Construtor principal
    public Color(string name, RgbColor rgb)
    {
        Name = name;
        Rgb = rgb;
        Cmyk = CmykColor.CalculateCmyk(rgb);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsMerged = false;
    }

    // Método de criação
    public static Color CreateColorFromHex(string name, string hex)
    {
        var rgb = RgbColor.ConvertHexToRgb(hex);

        return new Color(name, rgb);
    }

    // Método para atualização
    public void UpdateColorFromHex(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
            throw new ArgumentException("Invalid Hex");

        var rgb = RgbColor.ConvertHexToRgb(hex);

        Rgb = rgb;
        Cmyk = CmykColor.CalculateCmyk(rgb);
        UpdatedAt = DateTime.UtcNow;
    }
    // Construtor da Cor Mesclada

    public static Color CreateMergedColor(string name, IEnumerable<Color> sourceColors)
    {
        var colors = sourceColors.ToList();
        var red = (int)colors.Average(c => c.Rgb.Red);
        var green = (int)colors.Average(c => c.Rgb.Green);
        var blue = (int)colors.Average(c => c.Rgb.Blue);

        var rgb = new RgbColor(red, green, blue);
        
        var mergedColor = new Color(name, rgb)
        {
            IsMerged = true
        };

        mergedColor.SourceColorsId.AddRange(
        colors.Select(c => c.Id!)
       );

        return mergedColor;
    }

}