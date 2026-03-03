using FluentValidation;
using ProjetoCores.Api.DTOs;
using ProjetoCores.Api.Validators.Messages;

namespace ProjetoCores.Api.Validators
{
    public class UpdateColorDtoValidator : AbstractValidator<UpdateColorDto>
    {
        public UpdateColorDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Hex).NotEmpty()
           .WithMessage(ColorErrorMessages.HexRequired)
           .Matches("^#([0-9A-Fa-f]{6})$")
           .WithMessage(ColorErrorMessages.InvalidHexFormat);
        }
    }
}
