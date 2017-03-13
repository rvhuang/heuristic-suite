using System;

namespace AlgorithmForce.HeuristicSuite
{
    public class Step<TKey> : IStep<TKey>
    {
        private readonly TKey key;

        public int Depth
        {
            get; set;
        }

        public IStep<TKey> PreviousStep
        {
            get; set;
        }

        public bool IsValidStep
        {
            get { return true; }
        }

        public TKey Key
        {
            get { return this.key; }
        }

        public Step(TKey key)
        {
            if (key == null) throw new ArgumentNullException("key");

            this.key = key;
        }
    }

    public class Step<TKey, TValue> : Step<TKey>
    {
        private readonly TValue value;

        public TValue Value
        {
            get { return this.value; }
        }

        public Step(TKey key, TValue value)
            : base(key)
        {
            this.value = value;
        }
    }
}