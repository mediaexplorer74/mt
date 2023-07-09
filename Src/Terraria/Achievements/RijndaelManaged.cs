// RijndaelManaged

using System;
using System.Security.Cryptography;

namespace GameManager.Achievements
{
    internal class RijndaelManaged
    {
        internal PaddingMode Padding;

        //RnD
        public RijndaelManaged()
        {
            //
        }

        //RnD
        internal ICryptoTransform CreateDecryptor(byte[] numArray1, byte[] numArray2)
        {
            //throw new NotImplementedException();
            return default;
        }

        //RnD
        internal ICryptoTransform CreateEncryptor(byte[] numArray1, byte[] numArray2)
        {
            //throw new NotImplementedException();
            return default;
        }
    }
}