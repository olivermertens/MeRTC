namespace MeRtc.Base
{
    public class SrtpPolicy
    {
        /// <summary>
        /// Null Cipher, does not change the content of RTP payload
        /// </summary>
        public const int NULL_ENCRYPTION = 0;
        /// <summary>
        /// Counter Mode AES Cipher, defined in Section 4.1.1, RFC3711
        /// </summary>
        public const int AESCM_ENCRYPTION = 1;
        /// <summary>
        /// Galois/Counter Mode AES Cipher, defined in RFC 7714
        /// </summary>
        public const int AESGCM_ENCRYPTION = 5;
        /// <summary>
        /// Counter Mode TwoFish Cipher
        /// </summary>
        public const int TWOFISH_ENCRYPTION = 3;
        /// <summary>
        /// F8 mode AES Cipher, defined in Section 4.1.2, RFC 3711
        /// </summary>
        public const int AESF8_ENCRYPTION = 2;
        /// <summary>
        /// F8 Mode TwoFish Cipher
        /// </summary>
        public const int TWOFISHF8_ENCRYPTION = 4;
        /// <summary>
        /// Null Authentication, no authentication.   
        /// This should be set if GCM or other AEAD encryption is used.
        /// </summary>
        public const int NULL_AUTHENTICATION = 0;
        /// <summary>
        /// HMAC SHA1 Authentication, defined in Section 4.2.1, RFC3711
        /// </summary>
        public const int HMACSHA1_AUTHENTICATION = 1;
        /// <summary>
        /// Skein Authentication
        /// </summary>
        public const int SKEIN_AUTHENTICATION = 2;


        /// <summary>
        /// SRTP encryption type.
        /// </summary>  
        public int EncType { get; set; }

        /// <summary>
        /// SRTP encryption key length.
        /// </summary>
        public int EncKeyLength { get; set; }

        /// <summary>
        /// SRTP authentication type.
        /// </summary>
        public int AuthType { get; set; }

        /// <summary>
        /// SRTP authentication key length.
        /// </summary>
        public int AuthKeyLength { get; set; }

        /// <summary>
        /// SRTP authentication tag length.  Also used for GCM tag.
        /// </summary>
        public int AuthTagLength { get; set; }

        /// <summary>
        /// SRTP salt key length
        /// </summary>
        public int SaltKeyLength { get; set; }

        /// <summary>
        /// Whether send-side replay protection is enabled
        /// </summary>
        public bool SendReplayEnabled { get; set; } = true;

        /// <summary>
        /// Whether receive-side replay protection is enabled
        /// </summary>
        public bool ReceiveReplayEnabled { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encType">SRTP encryption type</param>
        /// <param name="encKeyLength">SRTP encryption key length</param>
        /// <param name="authType">SRTP authentication type</param>
        /// <param name="authKeyLength">SRTP authentication key length</param>
        /// <param name="authTagLength">SRTP authentication tag length</param>
        /// <param name="saltKeyLength">SRTP salt key length</param>
        public SrtpPolicy(int encType, int encKeyLength, int authType, int authKeyLength, int authTagLength, int saltKeyLength)
        {
            EncType = encType;
            EncKeyLength = encKeyLength;
            AuthType = authType;
            AuthKeyLength = authKeyLength;
            AuthTagLength = authTagLength;
            SaltKeyLength = saltKeyLength;
        }
    }
}