using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LoaderLibrary.Load
{
    /// <summary>
    /// This class is responsible for the creation of bulk insert files in proper format (binary or non-binary).
    /// Important note: use the 'nullable' method for a column writing only if the target column has NULL keyword
    /// (target column: where the content of bulk insert file will be inserted) and vice versa for NOT NULL.
    /// E.g.: the method WriteInt should be used for column [c] [int] NOT NULL and not WriteNullableInt.
    /// This principle of usage is very important because if the usage is improper then misleading error message
    /// emerges during bulk inserting: e.g. the error message indicates another column as bad.
    /// </summary>
    public class BulkFileWriter
    {
        private TextWriter outputWriter;
        private BinaryWriter outputBinary;
        private bool binary;

        private string fieldend;
        private string rowend;
        private bool inline;

        private StringBuilder row = new StringBuilder();

        public BulkFileWriter(TextWriter outputWriter)
        {
            InitializeMembers();

            this.outputWriter = outputWriter;
            this.binary = false;
        }

        public BulkFileWriter(BinaryWriter outputBinary)
        {
            InitializeMembers();

            this.outputBinary = outputBinary;
            this.binary = true;
        }

        private void InitializeMembers()
        {
            this.outputWriter = null;
            this.outputBinary = null;
            this.binary = false;

            this.fieldend = "\0";
            this.rowend = "\0\r";
            this.inline = false;
        }

        /*
        public void WriteValue(object value)
        {
            WriteFieldEnd();
            row.Append(value);
        }*/

        //
        public void WriteBit(bool value)
        {
            if (binary)
            {
                outputBinary.Write((byte)1);
                outputBinary.Write(value ? (byte)1 : (byte)0);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value ? '1' : '0');
            }
        }

        public void WriteNullableBit(bool? value)
        {
            if (value.HasValue)
            {
                if (binary)
                {
                    outputBinary.Write((byte)1);
                    outputBinary.Write(value.Value ? (byte)1 : (byte)0);
                }
                else
                {
                    WriteFieldEnd();
                    row.Append(value.Value ? '1' : '0');
                }
            }
            else
            {
                if (binary)
                {
                    outputBinary.Write((byte)0xFF);
                }
                else
                {
                    WriteFieldEnd();
                }
            }
        }

        public void WriteBinary(byte[] bytes, int length)
        {
            if (binary)
            {
                outputBinary.Write((ushort)length);
                byte[] outputBytes = DetermineOutputBytes(bytes, length);
                outputBinary.Write(outputBytes);
            }
            else
            {
                WriteFieldEnd();
                byte[] outputBytes = DetermineOutputBytes(bytes, length);
                foreach (byte outputByte in outputBytes)
                {
                    row.Append(outputByte);
                }
            }
        }
        
        private byte[] DetermineOutputBytes(byte[] bytes, int length)
        {
            byte[] result = Enumerable.Repeat((byte)0, length).ToArray();
            if (length < bytes.Length)
            {
                for (int index = 0; index < length; index++)
                {
                    result[index] = bytes[index];
                }
            }
            else if (length > bytes.Length)
            {
                for (int index = 0; index < bytes.Length; index++)
                {
                    result[index] = bytes[index];
                }
            }
            else
            {
                result = bytes;
            }
            return result;
        }

        public void WriteNullableBinary(byte[] bytes, int length)
        {
            if (bytes == null)
            {
                if (binary)
                {
                    outputBinary.Write((ushort)0xFFFF);
                }
                else
                {
                    WriteFieldEnd();
                }
            }
            else
            {
                WriteBinary(bytes, length);
            }
        }

        public void WriteVarBinary(byte[] bytes, int maxlength)
        {
            if (binary)
            {
                if (bytes == null)
                {
                    outputBinary.Write((ushort)0xFFFF);
                }
                else
                {
                    int len = Math.Min(bytes.Length, maxlength);
                    outputBinary.Write((ushort)len);
                    byte[] outputBytes = DetermineVarOutputBytes(bytes, len);
                    outputBinary.Write(outputBytes);
                }
            }
            else
            {
                if (bytes == null)
                {
                    WriteFieldEnd();
                }
                else
                {
                    WriteFieldEnd();
                    int len = Math.Min(bytes.Length, maxlength);
                    byte[] outputBytes = DetermineVarOutputBytes(bytes, len);
                    foreach (byte outputByte in outputBytes)
                    {
                        row.Append(outputByte);
                    }
                }
            }
        }

        private byte[] DetermineVarOutputBytes(byte[] bytes, int len)
        {
            byte[] outputBytes;
            if (len < bytes.Length)
            {
                outputBytes = new byte[len];
                for (int index = 0; index < len; index++)
                {
                    outputBytes[index] = bytes[index];
                }
            }
            else
            {
                outputBytes = bytes;
            }
            return outputBytes;
        }

        public void WriteSignedTinyInt(sbyte value)
        {
            if (binary)
            {
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value);
            }
        }

        public void WriteTinyInt(byte value)
        {
            if (binary)
            {
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value);
            }
        }

        /*
        public void WriteNullableTinyInt(sbyte value)
        {
            if (binary)
            {
                outputBinary.Write((byte)1);
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value);
            }
        }*/

        public void WriteSmallInt(short value)
        {
            if (binary)
            {
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value);
            }
        }

        /*
        public void WriteNullableSmallInt(short value)
        {
            if (binary)
            {
                outputBinary.Write((byte)2);
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value);
            }
        }*/

        public void WriteInt(int value)
        {
            if (binary)
            {
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value);
            }
        }

        public void WriteNullableInt(int? value)
        {
            if (value.HasValue)
            {
                if (binary)
                {
                    outputBinary.Write((byte)4);
                    outputBinary.Write(value.Value);
                }
                else
                {
                    WriteFieldEnd();
                    row.Append(value);
                }
            }
            else
            {
                if (binary)
                {
                    outputBinary.Write((byte)0xFF);
                }
                else
                {
                    WriteFieldEnd();
                }
            }
        }

        public void WriteBigInt(long value)
        {
            if (binary)
            {
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value);
            }
        }


        public void WriteNullableBigInt(Int64? value)
        {
            if (value.HasValue)
            {
                if (binary)
                {
                    outputBinary.Write((byte)8);
                    outputBinary.Write(value.Value);
                }
                else
                {
                    WriteFieldEnd();
                    row.Append(value);
                }
            }
            else
            {
                if (binary)
                {
                    outputBinary.Write((byte)0xFF);
                }
                else
                {
                    WriteFieldEnd();
                }
            }
        }

        public void WriteReal(float value)
        {
            if (binary)
            {
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /*
        public void WriteNullableReal(float value)
        {
            if (binary)
            {
                outputBinary.Write((byte)4);
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }*/

        public void WriteFloat(double value)
        {
            if (binary)
            {
                outputBinary.Write(value);
            }
            else
            {
                WriteFieldEnd();
                row.Append(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        public void WriteNullableFloat(double? value)
        {
            if (value.HasValue)
            {
                if (binary)
                {
                    outputBinary.Write((byte)8);
                    outputBinary.Write(value.Value);
                }
                else
                {
                    WriteFieldEnd();
                    row.Append(value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }
            else
            {
                if (binary)
                {
                    outputBinary.Write((byte)0xFF);
                }
                else
                {
                    WriteFieldEnd();
                }
            }
        }

        public void WriteDateTime(DateTime dateTime)
        {
            if (binary)
            {
                int date = (dateTime - new DateTime(1900, 1, 1)).Days;
                int time = (int)((dateTime.TimeOfDay).TotalSeconds * 300);
                outputBinary.Write(date);
                outputBinary.Write(time);
            }
            else
            {
                WriteFieldEnd();
                row.AppendFormat("{0:yyyy-MM-dd HH:mm:ss}", dateTime);
            }
        }

        /*
        public void WriteNullableDateTime(DateTime dateTime)
        {
            if (binary)
            {
                outputBinary.Write((byte)8);
            }
            WriteDateTime(dateTime);
        }*/

        public void WriteVarChar(string value, int maxlength)
        {
            if (binary)
            {
                if (value != null)
                {
                    int len = Math.Min(value.Length, maxlength);
                    byte[] bytes = Encoding.Unicode.GetBytes(value.Substring(0, len));

                    outputBinary.Write((ushort)bytes.Length);
                    outputBinary.Write(bytes);
                }
                else
                {
                    outputBinary.Write((ushort)0xFFFF);
                }
            }
            else
            {
                if (value != null)
                {
                    WriteFieldEnd();
                    row.Append(value.Substring(0, Math.Min(value.Length, maxlength)));
                }
                else
                {
                    WriteFieldEnd();
                }
            }
        }

        public void WriteChar(string value, int length)
        {
            if (binary)
            {
                if (value != null)
                {
                    byte[] bytes = Encoding.Unicode.GetBytes(value.PadRight(length));
                    outputBinary.Write(bytes, 0, 2 * length);
                }
                else
                {
                    outputBinary.Write((ushort)0xFFFF);
                }
            }
            else
            {
                if (value != null)
                {
                    WriteFieldEnd();
                    row.Append(value.Substring(0, Math.Min(value.Length, length)));
                }
                else
                {
                    WriteFieldEnd();
                }
            }
        }

        public void WriteNullableChar(string value, int length)
        {
            if (binary)
            {
                if (value == null)
                {
                    outputBinary.Write((ushort)0xFFFF);
                }
                else
                {
                    byte[] bytes = Encoding.Unicode.GetBytes(value.PadRight(length));
                    outputBinary.Write((ushort)bytes.Length);
                    outputBinary.Write(bytes);
                }
            }
            else
            {
                if (value != null)
                {
                    WriteFieldEnd();
                    row.Append(value.Substring(0, Math.Min(value.Length, length)));
                }
                else
                {
                    WriteFieldEnd();
                }
            }
        }

        /*
        public void WriteNull()
        {
            if (binary)
            {
                //outputBinary.Write((byte)0xFF);
            }
            else
            {
                WriteFieldEnd();
            }
        }*/

        public void WriteFieldEnd()
        {
            if (!binary)
            {
                if (inline)
                {
                    row.Append(fieldend);
                }
                inline = true;
            }
        }

        public void EndLine()
        {
            if (!binary)
            {
                row.Append(rowend);

                outputWriter.Write(row.ToString());

                row.Clear();
                inline = false;
            }
        }
    }
}
