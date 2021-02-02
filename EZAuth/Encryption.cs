using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAuth
{
    public class Encryption
    {

        /*
         * 
         * Copyright © 2021 Apollo Development. All rights reserved.
         * 
         * Attempts to reverse-engineer, leech, or exploit this encryption will result
         * in a $25,000 fine and federal prison. Do not resell, copy, redistribute, or
         * refactor these encryption methods. Thanks.
         * 
         *  - Apollo Team
         * 
         */

        // Regular characters (A-Z, 0-9)
        private char[] raw = { 
            'A', 
            'B',
            'C',
            'D', 
            'E', 
            'F', 
            'G', 
            'H', 
            'I', 
            'J', 
            'K',
            'L', 
            'M', 
            'N', 
            'O', 
            'P', 
            'Q', 
            'R',
            'S', 
            'T', 
            'U', 
            'V',
            'W', 
            'X', 
            'Y', 
            'Z', 
            '0',
            '1', 
            '2', 
            '3', 
            '4', 
            '5', 
            '6', 
            '7', 
            '8', 
            '9' 
        };

        // Ecryption Translation
        private static string[] translation = {
            "Y$2X",
            "8^H3",
            "*2X7",
            "70^X",
            "D$XH",
            "B$UU",
            "9X^A",
            "2K7G",
            "8H$G",
            "W7F5",
            "3IDY",
            "I$07",
            "2BTX",
            "6N^Z",
            "5&UX",
            "H*L3",
            "Y@XY",
            "2Y8D",
            "4^0D",
            "GH$P",
            "EXUV",
            "TU$H",
            "N&4S",
            "U8&D",
            "I0D&",
            "WY&G",
            "C8$Y",
            "AUXX",
            "&HJ5",
            "L&J8",
            "3G6D",
            "A$9G",
            "0X0D",
            "2YGD",
            "73$J",
            "L9$G"
        };

        /// <summary>
        /// Returns an encrypted version of a Serial Number
        /// </summary>
        /// <param name="RawSN"></param>
        /// <returns></returns>
        public string Encrypt(string RawSN)
        {
            string encryptedSN = "";
            for(int RawSNIndex = 0; RawSNIndex < RawSN.Length; RawSNIndex++)
            {
                for(int RawCharIndex = 0; RawCharIndex < raw.Length; RawCharIndex++)
                {
                   if(RawSN[RawSNIndex] == raw[RawCharIndex])
                    {
                        encryptedSN += translation[RawCharIndex];
                    }
                }
            }

            return encryptedSN;
        }

        /// <summary>
        /// Returns a decrypted version of an already-encrypted Serial Number
        /// </summary>
        /// <param name="EncryptedSN"></param>
        /// <returns></returns>
        public string Decrypt(string EncryptedSN)
        {
            // For every set of 4 characters, it should check for a match in the "translation" array
            // and then append the character at the corresponding index of the "raw" array to the decrypted SN.

            string decryptedSN = "";
            string currentCharSet = ""; // (i.e. "D$XH")
            // we init i to 1 instead of 0 so we can use the mod function (%) to get every 4 characters
            // and then just subtract 1 from i for the current index
            for (int i = 1; i <= EncryptedSN.Length; i++)
            {
                currentCharSet += EncryptedSN[i - 1];
                if (i % 4 == 0)
                {
                    for(int translationIndex = 0; translationIndex < translation.Length; translationIndex++)
                    {
                        if(currentCharSet == translation[translationIndex])
                        {
                            decryptedSN += raw[translationIndex];
                        }
                    }

                    // reset the charset AFTER we use that 4 characters to find the correlative "raw" value
                    currentCharSet = "";
                }
            }

            return decryptedSN;
        }

    }
}
