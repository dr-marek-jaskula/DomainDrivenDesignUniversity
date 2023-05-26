using Shopway.Tests.Integration.ControllersUnderTest;
using Shopway.Domain.Errors;
using static Shopway.Application.Constants.ProblemDetailsConstants;

namespace Shopway.Tests.Integration.Utilities;

public static class ProblemDetailsUtilities
{
    /// <summary>
    /// Asserts problem details after the model validation. Specific error should be asserted separately.
    /// </summary>
    /// <param name="problemDetails">Deserialized problem details</param>
    public static void ShouldContain(this ValidationProblemDetails problemDetails, Error error)
    {
        problemDetails.Should().NotBeNull();
        problemDetails.Type.Should().Be(ValidationError);
        problemDetails.Status.Should().Be(400);
        problemDetails.Title.Should().Be(ValidationError);
        problemDetails.Detail.Should().Be(Error.ValidationError.Message);
        problemDetails!.Errors.Should().Contain(error);
    }

    /// <summary>
    /// Asserts problem details in case when the request model is invalid. For instance, when some field is null. Specific error should be asserted separately.
    /// </summary>
    /// <param name="problemDetails">Deserialized problem details</param>
    public static void ShouldHaveErrorCount(this ModelProblemDetails problemDetails, int errorCount)
    {
        problemDetails.Should().NotBeNull();
        problemDetails.Type.Should().Be(InvalidRequest);
        problemDetails.Status.Should().Be(400);
        problemDetails.Title.Should().Be(InvalidRequestTitle);
        problemDetails!.Errors.Should().HaveCount(errorCount);
    }
}