using System.Collections.Generic;
using System.Linq;

namespace AlgorithmForce.HeuristicSuite
{
    public class AStar<TStep>
        where TStep : IStep<TStep>
    {
        #region Fields

        private readonly IComparer<TStep> _c; 
        private readonly IEqualityComparer<TStep> _ec;

        #endregion

        #region Properties

        public IComparer<TStep> Comparer { get { return this._c; } }

        public IEqualityComparer<TStep> EqualityComparer { get { return this._ec; } }

        #endregion

        #region Constructor

        internal AStar()
            : this(Comparer<TStep>.Default, EqualityComparer<TStep>.Default)
        {
        }

        internal AStar(IEqualityComparer<TStep> ec)
            : this(Comparer<TStep>.Default, ec)
        {
        }

        internal AStar(IComparer<TStep> c)
            : this(c, EqualityComparer<TStep>.Default)
        {
        }

        internal AStar(IComparer<TStep> c, IEqualityComparer<TStep> ec)
        {
            this._c = c;
            this._ec = ec;
        }

        #endregion
        
        #region Methods

        public IList<TStep> Execute(TStep startAt, TStep goal)
        {
            var solutions = new List<TStep>();
            var open = new SortedSet<TStep>(this._c);
            var closed = new HashSet<TStep>(this._ec);
            
            open.Add(goal);

            while (open.Count > 0)
            {
                var current = open.Min;

                closed.Add(current);

                if (this._ec.Equals(current, goal))
                    return solutions;

                foreach (var next in current.GetNextSteps())
                {
                    if (next == null || !next.IsValidStep) continue;
                    if (closed.Contains(next)) continue;

                    solutions.Add(next);

                    var prior = open.FirstOrDefault(s => this._ec.Equals(s, next));
                    //                                         next has better score
                    if (prior == null || !prior.IsValidStep || this._c.Compare(next, prior) < 0)
                    {
                        if (prior == null || !prior.IsValidStep)
                            open.Remove(prior);

                        open.Add(next);
                    }
                }
            }

            return solutions;
        }

        #endregion
    }
}