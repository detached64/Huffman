using System.IO;
using System.IO.Compression;

namespace Huffman.Legacy
{
    public sealed class HuffmanDecoder
    {
        public byte[] Decode(byte[] data, int decompressedLength)
        {
            using (MemoryStream input = new MemoryStream(data))
            {
                using (HuffmanCompression huffman = new HuffmanCompression(input, CompressionMode.Decompress, decompressedLength))
                {
                    return huffman.Decompress().ToArray();
                }
            }
        }
    }
}
