using AutoFixture;
using FluentAssertions;
using Moq;
using src.Models;
using src.Repository;
using src.Services;
using src.Utils;

namespace test.Services
{
    public class CateringServiceServiceTests
    {
        private readonly IFixture _fixture;
        private readonly ICateringServiceService _service;
        private readonly Mock<ICateringServiceRepository> _repositoryMock;

        public CateringServiceServiceTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<ICateringServiceRepository>>();
            _service = new CateringServiceService(_repositoryMock.Object);
        }

        // Test Get All
        [Fact]
        public async Task GetAll_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var cateringserviceMock = _fixture.CreateMany<CateringService>(50).ToList();

            _repositoryMock.Setup(repo => repo.GetAll(1, 10, null, null)).ReturnsAsync(cateringserviceMock);

            // Act
            var result = await _service.GetAll(1, 10,null, null).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<CateringService>>();
            result.Should().BeEquivalentTo(cateringserviceMock);
            _repositoryMock.Verify(repo => repo.GetAll(1, 10,null,null), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmpty_WhenNoDataFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetAll(1, 10,null,null)).ReturnsAsync(Enumerable.Empty<CateringService>());

            // Act
            var result = await _service.GetAll(1, 10, null, null).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<CateringService>>();
            result.Should().BeEmpty();
            _repositoryMock.Verify(repo => repo.GetAll(1, 10, null, null), Times.Once);
        }

        // Test Get Total
        [Fact]
        public async Task GetTotal_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var cateringserviceMock = _fixture.CreateMany<CateringService>(50).ToList();

            _repositoryMock.Setup(repo => repo.GetCount(null)).ReturnsAsync(cateringserviceMock.Count);

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
            var cateringserviceMock = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.GetByEmail(cateringserviceMock.Email)).ReturnsAsync(cateringserviceMock);

            // Act
            var result = await _service.GetByEmail(cateringserviceMock.Email).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CateringService>();
            result.Should().BeEquivalentTo(cateringserviceMock);
            _repositoryMock.Verify(repo => repo.GetByEmail(cateringserviceMock.Email), Times.Once);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnNull_WhenNoDataFound()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.GetByEmail(cateringserviceMock.Email)).ReturnsAsync((CateringService)null);

            // Act
            var result = await _service.GetByEmail(cateringserviceMock.Email).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.GetByEmail(cateringserviceMock.Email), Times.Once);
        }

        // Test Get By ID
        [Fact]
        public async Task GetByID_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.GetByID(cateringserviceMock.Id)).ReturnsAsync(cateringserviceMock);

            // Act
            var result = await _service.GetByID(cateringserviceMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CateringService>();
            result.Should().BeEquivalentTo(cateringserviceMock);
            _repositoryMock.Verify(repo => repo.GetByID(cateringserviceMock.Id), Times.Once);
        }

        [Fact]
        public async Task GetByID_ShouldReturnNull_WhenNoDataFound()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.GetByID(cateringserviceMock.Id)).ReturnsAsync((CateringService)null);

            // Act
            var result = await _service.GetByID(cateringserviceMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.GetByID(cateringserviceMock.Id), Times.Once);
        }

        // Test Create
        [Fact]
        public async Task Create_ShouldReturnData_WhenDataCreated()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.Create(cateringserviceMock)).ReturnsAsync(cateringserviceMock);

            // Act
            var result = await _service.Create(cateringserviceMock).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CateringService>();
            result.Should().BeEquivalentTo(cateringserviceMock);
            _repositoryMock.Verify(repo => repo.Create(cateringserviceMock), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenDataNotCreated()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.Create(cateringserviceMock)).ReturnsAsync((CateringService)null);

            // Act
            var result = await _service.Create(cateringserviceMock).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.Create(cateringserviceMock), Times.Once);
        }

        // Test Delete 
        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenDataDeleted()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();
            _repositoryMock.Setup(repo => repo.Delete(cateringserviceMock.Id)).ReturnsAsync(true);

            // Act
            var result = await _service.Delete(cateringserviceMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(repo => repo.Delete(cateringserviceMock.Id), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenDataNotDeleted()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.Delete(cateringserviceMock.Id)).ReturnsAsync(false);

            // Act
            var result = await _service.Delete(cateringserviceMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeFalse();
            _repositoryMock.Verify(repo => repo.Delete(cateringserviceMock.Id), Times.Once);
        }

        // Test Update
        [Fact]
        public async Task Update_ShouldReturnUpdatedData_WhenDataUpdated()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();
            var response = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.Update(cateringserviceMock.Id, cateringserviceMock)).ReturnsAsync(response);

            // Act
            var result = await _service.Update(cateringserviceMock.Id, cateringserviceMock).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CateringService>();
            result.Should().BeEquivalentTo(response);
            _repositoryMock.Verify(repo => repo.Update(cateringserviceMock.Id, cateringserviceMock), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenDataNotUpdated()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.Update(cateringserviceMock.Id, cateringserviceMock)).ReturnsAsync((CateringService)null);

            // Act
            var result = await _service.Update(cateringserviceMock.Id, cateringserviceMock).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.Update(cateringserviceMock.Id, cateringserviceMock), Times.Once);
        }

        // Test Change State
        [Fact]
        public async Task ChangeState_ShouldReturnUpdatedData_WhenDataUpdated()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();
            var response = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.ChangeState(cateringserviceMock.Id)).ReturnsAsync(response);

            // Act
            var result = await _service.ChangeState(cateringserviceMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CateringService>();
            // response.IsActive should be different from memberMock.IsActive
            result.IsActive.Should().NotBe(cateringserviceMock.IsActive);
            _repositoryMock.Verify(repo => repo.ChangeState(cateringserviceMock.Id), Times.Once);
        }

        [Fact]
        public async Task ChangeState_ShouldReturnNull_WhenDataNotUpdated()
        {
            // Arrange
            var cateringserviceMock = _fixture.Create<CateringService>();

            _repositoryMock.Setup(repo => repo.ChangeState(cateringserviceMock.Id)).ReturnsAsync((CateringService)null);

            // Act
            var result = await _service.ChangeState(cateringserviceMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.ChangeState(cateringserviceMock.Id), Times.Once);
        }
    }
}