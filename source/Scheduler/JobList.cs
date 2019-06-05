using System.Collections.Generic;
using System.Linq;

namespace SimpleScheduler.Scheduler
{
    public class JobList<T>
    {
        private readonly IList<T> _list = new List<T>();
        private readonly object _lock = new object();
        public int Count => _list.Count;

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


        public List<T> List()
        {
            lock (_lock)
            {
                return _list.ToList();
            }
        }
    }
}