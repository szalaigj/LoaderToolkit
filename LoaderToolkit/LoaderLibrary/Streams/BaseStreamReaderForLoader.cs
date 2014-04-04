using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Streams
{
    public abstract class BaseStreamReaderForLoader<T> : IDisposable, IEnumerable<string>, IEnumerator<string>
    {
        private string current;

        private ISelector<T> selector;

        public ISelector<T> Selector
        {
            get { return selector; }
            set { selector = value; }
        }

        public BaseStreamReaderForLoader(ISelector<T> selector)
        {
            this.selector = selector;
        }

        public void Dispose()
        {
            Close();
        }

        // Its type is string because there is "\r\n" for Windows-style line end
        public abstract string LineDelimiterChar
        {
            get;
        }

        public string Current
        {
            get { return current; }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return current; }
        }

        public bool MoveNext()
        {
            current = ReadLine();
            return current != null;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        public abstract IEnumerable<T> selectObjects(Chunk chunk);

        public abstract string ReadLine();

        public abstract void InitializeInputStream(string path, bool overlapped, bool compressed);

        public abstract void InitializeInputStream(Stream stream);

        protected abstract void Close();
    }
}
