using System;
using System.IO;

namespace ImageSortingThingy.Helpers;

public static class HashHelper
{
    public static string ComputeCrc32Hash(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);
        using CRC32 hasher = new();
        byte[] hash = hasher.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}