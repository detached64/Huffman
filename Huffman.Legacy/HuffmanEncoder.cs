using System.IO;

namespace Huffman.Legacy
{
    public sealed class HuffmanEncoder
    {
        public byte[] Encode(byte[] data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (HuffmanCompression huffman = new HuffmanCompression(output))
                {
                    huffman.Compress(data);
                }
                return output.ToArray();
            }
        }
    }
}
