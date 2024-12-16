using System;
using System.Security.Cryptography;
// ReSharper disable InconsistentNaming

namespace ImageSortingThingy.Helpers;

public sealed class CRC32 : HashAlgorithm
{
    private uint _crc;
    private static readonly uint[] Table;

    static CRC32()
    {
        Table = new uint[256];
        const uint polynomial = 0xedb88320;
        for (uint i = 0; i < 256; i++)
        {
            uint crc = i;
            for (int j = 8; j > 0; j--)
            {
                if ((crc & 1) == 1)
                    crc = (crc >> 1) ^ polynomial;
                else
                    crc >>= 1;
            }

            Table[i] = crc;
        }
    }

    public CRC32() => Initialize();

    public override void Initialize() => _crc = 0xffffffff;

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        for (int i = ibStart; i < ibStart + cbSize; i++)
        {
            byte index = (byte)((_crc & 0xff) ^ array[i]);
            _crc = (_crc >> 8) ^ Table[index];
        }
    }

    protected override byte[] HashFinal()
    {
        _crc ^= 0xffffffff;
        return BitConverter.GetBytes(_crc);
    }
}