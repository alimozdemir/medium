using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StreamSerializeAPI.Models
{
    public class ConcatenateModel<T>
    {
        public ConcurrentDictionary<int, T> Data { get; set; }
    }
}
