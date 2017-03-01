using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.Example.TwoZeroFourEight
{
    /// <summary>
    /// A wrapper that enables user to access specific column in a two-dimension array as a collection.
    /// </summary>
    /// <typeparam name="T">The type of element.</typeparam>
    public class ColumnWrapper<T> : IList<T>, IReadOnlyList<T>
    {
        private readonly T[][] array;
        private readonly int col;

        public ColumnWrapper(T[][] array, int col)
        {
            this.array = array;
            this.col = col;
        }

        public T this[int index]
        {
            get { return this.array[index][col]; }
            set { this.array[index][col] = value; }
        }

        public int Count
        {
            get { return this.array.Length; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Contains(T item)
        {
            return Enumerable.Range(0, this.array.Length).Select(i => this.array[i][col]).Contains(item);
        }

        public int IndexOf(T item)
        {
            var ec = EqualityComparer<T>.Default;

            foreach (var i in Enumerable.Range(0, this.array.Length))
                if (ec.Equals(this.array[i][col], item))
                    return i;

            return -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.ToArray().CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerable.Range(0, this.array.Length).Select(i => this.array[i][col]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Not Supported Methods

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
