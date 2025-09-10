using HHG.Common.Runtime;
using System;

namespace HHG.Messages.Runtime
{
    internal struct HandlerId : IEquatable<HandlerId>
    {
        public SubjectId SubjectId { get; private set; }
        public object Callback { get; private set; }

        public HandlerId(SubjectId subjectId, object callback)
        {
            SubjectId = subjectId;
            Callback = callback;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + SubjectId.GetHashCode();
                hash = hash * 29 + Callback.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (other is HandlerId)
            {
                return Equals((HandlerId) other);
            }
            return false;
        }

        public bool Equals(HandlerId other)
        {
            return Equals(SubjectId, other.SubjectId)
                && Equals(Callback, other.Callback);
        }

        public static bool operator ==(HandlerId left, HandlerId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HandlerId left, HandlerId right)
        {
            return !left.Equals(right);
        }
    }
}