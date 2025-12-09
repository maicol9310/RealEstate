using Moq;
using RealEstate.Application.Commands;
using RealEstate.Application.Handlers;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace RealEstate.Application.Tests.Handlers
{
    [TestFixture]
    public class ChangePriceHandlerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IPropertyRepository> _propertyRepoMock;
        private Mock<ILogger<ChangePriceHandler>> _loggerMock;

        private ChangePriceHandler _handler;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _propertyRepoMock = new Mock<IPropertyRepository>();
            _loggerMock = new Mock<ILogger<ChangePriceHandler>>();

            _uowMock.Setup(x => x.Properties).Returns(_propertyRepoMock.Object);

            _uowMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);

            _handler = new ChangePriceHandler(
                _uowMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_PropertyNotFound_ShouldReturnFailure()
        {
            _propertyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Property)null);

            var command = new ChangePriceCommand(
                PropertyId: Guid.NewGuid(),
                NewPrice: 500000
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Property not found", result.Error);

            _propertyRepoMock.Verify(r => r.Update(It.IsAny<Property>()), Times.Never);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Handle_InvalidPrice_ShouldReturnFailure()
        {
            var propertyId = Guid.NewGuid();

            var prop = new Property("Casa", "Calle 1", 500000, "CP1", 2020, Guid.NewGuid());

            _propertyRepoMock
                .Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(prop);

            var command = new ChangePriceCommand(propertyId, -50);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);

            StringAssert.Contains("Price must be > 0", result.Error);

            _propertyRepoMock.Verify(r => r.Update(It.IsAny<Property>()), Times.Never);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Handle_ValidRequest_ShouldUpdateProperty()
        {
            var prop = new Property(
                name: "Casa",
                address: "Calle 123",
                price: 200000,
                codeInternal: "C01",
                year: 2022,
                idOwner: Guid.NewGuid()
            );

            _propertyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(prop);

            var command = new ChangePriceCommand(
                PropertyId: Guid.NewGuid(),
                NewPrice: 350000
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);

            _propertyRepoMock.Verify(r => r.Update(It.IsAny<Property>()), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(prop.Price, Is.EqualTo(350000)); 
        }
    }
}