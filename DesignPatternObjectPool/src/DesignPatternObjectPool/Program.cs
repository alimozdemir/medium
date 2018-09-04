using System;
using System.Threading;
using System.Threading.Tasks;
using MyFramework;

namespace DesignPatternObjectPool
{
    class Program
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);
        static void Main(string[] args)
        {
            ParalleFor();
        }
        public static void ParalleFor()
        {
            // checking the thread-safety.
            Parallel.For(0, 100, (i, state) =>
            {
                Console.WriteLine($"Counter: {ObjectPool.Instance.TotalObject}/{ObjectPool.Instance.Size}");
                var obj = ObjectPool.Instance.AcquireObject();

                if (rnd.Next(0, 5) == 0)
                    ObjectPool.Instance.IncreaseSize();

                Thread.Sleep(rnd.Next(0, 100));

                if (obj != null)
                {
                    obj.Connect();
                    ObjectPool.Instance.ReleaseObject(obj);
                }
            });
        }
    }
}
