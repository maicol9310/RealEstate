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
    public class CreatePropertyImageHandlerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<CreatePropertyImageHandler>> _loggerMock;
        private Mock<IPropertyRepository> _propertyRepoMock;
        private Mock<IPropertyImageRepository> _propertyImageRepoMock;

        private CreatePropertyImageHandler _handler;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _fileServiceMock = new Mock<IFileService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreatePropertyImageHandler>>();
            _propertyRepoMock = new Mock<IPropertyRepository>();
            _propertyImageRepoMock = new Mock<IPropertyImageRepository>();

            _uowMock.Setup(u => u.Properties).Returns(_propertyRepoMock.Object);
            _uowMock.Setup(u => u.PropertyImage).Returns(_propertyImageRepoMock.Object);
            _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _handler = new CreatePropertyImageHandler(
                _uowMock.Object,
                _fileServiceMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_ValidRequest_ShouldReturnSuccess()
        {
            var propertyId = Guid.NewGuid();
            var property = new Property("Casa", "Address", 1000000, "C001", 2020, Guid.NewGuid());

            _propertyRepoMock.Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(property);

            _fileServiceMock.Setup(f => f.SaveBase64FileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync("imagePath.jpg");

            _propertyImageRepoMock.Setup(r => r.AddAsync(It.IsAny<PropertyImage>(), It.IsAny<CancellationToken>()))
                                  .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<PropertyImageDto>(It.IsAny<PropertyImage>()))
                       .Returns((PropertyImage img) => new PropertyImageDto
                       {
                           IdPropertyImage = img.IdPropertyImage
                       });

            var command = new CreatePropertyImageCommand(propertyId, "base64ImageString");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            _propertyImageRepoMock.Verify(r => r.AddAsync(It.IsAny<PropertyImage>(), It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_PropertyNotFound_ShouldReturnFailure()
        {
            var command = new CreatePropertyImageCommand(Guid.NewGuid(), "base64ImageString");

            _propertyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Property)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains("Property not found", result.Error);
            _propertyImageRepoMock.Verify(r => r.AddAsync(It.IsAny<PropertyImage>(), It.IsAny<CancellationToken>()), Times.Never);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Handle_FileServiceThrowsArgumentException_ShouldReturnFailure()
        {
            var propertyId = Guid.NewGuid();
            var property = new Property("Casa", "Address", 1000000, "C001", 2020, Guid.NewGuid());

            _propertyRepoMock.Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(property);

            _fileServiceMock.Setup(f => f.SaveBase64FileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new ArgumentException("Invalid base64"));

            var command = new CreatePropertyImageCommand(propertyId, "invalidBase64");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains("Invalid base64", result.Error);
            _propertyImageRepoMock.Verify(r => r.AddAsync(It.IsAny<PropertyImage>(), It.IsAny<CancellationToken>()), Times.Never);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
