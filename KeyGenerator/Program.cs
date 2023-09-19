using System.Security.Cryptography;
using System.Text.Json;

using var key = RSA.Create();
var privateKey = Convert.ToBase64String(key.ExportRSAPrivateKey());
var publicKey = Convert.ToBase64String(key.ExportRSAPublicKey());

string fileName = "rsaKey.json";
if (File.Exists(fileName))
{
    File.Delete(fileName);
}

Console.WriteLine("public:");
Console.WriteLine(publicKey);
Console.WriteLine("private:");
Console.WriteLine(privateKey);

var json = JsonSerializer.Serialize(new
{
    PublicKeyString = publicKey,
    PrivateKeyString = privateKey
}, new JsonSerializerOptions { WriteIndented = true });

File.WriteAllText("rsaKey.json", json);
Console.ReadKey();