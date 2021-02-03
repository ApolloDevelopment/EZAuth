using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;

namespace EZAuth
{
    public class Manager
    {
        private Encryption encryption = new Encryption();
        // Seed for all random string generators
        private string seed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
        private string keyPath = @"C:\temp\EZAuth.key";

        #region Public Methods


        /// <summary>
        /// Generates a unique Serial Number / Product ID
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        // Probably won't use this in C#. Will more than likely be handled in PHP
        public string GenerateSN(uint length)
        {
            string SerialNumber = "";
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                int range = rnd.Next(0, seed.Length); // Random number within the range of index's of 'seed'
                SerialNumber += seed[range];
            }

            return SerialNumber;
        }


        /// <summary>
        /// Matches local Serial Number log to Database. If one doesn't exist, it will create it. If Serial's match, will return true.
        /// </summary>
        public bool MatchLocalSN(string dbSN)
        {
            if (!File.Exists(keyPath)) return false;
            string encryptedKey = File.ReadAllText(keyPath);
            string decryptedKey = "";
            try
            {
                decryptedKey = encryption.Decrypt(encryptedKey);
            }
            catch { } // I don't want to do anything since decryptedKey is already initialized to an empty string

            return dbSN == decryptedKey;
        }


        /// <summary>
        /// Returns a List of type 'string' containing the system's Processor, Motherboard, and GPU
        /// </summary>
        /// <returns></returns>
        public List<string> GetSystemInfo()
        {
            List<string> systemInfo = new List<string>();

            string processor = GetComponent("Win32_Processor", "Name");
            string Mobo = GetComponent("Win32_BaseBoard", "Product");
            string GPU = GetComponent("Win32_VideoController", "Name");

            systemInfo.Add(processor);
            systemInfo.Add(Mobo);
            systemInfo.Add(GPU);

            return systemInfo;
        }

        public bool isFirstLaunch()
        {
            if(File.Exists(keyPath))
            {
                return File.ReadAllText(keyPath) == "";
            }

            return true;
        }

        #endregion
        #region Private Methods


        private void SetLocalSN(string rawSN)
        {
            File.WriteAllText(keyPath, encryption.Encrypt(rawSN));
        }

        private string GetLocalSN()
        {
            string key = "";
            string decryptedKey = "";
            if(File.Exists(keyPath))
            {
                key = File.ReadAllText(keyPath);
                if (key != "" && key != null)
                {
                    decryptedKey = encryption.Decrypt(key);
                }
            }

            return decryptedKey;
        }

        private string GetComponent(string hwClass, string identifier)
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM {hwClass}");
            string component = "";
            foreach(ManagementObject mo in mos.Get())
            {
                component = Convert.ToString(mo[identifier]);
            }
            return component;
        }


        #endregion


    }
}




/*
 * 
 * 1. isFirstLaunch() should run first to see if the EZAuth.key file exists, if returns false, should then trigger an event that requires a key to be input into a form to continue.
 * This will create the file and write the encrypted key to it.
 * 2. If not first launch, MatchLocalSN(dbSN) should be used to match the current local SN to the DB and if it doesn't match, will be required to re-enter the SN
 * 
 */
