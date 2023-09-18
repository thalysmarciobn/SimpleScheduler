using System.Collections.Generic;
using System.Linq;

namespace SimpleScheduler.Scheduler
{
    public class JobList<T>
    {
        private readonly List<T> _list = new();
        private readonly object _lock = new();

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _list.Count;
                }
            }
        }

        public bool TryAdd(T item)
        {
            lock (_lock)
            {
                if (_list.Contains(item)) return false;
                _list.Add(item);
                return true;
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _list.Remove(item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
            }
        }

        public List<T> List
        {
            get
            {
                lock (_lock)
                {
                    return new List<T>(_list);
                }
            }
        }
    }
}