using System.IO.Compression;

namespace Huffman
{
    public sealed class HuffmanDecoder
    {
        public byte[] Decode(byte[] data, int decompressedLength)
        {
            using MemoryStream input = new(data);
            using HuffmanCompression huffman = new(input, CompressionMode.Decompress, decompressedLength);
            return huffman.Decompress().ToArray();
        }
    }
}
