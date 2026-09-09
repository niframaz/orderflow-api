using FluentValidation;
using MediatR;
using OrderFlow.Application.Common.Behaviors;

namespace OrderFlow.Application.Tests.Common.Behaviors;

public sealed class ValidationBehaviorTests
{
    private sealed record TestRequest(string Name) : IRequest<string>;

    private sealed class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    [Fact]
    public async Task Handle_WhenRequestIsInvalid_ShouldNotCallNext()
    {
        // Arrange
        var validators = new IValidator<TestRequest>[] { new TestRequestValidator() };
        var behavior = new ValidationBehavior<TestRequest, string>(validators);
        var request = new TestRequest("");
        var nextCalled = false;

        RequestHandlerDelegate<string> next = _ =>
        {
            nextCalled = true;
            return Task.FromResult("Handled");
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            behavior.Handle(request, next, CancellationToken.None));

        Assert.False(nextCalled);
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldCallNext()
    {
        // Arrange
        var validators = new IValidator<TestRequest>[] { new TestRequestValidator() };
        var behavior = new ValidationBehavior<TestRequest, string>(validators);
        var request = new TestRequest("Order");
        var nextCalled = false;

        RequestHandlerDelegate<string> next = _ =>
        {
            nextCalled = true;
            return Task.FromResult("Handled");
        };

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal("Handled", result);
    }
}
