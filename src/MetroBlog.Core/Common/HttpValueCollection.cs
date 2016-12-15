using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.Runtime.Serialization;

namespace MetroBlog.Core.Common
{
    public class HttpValueCollection : NameValueCollection
    {
        private HashSet<string> _keysAwaitingValidation;
        private Action<string, string> _validationCallback;

        internal HttpValueCollection(string str, bool readOnly, bool urlencoded, Encoding encoding)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            if (!string.IsNullOrEmpty(str))
            {
                this.FillFromString(str, urlencoded, encoding);
            }
            base.IsReadOnly = readOnly;
        }


        internal void EnableGranularValidation(Action<string, string> validationCallback)
        {
            this._keysAwaitingValidation = new HashSet<string>(this.Keys.Cast<string>().Where<string>(new Func<string, bool>(HttpValueCollection.KeyIsCandidateForValidation)), StringComparer.OrdinalIgnoreCase);
            this._validationCallback = validationCallback;
            base.InvalidateCachedArrays();
        }

        private void EnsureKeyValidated(string key)
        {
            if ((this._keysAwaitingValidation != null) && this._keysAwaitingValidation.Contains(key))
            {
                string str = base.Get(key);
                if (!string.IsNullOrEmpty(str))
                {
                    this._validationCallback(key, str);
                }
                this._keysAwaitingValidation.Remove(key);
            }
        }

        internal void FillFromString(string s)
        {
            this.FillFromString(s, false, null);
        }

        internal void FillFromString(string s, bool urlencoded, Encoding encoding)
        {
            int num = (s != null) ? s.Length : 0;
            for (int i = 0; i < num; i++)
            {
                int startIndex = i;
                int num4 = -1;
                while (i < num)
                {
                    char ch = s[i];
                    if (ch == '=')
                    {
                        if (num4 < 0)
                        {
                            num4 = i;
                        }
                    }
                    else if (ch == '&')
                    {
                        break;
                    }
                    i++;
                }
                string str = null;
                string str2 = null;
                if (num4 >= 0)
                {
                    str = s.Substring(startIndex, num4 - startIndex);
                    str2 = s.Substring(num4 + 1, (i - num4) - 1);
                }
                else
                {
                    str2 = s.Substring(startIndex, i - startIndex);
                }
                if (!String.IsNullOrEmpty(str))
                {
                    if (urlencoded)
                    {
                        base.Add(UrlDecode(str, encoding), UrlDecode(str2, encoding));
                    }
                    else
                    {
                        base.Add(str, str2);
                    }
                }
                if ((i == (num - 1)) && (s[i] == '&'))
                {
                    base.Add(null, string.Empty);
                }
            }
        }

        public override string Get(int index)
        {
            string key = this.GetKey(index);
            this.EnsureKeyValidated(key);
            return base.Get(index);
        }

        public override string Get(string name)
        {
            this.EnsureKeyValidated(name);
            return base.Get(name);
        }

        public override string[] GetValues(int index)
        {
            string key = this.GetKey(index);
            this.EnsureKeyValidated(key);
            return base.GetValues(index);
        }

        public override string[] GetValues(string name)
        {
            this.EnsureKeyValidated(name);
            return base.GetValues(name);
        }

        internal static bool KeyIsCandidateForValidation(string key)
        {
            if ((key != null) && key.StartsWith("__", StringComparison.Ordinal))
            {
                return false;
            }
            return true;
        }

        internal void MakeReadOnly()
        {
            base.IsReadOnly = true;
        }

        internal void MakeReadWrite()
        {
            base.IsReadOnly = false;
        }

        internal void Reset()
        {
            base.Clear();
        }


