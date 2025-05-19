using System.Security.Cryptography;

public static class RSAKeyProvider {
    /// <summary>
    /// Generates a new RSA private key
    /// </summary>
    /// <returns>
    /// New RSA private key
    /// </returns>
    public static RSA GetPrivateKey() {
        string privateKeyText = File.ReadAllText("private.pem");
        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyText.ToCharArray());
        return rsa;
    }

    /// <summary>
    /// Generates a new RSA public key
    /// </summary>
    /// <returns>
    /// New RSA public key
    /// </returns>
    public static RSA GetPublicKey() {
        string publicKeyText = File.ReadAllText("public.pem");
        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyText.ToCharArray());
        return rsa;
    }
}