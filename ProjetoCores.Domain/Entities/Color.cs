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
        ValidateRgb(red, green, blue);

        Name = name;
        Red = red;
        Green = green;
        Blue = blue;

        CalculateCmyk();

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsMerged = false;
    }

    // Método para atualização
    public void Update(string name, int red, int green, int blue)
    {
        ValidateRgb(red, green, blue);

        Name = name;
        Red = red;
        Green = green;
        Blue = blue;

        CalculateCmyk();
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateRgb(int r, int g, int b)
    {
        if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
          throw new ArgumentException("RGB values must be between 0 and 255.");
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
        Cyan = (1 - ConvertedRed - Key) / (1 - Key);
        Magenta = (1 - ConertedGreen - Key) / (1 - Key);
        Yellow = (1 - ConvertedBlue - Key) / (1 - Key);
    }
}