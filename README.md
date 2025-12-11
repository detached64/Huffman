# Huffman

## Description

This repository implements a Huffman compression/decompression algorithm in C#.

The code includes functionality for encoding and decoding binary data, as well as an implementation of **BitStream** for efficient bit-level operations.

[Huffman](./Huffman) contains the implementation targeting .NET 10.0.
[Huffman.Legacy](./Huffman.Legacy) contains one targeting .NET Framework 4.8, including a custom `PriorityQueue` implementation.

## Usage

To decode a byte array `encoded`, the decompressed size is needed:

```csharp
HuffmanDecoder decoder = new HuffmanDecoder();
byte[] decoded = decoder.Decode(encoded, decompressedSize);
```

To encode a byte array `data`:

```csharp
HuffmanEncoder encoder = new HuffmanEncoder();
byte[] encoded = encoder.Encode(data);
```

## License

MIT License

Copyright (c) 2025 detached64