using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Dis;
using Objs;
using UnityEngine;
using Utils;

namespace Utils {
    public class StringUtils {
        public static void doSample () {
            string strFormated = string.Format ("最大值: {1:N0}   平均值: {2:N1}", 12, 11.5);
        }

        public static bool fastEqual (string a_, string b_) {
            if (String.CompareOrdinal (a_, b_) == 0) {
                return true;
            }
            return false;
        }
        //A是不是以B结束
        public static bool isAEndWithB (string a_, string b_) {
            if (a_.IndexOf (b_) > 0) {
                Char[] _chars = b_.ToCharArray ();
                string[] _nameSplit = a_.Split (_chars);
                if (String.CompareOrdinal (_nameSplit[_nameSplit.Length - 1], "") == 0) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        public static string[] splitAWithB (string a_, string b_) {
            Char[] _bChars = b_.ToCharArray ();
            string[] _aList = a_.Split (_bChars);
            return _aList;
        }

        /// <summary>
        /// 判断一个字符串是否为合法整数(不限制长度)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsInteger (string s) {
            string pattern = @"^\d*$";
            return Regex.IsMatch (s, pattern);
        }
        /// <summary>
        /// 判断一个字符串是否为合法数字(0-32整数)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsNumber (string s) {
            return IsNumber (s, 32, 0);
        }
        /**/ /// <summary>
        /// 判断一个字符串是否为合法数字(指定整数位数和小数位数)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="precision">整数位数</param>
        /// <param name="scale">小数位数</param>
        /// <returns></returns>
        public static bool IsNumber (string s, int precision, int scale) {
            if ((precision == 0) && (scale == 0)) {
                return false;
            }
            string pattern = @"(^\d{1," + precision + "}";
            if (scale > 0) {
                pattern += @"\.\d{0," + scale + "}$)|" + pattern;
            }
            pattern += "$)";
            return Regex.IsMatch (s, pattern);
        }

        public static Color GetColor (string color) {
            if (color.Length == 0) {
                return Color.black; //设为黑色
            } else {
                //#ff8c3 除掉#
                color = color.Substring (1);
                int v = int.Parse (color, System.Globalization.NumberStyles.HexNumber);
                //转换颜色
                return new Color (
                    //int>>移位 去低位
                    //&按位与 去高位
                    ((float) (((v >> 16) & 255))) / 255,
                    ((float) ((v >> 8) & 255)) / 255,
                    ((float) ((v >> 0) & 255)) / 255
                );
            }
        }
        public static bool isAContainsB (string a_, string b_, StringComparison comp_ = StringComparison.OrdinalIgnoreCase) {
            return a_.IndexOf (b_, comp_) >= 0;
        }
        public static bool isAEqualB (string a_, string b_, StringComparison comp_ = StringComparison.OrdinalIgnoreCase) {
            return String.Equals(a_,b_,comp_);
        }
    }
}