using System;
using System.Collections.Generic;

namespace pipeline
{
    public interface IPipeObject
    {
        IPipeObject NextPipe { get; set; }
        void Next(object state);
        void Invoke(object state);
    }

    public abstract class PipeObject : IPipeObject
    {
        public IPipeObject NextPipe { get; set; }

        public void Next(object state)
        {
            this.NextPipe?.Invoke(state);
        }

        public abstract void Invoke(object state);
    }

    public class Pipeline
    {
        private IPipeObject _pipeStart, _pipeEnd;

        public virtual IPipeObject Head => _pipeStart;
        public virtual IPipeObject Tail => _pipeEnd;

        public Pipeline Append(IPipeObject pipe)
        {
            if (_pipeStart == null)
                _pipeStart = pipe;

            if (_pipeEnd != null)
                _pipeEnd.NextPipe = pipe;
            
            _pipeEnd = pipe;
            return this;
        }
        public void Start(object state = null)
        {
            _pipeStart.Invoke(state);
        }
    }
}