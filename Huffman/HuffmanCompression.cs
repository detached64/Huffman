using System.IO.Compression;

namespace Huffman
{
    internal sealed class HuffmanCompression : IDisposable
    {
        private const ushort MAX = 512;
        private ushort index = 256;
        private readonly ushort[,] children = new ushort[MAX, 2];

        private readonly BitStream input;
        private readonly CompressionMode mode;
        private readonly int decompressedLength;
        private readonly MemoryStream output;

        public HuffmanCompression(Stream input, CompressionMode mode, int length)
        {
            this.mode = mode;
            if (mode is CompressionMode.Decompress)
            {
                this.input = new(input, BitStreamEndianness.Msb, BitStreamMode.Read);
                decompressedLength = length;
            }
            else
            {
                this.input = new(input, BitStreamEndianness.Msb, BitStreamMode.Write);
            }
            this.output = new();
        }

        public HuffmanCompression(Stream input, CompressionMode mode = CompressionMode.Compress)
        {
            this.mode = mode;
            this.input = new(input, BitStreamEndianness.Msb, BitStreamMode.Write);
            this.output = new();
        }

        private ushort RebuildTree()
        {
            switch (input.ReadBit())
            {
                case 0:
                    return (ushort)input.ReadBits(8);
                case 1:
                    ushort parent = index++;
                    if (parent >= MAX)
                    {
                        throw new InvalidDataException("Exceeded huffman tree.");
                    }
                    children[parent, 0] = RebuildTree();
                    children[parent, 1] = RebuildTree();
                    return parent;
                default:
                    throw new InvalidDataException("Invalid bit.");
            }
        }

        public MemoryStream Decompress()
        {
            if (mode != CompressionMode.Decompress)
            {
                throw new InvalidOperationException("Not in decompression mode.");
            }

            ushort root = RebuildTree();
            while (output.Length != decompressedLength)
            {
                ushort node = root;
                while (node >= 256)                 // Keep reading bits until we reach a leaf node
                {
                    int bit = input.ReadBit();
                    if (bit != -1)
                    {
                        node = children[node, bit]; // Traverse the tree based on the bit read
                    }
                }
                output.WriteByte((byte)node);
            }
            return output;
        }

        private HuffmanNode BuildTree(int[] freqs)
        {
            PriorityQueue<HuffmanNode, int> queue = new();
            for (int i = 0; i < freqs.Length; i++)
            {
                if (freqs[i] > 0)
                {
                    queue.Enqueue(new(freqs[i], (byte)i), freqs[i]);
                }
            }
            while (queue.Count > 1)
            {
                HuffmanNode left = queue.Dequeue();
                HuffmanNode right = queue.Dequeue();
                HuffmanNode parent = new(left, right);
                queue.Enqueue(parent, parent.Frequency);
            }
            return queue.Count > 0 ? queue.Dequeue() : null;
        }

        private void GenerateCodes(HuffmanNode node, Dictionary<byte, bool[]> codeTable)
        {
            if (node == null)
            {
                return;
            }

            HuffmanNode current;
            if (node.IsLeaf)
            {
                List<bool> code = [];
                current = node;
                while (!current.IsRoot)
                {
                    code.Add(current.Bit);
                    current = current.Parent;
                }
                code.Reverse();
                codeTable[node.Value] = [.. code];
            }
            else
            {
                if (node.LeftChild != null)
                {
                    GenerateCodes(node.LeftChild, codeTable);
                }
                if (node.RightChild != null)
                {
                    GenerateCodes(node.RightChild, codeTable);
                }
            }
        }

        private void WriteTree(HuffmanNode node)
        {
            if (node.IsLeaf)
            {
                input.WriteBit(0);
                input.WriteBits(node.Value);
            }
            else
            {
                input.WriteBit(1);
                WriteTree(node.LeftChild);
                WriteTree(node.RightChild);
            }
        }

        public void Compress(byte[] data)
        {
            if (mode != CompressionMode.Compress)
            {
                throw new InvalidOperationException("Not in compression mode.");
            }
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Data to compress cannot be null or empty.", nameof(data));
            }
            // Count frequency of each byte in the data
            int[] freqs = new int[256];
            foreach (byte b in data)
            {
                freqs[b]++;
            }
            // Build the Huffman tree
            HuffmanNode root = BuildTree(freqs) ?? throw new InvalidDataException("No data to compress.");
            // Generate codes for each byte
            Dictionary<byte, bool[]> codeTable = [];
            GenerateCodes(root, codeTable);
            // Write the Huffman tree to the stream
            WriteTree(root);
            // Write the compressed data
            foreach (byte b in data)
            {
                if (codeTable.TryGetValue(b, out bool[] code))
                {
                    input.WriteBits(code);
                }
                else
                {
                    throw new InvalidDataException($"No code found for byte {b}.");
                }
            }
        }

        #region IDisposable Members
        private bool is_disposed;

        private void Dispose(bool disposing)
        {
            if (!is_disposed)
            {
                if (disposing)
                {
                    input.Dispose();
                }
                is_disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
