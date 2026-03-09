using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjetoCores.Domain.Entities;

public class Color
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; private set; }
    public string Name { get; private set; }
    public int Red { get; private set; }
    public int Green { get; private set; }
    public int Blue { get; private set; }
    public double Cyan { get; private set; }
    public double Magenta { get; private set; }
    public double Yellow { get; private set; }
    public double Key { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsMerged { get; private set; }
    public List<string> SourceColorsId { get; private set; } = new();

    // Construtor principal
    public Color(string name, int red, int green, int blue)
    {
        Name = name;
        Red = red;
        Green = green;
        Blue = blue;
        CalculateCmyk();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsMerged = false;
    }
    
    // Construtor da Cor Mesclada

    public static Color CreateMergedColor(string name, IEnumerable<Color> sourceColors)
    {
        var colors = sourceColors.ToList();
        var red = (int)colors.Average(c => c.Red);
        var green = (int)colors.Average(c => c.Green);
        var blue = (int)colors.Average(c => c.Blue);

        var mergedColor = new Color(name, red, green, blue)
        {
            IsMerged = true
        };

        mergedColor.SourceColorsId.AddRange(
        colors.Select(c => c.Id!)
       );

        return mergedColor;
    }

    // Método para atualização
    public void Update(string name, int red, int green, int blue)
    {
        Name = name;
        Red = red;
        Green = green;
        Blue = blue;
        CalculateCmyk();
        UpdatedAt = DateTime.UtcNow;
    }
    private void CalculateCmyk()
    {
        if (Red == 0 && Green == 0 && Blue == 0)
        {
            Cyan = 0;
            Magenta = 0;
            Yellow = 0;
            Key = 1;
            return;
        }

        double ConvertedRed = Red / 255.0;
        double ConertedGreen = Green / 255.0;
        double ConvertedBlue = Blue / 255.0;

        Key = 1 - Math.Max(ConvertedRed, Math.Max(ConertedGreen, ConvertedBlue));
        Cyan = Math.Round((1 - ConvertedRed - Key) / (1 - Key), 4);
        Magenta = Math.Round((1 - ConertedGreen - Key) / (1 - Key), 4);
        Yellow = Math.Round((1 - ConvertedBlue - Key) / (1 - Key), 4);
    }
 }
