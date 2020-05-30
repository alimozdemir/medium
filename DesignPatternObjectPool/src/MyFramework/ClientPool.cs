using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace MyFramework
{
    public class ClientPool
    {
        private static Lazy<ClientPool> instance
            = new Lazy<ClientPool>(() => new ClientPool());
        public static ClientPool Instance { get; } = instance.Value;
        public int Size { get { return _currentSize; } }
        public int TotalObject { get { return _counter; } }

        private const int defaultSize = 5;
        private ConcurrentBag<Client> _bag = new ConcurrentBag<Client>();
        private volatile int _currentSize;
        private volatile int _counter;
        private object _lockObject = new object();

        private ClientPool()
            : this(defaultSize)
        {
        }
        private ClientPool(int size)
        {
            _currentSize = size;
        }

        public Client AcquireObject()
        {
            if (!_bag.TryTake(out Client item))
            {
                lock (_lockObject)
                {
                    if (item == null)
                    {
                        if (_counter >= _currentSize)
                            // or throw an exception, or wait for an object to return.
                            return null;

                        item = new RequestClient();

                        // it could be Interlocked.Increment(_counter). Since, we have locked the section, I don't think we need that.
                        _counter++;

                    }
                }

            }

            return item;
        }

        public void ReleaseObject(Client item)
        {
            _bag.Add(item);
        }
        public void IncreaseSize()
        {
            // lets say, AcquireJob() is doing its job on line "if (_counter >= _currentSize)", 
            // and there are no resource and method will gonna return null pointer
            // at the same time, IncreaseSize() is called and it is increased, 
            // so above the if condition shouldn't return true at all, and method should create another resource.
            // therefore, we should lock this method with _lockObject. This will put the operations on a line.
            lock (_lockObject)
            {
                _currentSize++;
            }
        }
    }
}