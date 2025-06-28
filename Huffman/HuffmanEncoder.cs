namespace Huffman
{
    public sealed class HuffmanEncoder
    {
        public byte[] Encode(byte[] data)
        {
            using MemoryStream output = new();
            using (HuffmanCompression huffman = new(output))
            {
                huffman.Compress(data);
            }
            return output.ToArray();
        }
    }
}
