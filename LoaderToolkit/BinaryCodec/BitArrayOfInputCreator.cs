using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCodec
{
    static class BitArrayOfInputCreator
    {
        public static BitArray DetermineBitArrayOfInput(string input, BidirectionalDictionary<string, BitArray> codecDomain)
        {
            BitArray bitArrayOfInput = new BitArray(0);
            for (int index = 0; index < input.Length; index++)
            {
                var inputPart = input.Substring(index, 1);
                BitArray bitArrayOfInputPart;
                if (codecDomain.Forward.TryGetValue(inputPart, out bitArrayOfInputPart))
                {
                    bitArrayOfInput = AppendBitArray(bitArrayOfInput, bitArrayOfInputPart);
                }
                else
                {
                    throw new ArgumentException("The input cannot be encoded because it contains invalid part!", inputPart);
                }
            }
            return bitArrayOfInput;
        }

        public static BitArray DetermineBitArrayOfBasesInput(string input, BidirectionalDictionary<string, BitArray> codecDomain,
            Dictionary<Constants.ColumnsFromSkipChars, string> byProductsBySkipChars)
        {
            BitArray bitArrayOfInput = new BitArray(0);
            var inputIndeces = new InputIndeces();
            while (inputIndeces.InputCharIndex < input.Length)
            {
                var inputPart = input.Substring(inputIndeces.InputCharIndex, 1);
                if ("^".Equals(inputPart))
                {
                    RecordReadStartingFactAndItsQual(input, byProductsBySkipChars, inputIndeces);
                }
                else if ("$".Equals(inputPart))
                {
                    RecordReadEndingFact(byProductsBySkipChars, inputIndeces);
                }
                // It is very important that the case "+" is preceded by case "^" because read mapping quality can contain symbol "+".
                else if ("+".Equals(inputPart))
                {
                    RecordExtraNucleotidesFact(input, byProductsBySkipChars, inputIndeces);
                }
                // It is very important that the case "-" is preceded by case "^" because read mapping quality can contain symbol "-".
                else if ("-".Equals(inputPart))
                {
                    RecordMissingNucleotidesFact(input, byProductsBySkipChars, inputIndeces);
                }
                else
                {
                    bitArrayOfInput = UpdateBitArray(codecDomain, bitArrayOfInput, inputIndeces, inputPart);
                }
            }
            return bitArrayOfInput;
        }

        private static BitArray UpdateBitArray(BidirectionalDictionary<string, BitArray> codecDomain, BitArray bitArrayOfInput, InputIndeces inputIndeces, string inputPart)
        {
            BitArray bitArrayOfInputPart;
            if (codecDomain.Forward.TryGetValue(inputPart, out bitArrayOfInputPart))
            {
                bitArrayOfInput = AppendBitArray(bitArrayOfInput, bitArrayOfInputPart);
                inputIndeces.InputCharIndex++;
                inputIndeces.ReadIndex++;
            }
            else
            {
                throw new ArgumentException("The input cannot be encoded because it contains invalid part!", inputPart);
            }
            return bitArrayOfInput;
        }

        private static BitArray AppendBitArray(BitArray original, BitArray appended)
        {
            var bools = new bool[original.Count + appended.Count];
            original.CopyTo(bools, 0);
            appended.CopyTo(bools, original.Count);
            return new BitArray(bools);
        }

        /// <summary>
        /// This method dockets the read indices which are started at actual position.
        /// Note: The read nucleotide value follow starting sign and read mapping quality in this case.
        /// E.g.: input: ,^~. when the input processing reach the char '^' then ReadIndex is 1
        ///       because the ReadIndex has been incremented after char ',' processing.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="byProductsBySkipChars"></param>
        /// <param name="inputIndeces"></param>
        private static void RecordReadStartingFactAndItsQual(string input,
            Dictionary<Constants.ColumnsFromSkipChars, string> byProductsBySkipChars, InputIndeces inputIndeces)
        {
            var readStartingSigns = byProductsBySkipChars[Constants.ColumnsFromSkipChars.StartingSigns];
            byProductsBySkipChars[Constants.ColumnsFromSkipChars.StartingSigns] 
                = readStartingSigns + inputIndeces.ReadIndex + Constants.separator;
            inputIndeces.InputCharIndex++;
            var readMappingQual = input.Substring(inputIndeces.InputCharIndex, 1);
            var readMappingQuals = byProductsBySkipChars[Constants.ColumnsFromSkipChars.MappingQual];
            byProductsBySkipChars[Constants.ColumnsFromSkipChars.MappingQual] = readMappingQuals + readMappingQual + Constants.separator;
            inputIndeces.InputCharIndex++;
        }

        /// <summary>
        /// This method dockets the read indices which are ended at actual position.
        /// Note: The read nucleotide value is followed by ending sign in this case
        ///       but the ReadIndex has already been incremented so it should be handled.
        /// E.g.: input: .$, the char '$' is assigned to the read '.'.
        /// </summary>
        /// <param name="byProductsBySkipChars"></param>
        /// <param name="inputIndeces"></param>
        private static void RecordReadEndingFact(Dictionary<Constants.ColumnsFromSkipChars, string> byProductsBySkipChars, 
            InputIndeces inputIndeces)
        {
            var readEndingSigns = byProductsBySkipChars[Constants.ColumnsFromSkipChars.EndingSigns];
            var previousReadIndex = inputIndeces.ReadIndex - 1;
            byProductsBySkipChars[Constants.ColumnsFromSkipChars.EndingSigns] = readEndingSigns + previousReadIndex + Constants.separator;
            inputIndeces.InputCharIndex++;
        }

        /// <summary>
        /// This method dockets the read indices which have extra nucleotides after actual position.
        /// Note: The read nucleotide value is followed by extra nucleotides signs in this case.
        ///       but the ReadIndex has already been incremented so it should be handled.
        /// E.g.: input: .+2AB, the char '+' is assigned to the read '.'.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="byProductsBySkipChars"></param>
        /// <param name="inputIndeces"></param>
        private static void RecordExtraNucleotidesFact(string input,
            Dictionary<Constants.ColumnsFromSkipChars, string> byProductsBySkipChars, InputIndeces inputIndeces)
        {
            inputIndeces.InputCharIndex++;
            var extraNucleotidesLength = Convert.ToInt32(input.Substring(inputIndeces.InputCharIndex, 1));
            inputIndeces.InputCharIndex++;
            var extraNucleotides = input.Substring(inputIndeces.InputCharIndex, extraNucleotidesLength);
            var extraNucleotidesStore = byProductsBySkipChars[Constants.ColumnsFromSkipChars.ExtraNuc];
            var previousReadIndex = inputIndeces.ReadIndex - 1;
            byProductsBySkipChars[Constants.ColumnsFromSkipChars.ExtraNuc] = extraNucleotidesStore + previousReadIndex + extraNucleotides + Constants.separator;
            inputIndeces.InputCharIndex += extraNucleotidesLength;
        }

        /// <summary>
        /// This method skips the description of missing nucleotides after actual position (because the asterisk will indicate this fact).
        /// Note: The read nucleotide value is followed by missing nucleotides signs in this case.
        /// E.g.: input: .-2AB, the char '-' is assigned to the read '.'.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputIndeces"></param>
        private static void RecordMissingNucleotidesFact(string input, 
            Dictionary<Constants.ColumnsFromSkipChars, string> byProductsBySkipChars, InputIndeces inputIndeces)
        {
            inputIndeces.InputCharIndex++;
            var missingNucleotidesLength = Convert.ToInt32(input.Substring(inputIndeces.InputCharIndex, 1));
            inputIndeces.InputCharIndex++;
            var missingNucleotides = input.Substring(inputIndeces.InputCharIndex, missingNucleotidesLength);
            var missingNucleotidesStore = byProductsBySkipChars[Constants.ColumnsFromSkipChars.MissingNuc];
            var previousReadIndex = inputIndeces.ReadIndex - 1;
            byProductsBySkipChars[Constants.ColumnsFromSkipChars.MissingNuc]
                = missingNucleotidesStore + previousReadIndex + missingNucleotides + Constants.separator;
            inputIndeces.InputCharIndex += missingNucleotidesLength;
        }
    }
}
