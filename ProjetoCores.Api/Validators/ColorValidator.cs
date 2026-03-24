using FluentValidation;
using ProjetoCores.Api.Validators.Messages;
using ProjetoCores.Domain.Entities;

namespace ProjetoCores.Api.Validators;
public class ColorValidator : AbstractValidator<Color>
{
    public ColorValidator()
    {

        RuleFor(x => x.Name)
        .NotEmpty();

        RuleFor(x => x.Rgb.Red)
        .NotNull()
        .InclusiveBetween(0, 255).WithMessage(ColorErrorMessages.InvalidRed);

        RuleFor(x => x.Rgb.Green)
        .NotNull()
        .InclusiveBetween(0, 255).WithMessage(ColorErrorMessages.InvalidGreen);

        RuleFor(x => x.Rgb.Blue)
        .NotNull()
        .InclusiveBetween(0, 255).WithMessage(ColorErrorMessages.InvalidBlue);

    }
}
