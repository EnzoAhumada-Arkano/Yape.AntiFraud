using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Yape.Application.Antifraud.Commands;
using Yape.Application.Antifraud.Handlers;
using Yape.Domain.Entities;
using Yape.Domain.Message;
using Yape.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Yape.UnitTest
{
    public class ValidateTransactionHandlerTests
    {
        private readonly Mock<ILogger<ValidateTransactionHandler>> _loggerMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IMessageProducer> _messageProducerMock;
        private readonly ValidateTransactionHandler _handler;

        public ValidateTransactionHandlerTests()
        {
            _loggerMock = new Mock<ILogger<ValidateTransactionHandler>>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _messageProducerMock = new Mock<IMessageProducer>();
            _handler = new ValidateTransactionHandler(
                _loggerMock.Object,
                _transactionRepositoryMock.Object,
                _messageProducerMock.Object
            );
        }

        [Fact]
        public async Task Handle_TransactionNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new ValidateTransactionCommand { TransactionExternalId = Guid.NewGuid() };
            _transactionRepositoryMock
                .Setup(repo => repo.GetTransactionsByExternalIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Transaction)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _messageProducerMock.Verify(
                producer => producer.ProduceMessageAsync(It.IsAny<QueueMessage>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_ValidTransaction_ReturnsTrue()
        {
            // Arrange
            var command = new ValidateTransactionCommand { TransactionExternalId = Guid.NewGuid() };
            var transaction = new Transaction
            {
                TransactionExternalId = command.TransactionExternalId,
                Value = 1000,
                SourceAccountId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            _transactionRepositoryMock
                .Setup(repo => repo.GetTransactionsByExternalIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(transaction);

            _transactionRepositoryMock
                .Setup(repo => repo.GetTransactionsSenderByAccountIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Transaction>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _messageProducerMock.Verify(
                producer => producer.ProduceMessageAsync(It.IsAny<QueueMessage>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}

