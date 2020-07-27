using System;

namespace JonBates.CheckThisOut.Core
{
    public class FieldError
    {
        public FieldError(string fieldName, string error)
        {
            FieldName = fieldName;
            Error = error;
        }

        public string FieldName { get; }
        public string Error { get; }

        protected bool Equals(FieldError other)
        {
            return FieldName == other.FieldName && Error == other.Error;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FieldError) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FieldName, Error);
        }
    }
}