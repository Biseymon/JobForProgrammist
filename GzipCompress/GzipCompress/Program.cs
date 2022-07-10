using System;
using System.IO.Compression;
using System.IO;

namespace GzipCompress
{

    class Program
    {
        
        static int Main(string[] args)
        {
            if (args[0] == "compress")
            {
                if (args[1] == null || args[2] == null) return 1;
                if (!File.Exists(args[1])) return 1;

                CompressFile(args[1], args[2]);
                return 0;
            }
            else if (args[0] == "decompress")
            {
                if (args[1] == null || args[2] == null) return 1;
                if (!File.Exists(args[2])) return 1;

                DecompressFile(args[1], args[2]);
                return 0;
            }
            
            return 1;
        }

        private static void CompressFile(string source, string target)
        {
            using FileStream originalFileStream = File.Open(source, FileMode.Open);
            using FileStream compressedFileStream = File.Create(target);
            using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);
            originalFileStream.CopyTo(compressor);
        }

        private static void DecompressFile(string source, string target)
        {
            using FileStream compressedFileStream = File.Open(target, FileMode.Open);
            using FileStream outputFileStream = File.Create(source);
            using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            decompressor.CopyTo(outputFileStream);
        }
    }
}
