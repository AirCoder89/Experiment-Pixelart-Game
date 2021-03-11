using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AirCoder.Extensions
{
    public static class StringExtensions 
    {
        private static StringComparison IgnoreCase { get; set; } = StringComparison.OrdinalIgnoreCase;
        private static Regex _validEmailRegex;
        
        public static string GenerateString(int inLength,bool includeNumeric)
        {
            var idChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var idNum = "0123456789";
            var val = string.Empty;
            var src = includeNumeric ? idChar + idNum : idChar;
            for (var i = 0; i < inLength; i++)
                val += src[UnityEngine.Random.Range(0, src.Length)];
            return val;
        }
            
        public static string TrimWhiteSpace( this string src )
        {
            if ( string.IsNullOrEmpty( src ) )
                return string.Empty;

            return src.Trim();
        }
        
        public static bool IsNullOrEmpty( this string src )
        {
            return string.IsNullOrEmpty( src );
        }
        
        public static bool StartsWithIgnoreCase( this string src, string val )
        {
            if ( string.IsNullOrEmpty( src ) || string.IsNullOrEmpty( val ) )
                return false;

            return src.StartsWith( val, IgnoreCase );
        }
        
        public static int ToInt( this string src, int defaultValue = default )
        {
            if ( string.IsNullOrEmpty( src ) )
                return defaultValue;

            if ( int.TryParse( src, out var result ) )
                return result;

            return defaultValue;
        }
        
        public static bool IsValidEmailFormat( this string src )
        {
            _validEmailRegex = CreateValidEmailRegex();
            return _validEmailRegex.IsMatch(src);
        }
        
        private static Regex CreateValidEmailRegex()
        {
            var validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                    + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                    + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        
        ///-----------------------------------------------------------------------------------------
        /// <summary>
        /// Determines whether 2 specified T:String objects have the same value ignoring case,
        /// uses StringComparison.OrdinalIgnoreCase
        /// </summary>
        /// <param name="src">the string to compare</param>
        /// <param name="val">the value to compare</param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase( this string src, string val )
        {
            if ( string.IsNullOrEmpty( src ) )
            {
                if ( string.IsNullOrEmpty( val ) )
                    return true;
            }

            if ( string.IsNullOrEmpty( val ) )
                return false;

            return string.Equals( src, val, IgnoreCase );
        }
        
        ///-----------------------------------------------------------------------------------------
        /// <summary>
        /// Returns a value indicating whether the specified String object occurs within this string ignoring case,
        /// uses StringComparison.OrdinalIgnoreCase
        /// </summary>
        /// <param name="src">the string to compare</param>
        /// <param name="val">the value to finde</param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase( this string src, string val )
        {
            if ( string.IsNullOrEmpty( src ) || string.IsNullOrEmpty( val ) )
                return false;

            return src.IndexOf( val, IgnoreCase ) >= 0;
        }
        
        /// <summary>
        ///     A string extension method that concatenate with.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>A string.</returns>
        public static string ConcatWith(this string @this, params string[] values)
        {
            return string.Concat(@this, string.Concat(values));
        }
        
        /// <summary>
        ///     A string extension method that query if '@this' contains any values.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it contains any values, otherwise false.</returns>
        public static bool ContainsAny(this string @this, params string[] values)
        {
            foreach (string value in values)
            {
                if (@this.IndexOf(value) != -1)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        ///     A string extension method that query if '@this' contains any values.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it contains any values, otherwise false.</returns>
        public static bool ContainsAny(this string @this, StringComparison comparisonType, params string[] values)
        {
            foreach (string value in values)
            {
                if (@this.IndexOf(value, comparisonType) != -1)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        ///     A string extension method that extracts the letter described by @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted letter.</returns>
        public static string ExtractLetter(this string @this)
        {
            return new string(@this.ToCharArray().Where(x => Char.IsLetter(x)).ToArray());
        }
        
        /// <summary>
        ///     A string extension method that extracts the number described by @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted number.</returns>
        public static string ExtractNumber(this string @this)
        {
            return new string(@this.ToCharArray().Where(x => Char.IsNumber(x)).ToArray());
        }
        
        /// <summary>
        ///     A string extension method that get the string after the specified string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value to search.</param>
        /// <returns>The string after the specified value.</returns>
        public static string GetAfter(this string @this, string value)
        {
            if (@this.IndexOf(value) == -1)
            {
                return "";
            }
            return @this.Substring(@this.IndexOf(value) + value.Length);
        }
        
        /// <summary>
        ///     A string extension method that get the string before the specified string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value to search.</param>
        /// <returns>The string before the specified value.</returns>
        public static string GetBefore(this string @this, string value)
        {
            if (@this.IndexOf(value) == -1)
            {
                return "";
            }
            return @this.Substring(0, @this.IndexOf(value));
        }
        
        /// <summary>
        ///     A string extension method that get the string between the two specified string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="before">The string before to search.</param>
        /// <param name="after">The string after to search.</param>
        /// <returns>The string between the two specified string.</returns>
        public static string GetBetween(this string @this, string before, string after)
        {
            int beforeStartIndex = @this.IndexOf(before);
            int startIndex = beforeStartIndex + before.Length;
            int afterStartIndex = @this.IndexOf(after, startIndex);

            if (beforeStartIndex == -1 || afterStartIndex == -1)
            {
                return "";
            }

            return @this.Substring(startIndex, afterStartIndex - startIndex);
        }
        
        /// <summary>
        ///     A string extension method that query if '@this' is Alpha.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if Alpha, false if not.</returns>
        public static bool IsAlpha(this string @this)
        {
            return !Regex.IsMatch(@this, "[^a-zA-Z]");
        }
        
        /// <summary>
        ///     A string extension method that query if '@this' is Alphanumeric.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if Alphanumeric, false if not.</returns>
        public static bool IsAlphaNumeric(this string @this)
        {
            return !Regex.IsMatch(@this, "[^a-zA-Z0-9]");
        }
        
        /// <summary>
        ///     A string extension method that query if '@this' is numeric.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if numeric, false if not.</returns>
        public static bool IsNumeric(this string @this)
        {
            return !Regex.IsMatch(@this, "[^0-9]");
        }
        
        /// <summary>
        ///     A string extension method that return the left part of the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="length">The length.</param>
        /// <returns>The left part.</returns>
        public static string Left(this string @this, int length)
        {
            return @this.Substring(0, length);
        }
        
        /// <summary>
        ///     A string extension method that left safe.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="length">The length.</param>
        /// <returns>A string.</returns>
        public static string LeftSafe(this string @this, int length)
        {
            return @this.Substring(0, Math.Min(length, @this.Length));
        }
        
        /// <summary>
        ///     A string extension method that removes the letter described by @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        public static string RemoveLetter(this string @this)
        {
            return new string(@this.ToCharArray().Where(x => !Char.IsLetter(x)).ToArray());
        }
        
        /// <summary>
        ///     A string extension method that removes the number described by @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        public static string RemoveNumber(this string @this)
        {
            return new string(@this.ToCharArray().Where(x => !Char.IsNumber(x)).ToArray());
        }
        
        /// <summary>
        ///     A string extension method that repeats the string a specified number of times.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="repeatCount">Number of repeats.</param>
        /// <returns>The repeated string.</returns>
        public static string Repeat(this string @this, int repeatCount)
        {
            if (@this.Length == 1)
            {
                return new string(@this[0], repeatCount);
            }

            var sb = new StringBuilder(repeatCount*@this.Length);
            while (repeatCount-- > 0)
            {
                sb.Append(@this);
            }

            return sb.ToString();
        }
        
        /// <summary>A string extension method that replaces.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="value">The value.</param>
        /// <returns>A string.</returns>
        public static string Replace(this string @this, int startIndex, int length, string value)
        {
            @this = @this.Remove(startIndex, length).Insert(startIndex, value);

            return @this;
        }
        
        /// <summary>
        ///     A string extension method that replace all values specified by an empty string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>A string with all specified values replaced by an empty string.</returns>
        public static string ReplaceByEmpty(this string @this, params string[] values)
        {
            foreach (string value in values)
            {
                @this = @this.Replace(value, "");
            }

            return @this;
        }
        
        /// <summary>
        ///     A string extension method that reverses the given string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The string reversed.</returns>
        public static string Reverse(this string @this)
        {
            if (@this.Length <= 1)
            {
                return @this;
            }

            char[] chars = @this.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
        
        /// <summary>
        ///     A string extension method that return the right part of the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="length">The length.</param>
        /// <returns>The right part.</returns>
        public static string Right(this string @this, int length)
        {
            return @this.Substring(@this.Length - length);
        }
        
        /// <summary>
        ///     A string extension method that right safe.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="length">The length.</param>
        /// <returns>A string.</returns>
        public static string RightSafe(this string @this, int length)
        {
            return @this.Substring(Math.Max(0, @this.Length - length));
        }
        
        /// <summary>
        ///     Gets first character in string
        /// </summary>
        /// <param name="val">val</param>
        /// <returns>System.string</returns>
        public static string FirstCharacter(this string val)
        {
            return (!string.IsNullOrEmpty(val))
                ? (val.Length >= 1)
                    ? val.Substring(0, 1)
                    : val
                : null;
        }
        
        /// <summary>
        ///     Gets last character in string
        /// </summary>
        /// <param name="val">val</param>
        /// <returns>System.string</returns>
        public static string LastCharacter(this string val)
        {
            return (!string.IsNullOrEmpty(val))
                ? (val.Length >= 1)
                    ? val.Substring(val.Length - 1, 1)
                    : val
                : null;
        }
        
        /// <summary>
        ///     Validate email address
        /// </summary>
        /// <param name="email">string email address</param>
        /// <returns>true or false if email if valid</returns>
        public static bool IsEmailAddress(this string email)
        {
            string pattern =
                "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";
            return Regex.Match(email, pattern).Success;
        }
        
        /// <summary>
        ///     Encrypt a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="key">Encryption key</param>
        /// <returns>A string representing a byte array separated by a minus sign.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
        public static string Encrypt(this string stringToEncrypt, string key)
        {
            var cspParameter = new CspParameters {KeyContainerName = key};
            var rsaServiceProvider = new RSACryptoServiceProvider(cspParameter) {PersistKeyInCsp = true};
            byte[] bytes = rsaServiceProvider.Encrypt(Encoding.UTF8.GetBytes(stringToEncrypt), true);
            return BitConverter.ToString(bytes);
        }


        /// <summary>
        ///     Decrypt a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToDecrypt">String that must be decrypted.</param>
        /// <param name="key">Decryption key.</param>
        /// <returns>The decrypted string or null if decryption failed.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
        public static string Decrypt(this string stringToDecrypt, string key)
        {
            var cspParamters = new CspParameters {KeyContainerName = key};
            var rsaServiceProvider = new RSACryptoServiceProvider(cspParamters) {PersistKeyInCsp = true};
            string[] decryptArray = stringToDecrypt.Split(new[] {"-"}, StringSplitOptions.None);
            byte[] decryptByteArray = Array.ConvertAll(decryptArray,
                (s => Convert.ToByte(byte.Parse(s, NumberStyles.HexNumber))));
            byte[] bytes = rsaServiceProvider.Decrypt(decryptByteArray, true);
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }
        
    }
}