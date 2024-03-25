using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using src.Controllers;
using src.Models;
using src.Services;
using src.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Controllers
{
    public class AssociatesControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IAssociateService> _serviceMock;
        private readonly AssociatesController _controller;

        public AssociatesControllerTests()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IAssociateService>>();
            _controller = new AssociatesController(_serviceMock.Object);
        }

        // Test Get All
        [Fact]
        public async Task GetAll_ShouldReturnOkResponse_WhenDataFound()
        {
            // Arrange
            var AssociatesMock = _fixture.CreateMany<Associate>(50).ToList();

            _serviceMock.Setup(service => service.GetAll(1, 10, null, null)).ReturnsAsync(AssociatesMock);

            // Act
            var result = await _controller.GetAssociates().ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<PagedResult<Associate>>>();
            result.Result.Should().BeOfType<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull();
              
            _serviceMock.Verify(service => service.GetAll(1, 10, null, null), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnNoContent_WhenNoDataFound()
        {
            // Arrange
            _serviceMock.Setup(service => service.GetAll(1, 10, null, null)).ReturnsAsync((List<Associate>)null);

            // Act
            var result = await _controller.GetAssociates().ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(service => service.GetAll(1, 10, null, null), Times.Once);
        }


        [Fact]
        public async Task GetAll_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _serviceMock.Setup(service => service.GetAll(1, 10, null, null)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.GetAssociates().ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<ObjectResult>();
            result.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            _serviceMock.Verify(service => service.GetAll(1, 10, null, null), Times.Once);
        }

        // Test Get By ID
        [Fact]
        public async Task GetByID_ShouldReturnOkResponse_WhenValidInput()
        {
            // Arrange
            var AssociateMock = _fixture.Create<Associate>();
            var id = _fixture.Create<int>();
            _serviceMock.Setup(service => service.GetByID(id)).ReturnsAsync(AssociateMock);

            // Act
            var result = await _controller.GetAssociateById(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<Associate>>();
            result.Result.Should().BeOfType<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(AssociateMock.GetType());
            _serviceMock.Verify(service => service.GetByID(id), Times.Once);
        }

        [Fact]
        public async Task GetByID_ShouldReturnNoContent_WhenNoDataFound()
        {
            // Arrange
            Associate response = null;
            var id = _fixture.Create<int>();
            _serviceMock.Setup(service => service.GetByID(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAssociateById(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(service => service.GetByID(id), Times.Once);
        }

        [Fact]
        public async Task GetByID_ShouldReturnBadRequest_WhenInputIsZero()
        {
            // Arrange
            var id = 0;

            // Act
            var result = await _controller.GetAssociateById(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(service => service.GetByID(id), Times.Never);
        }

        [Fact]
        public async Task GetByID_ShouldReturnBadRequest_WhenInputIsLessThanZero()
        {
            // Arrange
            var id = -1;

            // Act
            var result = await _controller.GetAssociateById(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(service => service.GetByID(id), Times.Never);
        }

        //test exception
        [Fact]
        public async Task GetByID_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var id = _fixture.Create<int>();
            _serviceMock.Setup(service => service.GetByID(id)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.GetAssociateById(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<ObjectResult>();
            result.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            _serviceMock.Verify(service => service.GetByID(id), Times.Once);
        }

        // Test Create
        [Fact]
        public async Task Create_ShouldReturnCreatedResponse_WhenValidInput()
        {
            // Arrange
            var request = _fixture.Create<Associate>();
            var response = _fixture.Create<Associate>();
            _serviceMock.Setup(service => service.Create(request)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateAssociate(request).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<Associate>>();
            result.Result.Should().BeOfType<CreatedAtActionResult>();

            _serviceMock.Verify(service => service.Create(request), Times.Once);    
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var request = _fixture.Create<Associate>();
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.CreateAssociate(request).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(service => service.Create(request), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenInputIsNull()
        {
            // Arrange
            Associate request = null;

            // Act
            var result = await _controller.CreateAssociate(request).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(service => service.Create(request), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var request = _fixture.Create<Associate>();
            _serviceMock.Setup(service => service.Create(request)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.CreateAssociate(request).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<ObjectResult>();
            result.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            _serviceMock.Verify(service => service.Create(request), Times.Once);
        }

        // Test Update
        [Fact]
        public async Task Update_ShouldReturnAcceptedAtAction_WhenValidInput()
        {
            // Arrange
            var request = _fixture.Create<Associate>();
            var response = _fixture.Create<Associate>();
            _serviceMock.Setup(service => service.Update(request.Id, request)).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateAssociate(request.Id, request).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<Associate>>();
            result.Result.Should().BeOfType<AcceptedAtActionResult>();

            _serviceMock.Verify(service => service.Update(request.Id, request), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var request = _fixture.Create<Associate>();
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.UpdateAssociate(request.Id, request).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(service => service.Update(request.Id, request), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenInputIsNull()
        {
            // Arrange
            Associate request = null;

            // Act
            var result = await _controller.UpdateAssociate(1, request).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(service => service.Update(request.Id, request), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var request = _fixture.Create<Associate>();
            _serviceMock.Setup(service => service.Update(request.Id, request)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.UpdateAssociate(request.Id, request).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<ObjectResult>();
            result.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            _serviceMock.Verify(service => service.Update(request.Id, request), Times.Once);
        }

        // Test Change State
        [Fact]
        public async Task ChangeState_ShouldReturnAcceptedAtAction_WhenValidInput()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var response = _fixture.Create<Associate>();
            _serviceMock.Setup(service => service.ChangeState(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.ChangeState(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<Associate>>();
            result.Result.Should().BeOfType<AcceptedAtActionResult>();

            _serviceMock.Verify(service => service.ChangeState(id), Times.Once);
        }

        [Fact]
        public async Task ChangeState_ShouldReturnBadRequest_WhenInputIsZero()
        {
            // Arrange
            var id = 0;

            // Act
            var result = await _controller.ChangeState(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(service => service.ChangeState(id), Times.Never);
        }

        [Fact]
        public async Task ChangeState_ShouldReturnBadRequest_WhenInputIsLessThanZero()
        {
            // Arrange
            var id = -1;

            // Act
            var result = await _controller.ChangeState(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(service => service.ChangeState(id), Times.Never);
        }

        [Fact]
        public async Task ChangeState_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var id = _fixture.Create<int>();
            _serviceMock.Setup(service => service.ChangeState(id)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.ChangeState(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<ObjectResult>();
            result.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            _serviceMock.Verify(service => service.ChangeState(id), Times.Once);
        }

    }
}
