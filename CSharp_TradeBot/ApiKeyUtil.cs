using System;

public class ApiKeyUtil
{
    public static ApiKeyClass ReadApiKey()
    {
        var apikey = new ApiKeyClass();

        var binanceApiKeyData = ApiKeyManager.ReadBinanceApiKey();
        if (binanceApiKeyData == null)
        {
            Console.WriteLine("No Binance API key found. Please enter your API key and secret.");
            Console.Write("API key: ");
            apikey.BinanceApiKey = Console.ReadLine();
            Console.Write("Secret: ");
            apikey.BinanceApiSecret = Console.ReadLine();

            ApiKeyManager.WriteBinanceApiKey(apikey.BinanceApiKey, apikey.BinanceApiSecret);
        }
        else
        {
            Console.WriteLine("Binance API key found. Using it.");
            apikey.BinanceApiKey = binanceApiKeyData.apiKey;
            apikey.BinanceApiSecret = binanceApiKeyData.secret;
        }

        var taApiKeyData = ApiKeyManager.ReadTAApiKey();
        if (taApiKeyData == null)
        {
            Console.WriteLine("No TAAPI API key found. Please enter your API key.");
            Console.Write("API key: ");
            apikey.TaApiApiKey = Console.ReadLine();

            ApiKeyManager.WriteTAApiKey(apikey.TaApiApiKey);
        }
        else
        {
            Console.WriteLine("TAAPI API key found. Using it.");
            apikey.TaApiApiKey = taApiKeyData.apikey;
        }

        return apikey;
    }
}
