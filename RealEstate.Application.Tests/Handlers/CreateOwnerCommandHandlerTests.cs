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
    public class CreateOwnerCommandHandlerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<CreateOwnerCommandHandler>> _loggerMock;

        private Mock<IOwnerRepository> _ownerRepoMock;
        private CreateOwnerCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _fileServiceMock = new Mock<IFileService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateOwnerCommandHandler>>();
            _ownerRepoMock = new Mock<IOwnerRepository>();

            _uowMock.Setup(u => u.Owners).Returns(_ownerRepoMock.Object);
            _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _handler = new CreateOwnerCommandHandler(
                _uowMock.Object,
                _fileServiceMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_ValidRequest_ShouldReturnSuccess()
        {
            var command = new CreateOwnerCommand(
                Name: "John Doe",
                Address: "123 Main St",
                Base64Photo: null,
                Birthday: DateTime.Now.AddYears(-30)
            );

            _ownerRepoMock.Setup(r => r.AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<OwnerDto>(It.IsAny<Owner>()))
                       .Returns((Owner src) => new OwnerDto
                       {
                           IdOwner = src.IdOwner,
                           Name = src.Name,
                           Address = src.Address,
                           Photo = src.Photo,
                           Birthday = src.Birthday
                       });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(command.Name, result.Value.Name);
            _ownerRepoMock.Verify(r => r.AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_FileServiceThrows_ShouldReturnFailure()
        {
            _fileServiceMock.Setup(f => f.SaveBase64FileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new Exception("Disk error"));

            var command = new CreateOwnerCommand(
                Name: "John Doe",
                Address: "123 Main St",
                Base64Photo: "invalidBase64",
                Birthday: DateTime.Now.AddYears(-30)
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains("Error saving photo", result.Error);

            _ownerRepoMock.Verify(r => r.AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Never);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

    }
}
