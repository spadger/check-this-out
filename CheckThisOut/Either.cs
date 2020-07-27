namespace JonBates.CheckThisOut
{
    public readonly struct Either<TLeft, TRight> where TLeft: class where TRight : class
    {
        internal Either(TLeft left, TRight right)
        {
            LeftValue = left;
            RightValue = right;
        }

        public bool IsLeft => LeftValue != null;
        public bool IsRight => RightValue != null;

        public TLeft LeftValue { get; }
        public TRight RightValue { get; }

        public object BaseValue => LeftValue ?? (object) RightValue;

        public static Either<TLeft, TRight> Left(TLeft value)
        {
            return new Either<TLeft, TRight>(value, null);
        }
        
        public static Either<TLeft, TRight> Right(TRight value) 
        {
            return new Either<TLeft, TRight>(null, value);
        }
    }
}