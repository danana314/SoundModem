using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundModem.Base
{
    public struct BinBool
    {
        // The two possible BinBool values:
        public static readonly BinBool BinFalse = new BinBool(0);
        public static readonly BinBool BinTrue = new BinBool(1);

        int _value;

        // Private constructor. The _value parameter must be  0, or 1:
        BinBool(int value)
        {
            this._value = value;
        }

        // Implicit conversion from bool to BinBool
        public static implicit operator BinBool(bool x)
        {
            return x ? BinTrue : BinFalse;
        }

        // Implicit conversion from bool to BinBool
        public static implicit operator BinBool(int x)
        {
            return x > 0 ? BinTrue : BinFalse;
        }

        // Explicit conversion from BinBool to bool
        public static explicit operator bool(BinBool x)
        {
            return x._value == 1;
        }

        // Explicit conversion from BinBool to int
        public static explicit operator int(BinBool x)
        {
            return x._value;
        }

        public static BinBool operator ==(BinBool x, BinBool y)
        {
            return x._value == y._value ? BinTrue : BinFalse;
        }

        public static BinBool operator !=(BinBool x, BinBool y)
        {
            return x._value != y._value ? BinTrue : BinFalse;
        }

        public static BinBool operator !(BinBool x)
        {
            return new BinBool(-x._value);
        }

        public static BinBool operator &(BinBool x, BinBool y)
        {
            return new BinBool(x._value < y._value ? x._value : y._value);
        }

        public static BinBool operator |(BinBool x, BinBool y)
        {
            return new BinBool(x._value > y._value ? x._value : y._value);
        }

        public static BinBool operator ^(BinBool x, BinBool y)
        {
            return new BinBool(x._value != y._value ? 1 : 0);
        }

        public static implicit operator string(BinBool x)
        {
            return x._value.ToString();
        }

        // Override the Object.Equals(object o) method:
        public override bool Equals(object o)
        {
            try
            {
                return (bool)(this == (BinBool)o);
            }
            catch
            {
                return false;
            }
        }

        // Override the ToString method to convert BinBool to a string:
        public override string ToString()
        {
            switch (_value)
            {
                case 0:
                    return "0";
                case 1:
                    return "1";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
