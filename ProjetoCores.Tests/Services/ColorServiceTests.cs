using ProjetoCores.Domain.Services;
using ProjetoCores.Domain.Interfaces;
using ProjetoCores.Domain.Entities;
using FluentValidation;
using FluentAssertions;
using NSubstitute;
using FluentValidation.Results;
namespace ProjetoCores.Tests.Services;

public class ColorServiceTests
{
    private readonly IColorRepository _repositoryMock; // simula o repositório
    private readonly IValidator<Color> _validatorMock; // simula o validator
    private readonly ColorService _service; // instancia real do service, que deve ser testada

    public ColorServiceTests()
    {
        _repositoryMock = Substitute.For<IColorRepository>(); // criado repositorio falso, que não acessa o mongo
        _validatorMock = Substitute.For<IValidator<Color>>(); // validador falso, não depende do validator real
        _service = new ColorService(_repositoryMock, _validatorMock); // service instanciando os objetos falsos, para manter o isolamento do banco
    }

    [Fact] // indica que é um teste
    public async Task Create_WhenHexIsInvalid_ShouldThrowArgumentException()
    {
        //Arrange
        var name = "Blue";
        var hex = "INVALID";

        //Act
        Func<Task> act = async () => await _service.Create(name, hex);

        //Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
    [Fact]
    public async Task Create_ShouldCallRepositoryOnlyOnce()
    {
        //Arrange
        var name = "Blue";
        var hex = "#0000FF";

        _repositoryMock.Create(Arg.Any<Color>())
        .Returns(Task.CompletedTask);

        //Act
        await _service.Create(name, hex);

        //Assert
        await _repositoryMock.Received(1).Create(Arg.Any<Color>()); // Verificando se o repository foi chamado exatamente 1 vez
    }

    [Fact]
    public async Task Create_WhenHexIsValid_ShouldConvertHexToRgb()
    {
        //Arrange
        var name = "Blue";
        var hex = "#0000FF";

        _repositoryMock.Create(Arg.Any<Color>())
         .Returns(Task.CompletedTask);

        //Act
        var result = await _service.Create(name, hex);

        //Assert
        result.Rgb.Red.Should().Be(0);
        result.Rgb.Green.Should().Be(0);
        result.Rgb.Blue.Should().Be(255);
    }

    [Fact]
    public async Task FindById_WhenSendTheCorrectId_ShouldNotBeNull()
    {
        //Arrange
        var blue = new RgbColor(0, 0, 255);
        var color = new Color("Blue", blue);
        _repositoryMock.GetById("1") // Se o método GetById for acionado, procurando pelo Id 1, retorne a cor criada
        .Returns(color);

        //Act
        var result = await _service.GetColorOrThrow("1");

        //Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Blue");
        result.Rgb.Red.Should().Be(0);
        result.Rgb.Green.Should().Be(0);
        result.Rgb.Blue.Should().Be(255);

    }

    [Fact]
    public async Task UpdateColor_WhenColorDoesNotExist_ShouldThrowException()
    {
        _repositoryMock.GetById("1")
        .Returns((Color?)null);

        Func<Task> act = async () => await _service.UpdateFromHex("1", "#0000FF");

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task UpdateColor_WhenColorExists_ShouldUpdateColor()
    {
        var blue = new RgbColor(0, 0, 255);
        var color = new Color("Blue", blue);
        _repositoryMock.GetById("1")
        .Returns(color);

        _validatorMock.Validate(Arg.Any<Color>())
        .Returns(new FluentValidation.Results.ValidationResult());

        var newHex = "#ADD8E6";

        await _service.UpdateFromHex("1", newHex);

        await _repositoryMock.Received(1)
        .Update(Arg.Is<Color>(
         c => c.Name == "LightBlue" 
         && c.Rgb.Red == 173 
         && c.Rgb.Green == 216 
         && c.Rgb.Blue == 230)
        );
    }

    [Fact]
    public async Task Update_WhenOnlyNameChanges_ShouldUpdateName()
    {
        // Arrange
        var blue = new RgbColor(0, 0, 255);
        var color = new Color("Blue", blue);
        _repositoryMock.GetById("1").Returns(color);

        _validatorMock.Validate(Arg.Any<Color>())
            .Returns(new ValidationResult());

        // Act
        await _service.UpdateFromHex("1", "#0000FF");

        // Assert
        await _repositoryMock.Received(1)
            .Update(Arg.Is<Color>(c => c.Name == "NewBlue"));
    }

    [Fact]
    public async Task Update_WhenHexChanges_ShouldConvertAndUpdateRgb()
    {
        var blue = new RgbColor(0, 0, 255);
        var color = new Color("Blue", blue);
        _repositoryMock.GetById("1").Returns(color);

        _validatorMock.Validate(Arg.Any<Color>())
            .Returns(new ValidationResult());

        await _service.UpdateFromHex("1", "#FF0000");

        await _repositoryMock.Received(1)
            .Update(Arg.Is<Color>(c =>
                c.Rgb.Red == 255 &&
                c.Rgb.Green == 0 &&
                c.Rgb.Blue == 0
            ));
    }

    [Fact]
    public async Task DeleteColor_WhenColorExists_ShouldReturnTrue()
    {
        _repositoryMock.Delete("1")
        .Returns(true);

        var result = await _service.Delete("1");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteColor_ShouldCallRepositoryDelete()
    {
        _repositoryMock.Delete("1")
            .Returns(true);

        await _service.Delete("1");

        await _repositoryMock.Received(1)
            .Delete("1");
    }

    [Fact]
    public async Task DeleteColor_WhenColorDoesNotExists_ShouldReturnFalse()
    {
        _repositoryMock.Delete("1")
        .Returns(false);

        var result = await _service.Delete("1");

        result.Should().BeFalse();
    }

    [Fact]
    public async Task MergeColors_WhenIdsCountIsLessThanTwo_ShouldThrowValidationException()
    {
        //Arrange
        var red = new RgbColor(255, 0, 0); // cor criada
        var color = new Color("Red", red);
        var colors = new List<Color> { color }; // lista de cores criadas

        _repositoryMock.GetByIdsAsync(Arg.Any<List<string?>>()) // Quando o método GetByIdsAsync for chamado, retorne a lista de cores criada
        .Returns(colors); // Quando o método GetByIdsAsync for chamado, retorne a lista de cores criada

        var ids = new List<string?> { "8605831c28069aab13782ace" }; // Id fictício para a cor

        //Act
        Func<Task> act = async () => await _service.MergeColors(ids); // Tentar mesclar as cores usando o método MergeColors

        //Assert
        await act.Should().ThrowAsync<ValidationException>(); // Verificar se a ação de mesclar as cores lança uma exceção de validação, pois o número de cores é menor que 2
    }

    [Fact]
    public async Task MergeColors_WhenIdIsDoesNotExist_ShouldThrowValidationException()
    {
        //Arrange
        var red = new RgbColor(255, 0, 0); // Cor criada
        var color = new Color("Red", red);
        var colors = new List<Color> { color }; // Lista de cores, pois assim que está definido no service

        _repositoryMock.GetByIdsAsync(Arg.Any<List<string?>>()) // Quando o método GetByIdsAsync for chamado, retorne a lista de cores criada
        .Returns(colors); // Quando o método GetByIdsAsync for chamado, retorne a lista de cores criada

        var ids = new List<string?> { "8605831c28069aab13782ace", "71922517a85dfb28edf81e35" }; // passados 2 ids, com apenas uma cor criada

        //Act
        Func<Task> act = async () => await _service.MergeColors(ids); // Tentar mesclar as cores usando o método MergeColors

        //Assert
        await act.Should().ThrowAsync<ValidationException>(); // Verificar se a ação de mesclar as cores lança uma exceção de validação, pois foi passado apenas um id existente
    }

    [Fact]
    public async Task MergeColors_AverageRgbCalculate_ShouldReturnAverageValues()
    {
        //Arrange
        var red = new RgbColor(255, 0, 0);
        var color1 = new Color("Red", red);
        var green = new RgbColor(0, 255, 0);
        var color2 = new Color("Green", green);
        var colors = new List<Color> { color1, color2 }; // Lista de cores criadas

        _repositoryMock.GetByIdsAsync(Arg.Any<List<string?>>())
        .Returns(colors); // Procurar os ids das cores criadas

        _repositoryMock.CountMergedAsync()
        .Returns(1); // Setar o contador para 1

        _repositoryMock.Create(Arg.Any<Color>())
        .Returns(Task.CompletedTask); // Quando o método for acionado, apenas termine

        _validatorMock.Validate(Arg.Any<Color>())
         .Returns(new ValidationResult());// Validação

        var ids = new List<string?> { "640acc334d0d70c2865a5aef", "76a711db19e7ac37f7184acd" }; // ids válidos

        //Act
        var result = await _service.MergeColors(ids); // Chamada método de merge

        //Assert
        result.Rgb.Red.Should().Be(127); // valor esperado do red
        result.Rgb.Green.Should().Be(127); // valor esperado do green
    }

    [Fact]
    public async Task MergeColors_WhenColorsAreMerged_ShouldReturnMergedColor()
    {
        //Arrange
        var red = new RgbColor(255, 0, 0);
        var color1 = new Color("Red", red);
        var blue = new RgbColor(0, 0, 255);
        var color2 = new Color("Red", blue);
        var colors = new List<Color> { color1, color2 }; // Lista de cores, pois assim que está definido no service

        _repositoryMock.GetByIdsAsync(Arg.Any<List<string?>>()) // Quando o método GetByIdsAsync for chamado, retorne a lista de cores criada
        .Returns(colors);

        _repositoryMock.CountMergedAsync().Returns(0); // Quando o método CountMergedAsync for chamado, retorne 0, indicando que ainda não existem cores mescladas

        _repositoryMock.Create(Arg.Any<Color>())
        .Returns(Task.CompletedTask); // Quando o método Create for chamado, apenas termine a tarefa

        _validatorMock.Validate(Arg.Any<Color>())
        .Returns(new ValidationResult()); // Quando o método Validate for chamado, retorne um resultado de validação bem-sucedido

        var ids = new List<string?> { "71922517a85dfb28edf81e35", "8605831c28069aab13782ace" }; // Ids fictícios para as cores

        //Act
        var result = await _service.MergeColors(ids); // Mesclar as cores e obter o resultado

        //Assert
        result.Should().NotBeNull(); // Verificar se o resultado não é nulo

        await _repositoryMock.Received(1).Create(Arg.Any<Color>()); // Verificar se o método Create do repositório foi chamado exatamente uma vez para criar a cor mesclada
    }
}
