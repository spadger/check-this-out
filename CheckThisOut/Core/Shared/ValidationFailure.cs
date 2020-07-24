using System;

namespace JonBates.CheckThisOut.Core.Shared
{
    public class ValidationFailure
    {
        public ValidationFailure(string fieldName, string error)
        {
            FieldName = fieldName;
            Error = error;
        }

        public string FieldName { get; }
        public string Error { get; }

        protected bool Equals(ValidationFailure other)
        {
            return FieldName == other.FieldName && Error == other.Error;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValidationFailure) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FieldName, Error);
        }
    }
}