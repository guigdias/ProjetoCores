using FluentValidation;
using ProjetoCores.Api.DTOs;
using ProjetoCores.Api.Validators.Messages;

namespace ProjetoCores.Api.Validators
{
    public class MergeColorsDtoValidator : AbstractValidator<MergeColorsDto>
    {
        public MergeColorsDtoValidator() 
        {
            RuleFor(x => x.colorsIds)
            .NotNull()
            .WithMessage(ColorErrorMessages.NullMergeList)
            .Must(list => list.Count >= 2)
            .WithMessage(ColorErrorMessages.MergeListTooShort);

            RuleForEach(x => x.colorsIds)
            .NotEmpty()
            .WithMessage(ColorErrorMessages.EmptyMergeList)
            .Length(24)
            .WithMessage(ColorErrorMessages.InvalidIds)
            .Matches("^[a-fA-F0-9]{24}$")
            .WithMessage("Id must be a valida ObjectId");
        }
    }
}
