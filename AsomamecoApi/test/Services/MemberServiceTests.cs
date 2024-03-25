using AutoFixture;
using FluentAssertions;
using Moq;
using src.Models;
using src.Repository;
using src.Services;
using src.Utils;

namespace test.Services
{
    public class MemberServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IMemberService _service;
        private readonly Mock<IMemberRepository> _repositoryMock;

        public MemberServiceTests()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IMemberRepository>>();
            _service = new MemberService(_repositoryMock.Object);
        }

        // Test Get All
        [Fact]
        public async Task GetAll_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var membersMock = _fixture.CreateMany<Member>(50).ToList();

            _repositoryMock.Setup(repo => repo.GetAll(1, 10, null, null)).ReturnsAsync(membersMock);

            // Act
            var result = await _service.GetAll(1, 10, null, null).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<Member>>();
            result.Should().BeEquivalentTo(membersMock);
            _repositoryMock.Verify(repo => repo.GetAll(1, 10, null, null), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmpty_WhenNoDataFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetAll(1, 10, null, null)).ReturnsAsync(Enumerable.Empty<Member>());

            // Act
            var result = await _service.GetAll(1, 10, null, null).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<Member>>();
            result.Should().BeEmpty();
            _repositoryMock.Verify(repo => repo.GetAll(1, 10, null, null), Times.Once);
        }

        // Test Get Total
        [Fact]
        public async Task GetTotal_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var membersMock = _fixture.CreateMany<Member>(50).ToList();

            _repositoryMock.Setup(repo => repo.GetCount(null)).ReturnsAsync(membersMock.Count);

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
            var memberMock = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.GetByEmail(memberMock.Email)).ReturnsAsync(memberMock);

            // Act
            var result = await _service.GetByEmail(memberMock.Email).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Member>();
            result.Should().BeEquivalentTo(memberMock);
            _repositoryMock.Verify(repo => repo.GetByEmail(memberMock.Email), Times.Once);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnNull_WhenNoDataFound()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.GetByEmail(memberMock.Email)).ReturnsAsync((Member)null);

            // Act
            var result = await _service.GetByEmail(memberMock.Email).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.GetByEmail(memberMock.Email), Times.Once);
        }

        // Test Get By ID
        [Fact]
        public async Task GetByID_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.GetByID(memberMock.Id)).ReturnsAsync(memberMock);

            // Act
            var result = await _service.GetByID(memberMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Member>();
            result.Should().BeEquivalentTo(memberMock);
            _repositoryMock.Verify(repo => repo.GetByID(memberMock.Id), Times.Once);
        }

        [Fact]
        public async Task GetByID_ShouldReturnNull_WhenNoDataFound()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.GetByID(memberMock.Id)).ReturnsAsync((Member)null);

            // Act
            var result = await _service.GetByID(memberMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.GetByID(memberMock.Id), Times.Once);
        }

        // Test Create
        [Fact]
        public async Task Create_ShouldReturnData_WhenDataCreated()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.Create(memberMock)).ReturnsAsync(memberMock);

            // Act
            var result = await _service.Create(memberMock).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Member>();
            result.Should().BeEquivalentTo(memberMock);
            _repositoryMock.Verify(repo => repo.Create(memberMock), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenDataNotCreated()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.Create(memberMock)).ReturnsAsync((Member)null);

            // Act
            var result = await _service.Create(memberMock).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.Create(memberMock), Times.Once);
        }

        // Test Delete 
        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenDataDeleted()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();
            _repositoryMock.Setup(repo => repo.Delete(memberMock.Id)).ReturnsAsync(true);

            // Act
            var result = await _service.Delete(memberMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(repo => repo.Delete(memberMock.Id), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenDataNotDeleted()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.Delete(memberMock.Id)).ReturnsAsync(false);

            // Act
            var result = await _service.Delete(memberMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeFalse();
            _repositoryMock.Verify(repo => repo.Delete(memberMock.Id), Times.Once);
        }

        // Test Update
        [Fact]
        public async Task Update_ShouldReturnUpdatedData_WhenDataUpdated()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();
            var response = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.Update(memberMock.Id, memberMock)).ReturnsAsync(response);

            // Act
            var result = await _service.Update(memberMock.Id, memberMock).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Member>();
            result.Should().BeEquivalentTo(response);
            _repositoryMock.Verify(repo => repo.Update(memberMock.Id, memberMock), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenDataNotUpdated()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.Update(memberMock.Id, memberMock)).ReturnsAsync((Member)null);

            // Act
            var result = await _service.Update(memberMock.Id, memberMock).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.Update(memberMock.Id, memberMock), Times.Once);
        }

        // Test Change State
        [Fact]
        public async Task ChangeState_ShouldReturnUpdatedData_WhenDataUpdated()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();
            var response = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.ChangeState(memberMock.Id)).ReturnsAsync(response);

            // Act
            var result = await _service.ChangeState(memberMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Member>();
            // response.IsActive should be different from memberMock.IsActive
            result.IsActive.Should().NotBe(memberMock.IsActive);
            _repositoryMock.Verify(repo => repo.ChangeState(memberMock.Id), Times.Once);
        }

        [Fact]
        public async Task ChangeState_ShouldReturnNull_WhenDataNotUpdated()
        {
            // Arrange
            var memberMock = _fixture.Create<Member>();

            _repositoryMock.Setup(repo => repo.ChangeState(memberMock.Id)).ReturnsAsync((Member)null);

            // Act
            var result = await _service.ChangeState(memberMock.Id).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(repo => repo.ChangeState(memberMock.Id), Times.Once);
        }
    }
}