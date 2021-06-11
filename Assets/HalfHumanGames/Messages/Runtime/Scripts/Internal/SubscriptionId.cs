using System;

namespace HHG.Messages
{
    internal struct SubscriptionId : IEquatable<SubscriptionId>
    {
        public SubjectId SubjectId { get; private set; }
        public object Callback { get; private set; }

        public SubscriptionId(SubjectId subjectId, object callback)
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
            if (other is SubscriptionId)
            {
                return Equals((SubscriptionId) other);
            }
            return false;
        }

        public bool Equals(SubscriptionId other)
        {
            return Equals(SubjectId, other.SubjectId)
                && Equals(Callback, other.Callback);
        }

        public static bool operator ==(SubscriptionId left, SubscriptionId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SubscriptionId left, SubscriptionId right)
        {
            return !left.Equals(right);
        }
    }
}