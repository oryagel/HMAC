using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHA1
{
    public class Digester
    {
        private string Digest(byte[] data)
        {
            var paddedData = pad(data);
            uint[] H = { 0x67452301, 0xEFCDAB89, 0x98BADCFE, 0x10325476, 0xC3D2E1F0 };
            uint[] K = { 0x5A827999, 0x6ED9EBA1, 0x8F1BBCDC, 0xCA62C1D6 };

            if (paddedData.Length % 64 != 0)
                Console.WriteLine("wrong padded data length");
            int passesReq = paddedData.Length / 64;
            var work = new byte[64];

            for (int i = 0; i < passesReq; i++)
            {
                Array.Copy(paddedData, 64 * i, work, 0, 64);

            }
        }

        private void ProcessBlock (byte[] work, int H[], int K[])
        {
            var W = new int[80];
            for (int i = 0; i < 16; i++)
			{
			    var temp = 0;
                for (int j = 0; j < 4; j++)
                {
                    temp = (work[i * 4 + j] & 0x000000FF) << (24 - j * 8);
                    W[i] = W[i] | temp;
                }
			}

            for (int i = 16; i < 80; i++)
			    W[j] = rotateLeft(W[i - 3] ^ W[i - 8] ^ W[i - 14] ^ W[i - 16], 1);

            A = H[0];
            B = H[1];
            C = H[2];
            D = H[3];
            E = H[4];
			

        }

        private int rotateLeft(int value, int bits)
        {
            return (int)((value << bits) | ((uint)value >> (32 - bits)));
        }

        private byte[] pad (byte[] data)
        {
            int dataLength = data.Length;
            int tailLength = dataLength % 64;
            int padLength = 0;

            if (64 - tailLength >= 9)
                padLength = 64 - tailLength;
            else
                padLength = 128 - tailLength;
            var pad = new byte[padLength];
            pad[0] = (byte)0x80;
            var lengthInBits = dataLength * 8;

            for (int i = 0; i < 8; i++)
                pad[pad.Length - 1 - i] = (byte)((lengthInBits >> (8 * i)) & 0x00000000000000FF);

            var output = new byte[dataLength + padLength];
            Array.Copy(data, 0, output, 0, dataLength);
            Array.Copy(pad, 0, output, dataLength, pad.Length);

            return output;
        }
    }
}
