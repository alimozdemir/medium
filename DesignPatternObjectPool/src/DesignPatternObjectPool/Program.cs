using System;
using System.Collections.Generic;
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
            BasicExample();
            // ParalleFor();
        }

        static void BasicExample()
        {
            Console.WriteLine("Size of the pool {0}", ClientPool.Instance.Size);

            Console.WriteLine("Connecting with client1");
            var client1 = ClientPool.Instance.AcquireObject();

            client1.Connect();

            Console.WriteLine("Releasing the client");
            if (client1 != null)
                ClientPool.Instance.ReleaseObject(client1);

            var clients = new List<Client>();
            for (int i = 0; i < ClientPool.Instance.Size; i++)
            {
                clients.Add(ClientPool.Instance.AcquireObject());
            }

            Console.WriteLine("All avaliable clients are acquired.");

            var nullClient = ClientPool.Instance.AcquireObject();

            if (nullClient == null)
                Console.WriteLine("No more client is avaliable");

            Console.WriteLine("Increasing the size of the pool");
            ClientPool.Instance.IncreaseSize();

            Console.WriteLine("Acquiring a new client");
            var newClient = ClientPool.Instance.AcquireObject();
            newClient.Connect();

            Console.WriteLine("Releasing the new client");
            if (newClient != null)
                ClientPool.Instance.ReleaseObject(newClient);

            Console.WriteLine("Releasing all clients");

            foreach(var item in clients)
                ClientPool.Instance.ReleaseObject(item);
        }
        
        static void ParalleFor()
        {
            // checking the thread-safety.
            Parallel.For(0, 100, (i, state) =>
            {
                Console.WriteLine($"Counter: {ClientPool.Instance.TotalObject}/{ClientPool.Instance.Size}");
                var obj = ClientPool.Instance.AcquireObject();

                Thread.Sleep(rnd.Next(0, 100));

                if (obj != null)
                {
                    obj.Connect();
                    ClientPool.Instance.ReleaseObject(obj);
                }
            });
        }
    }
}
