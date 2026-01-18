using System.Collections;

namespace VolcanoBlue.SampleApi.Tests.Api.Types
{
    public sealed class BlockingList<T> : ICollection<T>
    {
        private readonly List<T> _inner = [];
        private readonly Lock _lock = new();

        public int Count { get { lock(_lock){ return _inner.Count; }}}
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            using (_lock.EnterScope()) 
                _inner.Add(item);
        }

        public bool Contains(T item)
        {
            using (_lock.EnterScope())
                return _inner.Contains(item);
        }

        public bool Remove(T item)
        {
            using (_lock.EnterScope())
                return _inner.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            using(_lock.EnterScope())
                return _inner.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IReadOnlyCollection<T> GetSnapshot()
        {
            using (_lock.EnterScope())
                return _inner.AsReadOnly();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
    }
}
