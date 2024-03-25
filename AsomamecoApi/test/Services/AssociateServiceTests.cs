using AutoFixture;
using FluentAssertions;
using Moq;
using src.Models;
using src.Repository;
using src.Services;
using src.Utils;

namespace test.Services
{
    public class AssociateServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IAssociateService _service;
        private readonly Mock<IAssociateRepository> _repositoryMock;

        public AssociateServiceTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IAssociateRepository>>();
            _service = new AssociateService(_repositoryMock.Object);
        }

        // Test Get All
        [Fact]
        public async Task GetAll_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var AssociatesMock = _fixture.CreateMany<Associate>(50).ToList();

            _repositoryMock.Setup(repo => repo.GetAll(1, 10, null, null)).ReturnsAsync(AssociatesMock);

            // Act
            var result = await _service.GetAll(1, 10, null, null).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<Associate>>();
            result.Should().BeEquivalentTo(AssociatesMock);
            _repositoryMock.Verify(repo => repo.GetAll(1, 10, null, null), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmpty_WhenNoDataFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetAll(1, 10, null, null)).ReturnsAsync(Enumerable.Empty<Associate>());

            // Act
            var result = await _service.GetAll(1, 10, null, null).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<Associate>>();
            result.Should().BeEmpty();
            _repositoryMock.Verify(repo => repo.GetAll(1, 10, null, null), Times.Once);
        }

        // Test Get Total
        [Fact]
        public async Task GetTotal_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var AssociatesMock = _fixture.CreateMany<Associate>(50).ToList();

            _repositoryMock.Setup(repo => repo.GetCount(null)).ReturnsAsync(AssociatesMock.Count);

            // Act
            var result = await _service.GetCount(null).ConfigureAwait(false);

            // Assert
            result.Should().Be(50); // 50
            _repositoryMock.Verify(repo => repo.GetCount(null), Times.Once);
        }

        // Test Get By Email
        [Fact]
        public async Task GetByEmail_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.GetByEmail(AssociateMock.Email)).ReturnsAsync(AssociateMock);

            // Act
            var result = await _service.GetByEmail(AssociateMock.Email).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Associate>();
            result.Should().BeEquivalentTo(AssociateMock);
            _repositoryMock.Verify(repo => repo.GetByEmail(AssociateMock.Email), Times.Once);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnNull_WhenNoDataFound()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.GetByEmail(AssociateMock.Email)).ReturnsAsync((Associate)null);

            // Act
            var result = await _service.GetByEmail(AssociateMock.Email).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.GetByEmail(AssociateMock.Email), Times.Once);
        }

        // Test Get By ID
        [Fact]
        public async Task GetByID_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.GetByID(AssociateMock.Id)).ReturnsAsync(AssociateMock);

            // Act
            var result = await _service.GetByID(AssociateMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Associate>();
            result.Should().BeEquivalentTo(AssociateMock);
            _repositoryMock.Verify(repo => repo.GetByID(AssociateMock.Id), Times.Once);
        }

        [Fact]
        public async Task GetByID_ShouldReturnNull_WhenNoDataFound()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.GetByID(AssociateMock.Id)).ReturnsAsync((Associate)null);

            // Act
            var result = await _service.GetByID(AssociateMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.GetByID(AssociateMock.Id), Times.Once);
        }

        // Test Create
        [Fact]
        public async Task Create_ShouldReturnData_WhenDataCreated()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.Create(AssociateMock)).ReturnsAsync(AssociateMock);

            // Act
            var result = await _service.Create(AssociateMock).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Associate>();
            result.Should().BeEquivalentTo(AssociateMock);
            _repositoryMock.Verify(repo => repo.Create(AssociateMock), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenDataNotCreated()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.Create(AssociateMock)).ReturnsAsync((Associate)null);

            // Act
            var result = await _service.Create(AssociateMock).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.Create(AssociateMock), Times.Once);
        }

        // Test Delete 
        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenDataDeleted()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();
            _repositoryMock.Setup(repo => repo.Delete(AssociateMock.Id)).ReturnsAsync(true);

            // Act
            var result = await _service.Delete(AssociateMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(repo => repo.Delete(AssociateMock.Id), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenDataNotDeleted()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.Delete(AssociateMock.Id)).ReturnsAsync(false);

            // Act
            var result = await _service.Delete(AssociateMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeFalse();
            _repositoryMock.Verify(repo => repo.Delete(AssociateMock.Id), Times.Once);
        }

        // Test Update
        [Fact]
        public async Task Update_ShouldReturnUpdatedData_WhenDataUpdated()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();
            var response = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.Update(AssociateMock.Id, AssociateMock)).ReturnsAsync(response);

            // Act
            var result = await _service.Update(AssociateMock.Id, AssociateMock).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Associate>();
            result.Should().BeEquivalentTo(response);
            _repositoryMock.Verify(repo => repo.Update(AssociateMock.Id, AssociateMock), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenDataNotUpdated()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.Update(AssociateMock.Id, AssociateMock)).ReturnsAsync((Associate)null);

            // Act
            var result = await _service.Update(AssociateMock.Id, AssociateMock).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.Update(AssociateMock.Id, AssociateMock), Times.Once);
        }

        // Test Change State
        [Fact]
        public async Task ChangeState_ShouldReturnUpdatedData_WhenDataUpdated()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();
            var response = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.ChangeState(AssociateMock.Id)).ReturnsAsync(response);

            // Act
            var result = await _service.ChangeState(AssociateMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Associate>();
            // response.IsActive should be different from AssociateMock.IsActive
            result.IsActive.Should().NotBe(AssociateMock.IsActive);
            _repositoryMock.Verify(repo => repo.ChangeState(AssociateMock.Id), Times.Once);
        }

        [Fact]
        public async Task ChangeState_ShouldReturnNull_WhenDataNotUpdated()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();

            _repositoryMock.Setup(repo => repo.ChangeState(AssociateMock.Id)).ReturnsAsync((Associate)null);

            // Act
            var result = await _service.ChangeState(AssociateMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.ChangeState(AssociateMock.Id), Times.Once);
        }
    }
}