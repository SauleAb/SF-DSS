using NUnit.Framework;
using Moq;
using System.IO.Ports;

namespace ArduinoTest.Tests
{
    [TestFixture]
    public class ArduinoControllerTests
    {
        private ArduinoController arduinoController;
        private Mock<SerialPort> mockSerialPort;

        [SetUp]
        public void Setup()
        {
            arduinoController = new ArduinoController();
            mockSerialPort = new Mock<SerialPort>();
        }

        [Test]
        public void WriteMessage_OpenSerialPortAndWriteMessage_Success()
        {
            // Arrange
            string message = "Im gonna hug a car";


            arduinoController.WriteMessage(message);
        }

        [Test]
        public void WriteMessage_SerialPortOpenFails_ThrowsException()
        {
            // Arrange
            string message = "Test Message";
            var exceptionMessage = "Serial port open failed";

            mockSerialPort.Setup(sp => sp.Open()).Throws(new Exception(exceptionMessage));

            using (mockSerialPort.Object)
            {
                // Act & Assert
                Assert.Throws<Exception>(() => arduinoController.WriteMessage(message), exceptionMessage);
            }
        }
    }
}
