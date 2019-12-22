using Xunit;
using Moq;

namespace pipeline.tests
{
    public class PipeObjectTests
    {
        [Fact]
        public void LowerPipe_InputOutput_VerifyNextInvoke()
        {
            var mock = new Mock<IPipeObject>();
            var lower = new Lower();
            // Set a mock object to the next pipe
            lower.NextPipe = mock.Object;

            string input = "HelLo";
            // Do the work
            lower.Invoke(input);

            //Output
            string expected = input.ToLower();

            // Check if the next pipe's invoke method is called with right parameter
            mock.Verify(i => i.Invoke(expected));

            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SplitPipe_InputOutput_VerifyNextInvoke()
        {
            var mock = new Mock<IPipeObject>();
            var split = new Split();

            split.NextPipe = mock.Object;
            string input = "a sentence with split requires";
            split.Invoke(input);

            var expected = input.Split();
            mock.Verify(i => i.Invoke(expected[0]));
            mock.Verify(i => i.Invoke(expected[1]));
            mock.Verify(i => i.Invoke(expected[2]));
            mock.Verify(i => i.Invoke(expected[3]));
            mock.Verify(i => i.Invoke(expected[4]));

            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public void IgnoreWhiteSpacesPipe_Input_VerifyNextInvoke()
        {
            var mock = new Mock<IPipeObject>();
            var whiteSpaces = new IgnoreWhiteSpaces();

            whiteSpaces.NextPipe = mock.Object;
            string input = "1";
            whiteSpaces.Invoke(input);

            mock.Verify(i => i.Invoke(input));

            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public void IgnoreWhiteSpacesPipe_Input_VerifyNoNextInvoke()
        {
            var mock = new Mock<IPipeObject>();
            var whiteSpaces = new IgnoreWhiteSpaces();

            whiteSpaces.NextPipe = mock.Object;
            string input = "   ";
            whiteSpaces.Invoke(input);

            mock.Verify(i => i.Invoke(input), Times.Never);

            mock.VerifyNoOtherCalls();
        }
    }
}