namespace ProjetoCores.Api.DTOs;

public class UpdateColorDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
}