        internal string UrlDecode(string value, Encoding encoding)
        {
            if (value == null)
            {
                return null;
            }
            int length = value.Length;
            UrlDecoder decoder = new UrlDecoder(length, encoding);
            for (int i = 0; i < length; i++)
            {
                char ch = value[i];
                if (ch == '+')
                {
                    ch = ' ';
                }
                else if ((ch == '%') && (i < (length - 2)))
                {
                    if ((value[i + 1] == 'u') && (i < (length - 5)))
                    {
                        int num3 = HexToInt(value[i + 2]);
                        int num4 = HexToInt(value[i + 3]);
                        int num5 = HexToInt(value[i + 4]);
                        int num6 = HexToInt(value[i + 5]);
                        if (((num3 < 0) || (num4 < 0)) || ((num5 < 0) || (num6 < 0)))
                        {
                            goto Label_010B;
                        }
                        ch = (char)((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6);
                        i += 5;
                        decoder.AddChar(ch);
                        continue;
                    }
                    int num7 = HexToInt(value[i + 1]);
                    int num8 = HexToInt(value[i + 2]);
                    if ((num7 >= 0) && (num8 >= 0))
                    {
                        byte b = (byte)((num7 << 4) | num8);
                        i += 2;
                        decoder.AddByte(b);
                        continue;
                    }
                }
                Label_010B:
                if ((ch & 0xff80) == 0)
                {
                    decoder.AddByte((byte)ch);
                }
                else
                {
                    decoder.AddChar(ch);
                }
            }
            return Utf16StringValidator.ValidateString(decoder.GetString());
        }

        int HexToInt(char h)
        {
            if ((h >= '0') && (h <= '9'))
            {
                return (h - '0');
            }
            if ((h >= 'a') && (h <= 'f'))
            {
                return ((h - 'a') + 10);
            }
            if ((h >= 'A') && (h <= 'F'))
            {
                return ((h - 'A') + 10);
            }
            return -1;
        }

        char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 0x30);
            }
            return (char)((n - 10) + 0x61);
        }

        bool IsUrlSafeChar(char ch)
        {
            if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= '0') && (ch <= '9')))
            {
                return true;
            }
            switch (ch)
            {
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                case '!':
                    return true;
            }
            return false;
        }

        string UrlEncodeSpaces(string str)
        {
            if ((str != null) && (str.IndexOf(' ') >= 0))
            {
                str = str.Replace(" ", "%20");
            }
            return str;
        }
        private class UrlDecoder
        {
            private int _bufferSize;
            private byte[] _byteBuffer;
            private char[] _charBuffer;
            private Encoding _encoding;
            private int _numBytes;
            private int _numChars;

            internal UrlDecoder(int bufferSize, Encoding encoding)
            {
                this._bufferSize = bufferSize;
                this._encoding = encoding;
                this._charBuffer = new char[bufferSize];
            }

            internal void AddByte(byte b)
            {
                if (this._byteBuffer == null)
                {
                    this._byteBuffer = new byte[this._bufferSize];
                }
                this._byteBuffer[this._numBytes++] = b;
            }

            internal void AddChar(char ch)
            {
                if (this._numBytes > 0)
                {
                    this.FlushBytes();
                }
                this._charBuffer[this._numChars++] = ch;
            }

            private void FlushBytes()
            {
                if (this._numBytes > 0)
                {
                    this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
                    this._numBytes = 0;
                }
            }

            internal string GetString()
            {
                if (this._numBytes > 0)
                {
                    this.FlushBytes();
                }
                if (this._numChars > 0)
                {
                    return new string(this._charBuffer, 0, this._numChars);
                }
                return string.Empty;
            }
        }

        internal static class Utf16StringValidator
        {
            private static readonly bool _skipUtf16Validation = true;
            private const char UNICODE_NULL_CHAR = '\0';
            private const char UNICODE_REPLACEMENT_CHAR = '�';

            public static string ValidateString(string input)
            {
                return ValidateString(input, _skipUtf16Validation);
            }

            internal static string ValidateString(string input, bool skipUtf16Validation)
            {
                if (skipUtf16Validation || string.IsNullOrEmpty(input))
                {
                    return input;
                }
                int num = -1;
                for (int i = 0; i < input.Length; i++)
                {
                    if (char.IsSurrogate(input[i]))
                    {
                        num = i;
                        break;
                    }
                }
                if (num < 0)
                {
                    return input;
                }
                char[] chArray = input.ToCharArray();
                for (int j = num; j < chArray.Length; j++)
                {
                    char c = chArray[j];
                    if (char.IsLowSurrogate(c))
                    {
                        chArray[j] = (char)0xfffd;
                    }
                    else if (char.IsHighSurrogate(c))
                    {
                        if (((j + 1) < chArray.Length) && char.IsLowSurrogate(chArray[j + 1]))
                        {
                            j++;
                        }
                        else
                        {
                            chArray[j] = (char)0xfffd;
                        }
                    }
                }
                return new string(chArray);
            }
        }


    }
}
