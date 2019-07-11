using System;

namespace pipeline
{
    public class ConsoleOutput : PipeObject
    {
        public override void Invoke(object state)
        {
            var item = (string)state;

            Console.WriteLine(item);
        }
    }

    public class IgnoreWhiteSpaces : PipeObject
    {
        public override void Invoke(object state)
        {
            var str = (string)state;
            
            if (!string.IsNullOrWhiteSpace(str))
                this.Next(str);
        }
    }

    public class Lower : PipeObject
    {
        public override void Invoke(object state)
        {
            var str = (string)state;
            
            this.Next(str.ToLower());
        }
    }

    public class Split : PipeObject
    {
        public override void Invoke(object state)
        {
            var str = (string)state;

            var split = str.Split();

            foreach (var item in split)
            {
                this.Next(item);
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Pipeline pipeline = new Pipeline();

            pipeline.Append(new Lower())
                .Append(new Split())
                .Append(new IgnoreWhiteSpaces())
                .Append(new ConsoleOutput());

            string line = "   Lorem    ipsum dolor sit amet, consectetur adipiscing   elit.  ";
            
            pipeline.Start(line);
        }
    }
}
