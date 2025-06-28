namespace Huffman
{
    internal sealed class HuffmanNode
    {
        /// <summary>
        /// Creates a new huffman leaf node.
        /// </summary>
        /// <param name="freq"> Frequency of this leaf node. </param>
        /// <param name="val"> Leaf node value. </param>
        public HuffmanNode(int freq, byte val)
        {
            Frequency = freq;
            Value = val;
            IsLeaf = true;
        }

        /// <summary>
        /// Creates a new huffman node with two children.
        /// </summary>
        /// Used for building the huffman tree.
        /// <param name="leftChild"> Left side child node. </param>
        /// <param name="rightChild"> Right side child node. </param>
        public HuffmanNode(HuffmanNode leftChild, HuffmanNode rightChild)
        {
            LeftChild = leftChild;
            RightChild = rightChild;
            leftChild.Bit = false;
            rightChild.Bit = true;
            Frequency = leftChild.Frequency + rightChild.Frequency;
            leftChild.Parent = rightChild.Parent = this;
            IsLeaf = false;
        }

        /// <summary>
        /// Parent node of this Huffman node.
        /// </summary>
        public HuffmanNode Parent { get; private set; }

        /// <summary>
        /// Left child node of this Huffman node.
        /// </summary>
        public HuffmanNode LeftChild { get; }

        /// <summary>
        /// Right child node of this Huffman node.
        /// </summary>
        public HuffmanNode RightChild { get; }

        /// <summary>
        /// Represents whether this node is a leaf node.
        /// </summary>
        /// This property is set when the node is created.
        public bool IsLeaf { get; }

        /// <summary>
        /// Represents whether this node is the root of the Huffman tree.
        /// </summary>
        public bool IsRoot => Parent == null;

        /// <summary>
        /// Represents the value of the edge leading to this node.
        /// </summary>
        /// 0 for left child, 1 for right child.
        /// Part of the Huffman Code.
        public bool Bit { get; private set; }

        /// <summary>
        /// Leaf node value. Represents the byte value of the leaf node.
        /// </summary>
        public byte Value { get; }

        /// <summary>
        /// Node frequency.
        /// </summary>
        public int Frequency { get; }
    }
}
