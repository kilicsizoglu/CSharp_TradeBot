using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Futures;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Options;

public class BinanceApi
{
    public static async Task<decimal> GetPrice(string apiKey, string apiSecret, String CoinName)
    {
        var client = new BinanceRestClient(options =>
        {
            options.ApiCredentials = new ApiCredentials(apiKey, apiSecret);
        });
        var res = await client.CoinFuturesApi.ExchangeData.GetPricesAsync(CoinName);
        if (res.Success)
        {
            foreach (var price in res.Data)
            {
                if (price.Symbol == CoinName)
                {
                    return price.Price;
                }
            }
        }
        return 0;
    }
    public static async Task<bool> ChangeLeverageAsync(string apiKey, string apiSecret, String CoinName)
    {
        var client = new BinanceRestClient(options =>
        {
            options.ApiCredentials = new ApiCredentials(apiKey, apiSecret);
        });
        var res = await client.CoinFuturesApi.Account.ChangeInitialLeverageAsync(CoinName, 10);
        return res.Success;
    }
    public static async Task<decimal> GetUSDTBalanceAsync(string apiKey, string apiSecret)
    {
        try
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(apiKey, apiSecret);
            });
            var result = await client.UsdFuturesApi.Account.GetBalancesAsync();
            if (result.Success)
            {
                var usdtBalance = result.Data;
                foreach (var balance in usdtBalance)
                {
                    if (balance.Asset == "USDT")
                    {
                        return balance.AvailableBalance;
                    }
                }
            }
            return 0; // Eğer USDT bulunamazsa veya başka bir hata olursa
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 0;
        }
    }
    public static async Task<bool> BuyLossProtect(string apiKey, string apiSecretKey, string coinName, decimal quantity, decimal price, decimal stopPrice)
    {
        try
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(apiKey, apiSecretKey);
            });
            // Kaldıraç ayarla
            var leverageResult = await ChangeLeverageAsync(apiKey, apiSecretKey, coinName);
            while (!leverageResult)
            {
                leverageResult = await ChangeLeverageAsync(apiKey, apiSecretKey, coinName);
            }

            // Zarar durdurma emri oluştur
            var orderResult = await client.UsdFuturesApi.Trading.PlaceOrderAsync(coinName,
                                                                            OrderSide.Sell,
                                                                            FuturesOrderType.Stop,
                                                                            quantity,
                                                                            price: price,
                                                                            stopPrice: stopPrice,
                                                                            timeInForce: TimeInForce.GoodTillDate);

            if (!orderResult.Success)
                return false;

            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public static async Task<bool> Buy(string apiKey, string apiSecretKey, string coinName, decimal quantity)
    {
        try
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(apiKey, apiSecretKey);
            });

            // Kaldıraç ayarla
            var leverageResult = await ChangeLeverageAsync(apiKey, apiSecretKey, coinName);
            while (!leverageResult)
            {
                leverageResult = await ChangeLeverageAsync(apiKey, apiSecretKey, coinName);
            }

            // Fiyatı al
            decimal price = await GetPrice(apiKey, apiSecretKey, coinName);

            // Alım emri oluştur
            var orderResult = await client.UsdFuturesApi.Trading.PlaceOrderAsync(coinName,
                                                                                 Binance.Net.Enums.OrderSide.Buy,
                                                                                 Binance.Net.Enums.FuturesOrderType.Limit,
                                                                                 quantity,
                                                                                 price, 
                                                                                 timeInForce: Binance.Net.Enums.TimeInForce.GoodTillDate);

            if (!orderResult.Success)
                return false;

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public static async Task<bool> Sell(string apiKey, string apiSecretKey, string coinName, decimal quantity)
    {
        try
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(apiKey, apiSecretKey);
            });

            // Kaldıraç ayarla
            var leverageResult = await ChangeLeverageAsync(apiKey, apiSecretKey, coinName);
            while (!leverageResult)
            {
                leverageResult = await ChangeLeverageAsync(apiKey, apiSecretKey, coinName);
            }

            // Fiyatı al
            decimal price = await GetPrice(apiKey, apiSecretKey, coinName);

            // Alım emri oluştur
            var orderResult = await client.UsdFuturesApi.Trading.PlaceOrderAsync(coinName,
                                                                                 Binance.Net.Enums.OrderSide.Sell,
                                                                                 Binance.Net.Enums.FuturesOrderType.Limit,
                                                                                 quantity,
                                                                                 price,
                                                                                 timeInForce: Binance.Net.Enums.TimeInForce.GoodTillDate);

            if (!orderResult.Success)
                return false;

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public static async Task<bool> SellLossProtect(string apiKey, string apiSecretKey, string coinName, decimal quantity, decimal price, decimal stopPrice)
    {
        try
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(apiKey, apiSecretKey);
            });
            // Kaldıraç ayarla
            var leverageResult = await ChangeLeverageAsync(apiKey, apiSecretKey, coinName);
            while (!leverageResult)
            {
                leverageResult = await ChangeLeverageAsync(apiKey, apiSecretKey, coinName);
            }

            // Zarar durdurma emri oluştur
            var orderResult = await client.UsdFuturesApi.Trading.PlaceOrderAsync(coinName,
                                                                            OrderSide.Buy,
                                                                            FuturesOrderType.Stop,
                                                                            quantity,
                                                                            price: price,
                                                                            stopPrice: stopPrice,
                                                                            timeInForce: TimeInForce.GoodTillDate);

            if (!orderResult.Success)
                return false;

            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public static async Task<WebCallResult<IEnumerable<BinanceFuturesOrder>>> GetOrder(string apiKey, string apiSecretKey, string coinName)
    {
        try
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(apiKey, apiSecretKey);
            });

            var res = await client.UsdFuturesApi.Trading.GetOpenOrdersAsync();

            if (!res.Success)
                return null;

            return res;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
}