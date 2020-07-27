using System;
using System.Collections.Generic;

namespace JonBates.CheckThisOut.Core
{
    public readonly struct Either<TLeft, TRight> where TLeft: class where TRight : class
    {
        internal Either(TLeft left, TRight right)
        {
            _leftValue = left;
            _rightValue = right;
        }

        private readonly TLeft _leftValue;
        private readonly TRight _rightValue;

        public bool IsLeft => _leftValue != null;
        public bool IsRight => _rightValue != null;

        public TLeft LeftValue
        {
            get
            {
                if (IsRight)
                {
                    throw new InvalidOperationException("Cannot get left-value of a right instance");
                }

                return _leftValue;
            }
        }

        public TRight RightValue
        {
            get
            {
                if (IsLeft)
                {
                    throw new InvalidOperationException("Cannot get right-value of a left instance");
                }

                return _rightValue;
            }
        }

        public static Either<TLeft, TRight> Left(TLeft value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return new Either<TLeft, TRight>(value, null);
        }
        
        public static Either<TLeft, TRight> Right(TRight value) 
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return new Either<TLeft, TRight>(null, value);
        }

        public bool Equals(Either<TLeft, TRight> other)
        {
            return EqualityComparer<TLeft>.Default.Equals(_leftValue, other._leftValue) && EqualityComparer<TRight>.Default.Equals(_rightValue, other._rightValue);
        }

        public override bool Equals(object obj)
        {
            return obj is Either<TLeft, TRight> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_leftValue, _rightValue);
        }
    }
}