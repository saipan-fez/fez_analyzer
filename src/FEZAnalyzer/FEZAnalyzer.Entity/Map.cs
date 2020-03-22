using System;

namespace FEZAnalyzer.Entity
{
    public class Map
    {
        private const string UnknownMapName = null;

        public static Map UnknownMap => new Map();
        public string Name { get; }

        private Map()
        {
            Name = UnknownMapName;
        }

        public Map(string name)
        {
            Name = name ?? throw new ArgumentNullException();
        }

        public bool IsUnknownMap()
        {
            return Name == UnknownMapName;
        }

        public override bool Equals(object obj)
        {
            var map = obj as Map;
            return map != null &&
                   Name == map.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Map map1, Map map2)
        {
            return map1.Name == map2.Name;
        }

        public static bool operator !=(Map map1, Map map2)
        {
            return map1.Name != map2.Name;
        }
    }
}
