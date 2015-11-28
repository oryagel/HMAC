using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHA1
{
    public class Digester
    {
        uint A, B, C, D, E, F;
        uint[] H = { 0x67452301, 0xEFCDAB89, 0x98BADCFE, 0x10325476, 0xC3D2E1F0 };
        uint temp;

        private const string ZEROES = "00000000";
        

        public string Digest(byte[] data)
        {
            var paddedData = pad(data);
            
            uint[] K = { 0x5A827999, 0x6ED9EBA1, 0x8F1BBCDC, 0xCA62C1D6 };

            if (paddedData.Length % 64 != 0)
                Console.WriteLine("wrong padded data length");
            int passesReq = paddedData.Length / 64;
            var work = new byte[64];

            for (int i = 0; i < passesReq; i++)
            {
                Array.Copy(paddedData, 64 * i, work, 0, 64);
                processBlock(work, H, K);
            }

            return intArrayToHexStr(H);
        }

        private void processBlock (byte[] work, uint[] H, uint[] K)
        {
            var W = new uint[80];
            for (int i = 0; i < 16; i++)
			{
			    var temp = 0;
                for (int j = 0; j < 4; j++)
                {
                    temp = (work[i * 4 + j] & 0x000000FF) << (24 - j * 8);
                    W[i] = (uint)(W[i] | temp);
                }
			}

            for (int i = 16; i < 80; i++)
			    W[i] = rotateLeft((uint)(W[i - 3] ^ W[i - 8] ^ W[i - 14] ^ W[i - 16]), 1);

            A = H[0];
            B = H[1];
            C = H[2];
            D = H[3];
            E = H[4];

            for (int i = 0; i < 20; i++)
            {
                F = (B & C) | ((~B) & D);
                //	K = 0x5A827999;
                temp = (uint)(rotateLeft(A, 5) + F + E + K[0] + W[i]);
                Console.WriteLine(K[0].ToString("X4"));
                E = D;
                D = C;
                C = rotateLeft(B, 30);
                B = A;
                A = temp;
            }

            for (int i = 20; i < 40; i++)
            {
                F = B ^ C ^ D;
                //   K = 0x6ED9EBA1;
                temp = (uint)(rotateLeft(A, 5) + F + E + K[1] + W[i]);
                Console.WriteLine(K[1].ToString("X4"));
                E = D;
                D = C;
                C = rotateLeft(B, 30);
                B = A;
                A = temp;
            }

            for (int i = 40; i < 60; i++)
            {
                F = (B & C) | (B & D) | (C & D);
                //   K = 0x8F1BBCDC;
                temp = (uint)(rotateLeft(A, 5) + F + E + K[2] + W[i]);
                E = D;
                D = C;
                C = rotateLeft(B, 30);
                B = A;
                A = temp;


            }

            for (int j = 60; j < 80; j++)
            {
                F = B ^ C ^ D;
                //   K = 0xCA62C1D6;
                temp = (uint)(rotateLeft(A, 5) + F + E + K[3] + W[j]);
                E = D;
                D = C;
                C = rotateLeft(B, 30);
                B = A;
                A = temp;
            }

            H[0] += A;
            H[1] += B;
            H[2] += C;
            H[3] += D;
            H[4] += E;

            int n;
            for (n = 0; n < 16; n++)
                Console.WriteLine("W[" + n + "] = " + W[n].ToString("X4"));
            
            
			

        }

        private uint rotateLeft(uint value, int bits)
        {
            return (value << bits) | ((uint)value >> (32 - bits));
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
            long lengthInBits = dataLength * 8;

            for (int i = 0; i < 8; i++)
                pad[pad.Length - 1 - i] = (byte)((lengthInBits >> (8 * i)) & 0x00000000000000FF);

            var output = new byte[dataLength + padLength];
            Array.Copy(data, 0, output, 0, dataLength);
            Array.Copy(pad, 0, output, dataLength, pad.Length);

            return output;
        }

        private static string toHexString(int x)
        {
            return padSrt(x.ToString("X4"));
        }

        private static string padSrt(string s)
        {
            if (s.Length > 8)
                return s.Substring(s.Length - 8);
            return ZEROES.Substring(s.Length) + s;
        }

        private string intArrayToHexStr(uint[] data)
        {
            string output = "";
            string tempStr = "";
            uint tempInt = 0;
            for (int i = 0; i < data.Length; i++)
            {

                tempInt = data[i];

                tempStr = tempInt.ToString("X4");

                if (tempStr.Length == 1)
                {
                    tempStr = "0000000" + tempStr;
                }
                else if (tempStr.Length == 2)
                {
                    tempStr = "000000" + tempStr;
                }
                else if (tempStr.Length == 3)
                {
                    tempStr = "00000" + tempStr;
                }
                else if (tempStr.Length == 4)
                {
                    tempStr = "0000" + tempStr;
                }
                else if (tempStr.Length == 5)
                {
                    tempStr = "000" + tempStr;
                }
                else if (tempStr.Length == 6)
                {
                    tempStr = "00" + tempStr;
                }
                else if (tempStr.Length == 7)
                {
                    tempStr = "0" + tempStr;
                }
                output = output + tempStr;
            }
            
            return output;
        }
    }
}