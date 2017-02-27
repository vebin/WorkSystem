namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string VerifyIsEmpty(this string val, string defVal, bool isEmptyThrowOut = false)
        {
            if (string.IsNullOrEmpty(val))
            {
                if (isEmptyThrowOut) throw new ArgumentNullException("val 不能为空");
                else return defVal;
            }
            else
            {
                return val;
            }
        }

        public static string Append(this string val, string appendStr, bool verifyIsNotEmpty, string removeEndsWithStr = null)
        {
            if (verifyIsNotEmpty)
            {
                if (!string.IsNullOrEmpty(val))
                {
                    if (!string.IsNullOrEmpty(removeEndsWithStr))
                    {
                        if (val.EndsWith(removeEndsWithStr))
                        {
                            string.Format("{0}", (val = val.Remove(val.Length - removeEndsWithStr.Length, removeEndsWithStr.Length)), appendStr);
                        }
                    }
                    return string.Format("{0}{1}", val, appendStr);
                }
                else
                {
                    return val;
                }
            }
            else
            {
                return string.Format("{0}{1}", val, appendStr);
            }
        }

        public static int ConvertToInt32(this string val, int defVal, bool isThrowOut = false)
        {
            int returnVal = -1;
            if (int.TryParse(val, out returnVal))
            {
                return returnVal;
            }
            else
            {
                if (isThrowOut) throw new InvalidCastException("类型转换失败");
                else return defVal;
            }
        }

        public static string SubString(this string v, int subLen, int startIndex = 0, string suffix = "...", bool throwOut = false, bool appendOverCode = false)
        {
            if (subLen < 0 || startIndex < 0) throw new ArgumentException("截取位置或截取长度不能小于0");
            if (v.IsEmpty(false)) return string.Empty;
            if (v.Length <= subLen || v.Length <= startIndex) return v;
            try
            {
                if (appendOverCode)
                    return v.Substring(startIndex, subLen) + suffix;
                else
                    return v.Substring(startIndex, subLen);
            }
            catch (Exception ex)
            {
                if (throwOut) throw;
                else return v;
            }
        }

        public static bool IsEmpty(this string s, bool isEmptyThowOut = false)
        {
            if (s == string.Empty || s == null)
            {
                if (!isEmptyThowOut)
                    return true;
                else
                    throw new ArgumentNullException(string.Format("{0} null", s));
            }
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim()))
            {
                if (!isEmptyThowOut)
                    return true;
                else
                    throw new ArgumentNullException(string.Format("{0} null", s));
            }
            return false;
        }

        public static string IsEmpty(this string s, string DefV)
        {
            return IsEmpty(s) ? DefV : s;
        }

        public static string GetIndexValue(this string[] v, int index, string defV = null)
        {
            if (v.IsEmpty<string>()) return defV;
            if (index < 0 || index > v.Count()) return defV;
            if (index == v.Count() - 1)
                return v[v.Count() - 1];
            else if (v.Count() == 1)
                return v[0];
            return v[index];
        }

        public static string SubString(this string val, string splitChar, int[] returnValueIndex, string defReturnVal = null, bool throwOut = false)
        {
            val.IsEmpty(throwOut);

            var splitArray = val.Split(new string[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
            var returnStr = new StringBuilder();
            for (var i = 0; i < returnValueIndex.Length; i++)
            {
                if (splitArray.Length > i)
                {
                    returnStr.Append(splitArray[i]);
                    if (i != returnValueIndex.Length - 1)
                        returnStr.Append(splitChar);
                }
                else
                {
                    if (throwOut) throw new IndexOutOfRangeException("索引超出范围");
                }
            }

            return returnStr.ToString();
        }

        public static bool IsMobile(this string val, bool notMatchThrowOut = false, string errMessage = null, string pattern = null)
        {
            if (string.IsNullOrEmpty(val))
            {
                if (notMatchThrowOut) throw new ArgumentException(errMessage);
                else return false;
            }
            var _pattern = @"^[1]+[3,4,5,7,8]+\d{9}";
            if (!string.IsNullOrEmpty(pattern))
                _pattern = pattern;
            if (Regex.IsMatch(val, _pattern)) return true;
            if (notMatchThrowOut)
                throw new ArgumentException(errMessage);
            else
                return false;
        }
        public static bool IsEMail(this string val, bool notMatchThrowOut = false, string errMessage = null, string pattern = null)
        {
            if (string.IsNullOrEmpty(val))
            {
                if (notMatchThrowOut) throw new ArgumentException(errMessage);
                else return false;
            }
            //^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$
            var _pattern = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            if (!string.IsNullOrEmpty(pattern))
                _pattern = pattern;
            if (Regex.IsMatch(val, _pattern)) return true;
            if (notMatchThrowOut)
                throw new ArgumentException(errMessage);
            else
                return false;
        }
        public static string AtUserPassword(string val, bool throwOut = false, string errMessage = null, string defRetun = null)
        {
            try
            {
                if (string.IsNullOrEmpty(val))
                {
                    if (throwOut) throw new ArgumentException(errMessage);
                    else return defRetun;
                }
                return new YmtSystem.CrossCutting.Utility.Encrypt().EncryptUserPassword(val);
            }
            catch (Exception ex)
            {
                if (throwOut)
                    throw;
                else return defRetun;
            }
        }
        public static string AtUserTradingPassword(string val, bool throwOut = false, string errMessage = null, string defRetun = null)
        {
            try
            {
                if (string.IsNullOrEmpty(val))
                {
                    if (throwOut) throw new ArgumentException(errMessage);
                    else return defRetun;
                }
                return new YmtSystem.CrossCutting.Utility.Encrypt().EncryptTradingPassword(val);
            }
            catch (Exception ex)
            {
                if (throwOut)
                    throw;
                else return defRetun;
            }
        }
    }
}
