using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Yape.Application.Transaction.Commands;
using Yape.Application.Transaction.Handlers;
using Yape.Domain.Entities;
using Yape.Domain.Message;
using Yape.Domain.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Yape.UnitTest
{
    public class CreateTransactionHandlerTests
    {
        private readonly Mock<ILogger<CreateTransactionHandler>> _loggerMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IMessageProducer> _messageProducerMock;
        private readonly CreateTransactionHandler _handler;

        public CreateTransactionHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CreateTransactionHandler>>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _messageProducerMock = new Mock<IMessageProducer>();
            _handler = new CreateTransactionHandler(
                _loggerMock.Object,
                _transactionRepositoryMock.Object,
                _messageProducerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsTransactionId()
        {
            // Arrange
            var command = new CreateTransactionCommand
            {
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferTypeId = 1,
                Value = 1000
            };

            var transactionId = Guid.NewGuid();
            _transactionRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(t => t.TransactionExternalId = transactionId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(transactionId, result);
            _messageProducerMock.Verify(
                producer => producer.ProduceMessageAsync(It.IsAny<QueueMessage>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_NullRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));
        }
    }
}


