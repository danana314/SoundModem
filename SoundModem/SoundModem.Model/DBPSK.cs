using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Utils;
using SoundModem.Base;

namespace SoundModem.Model
{
    public class DBPSK
    {
        private int SR;
        private double FREQ;
        private int BIT_RATE;
        private int SAMPLE_PER_BIT;
        private int SAMPLE_PER_BYTE;
        private readonly int BARKER_LEN = 13;
        private int[] _barkerCode = {1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1};
        private readonly bool _encodedStartBit = true;
        private readonly bool _byteStartBit = false;
        private readonly bool _byteStopBit = true;
        private readonly bool _evenParity = true;

        public DBPSK()
        {
            int SR = 44100;
            double FREQ = SR*8/18;
            int BIT_RATE = (SR/36);
            int SAMPLE_PER_BIT = (SR/BIT_RATE);
            int SAMPLE_PER_BYTE = (8*SAMPLE_PER_BIT);
        }

        public string Encode(string message)
        {
            var temp = System.Text.Encoding.ASCII.GetBytes(message);
            return Encode(temp);
        }

        public string Decode(string message)
        {
            var binMsg = message.Select(bit => bit.Equals('1') ? BinBool.BinTrue : BinBool.BinFalse).ToArray();
            var temp = Decode(binMsg); //new BinBool[] {1, 1, 0, 1, 1, 1, 0, 0, 1});
            var result = System.Text.Encoding.ASCII.GetString(temp);
            return result; //"Not Implemented";
        }

        private string Encode(byte[] message)
        {
            // Arbitrary high bit in the beginning of msg to start XOR differential encoding
            // Each byte gets framed by 4 additional bits (12 total):
            var encodedMessage = new BinArray(12*message.Length + 1);
            encodedMessage[0] = _encodedStartBit;

            for (int i = 1; i < encodedMessage.Length; i++)
            {
                switch ((i - 1)%12)
                {
                    case 0:
                        // Start bit = 0
                        encodedMessage[i] = _byteStartBit ^ encodedMessage[i - 1];
                        break;
                    case 10:
                    case 11:
                        // Stop bits = 1
                        encodedMessage[i] = _byteStopBit ^ encodedMessage[i - 1];
                        break;
                    case 9:
                        // Parity bit (Even)
                        encodedMessage[i] = message[(i - 1)/12].GetParity(_evenParity) ^ encodedMessage[i - 1];
                        break;
                    default:
                        // Data bits
                        encodedMessage[i] = message[(i - 1)/12].GetBit((i - 1)%12) ^ encodedMessage[i - 1];
                        break;
                }
            }
            return encodedMessage.ToString();
        }

        private byte[] Decode(BinBool[] message)
        {
            var decodedMessageFramed = new List<BinBool>();
            //decodedMessageFramed[0] = _encodedStartBit;   // Ignore first bit, needed to seed encoding XOR
            for (var i = 1; i < message.Length; i++)
            {
                decodedMessageFramed.Add(message[i] ^ message[i - 1]);
            }

            // Unframe the bytes
            var decodedMessage = new List<BinBool>();
            for (var i = 0; i < decodedMessageFramed.Count; i += 12)
            {
                decodedMessage.AddRange(decodedMessageFramed.GetRange(i, 12).Skip(1).Take(8).ToArray());
            }
            var temp = decodedMessage.ToArray().ToByteArray();
            return temp;
        }

        private int[] UniToBipolar(BitArray encodedMsg)
        {
            // Convert from unipolar to bipolar
            var bpsk = new int[encodedMsg.Length*BIT_RATE];
            for (var i = 0; i < encodedMsg.Length; i++)
            {
                for (var j = 0; j < SAMPLE_PER_BIT; j++)
                {
                    bpsk[i*SAMPLE_PER_BIT + j] = 2*(encodedMsg[i] ? 1 : 0) - 1;
                }
            }
            return bpsk;
        }

        private void Reverse(BitArray array)
        {
            var length = array.Length;
            var mid = (length/2);

            for (var i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }
        }
    }
}