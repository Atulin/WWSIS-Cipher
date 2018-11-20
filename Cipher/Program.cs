using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cipher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input string: ");
            string input = Console.ReadLine();
            byte[] bin = Encoding.Default.GetBytes(input);
            
            Console.Write("Input key: ");
            string key = Console.ReadLine();
            byte[] binkey = Encoding.Default.GetBytes(Helpers.FillToSize(key, bin.Length));

            List<byte> bin2 = new List<byte>();
            foreach (byte b in bin)
            {
                bin2.Add((byte) ~b);
            }
            
            string hex = Convert.ToBase64String(bin2.ToArray());
            
            Console.Write("Output: ");
            Console.WriteLine(hex);

            byte[] obin = Convert.FromBase64String(hex);
            
            List<byte> obin2 = new List<byte>();
            foreach (byte b in obin)
            {
                obin2.Add((byte) ~b);
            }
            
            string output = Encoding.Default.GetString(obin2.ToArray());

            Console.Write("Decipher: ");
            Console.WriteLine(output);
        }
    }
    
    class Helpers {
        public static string FillToSize(string str, int length)
        {
            char[] chars = str.ToCharArray();
            List<char> output = new List<char>();
            int amount = chars.Length / length;

            for (int i = 0; i < amount; i++)
            {
                foreach (char c in chars)
                {
                    output.Add(c);
                }
            }

            return new string(output.ToArray());
        }
    }
}