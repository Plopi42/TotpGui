using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using TotpGui.Crypto;

namespace TotpGui
{
    class ProgramData
    {
        private const string REG_KEY = "Software\\totpGui";
        private const string MAGIC   = "Ce mot de passe est correct";

        private static Dictionary<string, string> totpList = null;
        private static string password = null;

        public static List<string> getTotpTitles()
        {
            if ((totpList == null) && !loadTotpFromReg())
                return null;

            return totpList.Keys.ToList();
        }

        public static string getTotpSecret(string name)
        {
            if (totpList == null)
                return null;

            return totpList[name];
        }

        public static bool addTotp(string name, string value)
        {
            if (!Base32.isBase32(value))
                return false;

            if ((totpList != null) && totpList.ContainsKey(name))
                return false;

            if (!addTotpToReg(name, value))
                return false;

            if (totpList == null)
                totpList = new Dictionary<string, string>();

            totpList[name] = value;

            return true;
        }

        public static bool removeTotp(string name)
        {
            return removeTotpFromReg(name) && totpList.Remove(name);
        }

        private static bool removeTotpFromReg(string name)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REG_KEY + "\\totps", true))
            {
                if (key == null)
                    return false;

                key.DeleteValue(name, false);
            }

            return true;
        }

        private static bool addTotpToReg(string name, string value)
        {
            if (password == null)
                return false;

            try
            {
                Registry.SetValue("HKEY_CURRENT_USER\\" + REG_KEY + "\\totps", name, Encryption.encrypt(password, value));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool loadTotpFromReg()
        {
            if (password == null)
                return false;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REG_KEY + "\\totps"))
            {
                if (key == null)
                    return false;

                string[] valueNames = key.GetValueNames();

                if (valueNames.Length <= 0)
                    return false;

                totpList = new Dictionary<string, string>();

                foreach (string currSubKey in valueNames)
                {
                    string value = (string)key.GetValue(currSubKey);
                    totpList[currSubKey] = Encryption.decrypt(password, value);
                }
                key.GetValueNames();
            }
            return true;
        }

        public static bool setPassword(string passwd)
        {
            byte[] hash     = Encryption.sha256(passwd, 42);
            string encMagic = Encryption.encrypt(hash, MAGIC);

            try
            {
                Registry.SetValue("HKEY_CURRENT_USER\\" + REG_KEY, "passwd", encMagic);
                password = passwd;
                return true;
            }
            catch
            {
                return false;
            }
        }

        // -1 : Not present
        //  0 : KO
        //  1 : OK
        public static int checkPassword(string passwd)
        {
            string regMagic = (string)Registry.GetValue("HKEY_CURRENT_USER\\" + REG_KEY, "passwd", "NotPresent");
            if ((regMagic == null) || (regMagic == "NotPresent"))
                return -1;

            byte[] hash     = Encryption.sha256(passwd, 42);
            string decMagic = Encryption.decrypt(hash, regMagic);

            if (MAGIC != decMagic)
                return 0;

            password = passwd;
            return 1;
        }

        public static bool hasPassword()
        {
            string regMagic = (string)Registry.GetValue("HKEY_CURRENT_USER\\" + REG_KEY, "passwd", "NotPresent");
            if ((regMagic == null) || (regMagic == "NotPresent"))
                return false;

            return true;
        }
    }
}
