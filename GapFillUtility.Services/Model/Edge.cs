using System.Collections.Generic;

namespace GapFillUtility.Services.Model
{
    public sealed class Edge : Entity
    {
        public string OutKey { get; set; }
        public string InKey { get; set; }

        public override bool Equals(object obj)
        {
            var edge = obj as Edge;
            return edge != null &&
                   base.Equals(obj) &&
                   OutKey == edge.OutKey &&
                   InKey == edge.InKey;
        }

        public override int GetHashCode()
        {
            var hashCode = 2135894497;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OutKey);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InKey);
            return hashCode;
        }
    }
}
