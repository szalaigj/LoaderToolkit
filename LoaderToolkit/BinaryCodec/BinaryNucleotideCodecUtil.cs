using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public static class BinaryNucleotideCodecUtil
    {
        public static readonly Dictionary<string, byte> encodedNucleotidePairs;

        static BinaryNucleotideCodecUtil()
        {
            encodedNucleotidePairs = new Dictionary<string, byte>();
            Dictionary<string, int> encodedNucleotides = new Dictionary<string, int>() 
            { 
                {"A",0x8}, 
                {"C",0x9}, 
                {"G",0xA}, 
                {"T",0xB}, 
                {"N",0xC},
                {"R",0xC},
                {"Y",0xC},
                {"K",0xC},
                {"M",0xC},
                {"S",0xC},
                {"W",0xC},
                {"B",0xC},
                {"D",0xC},
                {"H",0xC},
                {"V",0xC},
                {" ",0X4} 
            };
            foreach (var firstNucleotideEntry in encodedNucleotides)
            {
                foreach (var lastNucleotideEntry in encodedNucleotides)
                {
                    string currentNucleotidePair = firstNucleotideEntry.Key + lastNucleotideEntry.Key;
                    int encodedNucleotidePair = (firstNucleotideEntry.Value << 4) + lastNucleotideEntry.Value;
                    encodedNucleotidePairs.Add(currentNucleotidePair, (byte)encodedNucleotidePair);
                }
            }
        }

        public static byte GetEncodedPart(string nucleotidePair)
        {
            return encodedNucleotidePairs[nucleotidePair];
        }
    }
}
