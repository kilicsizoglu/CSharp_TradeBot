using System;
using System.IO;
using Newtonsoft.Json;

namespace CSharp_TradeBot
{
    internal class CryptoOrder
    {
        public string Symbol { get; set; }
        public string Side { get; set; }

        private static readonly string FilePath = "cryptoOrder.json";

        public static void WriteToJsonFile(CryptoOrder order)
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            string json = JsonConvert.SerializeObject(order, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static CryptoOrder ReadFromJsonFile()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("File not found.");
                return null;
            }

            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<CryptoOrder>(json);
        }

        public static void DeleteJsonFile()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                Console.WriteLine("File deleted.");
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
    }
}
