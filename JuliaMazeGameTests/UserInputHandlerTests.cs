using System;
using System.Threading.Tasks;
using JuliaMazeGame.Models.Enums;
using JuliaMazeGame.Services;
using JuliaMazeGame.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace JuliaMazeGameTests
{
    [TestClass]
    public class UserInputHandlerTests
    {
        private UserInputHandler _userInputHandler;
        private Mock<IGameStateHandler> _gameStateHandlerMock;
        private Mock<IMazeRenderer> _mazeRendererMock;

        [TestInitialize]
        public void TestSetup()
        {
            _gameStateHandlerMock = new Mock<IGameStateHandler>();
            _mazeRendererMock = new Mock<IMazeRenderer>();
            _userInputHandler = new UserInputHandler(_gameStateHandlerMock.Object, _mazeRendererMock.Object);
        }

        [TestMethod]
        public async Task WhenConsoleKeyCodeIsLeft_MovePlayerCalledWithLeftDirection()
        {
            // Arrange
            _gameStateHandlerMock.Setup(x => x.IsRunning).Returns(true);
            var keyCode = 37;

            // Act
            await _userInputHandler.OnKeyPress(keyCode);
            //Assert
            _gameStateHandlerMock.Verify(x => x.MovePlayer(Direction.Left), Times.Once);
        }

        [TestMethod]
        public async Task WhenConsoleKeyCodeIsUpArrow_MovePlayerCalledWithUpDirection()
        {
            // Arrange
            _gameStateHandlerMock.Setup(x => x.IsRunning).Returns(true);
            var keyCode = 38;

            // Act
            await _userInputHandler.OnKeyPress(keyCode);
            //Assert
            _gameStateHandlerMock.Verify(x => x.MovePlayer(Direction.Up), Times.Once);
        }

        [TestMethod]
        public async Task WhenConsoleKeyCodeIsRightArrow_MovePlayerCalledWithRightDirection()
        {
            // Arrange
            _gameStateHandlerMock.Setup(x => x.IsRunning).Returns(true);
            var keyCode = 39;

            // Act
            await _userInputHandler.OnKeyPress(keyCode);
            //Assert
            _gameStateHandlerMock.Verify(x => x.MovePlayer(Direction.Right), Times.Once);
        }

        [TestMethod]
        public async Task WhenConsoleKeyCodeIsDownArrow_MovePlayerCalledWithDownDirection()
        {
            // Arrange
            _gameStateHandlerMock.Setup(x => x.IsRunning).Returns(true);
            var keyCode = 40;

            // Act
            await _userInputHandler.OnKeyPress(keyCode);
            //Assert
            _gameStateHandlerMock.Verify(x => x.MovePlayer(Direction.Down), Times.Once);
        }
    }
}