using System.Security.Cryptography;
using System.Text;

namespace PromotionCodeGenerator;

public class CodeGenerator : IDisposable
{
    private static readonly char[] CharacterSet = "ACDEFGHKLMNPRTXYZ234579".ToCharArray();
    private const int BodyPartLength = 7;
    private const int FullPartLength = 8;

    public string GeneratePromotionCode()
    {
        var body = GenerateRandomPromotionCodeBody();
        var checksum = GetChecksum(body);
        
        Console.WriteLine($"Generated code: {body}{checksum} Is Valid: {IsPromotionCodeValid(body + checksum)}");
        return $"{body}{checksum}";
    }

    private string GenerateRandomPromotionCodeBody()
    {
        var bytes = new byte[BodyPartLength];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        
        var chars = new char[BodyPartLength];
        for (var index = 0; index < BodyPartLength; index++)
        {
            chars[index] = CharacterSet[bytes[index] % CharacterSet.Length];
        }
        return new string(chars);
    }
    
    private char GetChecksum(string value)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
        var checksumIndex = (hash[0] + hash[1] * 256) % CharacterSet.Length;
        return CharacterSet[checksumIndex];
    }

    public bool IsPromotionCodeValid(string code)
    {
        if (string.IsNullOrEmpty(code) || code.Length != FullPartLength)
        {
            return false;
        }

        var codeBody = code.Substring(0, BodyPartLength);
        var expectedChecksum = GetChecksum(codeBody);
        return code[^1] == expectedChecksum;
    }

    public void GeneratePromotionCodeWithQuantity(int quantity = 50)
    {
        for (var index = 0; index < quantity; index++)
        {
            GeneratePromotionCode();
        }
    }

    public void Dispose()
    {
    }
}