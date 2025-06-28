using System;
using System.Collections.Generic;

namespace Huffman.Legacy
{
    internal sealed class HuffmanNodeComparer : IComparer<HuffmanNode>
    {
        public int Compare(HuffmanNode x, HuffmanNode y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentNullException("HuffmanNode cannot be null");
            }
            return x.Frequency.CompareTo(y.Frequency);
        }
    }
}
