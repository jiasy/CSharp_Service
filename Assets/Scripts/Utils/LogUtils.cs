using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Utils {
    public class LogUtils {
        public static void printDict(Dictionary<string, JsonData> dict_) {
            string logStr = "";
            foreach (KeyValuePair<string,JsonData> _keyValue in dict_) {
                logStr = logStr + (_keyValue.Key + " = " + _keyValue.Value.ToJson ()) + System.Environment.NewLine;
            }
            Debug.Log (logStr);
        }
        public static void printArrayList (ArrayList arrlist_) {
            string logStr = "";
            for (int _idx = 0; _idx < arrlist_.Count; _idx++) {
                logStr = logStr + arrlist_[_idx].ToString () + System.Environment.NewLine;
            }
            Debug.Log (logStr);
        }
    }
}