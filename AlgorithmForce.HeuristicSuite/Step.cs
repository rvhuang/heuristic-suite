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

        public virtual bool IsValidStep
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

        public override string ToString()
        {
            return string.Format("Key: {0} Depth: {1}", this.key, this.Depth);
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

        public override string ToString()
        {
            return string.Format("Key: {0} Value: {1} Depth: {2}", base.Key, this.value, base.Depth);
        }
    }

    public static class Step
    {
        public static Step<TKey> Create<TKey>(TKey key)
        {
            return new Step<TKey>(key);
        }

        public static Step<TKey> Create<TKey, TValue>(TKey key, TValue value)
        {
            return new Step<TKey, TValue>(key, value);
        }
    }
}