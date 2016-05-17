using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ArrayTrie
{
    public interface IArrayTrie<T> : IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
        IArrayTrie<T> Push(T value);
        IArrayTrie<T> Pop(out T value);
    }
}
