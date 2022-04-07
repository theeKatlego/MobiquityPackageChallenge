using FluentAssertions;
using MobiquityPackageChallenge.Application.Packer;
using MobiquityPackageChallenge.Application.Test.Properties;
using System.Threading.Tasks;
using Xunit;

namespace MobiquityPackageChallenge.Application.Test.Packer
{
    public class PackHandlerTest
    {
        private PackHandler _Handler;

        public PackHandlerTest()
        {
            _Handler = new PackHandler(new Extractor());
        }

        [Fact]
        public async Task Hadle_WhenCalled_ReturnCorrrect8Async()
        {
            // Arrange
            var command = new PackCommand(Resources.example_input);

            var expected = @"4-
2,7
8,9";

            // Act
            var actual = await _Handler.Handle(command, default);

            // Assert
            actual.Should().Be(expected);

        }
    }
}
