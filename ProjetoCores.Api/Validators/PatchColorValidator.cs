using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using ProjetoCores.Api.DTOs;


namespace ProjetoCores.Api.Validators;

public class PatchColorValidator : AbstractValidator<JsonPatchDocument<UpdateColorDto>> // Arquivo criado para o patch validar do controller
{
    public PatchColorValidator()
    {
        RuleFor(x => x).NotNull();

        RuleForEach(x => x.Operations).ChildRules(op => {
            op.RuleFor(o => o.op).NotEmpty();
            op.RuleFor(o => o.path).NotEmpty();
        });
    }
}
