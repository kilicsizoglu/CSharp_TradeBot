using System;
using System.IO;
using Newtonsoft.Json;

public class ApiKeyManager
{
    public static dynamic ReadBinanceApiKey()
    {
        try
        {
            string filePath = "binance-api-key.txt";
            if (!File.Exists(filePath))
                return null;

            string jsonData = File.ReadAllText(filePath);
            dynamic data = JsonConvert.DeserializeObject(jsonData);
            if (data.apiKey == "" || data.secret == "")
                return null;

            return data;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    public static void WriteBinanceApiKey(string apiKey, string secret)
    {
        string filePath = "binance-api-key.txt";
        string jsonData = JsonConvert.SerializeObject(new { apiKey, secret });
        File.WriteAllText(filePath, jsonData);
    }

    public static void WriteTAApiKey(string apikey)
    {
        string filePath = "ta-api-key.txt";
        string jsonData = JsonConvert.SerializeObject(new { apikey });
        File.WriteAllText(filePath, jsonData);
    }

    public static dynamic ReadTAApiKey()
    {
        try
        {
            string filePath = "ta-api-key.txt";
            if (!File.Exists(filePath))
                return null;

            string jsonData = File.ReadAllText(filePath);
            dynamic data = JsonConvert.DeserializeObject(jsonData);
            if (data.apikey == "")
                return null;

            return data;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }
}