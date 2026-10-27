using SelfKOrderWeb.Models;
using System.Security.Cryptography;
using System.Text;

namespace SelfKOrderWeb.Factory
{
    public class Security
    {
        static readonly char[] padding = { '=' };
        public static string Encrypt(string clearText,int kiosikID,int Brand,int Brandis,int subDid,string subid)
        {
            try
            {
                string EncryptionKey =  kiosikID + "" + Brand + ""+ Brandis + "" + subDid + "" + subid + "" + "RBSME@!$RTYEWQRGH";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray()).TrimEnd(padding).Replace('+', '-').Replace('/', '_');
                    }
                }
                return clearText;
            }
            catch
            {

            }
            return "";
        }

        public static string Decrypt(string cipherText, int kiosikID, int Brand, int Brandis, int subDid, string subid)
        {
            string incoming = cipherText.Replace('_', '/').Replace('-', '+');
            switch (cipherText.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            try
            {
                string EncryptionKey = kiosikID + "" + Brand + "" + Brandis + "" + subDid + "" + subid + "" + "RBSME@!$RTYEWQRGH";
                byte[] cipherBytes = Convert.FromBase64String(incoming);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch
            {

            }
            return "";
        }

    }
}
