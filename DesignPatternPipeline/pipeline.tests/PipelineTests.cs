using System;
using Moq;
using Xunit;

namespace pipeline.tests
{
    public class PipelineTests
    {

        [Fact]
        public void AppendPipe_ToHead()
        {
            var mock = new Mock<IPipeObject>();
            var pipeline = new Pipeline();

            pipeline.Append(mock.Object);

            Assert.Equal(mock.Object, pipeline.Head);
        }

        [Fact]
        public void AppendPipe_ToHead_Multiple()
        {
            var mock1 = new Mock<IPipeObject>();
            var mock2 = new Mock<IPipeObject>();
            var mock3 = new Mock<IPipeObject>();
            var mock4 = new Mock<IPipeObject>();
            var pipeline = new Pipeline();

            pipeline.Append(mock1.Object)
                .Append(mock2.Object)
                .Append(mock3.Object)
                .Append(mock4.Object);

            Assert.Equal(mock1.Object, pipeline.Head);
        }

        [Fact]
        public void AppendPipe_Head_And_Tail()
        {
            var mock1 = new Mock<IPipeObject>();
            var mock2 = new Mock<IPipeObject>();
            var mock3 = new Mock<IPipeObject>();
            var mock4 = new Mock<IPipeObject>();
            var pipeline = new Pipeline();

            pipeline.Append(mock1.Object)
                .Append(mock2.Object)
                .Append(mock3.Object)
                .Append(mock4.Object);

            Assert.Equal(mock1.Object, pipeline.Head);
            Assert.Equal(mock4.Object, pipeline.Tail);
        }

        [Fact]
        public void AppendPipe_PointNextPipe()
        {
            // This test verifies that pipeobjects are connected with each other
            var mock1 = new Mock<IPipeObject>();
            var mock2 = new Mock<IPipeObject>();
            var mock3 = new Mock<IPipeObject>();
            var mock4 = new Mock<IPipeObject>();
            var pipeline = new Pipeline();

            pipeline.Append(mock1.Object)
                .Append(mock2.Object)
                .Append(mock3.Object)
                .Append(mock4.Object);

            mock1.VerifySet(i => i.NextPipe = mock2.Object);
            mock2.VerifySet(i => i.NextPipe = mock3.Object);
            mock3.VerifySet(i => i.NextPipe = mock4.Object);

            Assert.Equal(mock1.Object, pipeline.Head);
            Assert.Equal(mock4.Object, pipeline.Tail);
        }

        [Fact]
        public void PipelineStart()
        {
            // This test verify that Invoke method is called properly.
            var mock = new Mock<IPipeObject>();
            var pipeline = new Pipeline();
            pipeline.Append(mock.Object);
            
            object startState = new object();
            pipeline.Start(startState);

            mock.Verify(i => i.Invoke(startState));
        }
    }
}
