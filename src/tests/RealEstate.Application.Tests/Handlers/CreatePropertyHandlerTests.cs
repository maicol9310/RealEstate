using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstate.Application.Commands;
using RealEstate.Application.Handlers;
using RealEstate.Application.Interfaces;
using RealEstate.Contracts.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Tests.Handlers
{
    [TestFixture]
    public class CreatePropertyHandlerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IPropertyRepository> _propertyRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<ILogger<CreatePropertyHandler>> _loggerMock;

        private CreatePropertyHandler _handler;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _propertyRepoMock = new Mock<IPropertyRepository>();
            _mapperMock = new Mock<IMapper>();
            _fileServiceMock = new Mock<IFileService>();
            _loggerMock = new Mock<ILogger<CreatePropertyHandler>>();

            _uowMock.Setup(x => x.Properties).Returns(_propertyRepoMock.Object);

            _uowMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);

            _handler = new CreatePropertyHandler(
                _uowMock.Object,
                _fileServiceMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_ValidRequest_ShouldReturnSuccess()
        {
            var dtoResponse = new PropertyDto { Name = "Casa Bonita" };

            _mapperMock.Setup(m => m.Map<PropertyDto>(It.IsAny<Property>()))
                       .Returns(dtoResponse);

            var command = new CreatePropertyCommand(
                Name: "Casa Bonita",
                Address: "Calle 123",
                Price: 450000000,
                CodeInternal: "CP-01",
                Year: 2024,
                IdOwner: Guid.NewGuid(),
                ImageBase64: null
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);

            _propertyRepoMock.Verify(
                r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _uowMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task Handle_InvalidProperty_ShouldReturnFailure()
        {

            var command = new CreatePropertyCommand(
                Name: "Test House",
                Address: "Calle 9",
                Price: -50,  
                CodeInternal: "CP-ERR",
                Year: 2020,
                IdOwner: Guid.NewGuid(),
                ImageBase64: null
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);

            _propertyRepoMock.Verify(
                r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task Handle_WithBase64Image_ShouldSaveImage()
        {
            var command = new CreatePropertyCommand(
                Name: "Casa con Imagen",
                Address: "Calle Imagen",
                Price: 1000000,
                CodeInternal: "IMG-01",
                Year: 2023,
                IdOwner: Guid.NewGuid(),
                ImageBase64: "base64data"
            );

            _fileServiceMock.Setup(f => f.SaveBase64FileAsync("base64data", It.IsAny<CancellationToken>()))
                            .ReturnsAsync("images/prop1.jpg");

            _mapperMock.Setup(m => m.Map<PropertyDto>(It.IsAny<Property>()))
                       .Returns(new PropertyDto());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);

            _fileServiceMock.Verify(
                f => f.SaveBase64FileAsync("base64data", It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
