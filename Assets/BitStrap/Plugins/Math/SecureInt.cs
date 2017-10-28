using System.Collections;
using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// <para>Use this class if you want to protect a sensitive value in your game from Memory Modifying attacks, such as memory lock.
    ///
    /// This does not fully protect your game against malicious attacker, though, it justs makes it more difficult.</para>
    ///
    /// <para>Performance impact is quite small, however, use it only on sensitive informations like Health or currency, for instance.</para>
    ///
    /// A downside of this class is that you can't change it's values while in play mode through the inspector, as it's encrypted during play.
    /// </summary>
    [System.Serializable]
    public class SecureInt
    {
        private static System.Random privateKeyGenerator = new System.Random();

        [SerializeField]
        private int value;

        private int privateKey;
        private bool isEncrypted = false;

        /// <summary>
        /// The decrypted value. Use this property to read and assign values.
        /// </summary>
		public int Value
        {
            get
            {
                TryEncrypt();
                return Xor( BitHelper.UnshuffleBits( value, privateKey ) );
            }
            set
            {
                this.value = BitHelper.ShuffleBits( Xor( value ), privateKey );
                isEncrypted = true;
            }
        }

        /// <summary>
        /// The raw and encrypted value that is stored in memory.
        /// </summary>
		public int EncryptedValue
        {
            get { return value; }
        }

        public SecureInt( int intValue = 0 )
        {
            privateKey = Mathf.Abs( privateKeyGenerator.Next() );
            value = intValue;
        }

        public static bool operator ==( SecureInt a, SecureInt b )
        {
            return a.Value == b.Value;
        }

        public static bool operator !=( SecureInt a, SecureInt b )
        {
            return a.Value != b.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals( object obj )
        {
            SecureInt other = obj as SecureInt;
            return other != null && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        private void TryEncrypt()
        {
            if( !isEncrypted )
            {
                Value = value;
            }
        }

        private int Xor( int v )
        {
            return v ^ privateKey;
        }
    }
}
