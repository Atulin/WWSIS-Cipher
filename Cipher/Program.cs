using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cipher
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input
            Console.Write("Input string: ");
            string input = Console.ReadLine();

            // Get key
            Console.Write("Input key: ");
            string key = Console.ReadLine();

            // Print encoded
            Console.Write("Output: ");
            string c = Crypto.Encode(input, key);
            Console.WriteLine(c);

            // Print decoded
            Console.Write("Decipher: ");
            string d = Crypto.Decode(c, key);
            Console.WriteLine(d);
        }
    }

    static class Helpers
    {
        public static BitArray FillToSize(BitArray filler, int length)
        {
            BitArray output = new BitArray(length);
            
            for (int i = 0; i < length; i++)
            {
                output[i] = filler[i % filler.Length];
            }

            return output;
        }

        public static string BitArrToHex(BitArray bits)
        {
            StringBuilder sb = new StringBuilder(bits.Length / 4);

            for (int i = 0; i < bits.Length; i += 4) {
                int v = (bits[i] ? 8 : 0) | 
                        (bits[i + 1] ? 4 : 0) | 
                        (bits[i + 2] ? 2 : 0) | 
                        (bits[i + 3] ? 1 : 0);

                sb.Append(v.ToString("x1")); // Or "X1"
            }

            return sb.ToString();
        }
        
        public static BitArray HexToBitArr(string hexData)
        {
            BitArray ba = new BitArray(4 * hexData.Length);
            for (int i = 0; i < hexData.Length; i++)
            {
                byte b = byte.Parse(hexData[i].ToString(), NumberStyles.HexNumber);
                for (int j = 0; j < 4; j++)
                {
                    ba.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
                }
            }
            return ba;
        }
        
        public static byte[] BitArrToByteArr(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }

    static class Crypto
    {
        public static string Encode(string message, string key)
        {
            // String to byte[]
            byte[] inputBytes = Encoding.Default.GetBytes(message);
            byte[] keyBytes = Encoding.Default.GetBytes(key);
            
            // byte[] to BitArray
            BitArray inputBits = new BitArray(inputBytes);
            BitArray keyBits = new BitArray(keyBytes);
            
            // Create full key
            BitArray finalKey = Helpers.FillToSize(keyBits, inputBits.Length);
            
            // Encode message BitArray with full key
            BitArray outputBits = inputBits.Not().Xor(finalKey);
            
            // Return
            string hex = Helpers.BitArrToHex(outputBits);
            return hex;
        }

        public static string Decode(string code, string key)
        {
            byte[] keyBytes = Encoding.Default.GetBytes(key);
            
            BitArray keyBits = new BitArray(keyBytes);
            BitArray inBits = Helpers.HexToBitArr(code);
            
            // Create full key
            BitArray finalKey = Helpers.FillToSize(keyBits, inBits.Length);
            
            BitArray msgBits = inBits.Xor(finalKey).Not();
            byte[] msgBytes = Helpers.BitArrToByteArr(msgBits);
            string msgStr = Encoding.Default.GetString(msgBytes);

            return msgStr;
        }
    }
}