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
    public class RegisterSaleCommandHandlerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<RegisterSaleCommandHandler>> _loggerMock;
        private Mock<IPropertyTraceRepository> _traceRepoMock;

        private RegisterSaleCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RegisterSaleCommandHandler>>();
            _traceRepoMock = new Mock<IPropertyTraceRepository>();

            _uowMock.Setup(u => u.PropertyTrace).Returns(_traceRepoMock.Object);
            _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _handler = new RegisterSaleCommandHandler(_uowMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ShouldReturnSuccess()
        {
            var command = new RegisterSaleCommand(
                dateSale: DateTime.Now,
                name: "Juan Perez",
                value: 500000m,
                tax: 50000m,
                idProperty: Guid.NewGuid()
            );

            _traceRepoMock.Setup(r => r.AddAsync(It.IsAny<PropertyTrace>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<PropertyTraceDto>(It.IsAny<PropertyTrace>()))
                       .Returns((PropertyTrace trace) => new PropertyTraceDto
                       {
                           IdPropertyTrace = trace.IdPropertyTrace,
                           Name = trace.Name,
                           DateSale = trace.DateSale,
                           Value = trace.Value,
                           Tax = trace.Tax
                       });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(command.name, result.Value.Name);
            _traceRepoMock.Verify(r => r.AddAsync(It.IsAny<PropertyTrace>(), It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}

