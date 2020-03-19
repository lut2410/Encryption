using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;

public static class EncryptionHelper
{
    static string ENCRYPTION_KEY = "XvH+Wklv+ciU8qJjLfXQ6OitR1w4ZJjp0DmookOrEvE=";//need set in web,appconfig

    /// <summary>
    /// Encrypt plain text using AES algorithm
    /// </summary>
    /// <param name="plainText">The text need to be encrypt</param>
    /// <returns>An tuple contains cipher text, and symmetric encryptor obj(key, initialization vector)</returns>
    public static Tuple<byte[], byte[]> EncryptStringToBytes_Aes(string plainText)
    {

        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");

        byte[] aesKey = Convert.FromBase64String(ENCRYPTION_KEY);
        byte[] encrypted;
        byte[] aesIV;

        // Create an AesCryptoServiceProvider object
        // with the specified key and IV.
        using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
        {
            aesKey = aesAlg.Key = Convert.FromBase64String(ENCRYPTION_KEY);
            aesIV = aesAlg.IV;
            var xx =  Convert.ToBase64String(aesKey);
            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesKey, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return new Tuple<byte[], byte[]>(encrypted,aesIV);
    }

    /// <summary>
    /// Decrypt cipherText using AES algorithm
    /// </summary>
    /// <param name="cipherText">The text need to be decrypt</param>
    /// <param name="aesKey">Key of symmetric encryptor obj</param>
    /// <param name="aesIV">Initialization vector of symmetric encryptor obj</param>
    /// <returns>The original text</returns>
    public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] aesIV)
    {
        var aesKey = Convert.FromBase64String(ENCRYPTION_KEY);
        // Validate arguments
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (aesKey == null || aesKey.Length <= 0)
            throw new ArgumentNullException("Key");
        if (aesIV == null || aesIV.Length <= 0)
            throw new ArgumentNullException("IV");

        string plaintext;

        // Create an AesCryptoServiceProvider object
        // with the specified key and IV.
        using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
        {
            aesAlg.Key = aesKey;
            aesAlg.IV = aesIV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }

    /// <summary>
    /// Encrypt plain text 
    /// </summary>
    /// <param name="plainText">The text need to be encrypt</param>
    /// <returns>An string following format {Cipher Text}{AES Key}{AES IV}</returns>
    public static string Encrypt(string plainText)
    {
        var encryptTuple = EncryptStringToBytes_Aes(plainText);
        return string.Format("{0}.{1}", Convert.ToBase64String(encryptTuple.Item1),
            Convert.ToBase64String(encryptTuple.Item2));
    }

    /// <summary>
    /// Decrypt cipher text 
    /// </summary>
    /// <param name="cipherText">The text need to be decrypt</param>
    /// <returns>An string</returns>
    public static string Decrypt(string cipherText)
    {
        var encryptTuple = cipherText.Split('.'); //trust encryptTuple is an array contains 3 elements, not need to be validated for reducing cost
        return DecryptStringFromBytes_Aes(Convert.FromBase64String(encryptTuple[0]),
            Convert.FromBase64String(encryptTuple[1])); ;
    }
}