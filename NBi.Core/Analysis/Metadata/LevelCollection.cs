using System.Collections.Generic;

namespace NBi.Core.Analysis.Metadata
{
    public class LevelCollection : List<Level>
    {
        public void InsertOrIgnore(int level, string uniqueName, string caption)
        {
            if (level>=this.Count || this[level]!=null)
                this.Insert(level, new Level(uniqueName, caption));
        }

        public LevelCollection Clone()
        {
            var levels = new LevelCollection();
            foreach (var level in this)
                levels.Add(level.Clone());
            return levels;
        }
    }
}
