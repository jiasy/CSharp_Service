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
        //不推荐使用，要知道完整路径键才能再取出来，因为没有遍历方法。而且不支持boolean类型。
        public static void saveByJsonData (JsonDataWrapObj jsonDataWrap_, string path_) {
            Dictionary<string, JsonData> _keyValueDIct = JsonDataWrapObj.convertDictToKeyValueDict(jsonDataWrap_,path_);
            string logStr = "";
            foreach (KeyValuePair<string, JsonData> _keyValue in _keyValueDIct) {
                if (_keyValue.Value.IsInt) {
                    PlayerPrefs.SetInt (_keyValue.Key, ((IJsonWrapper)_keyValue.Value).GetInt ());
                } else if (_keyValue.Value.IsDouble) {
                    PlayerPrefs.SetFloat (_keyValue.Key, (float) ((IJsonWrapper)_keyValue.Value).GetDouble ());
                } else if (_keyValue.Value.IsLong) {
                    Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                        "PlayerPrefs 不支持 Long 型"
                    );
                } else if (_keyValue.Value.IsBoolean) {//不支持Bool，利用 String 的特殊形式代替
                    if (((IJsonWrapper)_keyValue.Value).GetBoolean ()) {
                        PlayerPrefs.SetString (_keyValue.Key, "<True>");
                    } else {
                        PlayerPrefs.SetString (_keyValue.Key, "<False>");
                    }
                } else if (_keyValue.Value.IsString) {
                    PlayerPrefs.SetString (_keyValue.Key, ((IJsonWrapper)_keyValue.Value).GetString ());
                }
                logStr = logStr + (_keyValue.Key + " = " + _keyValue.Value.ToJson ()) + System.Environment.NewLine;
            }
            Debug.Log(logStr);
        }
        //推荐使用 json 的存取方式
        //将 ValueObj 的指定路径 savePath_ 中 JsonData 内容转换成 json 字符串，并保存到本地。
        public static bool saveValueObjToLocal (ValueObj valueObj_, string savePath_) {
            try {
                PlayerPrefs.SetString (savePath_, valueObj_.getJsonStr(savePath_));
                return true;
            } catch (System.Exception err) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    err
                );
            }
            return false;
        }
        //将本地 loadPath_ 键对应的字符串转换成 JsonData ，按照 loadPath_ 路径设置回 ValueObj_ 中。
        public static bool loadLocalToValueObj (ValueObj valueObj_, string loadPath_) {
            if (PlayerPrefs.HasKey (loadPath_)) { //有这个路径存储那么就取的
                string _saveStr = PlayerPrefs.GetString (loadPath_); //获取字符串
                JsonData _saveJsonData = JsonMapper.ToObject (_saveStr); //转换json对象
                valueObj_.setValueToPath(loadPath_,_saveJsonData,true);//json对象对应到指定节点上
                return true;
            } else { //没有就返回false
                return false;
            }
        }
    }
}