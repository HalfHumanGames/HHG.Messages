using System;

namespace HHG.Messages
{
    internal struct SubjectId : IEquatable<SubjectId>
    {
        public Type Type { get; set; }
        public object Identifier { get; set; }

        public SubjectId(Type type, object id)
        {
            Type = type;
            Identifier = id;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + Type.GetHashCode();
                hash = hash * 29 + (Identifier == null ? 0 : Identifier.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (other is SubjectId)
            {
                SubjectId otherId = (SubjectId) other;
                return otherId == this;
            }

            return false;
        }

        public bool Equals(SubjectId other)
        {
            return this == other;
        }

        public static bool operator ==(SubjectId left, SubjectId right)
        {
            return left.Type == right.Type && Equals(left.Identifier, right.Identifier);
        }

        public static bool operator !=(SubjectId left, SubjectId right)
        {
            return !left.Equals(right);
        }
    }
}