using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloader.Rule
{
    public class PathBuilder
    {
        private List<string> _leaves = new List<string>();

        public PathBuilder()
        {
        }

        public PathBuilder Append(string leaf)
        {
            _leaves.Add(leaf);
            return this;
        }

        public string Build(params int[] skips)
        {
            string[] leaves = _leaves.ToArray();
            if (skips.Length > 0)
            {
                var unskippedLeaves = _leaves.Where((leaf, i) => !(skips.Contains(i)));
                leaves = unskippedLeaves.ToArray();
            }
            return Path.Combine(leaves);
        }
    }

}
