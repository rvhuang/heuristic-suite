using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgorithmForce.Example.RubikCube
{
    using HeuristicSuite;

    public class CubeState : IStep<CubeState>, INextStepFactory<CubeState, CubeState>,
        IEquatable<CubeState>, IComparable<CubeState>
    {
        #region Fields

        public static readonly IEqualityComparer<CubeColor[][]> CubeEqualityComparer;
        
        public const int EdgeLength = 2; // A 2 x 2 size of face.

        public const int TopFaceIndex = 0;
        public const int ButtomFaceIndex = 1;
        public const int LeftFaceIndex = 2;
        public const int RightFaceIndex = 3;
        public const int FrontFaceIndex = 4;
        public const int BackFaceIndex = 5;

        private readonly CubeColor[][] faces = new CubeColor[6][];
        private readonly int hashCode;
        
        #endregion

        #region Properties

        int IStep<CubeState>.Depth
        {
            get; set;
        }

        bool IStep<CubeState>.IsValidStep
        {
            get { return true; }
        }

        CubeState IStep<CubeState>.Key
        {
            get { return this; }
        }

        IStep<CubeState> IStep<CubeState>.PreviousStep
        {
            get; set;
        }

        #endregion

        #region Constructors

        static CubeState()
        {
            CubeEqualityComparer = new SequenceEqualityComparer<CubeColor[]>(SequenceEqualityComparer<CubeColor>.Default);
        }

        public CubeState(IReadOnlyList<CubeColor[]> faces)
        {
            if (faces == null)
                throw new ArgumentNullException("faces");

            var faceSize = EdgeLength * EdgeLength;

            if (faces.Count != 6 || faces.Any(f => f == null || f.Length != faceSize))
                throw new ArgumentException(string.Format("Must be a {0} x 6 2-dimension array.", faceSize), "faces");
            
            this.faces = faces.Select(f => f.ToArray()).ToArray(); // create a copy
            this.hashCode = CubeEqualityComparer.GetHashCode(this.faces);
        }

        private CubeState(CubeColor[][] faces)
        {
            this.faces = faces; // create a copy
            this.hashCode = CubeEqualityComparer.GetHashCode(this.faces);
        }

        #endregion

        #region Methods
        
        public int CompareTo(CubeState other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(CubeState other)
        {
            if (other != null)
                return CubeEqualityComparer.Equals(this.faces, other.faces);
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CubeState);
        }

        public override int GetHashCode()
        {
            return this.hashCode;
        }

        #endregion

        #region INextStepFactory<CubeState, CubeState> Related

        IEnumerable<CubeState> INextStepFactory<CubeState, CubeState>.GetNextSteps()
        {
            for (var row = 0; row < EdgeLength; row++)
                yield return new CubeState(this.FlipLeft(row));

            for (var row = 0; row < EdgeLength; row++)
                yield return new CubeState(this.FlipRight(row));

            for (var col = 0; col < EdgeLength; col++)
                yield return new CubeState(this.FlipUp(col));

            for (var col = 0; col < EdgeLength; col++)
                yield return new CubeState(this.FlipDown(col));
        }

        private CubeColor[][] FlipLeft(int row)
        {
            var cloned = this.faces.Select(f => f.ToArray()).ToArray();

            // TODO:

            return cloned;
        }

        private CubeColor[][] FlipRight(int row)
        {
            var cloned = this.faces.Select(f => f.ToArray()).ToArray();

            // TODO:

            return cloned;
        }

        private CubeColor[][] FlipUp(int col)
        {
            var cloned = this.faces.Select(f => f.ToArray()).ToArray();

            // TODO:

            return cloned;
        }

        private CubeColor[][] FlipDown(int col)
        {
            var cloned = this.faces.Select(f => f.ToArray()).ToArray();

            // TODO:

            return cloned;
        }

        private static void Swap<T>(IList<T> array, int indexA, int indexB)
        {
            var temp = array[indexA];

            array[indexA] = array[indexB];
            array[indexB] = temp;
        }

        private static void Swap<T>(IList<T> arrayA, IList<T> arrayB, int index)
        {
            var temp = arrayA[index];

            arrayA[index] = arrayB[index];
            arrayB[index] = temp;
        }
        
        private static void Swap<T>(ref T itemA, ref T itemB)
        {
            var temp = itemA;

            itemA = itemB;
            itemB = temp;
        }

        #endregion
    }

    public enum CubeColor
    {
        White,

        Red,

        Blue,

        Orange,

        Green,

        Yellow
    }
}
