using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.HeuristicSuite
{
    public class SequenceEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        #region Fields

        public static readonly SequenceEqualityComparer<T> Default = new SequenceEqualityComparer<T>();

        private readonly IEqualityComparer<T> _ec;

        #endregion

        #region Properties

        public IEqualityComparer<T> ElementComparer
        {
            get { return this._ec; }
        }
        
        #endregion

        #region Constructor

        public SequenceEqualityComparer()
        {
            this._ec = EqualityComparer<T>.Default;
        }

        public SequenceEqualityComparer(IEqualityComparer<T> comparer)
        {
            this._ec = comparer == null ? EqualityComparer<T>.Default : comparer;
        }

        #endregion

        #region Methods

        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (x != null && y != null)
                return x.SequenceEqual(y, this._ec);
            else
                return false;
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            var r = 0;

            if (obj == null)
                return r;

            foreach (var h in obj.Select(GetElementHashCode))
                r = r ^ h;

            return r;
        }

        private int GetElementHashCode(T element, int index)
        {
            return index - this._ec.GetHashCode(element);
        }

        #endregion
    }
}
