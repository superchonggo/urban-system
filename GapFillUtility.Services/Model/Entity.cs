using System.Collections.Generic;

namespace GapFillUtility.Services.Model
{
    public abstract class Entity
    {
        public Entity()
        {
            Properties = new Dictionary<string, string>();
        }

        public string Key { get; set; }
        public string Label { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public override bool Equals(object obj)
        {
            var entity = obj as Entity;
            return entity != null &&
                   Key == entity.Key &&
                   Label == entity.Label &&
                   EqualityComparer<Dictionary<string, string>>.Default.Equals(Properties, entity.Properties);
        }

        public override int GetHashCode()
        {
            var hashCode = 1130423789;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Label);
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(Properties);
            return hashCode;
        }
    }
}
