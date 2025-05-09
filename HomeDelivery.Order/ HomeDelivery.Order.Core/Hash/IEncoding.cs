using System.Security.Cryptography;
using System.Text;
using SimpleBase;

namespace HomeDelivery.Order.Core.Hash;

public interface IEncoding
{
    byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes);
    byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes);
    string Encrypt(string text);
    string Decrypt(string decryptedText);
    bool TryToDecrypt(string decryptedText);
    byte[] GetBytesToDecode(string text);
    byte[] GetRandomBytes();
    string DecryptionNormalizedEmail(string encriptedText);

}


public class EncodingService : IEncoding
{
    public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
    {
        byte[] encryptedBytes = null;
            
        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
    
        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.Close();
                }
                encryptedBytes = ms.ToArray();
            }
        }

        return encryptedBytes;
    }

    public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
    {
        byte[] decryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    cs.Close();
                }
                decryptedBytes = ms.ToArray();
            }
        }

        return decryptedBytes;
    }

    public string Encrypt(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }
        byte[] originalBytes = Encoding.UTF8.GetBytes(text);
        byte[] encryptedBytes = null;
        byte[] passwordBytes = Encoding.UTF8.GetBytes("EvENtUmONe!@dotkzJDIWL!Lqwe31c@3123fsaefkl465");
            
        // Hash the password with SHA256
        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            
        encryptedBytes = AES_Encrypt(originalBytes, passwordBytes);
            
        return Convert.ToBase64String(encryptedBytes);
    }

    public string Decrypt(string decryptedText)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(decryptedText))
            {
                return decryptedText;
            }
            var originalBytes = GetBytesToDecode(decryptedText);
            return Encoding.UTF8.GetString(originalBytes);
        }
        catch (Exception exception)
        {
            return decryptedText;
        }
    }

    public bool TryToDecrypt(string decryptedText)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(decryptedText))
            {
                return true;
            }
            var originalBytes = GetBytesToDecode(decryptedText);
            var text = Encoding.UTF8.GetString(originalBytes);
            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }

    public byte[] GetBytesToDecode(string text)
    {
        byte[] bytesToBeDecrypted = Convert.FromBase64String(text);
        byte[] passwordBytes = Encoding.UTF8.GetBytes("EvENtUmONe!@dotkzJDIWL!Lqwe31c@3123fsaefkl465");

        // Hash the password with SHA256
        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

        return decryptedBytes;
    }

    public byte[] GetRandomBytes()
    {
        int _saltSize = 4;
        byte[] ba = new byte[_saltSize];
        RNGCryptoServiceProvider.Create().GetBytes(ba);
        return ba;
    }

    public string DecryptionNormalizedEmail(string encriptedText)
    {
        //return encriptedText;
        if (encriptedText is not null)
        {
            var encriptedTextBytes = Base32.Crockford.Decode(encriptedText.Trim());
            string decryptedText = Encoding.UTF8.GetString(encriptedTextBytes);
            //return encriptedText;
            return decryptedText;
        }
        else { return encriptedText; }
    }
}

