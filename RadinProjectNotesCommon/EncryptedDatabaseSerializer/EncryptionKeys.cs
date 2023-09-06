using System;

namespace RadinProjectNotesCommon.EncryptedDatabaseSerializer
{
    /// <summary>
    /// Encryption keys to be used by the serializer.
    /// </summary>
    public class EncryptionKeys
    {
        private static byte[] _desKey;
        private static byte[] _desIV;
        private static bool _initialized;

        /// <summary>
        /// Set the encryption keys for the serializer.
        /// </summary>
        /// <param name="desKey"></param>
        /// <param name="desIV"></param>
        public static void SetEncryptionKeys(byte[] desKey, byte[] desIV)
        {
            _desKey = desKey;
            _desIV = desIV;
            _initialized = true;
        }

        /// <summary>
        /// DES service encryption key.
        /// </summary>
        public static byte[] DesKey
        {
            get
            {
                if (!_initialized)
                {
                    throw new Exception("The encryption keys have not been initialized."); ;
                }
                return _desKey;
            }
        }

        /// <summary>
        /// DES service IV key.
        /// </summary>
        public static byte[] DesIV
        {
            get
            {
                if (!_initialized)
                {
                    throw new Exception("The encryption keys have not been initialized."); ;
                }
                return _desIV;
            }
        }

    }
}
