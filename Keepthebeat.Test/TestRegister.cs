using KeepTheBeat.Classes;
using KeepTheBeat.Interfaces;
using Moq;
using Xunit;

namespace Keepthebeat.Test
{
    public class RegisterValidatorTests
    {
        [Fact]
        public async Task ValidateFields_ShouldReturnTrue_WhenAllFieldsAreValid()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.IsEmailTaken(It.IsAny<string>())).ReturnsAsync(false);
            mockUserService.Setup(s => s.IsUsernameTaken(It.IsAny<string>())).ReturnsAsync(false);

            var validator = new RegisterValidator(mockUserService.Object);
            var firstname = "John";
            var lastname = "Doe";
            var username = "johndoe";
            var email = "john.doe@example.com";
            var password = "password123";

            // Act
            var result = await validator.ValidateFieldsAsync(firstname, lastname, username, email, password);

            // Assert
            Assert.True(result);
            Assert.Null(validator.FirstnameError);
            Assert.Null(validator.LastnameError);
            Assert.Null(validator.UsernameError);
            Assert.Null(validator.EmailError);
            Assert.Null(validator.PasswordError);
        }

        [Fact]
        public async Task ValidateFields_ShouldReturnFalse_WhenFirstnameIsEmpty()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var validator = new RegisterValidator(mockUserService.Object);
            var firstname = "";
            var lastname = "Doe";
            var username = "johndoe";
            var email = "john.doe@example.com";
            var password = "password123";

            // Act
            var result = await validator.ValidateFieldsAsync(firstname, lastname, username, email, password);

            // Assert
            Assert.False(result);
            Assert.NotNull(validator.FirstnameError);
            Assert.Equal("Firstname is required.", validator.FirstnameError);
        }

        [Fact]
        public async Task IsValidEmail_ShouldReturnFalse_WhenEmailIsInvalid()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var validator = new RegisterValidator(mockUserService.Object);
            var email = "invalid-email";

            // Act
            var result = validator.IsValidEmail(email);

            // Assert
            Assert.False(result);
            Assert.Equal("Invalid email format", validator.EmailError);
        }

        [Fact]
        public async Task IsValidEmail_ShouldReturnTrue_WhenEmailIsValid()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var validator = new RegisterValidator(mockUserService.Object);
            var email = "john.doe@example.com";

            // Act
            var result = validator.IsValidEmail(email);

            // Assert
            Assert.True(result);
            Assert.Null(validator.EmailError);
        }

        [Fact]
        public async Task IsValidPassword_ShouldReturnFalse_WhenPasswordIsShort()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var validator = new RegisterValidator(mockUserService.Object);
            var password = "short";

            // Act
            var result = validator.IsValidPassword(password);

            // Assert
            Assert.False(result);
            Assert.Equal("Password must be at least 8 characters long", validator.PasswordError);
        }

        [Fact]
        public async Task IsValidPassword_ShouldReturnTrue_WhenPasswordIsLongEnough()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var validator = new RegisterValidator(mockUserService.Object);
            var password = "password123";

            // Act
            var result = validator.IsValidPassword(password);

            // Assert
            Assert.True(result);
            Assert.Null(validator.PasswordError);
        }

        [Fact]
        public async Task ValidateFields_ShouldReturnFalse_WhenEmailIsTaken()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.IsEmailTaken(It.IsAny<string>())).ReturnsAsync(true);
            mockUserService.Setup(s => s.IsUsernameTaken(It.IsAny<string>())).ReturnsAsync(false);

            var validator = new RegisterValidator(mockUserService.Object);
            var firstname = "John";
            var lastname = "Doe";
            var username = "johndoe";
            var email = "john.doe@example.com";
            var password = "password123";

            // Act
            var result = await validator.ValidateFieldsAsync(firstname, lastname, username, email, password);

            // Assert
            Assert.False(result);
            Assert.Equal("Email is already taken.", validator.EmailError);
        }

        [Fact]
        public async Task ValidateFields_ShouldReturnFalse_WhenUsernameIsTaken()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.IsEmailTaken(It.IsAny<string>())).ReturnsAsync(false);
            mockUserService.Setup(s => s.IsUsernameTaken(It.IsAny<string>())).ReturnsAsync(true);

            var validator = new RegisterValidator(mockUserService.Object);
            var firstname = "John";
            var lastname = "Doe";
            var username = "johndoe";
            var email = "john.doe@example.com";
            var password = "password123";

            // Act
            var result = await validator.ValidateFieldsAsync(firstname, lastname, username, email, password);

            // Assert
            Assert.False(result);
            Assert.Equal("Username is already taken.", validator.UsernameError);
        }
    }
}
