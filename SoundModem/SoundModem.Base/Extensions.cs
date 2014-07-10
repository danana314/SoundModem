using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundModem.Base
{
    public static class Extensions
    {
        public static bool GetBit(this byte b, int bitNumber)
        {
            //return (b & (1 << bitNumber - 1)) != 0;
            return (b & (1 << (8 - bitNumber))) != 0;
        }

        public static bool GetParity(this byte b, bool isEvenParity)
        {
            int bitsSet = 0;
            for (int i = 0; i < 8; i++)
                if ((b & (1 << i)) > 0)
                    bitsSet++;

            // return 0 if bitsSet even, 1 if bitsSet odd
            return isEvenParity ? bitsSet % 2 == 1 : bitsSet % 2 == 0;
        }

        public static byte[] ToByteArray(this BinBool[] bits)
        {
            int numBytes = bits.Length / 8;
            if (bits.Length % 8 != 0) numBytes++;

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                if ((bool)bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }
    }


}
