using Api.Contracts;
using Api.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Api.Tests.Validation
{
    public class UserValidatorsTests
    {
        private readonly UserValidators _validator;

        public UserValidatorsTests()
        {
            _validator = new UserValidators();
        }

        [Fact]
        public void Should_Have_Error_When_Username_Is_Empty()
        {
            var dto = new UserCreateDto("", "Password1");
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("Le nom d'utilisateur est requis.");
        }

        [Fact]
        public void Should_Have_Error_When_Username_Is_Too_Short()
        {
            var dto = new UserCreateDto("abc", "Password1");
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("Le nom d'utilisateur doit contenir au moins 4 caractères.");
        }

        [Theory]
        [InlineData("")]             
        [InlineData("abc")]         
        [InlineData("abcdef")]      
        [InlineData("ABCDEF")]     
        [InlineData("Abcdef")]       
        [InlineData("123456")]      
        public void Should_Have_Error_When_Password_Invalid(string password)
        {
            var dto = new UserCreateDto("ValidUser", password);
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Username_And_Password_Are_Valid()
        {
            var dto = new UserCreateDto("ValidUser", "Password1");
            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
