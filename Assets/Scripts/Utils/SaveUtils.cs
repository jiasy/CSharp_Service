using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using LitJson;
using Objs;
using UnityEngine;

namespace Utils {
    public class SaveUtils {
        public static void doSample () {


        }

        // public static void saveByJsonData (JsonData jsonData_, string currentPath_) {
        //     Dictionary<string, JsonData> _changeDict = JsonDataUtils.convertToKeyValueDict (jsonData_, currentPath_);
        //     foreach (KeyValuePair<string, JsonData> _keyValue in _changeDict) {
        //         if (_keyValue.Value.IsInt) {
        //             PlayerPrefs.SetInt (_keyValue.Key, _keyValue.Value.GetInt ());
        //         } else if (_keyValue.Value.IsDouble) {
        //             PlayerPrefs.SetFloat (_keyValue.Key, (float) _keyValue.Value.GetDouble ());
        //         } else if (_keyValue.Value.IsLong) {
        //             Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
        //                 "PlayerPrefs 不支持 Long 型"
        //             );
        //         } else if (_keyValue.Value.IsBoolean) {
        //             if (_keyValue.GetBoolean ()) {
        //                 PlayerPrefs.SetString (_keyValue.Key, "True");
        //             } else {
        //                 PlayerPrefs.SetString (_keyValue.Key, "False");
        //             }
        //         } else if (_keyValue.Value.IsString) {
        //             PlayerPrefs.SetString (_keyValue.Key, _keyValue.Value.GetString ());
        //         }
        //         logStr = logStr + (_keyValue.Key + " = " + _keyValue.Value.ToJson ()) + System.Environment.NewLine;
        //     }
        // }

        //设置给VO对象
        public static bool saveValueObj (ValueObj valueObj_, string keyName_) {
            JsonData _dataPosition = JsonDataUtils.getValueFromDataPath (valueObj_.jsonRoot, "");
            try {
                PlayerPrefs.SetString (keyName_, _dataPosition.ToJson ());
                return true;
            } catch (System.Exception err) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    err
                );
            }
            return false;
        }

        public static bool loadValueObj (ValueObj valueObj_, string keyName_) {
            if (PlayerPrefs.HasKey (keyName_)) { //有这个路径存储那么就取的
                string _saveStr = PlayerPrefs.GetString (keyName_); //获取字符串
                JsonData _saveJsonData = JsonMapper.ToObject (_saveStr); //转换json对象
                JsonDataUtils.setValueToDataPath (valueObj_.jsonRoot, "", _saveJsonData, false); //json对象对应到指定节点上，不要触发事件。
                return true;
            } else { //没有就返回false
                return false;
            }
        }
    }
}