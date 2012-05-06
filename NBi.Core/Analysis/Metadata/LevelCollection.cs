using System.Collections.Generic;

namespace NBi.Core.Analysis.Metadata
{
    public class LevelCollection : Dictionary<string, Level>
    {
        public void AddOrIgnore( string uniqueName, string caption, int number)
        {
            if (!this.ContainsKey(uniqueName))
                this.Add(uniqueName, new Level(uniqueName, caption, number));
        }

        public void Add(Level level)
        {
            this.Add(level.UniqueName, level);
        }
        
        public LevelCollection Clone()
        {
            var levels = new LevelCollection();
            foreach (var level in this)
                levels.Add(level.Value.Clone());
            return levels;
        }
    }
}
