using Microsoft.Extensions.Logging;
using Moq;
using RealEstate.Application.Commands;
using RealEstate.Application.Handlers;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Tests.Handlers
{
    [TestFixture]
    public class UpdatePropertyHandlerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IPropertyRepository> _propertyRepoMock;
        private Mock<ILogger<UpdatePropertyHandler>> _loggerMock;
        private UpdatePropertyHandler _handler;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _propertyRepoMock = new Mock<IPropertyRepository>();
            _loggerMock = new Mock<ILogger<UpdatePropertyHandler>>();

            _uowMock.Setup(u => u.Properties).Returns(_propertyRepoMock.Object);
            _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _handler = new UpdatePropertyHandler(_uowMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ShouldReturnSuccess()
        {
            var propertyId = Guid.NewGuid();
            var property = new Property("Casa", "Address", 1000000, "C001", 2020, Guid.NewGuid());

            _propertyRepoMock.Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(property);

            var command = new UpdatePropertyCommand(
                PropertyId: propertyId,
                Name: "Casa Actualizada",
                Address: "Calle 123",
                Price: 1200000,
                CodeInternal: "C002",
                Year: 2021,
                IdOwner: Guid.NewGuid()
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            _propertyRepoMock.Verify(r => r.Update(It.IsAny<Property>()), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_PropertyNotFound_ShouldReturnFailure()
        {
            _propertyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Property)null);

            var command = new UpdatePropertyCommand(
                PropertyId: Guid.NewGuid(),
                Name: "Dummy",
                Address: "Dummy",
                Price: 100,
                CodeInternal: "DUMMY",
                Year: 2025,
                IdOwner: Guid.NewGuid()
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains("Property not found", result.Error);
            _propertyRepoMock.Verify(r => r.Update(It.IsAny<Property>()), Times.Never);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

    }
}