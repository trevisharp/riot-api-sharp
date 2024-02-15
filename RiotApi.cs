/* Date: 15/02/2024
 * Author: Leonardo Trevisan Silio
 */
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RiotApi;

using Exceptions;

public class RiotApi
{
    private readonly HttpClient http;

    public readonly string ApiKey;

    public RiotApi(ApiKey apiKey, Region region)
    {
        this.ApiKey = apiKey.key;
        var service = region switch
        {
            Region.Americas => "americas",
            Region.Asia => "asia",
            Region.Europe => "europe",
            Region.Esports => "esports",
            _ => throw new InvalidRegionException()
        };
        var url = $"https://{service}.api.riotgames.com/";
        this.http = new HttpClient{
            BaseAddress = new Uri(url)
        };
    }
}