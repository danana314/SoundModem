using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Documents;

namespace SoundModem.Base
{
    public class BinArray
    {
        private int[] _array;

        public int Length
        {
            get { return _array.Length; }
        }

        public BinArray(int size)
        {
            _array = new int[size];
        }

        public bool this[int index]
        {
            get { return GetBool(index); }
            set { SetBit(index, value); }
        }

        public void SetBit(int bitNumber, bool value)
        {
            _array[bitNumber] = value ? 1 : 0; 
        }

        public int GetBit(int bitNumber)
        {
            return _array[bitNumber];
        }

        public bool GetBool(int bitNumber)
        {
            return _array[bitNumber] == 1;
        }

        public override string ToString()
        {
            return String.Join("", new List<int>(_array).ConvertAll(i => i.ToString()).ToArray());
        }

        public int[] ToArray()
        {
            return _array;
        }
    }
}
