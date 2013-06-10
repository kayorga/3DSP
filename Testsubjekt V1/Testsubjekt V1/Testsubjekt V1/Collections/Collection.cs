using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    abstract class Collection<T>
    {
        internal List<T> _content;
        public Collection(int cap)
        {
            _content = new List<T>(cap);
        }
        public abstract void clear();
        public abstract void generate();
        public T this[int i]
        {
            get
            {
                return _content[i];
            }
            set
            {
                _content[i] = value;
            }

        }
    }
}
