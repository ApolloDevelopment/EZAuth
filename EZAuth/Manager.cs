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
            for(int i = 0; i < length; i++)
            {
                int range = rnd.Next(0, seed.Length); // Random number within the range of index's of 'seed'
                SerialNumber += seed[range];
            }

            return SerialNumber;
        }

        private void checkFile()
        {
            //check if keyPath file exists, if not, create it
            if (!File.Exists(keyPath))
            {
                File.Create(keyPath).Close();
            }
            
        }

        private void SetLocalSN(string rawSN)
        {
            File.WriteAllText(keyPath, encryption.Encrypt(rawSN));
        }

        private string GetLocalSN(string dbSN)
        {
            checkFile();
            string key = File.ReadAllText(keyPath);

            // POSSIBLE VULNERABILITY: The end user can delete their key file and it will create a new one with the updated encrypted serial number.
            if (key == "")
            {
                SetLocalSN(dbSN);
                key = File.ReadAllText(keyPath);
            }

            return encryption.Decrypt(key);
        }

        /// <summary>
        /// Matches local Serial Number log to Database. If one doesn't exist, it will create it. If Serial's match, will return true.
        /// </summary>
        public bool MatchLocalSN(string dbSN)
        {
            // C:\temp\EZAuth.key

            // TODO - get local SN using GetLocalSN() and compare it to dbSN.
            // GetLocalSN(dbSN) will be used and we pass in the dbSN in case we have to create the file
            return true;
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
    }
}
