/* Date: 15/02/2024
 * Author: Leonardo Trevisan Silio
 */
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RiotApi;

/// <summary>
/// Represents a ApiKey to Riot Developer Plataform.
/// </summary>
public class ApiKey
{
    internal string key;

    public static ApiKey FromString(string apiKey)
        => new() { key = apiKey };
    
    public static async Task<ApiKey> FromEnv(string envPath)
    {
        var lines = await File.ReadAllLinesAsync(envPath);
        var apiKey = lines
            .Select(extractKeyValue)
            .Select(pair => new {
                pair.value,
                similarity = apiKeySimilarity(pair.key)
            })
            .OrderByDescending(obj => obj.similarity)
            .FirstOrDefault()?
            .value;
        
        return FromString(apiKey);
    }

    public static async Task<ApiKey> FromEnv()
        => await FromEnv(".env");

    private static (string key, string value) extractKeyValue(string value)
    {
        if (!value.Contains('='))
            return (string.Empty, value);
        
        var parts = value.Split("=");
        return (parts[0], string.Concat(parts[1..]));
    }

    private static float apiKeySimilarity(string key)
    {
        int similarity = 0;
        var lowerKey = key.ToLower();

        if (key == string.Empty)
            similarity++;

        if (lowerKey.Contains("riot"))
            similarity++;
        
        if (lowerKey.Contains("lol"))
            similarity++;
        
        if (lowerKey.Contains("api"))
            similarity += 2;
        
        if (lowerKey.Contains("key"))
            similarity += 2;
        
        return similarity;
    }
}