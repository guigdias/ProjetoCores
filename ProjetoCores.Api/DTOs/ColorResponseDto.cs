namespace ProjetoCores.Api.DTOs;

public class ColorResponseDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Hex { get; set; } = null!;
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
    public double Cyan { get; set; }
    public double Magenta { get; set; }
    public double Yellow { get; set; }
    public double Key { get; set; }

}
