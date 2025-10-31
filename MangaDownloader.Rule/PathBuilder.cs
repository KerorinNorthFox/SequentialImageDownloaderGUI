using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public PathBuilder Append(string[] leaves)
        {
            foreach (var leaf in leaves)
            {
                _leaves.Add(leaf);
            }
            return this;
        }

        /// <summary>
        /// 追加したleafを全て結合してパスにする
        /// </summary>
        /// <param name="skips">スキップするleafのインデックス. 範囲外は削る</param>
        /// <returns></returns>
        public string Build(params int[] skips)
        {
            string[] leaves = _leaves.ToArray();

            if (leaves.Length <= skips.Length)
            {
                skips = skips[0..leaves.Length];
            }

            if (skips.Length > 0)
            {
                var unskippedLeaves = _leaves.Where((leaf, i) => !(skips.Contains(i)));
                leaves = unskippedLeaves.ToArray();
            }
            return Path.Combine(leaves);
        }
    }
}
