using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Services
{
    public class BitArrayEqualityComparer : IEqualityComparer<BitArray>
    {
        public int GetHashCode(BitArray bitArray)
        {
            return 0;
        }

        public bool Equals(BitArray bitArray1, BitArray bitArray2)
        {
            if (ReferenceEquals(bitArray1, bitArray2)) 
            {
                return true;
            } 
            else if (bitArray1.Count != bitArray2.Count)
            {
                return false;
            }
            else
            {
                bool[] bits1 = new bool[bitArray1.Count];
                bitArray1.CopyTo(bits1, 0);
                bool[] bits2 = new bool[bitArray2.Count];
                bitArray2.CopyTo(bits2, 0);
                for (int index = 0; index < bitArray1.Count; index++)
                {
                    bool bit1 = bits1[index];
                    bool bit2 = bits2[index];
                    if (bit1 != bit2)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
