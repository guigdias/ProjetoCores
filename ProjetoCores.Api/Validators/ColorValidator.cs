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

        RuleFor(x => x.Red)
        .NotNull()
        .InclusiveBetween(0, 255).WithMessage(ColorErrorMessages.InvalidRed);

        RuleFor(x => x.Green)
        .NotNull()
        .InclusiveBetween(0, 255).WithMessage(ColorErrorMessages.InvalidGreen);

        RuleFor(x => x.Blue)
        .NotNull()
        .InclusiveBetween(0, 255).WithMessage(ColorErrorMessages.InvalidBlue);

    }
}
