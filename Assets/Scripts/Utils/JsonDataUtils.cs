using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using LitJson;
using UnityEngine;
using Objs;

namespace Utils {
    public class JsonDataUtils {
        //Aviod to create dictionary when data change.
        public static Dictionary<string, IJsonWrapper>[] _currentChangingDict = getEmptyChangeDict ();
        public enum DataType {
            Int = 0,
            Float = 1,
            String = 2,
            Bool = 3,
            Array = 4,
            Dict = 5
        }
        public static void doSample () {
            //testJsonFileToJson ();
            //testDictToJson ();
        }
        public static void testJsonFileToJson () {
            string _path = "/Resources/testRes/testJson.json";
            string _jsonStr = FileUtils.ReadFileToString (_path);
            JsonData _jsonRoot = JsonMapper.ToObject (_jsonStr);
            JsonData _dataSet = new JsonData ();
            Dictionary<string, IJsonWrapper>[] _changeDictList = setValueToPath (_dataSet, "dict", _jsonRoot);
            LogUtils.printDict (convertToKeyValueDict (_dataSet, ""));
            Debug.Log (_dataSet.ToJson ());
        }
        public static void testDictToJson () {
            object _dict = new {
                A = "C-180",
                B = new [] { "tag1", "tag2", "tag3" },
                C = new string[] { },
                D = new [] { "SE" },
                E = new [] {
                    new {
                        market = "SE",
                        value = new {
                            amount = 6.39,
                            currency = "USD"
                        }
                    }
                }
                // ,
                // f = new [] { 
                //     new [] {1,2 },
                //     new [] {3,4 }
                // }
            };
            JsonData _dataSet = new JsonData ();
            setValueToPath (_dataSet, null, _dict);
            LogUtils.printDict (convertToKeyValueDict (_dataSet, ""));

            object _dict2 = new {
                A2 = "C-180",
                B = new [] { "tag1", "tag2" },
                E = new [] {
                    new {
                        market = "sss",
                        value = new {
                            amount = 4,
                            currency = "1"
                        }
                    }
                }
            };
            setValueToPath (_dataSet, null, _dict2); //Overwrite data value in same data path.
            LogUtils.printDict (convertToKeyValueDict (_dataSet, ""));

            var _value = getValueFromDataPath (_dataSet, "E[0].market");

        }
        //Create dictionary to record data path and data value.
        public static Dictionary<string, IJsonWrapper>[] getEmptyChangeDict () {
            Dictionary<string, IJsonWrapper>[] _changeDict = new Dictionary<string, IJsonWrapper>[6];
            _changeDict[0/*(int) DataType.Int*/] = new Dictionary<string, IJsonWrapper> ();
            _changeDict[1/*(int) DataType.Float*/] = new Dictionary<string, IJsonWrapper> ();
            _changeDict[2/*(int) DataType.String*/] = new Dictionary<string, IJsonWrapper> ();
            _changeDict[3/*(int) DataType.Bool*/] = new Dictionary<string, IJsonWrapper> ();
            _changeDict[4/*(int) DataType.Array*/] = new Dictionary<string, IJsonWrapper> ();
            _changeDict[5/*(int) DataType.Dict*/] = new Dictionary<string, IJsonWrapper> ();
            return _changeDict;
        }
        //EventObserverObj,Data change happened.one at a time.
        public static Dictionary<string, IJsonWrapper>[] clearChangeDict () {
            _currentChangingDict[0/*(int) DataType.Int*/].Clear ();
            _currentChangingDict[1/*(int) DataType.Float*/].Clear ();
            _currentChangingDict[2/*(int) DataType.String*/].Clear ();
            _currentChangingDict[3/*(int) DataType.Bool*/].Clear ();
            _currentChangingDict[4/*(int) DataType.Array*/].Clear ();
            _currentChangingDict[5/*(int) DataType.Dict*/].Clear ();
            return _currentChangingDict;
        }

        //deal with data path with array index.
        // a[1][12] -> ['a',1,12]
        // a[1][12][0] -> ['a',1,12,0]
        private static ArrayList dealKey (string key_) {
            if (key_.Contains ("[")) {
                string[] _keyList = key_.Split ('[');
                ArrayList _indexArr = new ArrayList ();
                _indexArr.Add (_keyList[0]);
                string _keySplit;
                string _numStr;
                int _index;
                for (int _idx = 1; _idx < _keyList.Length; _idx++) {
                    _keySplit = _keyList[_idx];
                    _numStr = _keySplit.Split (']') [0];
                    _index = Convert.ToInt32 (_numStr);
                    _indexArr.Add (_index);
                }
                return _indexArr;
            } else {
                return null;
            }
        }

        public static string getStringFromDataPath (JsonData jsonNode_, string dataPath_) {
            JsonData _dataPosition = getValueFromDataPath (jsonNode_, dataPath_);
            if (_dataPosition.IsArray) {
                return "<Array>";
            } else if (_dataPosition.IsObject) {
                return "<object>";
            } else if (
                _dataPosition.IsInt ||
                _dataPosition.IsBoolean ||
                _dataPosition.IsDouble ||
                _dataPosition.IsLong ||
                _dataPosition.IsString
            ) {
                return _dataPosition.ToJson ();
            } else {
                return null;
            }
        }

        public static JsonData getJsonDataFromDataPath (JsonData jsonNode_, string dataPath_) {
            JsonData _dataPosition = getValueFromDataPath (jsonNode_, dataPath_);
            if (_dataPosition.IsArray) {
                return _dataPosition;
            } else if (_dataPosition.IsObject) {
                return _dataPosition;
            } else if (
                _dataPosition.IsInt ||
                _dataPosition.IsBoolean ||
                _dataPosition.IsDouble ||
                _dataPosition.IsLong ||
                _dataPosition.IsString
            ) {
                return _dataPosition;
            } else {
                return null;
            }
        }

        public static JsonData getValueFromDataPath (JsonData jsonNode_, string dataPath_) {
            ArrayList _dataPathList = null;
            if (dataPath_ != "") { //多层路径下
                if (dataPath_.Contains (".")) {
                    _dataPathList = new ArrayList (dataPath_.Split ('.'));
                } else { //根路径下的第一层
                    _dataPathList = new ArrayList ();
                    _dataPathList.Add (dataPath_);
                }
            } else { //根路径上，直接赋值
                _dataPathList = new ArrayList ();
            }
            JsonData _dataPosition = jsonNode_;
            while (_dataPathList.Count > 0) {
                string _currentKey = (string) _dataPathList[0];
                _dataPathList.RemoveAt (0);

                var _dealedKeyList = dealKey (_currentKey);
                if (_dealedKeyList == null) { //取路径
                    if (_dataPosition.ContainsKey (_currentKey)) {
                        _dataPosition = _dataPosition[_currentKey];
                    } else {
                        _dataPosition = null;
                        break;
                    }
                } else { //取列表元素
                    _currentKey = (string) _dealedKeyList[0]; //获取键值
                    _dealedKeyList.RemoveAt (0); //列表中移除键值
                    if (_dataPosition.ContainsKey (_currentKey)) { //是否存在这个键值
                        _dataPosition = _dataPosition[_currentKey]; //通过键值取得列表
                        if (_dataPosition.IsArray) {
                            while (_dealedKeyList.Count > 0) { //递归参照元素
                                int _idx = (int) _dealedKeyList[0];
                                _dealedKeyList.RemoveAt (0);
                                _dataPosition = _dataPosition[_idx];
                            }
                        } else {
                            Debug.LogError ("非数组JsonData 不能用 [_idx] 来取元素");
                            break;
                        }
                    } else {
                        _dataPosition = null;
                        break;
                    }
                }
            }
            return _dataPosition;
        }

        public static Dictionary<string, IJsonWrapper>[] setValueToPath (JsonData jsonNode_, string dataPath_, JsonData value_, bool dispatchEvent_ = true) {
            Dictionary<string, IJsonWrapper>[] _changeDict = clearChangeDict ();
            ArrayList _dataPathList = null;
            if (!string.IsNullOrEmpty (dataPath_)) { //多层路径下
                if (dataPath_.Contains (".")) {
                    _dataPathList = new ArrayList (dataPath_.Split ('.'));
                } else { //根路径下的第一层
                    _dataPathList = new ArrayList ();
                    _dataPathList.Add (dataPath_);
                }
            } else { //根路径上，直接赋值
                //赋值
                if (value_ != null) {
                    //直接往根目录上赋值，所以，根本
                    if (
                        value_.IsInt ||
                        value_.IsBoolean ||
                        value_.IsDouble ||
                        value_.IsLong ||
                        value_.IsString ||
                        value_.IsArray
                    ) { //数组替换
                        Debug.Log ("第一层级对应的只能是对象，值和数组都不能直接复制给对象");
                    } else if (value_.IsObject) { //字典覆盖
                        recursiveDataPath (jsonNode_, value_, dataPath_, _changeDict, dispatchEvent_);
                    }
                    return _changeDict;
                } else { //清空
                    Debug.LogError ("ERROR 清空一个 JsonData 不用调用这个方法，直接将其重置为null即可，xx = null;");
                    return null;
                }
            }

            JsonData _dataPosition = jsonNode_;
            while (_dataPathList.Count > 0) {
                string _currentKey = (string) _dataPathList[0];
                _dataPathList.RemoveAt (0);
                if (_dataPathList.Count == 0) {
                    if (value_ == null) {
                        if (_dataPosition.ContainsKey (_currentKey) == true) {
                            _dataPosition[_currentKey].Clear (); //如果是对象的话，先释放一下
                            _dataPosition.Remove (_dataPosition[_currentKey]);
                        }
                        return null;
                    } else {
                        if (
                            value_.IsInt ||
                            value_.IsBoolean ||
                            value_.IsDouble ||
                            value_.IsLong ||
                            value_.IsString
                        ) {
                            if (_dataPosition.ContainsKey (_currentKey)) {
                                _dataPosition[_currentKey].Clear ();
                            }
                            _dataPosition[_currentKey] = value_;
                            if (dispatchEvent_) {
                                if (value_.IsInt) {
                                    _changeDict[0/*(int) DataType.Int*/].Add (dataPath_, value_);
                                }
                                if (value_.IsBoolean) {
                                    _changeDict[3/*(int) DataType.Bool*/].Add (dataPath_, value_);
                                }
                                if (value_.IsDouble || value_.IsLong) {
                                    _changeDict[1/*(int) DataType.Float*/].Add (dataPath_, value_);
                                }
                                if (value_.IsString) {
                                    _changeDict[2/*(int) DataType.String*/].Add (dataPath_, value_);
                                }
                            }
                        } else if (value_.IsArray) { //数组替换
                            Debug.LogError ("ERROR 不能直接向一个路径赋值数组。\n  如果向 path.arr 赋值数组\n    请使用 \n      objet _arrDict = {arr:[数组]};\n      setValueToPath(path,_arrDict)");
                            //resetArrayOnDataPath (_dataPosition, dataPath_, _currentKey, value_, _changeDict, dispatchEvent_);
                        } else if (value_.IsObject) { //字典覆盖
                            if (_dataPosition.ContainsKey (_currentKey) == false) { //原来在这个数据路径上没有，就可以直接引用
                                JsonData _jsonDict = new JsonData ();
                                _jsonDict.SetJsonType (JsonType.Object);
                                _dataPosition[_currentKey] = _jsonDict;
                            }
                            //不用赋值，只需层层解析就行可以了
                            recursiveDataPath (_dataPosition[_currentKey], value_, dataPath_, _changeDict, dispatchEvent_);
                        }
                    }
                } else {
                    if (_dataPosition.ContainsKey (_currentKey) == false) {
                        _dataPosition[_currentKey] = new JsonData ();
                        _dataPosition[_currentKey].SetJsonType (JsonType.Object);
                    }
                    _dataPosition = _dataPosition[_currentKey];
                }
            }
            return _changeDict;
        }

        //遍历字段，提醒数据路径的监听者改变数据
        private static void recursiveDataPath (JsonData dataOnParentPath_, JsonData valueDict_, string dataPath_, Dictionary<string, IJsonWrapper>[] changeListDict_, bool dispatchEvent_ = true) {
            bool _isSameJsonData = dataOnParentPath_ == valueDict_;
            foreach (string _key in valueDict_.Keys) {

                var _value = valueDict_[_key];
                //当前路径
                string _currentPath;
                if (String.CompareOrdinal (dataPath_, "") == 0) {
                    _currentPath = _key;
                } else {
                    StringBuilder _sb = new StringBuilder (20);
                    _sb.Append (dataPath_);
                    _sb.Append (".");
                    _sb.Append (_key);
                    _currentPath = _sb.ToString ();
                    _sb.Clear ();
                }

                if (
                    _value.IsInt ||
                    _value.IsBoolean ||
                    _value.IsDouble ||
                    _value.IsLong ||
                    _value.IsString
                ) {
                    if (!_isSameJsonData) { //不是自己赋值自己的。
                        if (dataOnParentPath_.ContainsKey (_key)) { //有的就清理
                            dataOnParentPath_[_key].Clear ();
                        }
                        dataOnParentPath_[_key] = _value; //没有赋的换新值
                    }
                    if (dispatchEvent_) {
                        if (_value.IsInt) {
                            changeListDict_[0/*(int) DataType.Int*/].Add (_currentPath, _value);
                        }
                        if (_value.IsBoolean) {
                            changeListDict_[3/*(int) DataType.Bool*/].Add (_currentPath, _value);
                        }
                        if (_value.IsDouble || _value.IsLong) {
                            changeListDict_[1/*(int) DataType.Float*/].Add (_currentPath, _value);
                        }
                        if (_value.IsString) {
                            changeListDict_[2/*(int) DataType.String*/].Add (_currentPath, _value);
                        }
                    }
                } else if (_value.IsArray) {
                    resetArrayOnDataPath (dataOnParentPath_, dataPath_, _key, _value, changeListDict_, dispatchEvent_);
                } else if (_value.IsObject) {
                    if (dataOnParentPath_.ContainsKey (_key) == false) {
                        JsonData _dictJsonData = new JsonData ();
                        _dictJsonData.SetJsonType (JsonType.Object);
                        dataOnParentPath_[_key] = _dictJsonData;
                    }
                    recursiveDataPath (dataOnParentPath_[_key], _value, _currentPath, changeListDict_, dispatchEvent_);
                }
            }
        }
        //递归设置数据
        private static void resetArrayOnDataPath (JsonData dataOnCurrentDataPath_, string dataPath_, string lastKey_, JsonData arrayValue_, Dictionary<string, IJsonWrapper>[] changeListDict_, bool dispatchEvent_ = true) {
            if (dataOnCurrentDataPath_.ContainsKey (lastKey_)) {
                dataOnCurrentDataPath_[lastKey_].Clear ();
            }
            //不管原来是否有，重新创建一个新的，用来接数组
            JsonData _currentJsonArr = new JsonData ();
            _currentJsonArr.SetJsonType (JsonType.Array);
            dataOnCurrentDataPath_[lastKey_] = _currentJsonArr;

            StringBuilder _sb = new StringBuilder (20);
            string _arrayPath = null;
            if (String.CompareOrdinal (dataPath_, "") == 0) {
                _arrayPath = lastKey_;
            } else {
                if (dataOnCurrentDataPath_.IsObject) { //如果上一层是字
                    _sb.Append (dataPath_);
                    _sb.Append (".");
                    _sb.Append (lastKey_);
                    _arrayPath = _sb.ToString ();
                    _sb.Clear ();
                } else if (dataOnCurrentDataPath_.IsArray) { //如果上一层是数组，就按照数组序号解析 lastKey_
                    _sb.Append (dataPath_);
                    _sb.Append ("[");
                    _sb.Append (lastKey_);
                    _sb.Append ("]");
                    _arrayPath = _sb.ToString ();
                    _sb.Clear ();
                }
            }
            for (int _idx = 0; _idx < arrayValue_.Count; _idx++) {
                _sb.Append (_arrayPath);
                _sb.Append ("[");
                _sb.Append (_idx);
                _sb.Append ("]");
                string _itemPath = _sb.ToString (); //元素路径
                _sb.Clear ();

                var _item = arrayValue_[_idx];
                if (
                    _item.IsInt ||
                    _item.IsBoolean ||
                    _item.IsDouble ||
                    _item.IsLong ||
                    _item.IsString
                ) {
                    _currentJsonArr.Add (_item);
                    if (dispatchEvent_) {
                        if (_item.IsInt) {
                            changeListDict_[0/*(int) DataType.Int*/].Add (_itemPath, _item);
                        }
                        if (_item.IsBoolean) {
                            changeListDict_[3/*(int) DataType.Bool*/].Add (_itemPath, _item);
                        }
                        if (_item.IsDouble || _item.IsLong) {
                            changeListDict_[1/*(int) DataType.Float*/].Add (_itemPath, _item);
                        }
                        if (_item.IsString) {
                            changeListDict_[2/*(int) DataType.String*/].Add (_itemPath, _item);
                        }
                    }
                } else if (_item.IsArray) {
                    resetArrayOnDataPath (_currentJsonArr, _arrayPath, Convert.ToString (_idx), _item, changeListDict_, dispatchEvent_);
                } else if (_item.IsObject) {
                    JsonData _dataOnPath = new JsonData ();
                    _dataOnPath.SetJsonType (JsonType.Object);
                    _currentJsonArr.Add (_dataOnPath);
                    recursiveDataPath (_dataOnPath, _item, _itemPath, changeListDict_, dispatchEvent_);
                }
            }
        }

        public static Dictionary<string, IJsonWrapper>[] setValueToPath (JsonData jsonNode_, string dataPath_, object value_, bool dispatchEvent_ = true) {
            Dictionary<string, IJsonWrapper>[] _changeDict = clearChangeDict ();

            ArrayList _dataPathList = null;
            if (!string.IsNullOrEmpty (dataPath_)) { //多层路径下
                if (dataPath_.Contains (".")) {
                    _dataPathList = new ArrayList (dataPath_.Split ('.'));
                } else { //根路径下的第一层
                    _dataPathList = new ArrayList ();
                    _dataPathList.Add (dataPath_);
                }
            } else { //根路径上，直接赋值
                //赋值
                if (value_ != null) {
                    //直接往根目录上赋值，所以，根本
                    if (
                        value_ is bool ||
                        value_ is string ||
                        value_ is int ||
                        value_ is double ||
                        value_ is Array
                    ) { //数组替换
                        Debug.Log ("第一层级对应的只能是对象，值和数组都不能直接复制给对象");
                    } else if (value_ is object) { //字典覆盖
                        recursiveDataPath (jsonNode_, value_, dataPath_, _changeDict, dispatchEvent_);
                    }
                    return _changeDict;
                } else { //清空
                    Debug.LogError ("ERROR 清空一个 JsonData 不用调用这个方法，直接将其重置为null即可，xx = null;");
                    return null;
                }
            }

            JsonData _dataPosition = jsonNode_;
            while (_dataPathList.Count > 0) {
                string _currentKey = (string) _dataPathList[0];
                _dataPathList.RemoveAt (0);
                if (_dataPathList.Count == 0) {
                    if (value_ == null) { //之前，赋值过数组元素的数据路径，就先清理掉之前的
                        if (_dataPosition.ContainsKey (_currentKey) == true) {
                            _dataPosition[_currentKey].Clear (); //如果是对象的话，先释放一下
                            _dataPosition.Remove (_dataPosition[_currentKey]);
                        }
                        return null;
                    } else if (
                        value_ is bool ||
                        value_ is string ||
                        value_ is int ||
                        value_ is double
                    ) {
                        if (_dataPosition.ContainsKey (_currentKey)) {
                            _dataPosition[_currentKey].Clear (); //如果是对象的话，先释放一下
                        }
                        JsonData _valueJsonData = new JsonData (value_);
                        _dataPosition[_currentKey] = _valueJsonData;
                        if (dispatchEvent_) {
                            if (value_ is int) {
                                _changeDict[0/*(int) DataType.Int*/].Add (dataPath_, _valueJsonData);
                            }
                            if (value_ is bool) {
                                _changeDict[3/*(int) DataType.Bool*/].Add (dataPath_, _valueJsonData);
                            }
                            if (value_ is double) {
                                _changeDict[1/*(int) DataType.Float*/].Add (dataPath_, _valueJsonData);
                            }
                            if (value_ is string) {
                                _changeDict[2/*(int) DataType.String*/].Add (dataPath_, _valueJsonData);
                            }
                        }
                    } else if (value_ is Array) {
                        Debug.LogError ("ERROR 不能直接向一个路径赋值数组。\n  如果向 path.arr 赋值数组\n    请使用 \n      objet _arrDict = {arr:[数组]};\n      setValueToPath(path,_arrDict)");
                        //resetArrayOnDataPath (_dataPosition, dataPath_, _currentKey, (Array) value_, _changeDict, dispatchEvent_);
                    } else if (value_ is object) {
                        if (_dataPosition.ContainsKey (_currentKey) == false) {
                            JsonData _dictJsonData = new JsonData ();
                            _dictJsonData.SetJsonType (JsonType.Object);
                            _dataPosition[_currentKey] = _dictJsonData;
                        }
                        recursiveDataPath (_dataPosition[_currentKey], value_, dataPath_, _changeDict, dispatchEvent_);
                    } else {
                        Debug.LogError ("ERROR DataCenter -> sv 意外的类型 : " + value_.GetType ().ToString () + "," + dataPath_);
                    }
                } else {
                    if (_dataPosition.ContainsKey (_currentKey) == false) {
                        _dataPosition[_currentKey] = new JsonData ();
                        _dataPosition[_currentKey].SetJsonType (JsonType.Object);
                    }
                    _dataPosition = _dataPosition[_currentKey];
                }
            }
            return _changeDict;
        }

        //遍历字段，提醒数据路径的监听者改变数据
        private static void recursiveDataPath (JsonData dataOnParentPath_, object valueDict_, string dataPath_, Dictionary<string, IJsonWrapper>[] changeListDict_, bool dispatchEvent_ = true) {
            foreach (System.Reflection.PropertyInfo _kvObj in valueDict_.GetType ().GetProperties ()) {
                if (_kvObj.DeclaringType != _kvObj.ReflectedType) { //非自定义方法
                    continue;
                }
                string _key = _kvObj.Name;
                var _value = _kvObj.GetValue (valueDict_);
                //当前路径
                string _currentPath = "";
                if (String.CompareOrdinal (dataPath_, "") == 0) {
                    _currentPath = _key;
                } else {
                    StringBuilder _sb = new StringBuilder (20);
                    _sb.Append (dataPath_);
                    _sb.Append (".");
                    _sb.Append (_key);
                    _currentPath = _sb.ToString ();
                    _sb.Clear ();
                }

                if (
                    _value is bool ||
                    _value is string ||
                    _value is int ||
                    _value is double
                ) {
                    if (dataOnParentPath_.ContainsKey (_key)) {
                        dataOnParentPath_[_key].Clear ();
                    }
                    JsonData _valueJsonData = new JsonData (_value);
                    dataOnParentPath_[_key] = _valueJsonData;
                    if (dispatchEvent_) {
                        if (_value is int) {
                            changeListDict_[0/*(int) DataType.Int*/].Add (_currentPath, _valueJsonData);
                        }
                        if (_value is bool) {
                            changeListDict_[3/*(int) DataType.Bool*/].Add (_currentPath, _valueJsonData);
                        }
                        if (_value is double) {
                            changeListDict_[1/*(int) DataType.Float*/].Add (_currentPath, _valueJsonData);
                        }
                        if (_value is string) {
                            changeListDict_[2/*(int) DataType.String*/].Add (_currentPath, _valueJsonData);
                        }
                    }
                } else if (_value is Array) {
                    resetArrayOnDataPath (dataOnParentPath_, dataPath_, _key, (Array) _value, changeListDict_, dispatchEvent_);
                } else if (_value is object) {
                    if (dataOnParentPath_.ContainsKey (_key) == false) {
                        JsonData _dictJsonData = new JsonData ();
                        _dictJsonData.SetJsonType (JsonType.Object);
                        dataOnParentPath_[_key] = _dictJsonData;
                    }
                    recursiveDataPath (dataOnParentPath_[_key], _value, _currentPath, changeListDict_, dispatchEvent_);
                } else {
                    Debug.LogError ("ERROR DataCenter -> recursiveDataPath 意外的类型 : " + _value.GetType ().ToString () + "," + dataPath_);
                }
            }
        }
        //递归设置数据
        private static void resetArrayOnDataPath (JsonData dataOnCurrentDataPath_, string dataPath_, string lastKey_, Array arrayValue_, Dictionary<string, IJsonWrapper>[] changeListDict_, bool dispatchEvent_ = true) {
            if (dataOnCurrentDataPath_.ContainsKey (lastKey_)) {
                dataOnCurrentDataPath_[lastKey_].Clear ();
                dataOnCurrentDataPath_.Remove (lastKey_);
            }
            //不管原来是否有，重新创建一个新的，用来接数组
            JsonData _currentJsonArr = new JsonData ();
            _currentJsonArr.SetJsonType (JsonType.Array);
            dataOnCurrentDataPath_[lastKey_] = _currentJsonArr;

            StringBuilder _sb = new StringBuilder (20);
            string _arrayPath = null;
            if (String.CompareOrdinal (dataPath_, "") == 0) {
                _arrayPath = lastKey_;
            } else {
                _sb.Append (dataPath_);
                _sb.Append (".");
                _sb.Append (lastKey_);
                _arrayPath = _sb.ToString ();
                _sb.Clear ();
            }

            for (int _idx = 0; _idx < arrayValue_.Length; _idx++) {
                _sb.Append (_arrayPath);
                _sb.Append ("[");
                _sb.Append (_idx);
                _sb.Append ("]");
                string _itemPath = _sb.ToString (); //元素路径
                _sb.Clear ();
                var _item = arrayValue_.GetValue (_idx);
                if (
                    _item is bool ||
                    _item is string ||
                    _item is int ||
                    _item is double
                ) {
                    JsonData _valueJsonData = new JsonData (_item);
                    _currentJsonArr.Add (_valueJsonData);
                    if (dispatchEvent_) {
                        if (_item is int) {
                            changeListDict_[0/*(int) DataType.Int*/].Add (_itemPath, _valueJsonData);
                        }
                        if (_item is bool) {
                            changeListDict_[3/*(int) DataType.Bool*/].Add (_itemPath, _valueJsonData);
                        }
                        if (_item is double) {
                            changeListDict_[1/*(int) DataType.Float*/].Add (_itemPath, _valueJsonData);
                        }
                        if (_item is string) {
                            changeListDict_[2/*(int) DataType.String*/].Add (_itemPath, _valueJsonData);
                        }
                    }
                } else if (_item is Array) {
                    Debug.LogError ("ERROR DataCenter -> resetArrayOnDataPath dict 转 JsonData 不支持多维数组 : " + _itemPath);
                } else if (_item is object) {
                    JsonData _dataOnPath = new JsonData ();
                    _dataOnPath.SetJsonType (JsonType.Object);
                    _currentJsonArr.Add (_dataOnPath);
                    recursiveDataPath (_dataOnPath, _item, _itemPath, changeListDict_, dispatchEvent_);
                } else {
                    Debug.LogError ("ERROR DataCenter -> resetArrayOnDataPath 意外的类型 : " + _item.GetType ().ToString () + "," + dataPath_);
                }
            }
        }
        
        //以 currentPath_ 为原始路径 打印 jsonData_。
        public static Dictionary<string, JsonData> convertToKeyValueDict (JsonData jsonData_, string currentPath_, Dictionary<string, JsonData> changeListDict_ = null) {
            Dictionary<string, JsonData> _changeDict = null;
            if (changeListDict_ != null) {
                _changeDict = changeListDict_;
            } else {
                _changeDict = new Dictionary<string, JsonData> ();
            }

            if (jsonData_.IsObject) {
                convertDictToKeyValueDict (jsonData_, currentPath_, _changeDict);
            } else if (jsonData_.IsArray) {
                convertListToKeyValueDict (jsonData_, currentPath_, _changeDict);
            }
            return _changeDict;
        }
        private static void convertDictToKeyValueDict (JsonData jsonData_, string currentPath_, Dictionary<string, JsonData> changeListDict_) {
            string _currentPath = null;
            foreach (string _key in jsonData_.Keys) {
                if (String.CompareOrdinal (currentPath_, "") == 0) {
                    _currentPath = _key;
                } else {
                    _currentPath = currentPath_ + "." + _key;
                }
                JsonData _value = jsonData_[_key];
                if (_value.IsObject) {
                    convertDictToKeyValueDict (_value, _currentPath, changeListDict_);
                } else if (_value.IsArray) {
                    convertListToKeyValueDict (_value, _currentPath, changeListDict_);
                } else if (_value.IsInt) {
                    changeListDict_.Add (_currentPath, _value);
                } else if (_value.IsBoolean) {
                    changeListDict_.Add (_currentPath, _value);
                } else if (_value.IsDouble) {
                    changeListDict_.Add (_currentPath, _value);
                } else if (_value.IsLong) {
                    changeListDict_.Add (_currentPath, _value);
                } else if (_value.IsString) {
                    changeListDict_.Add (_currentPath, _value);
                }
            }
        }
        private static void convertListToKeyValueDict (JsonData jsonData_, string currentPath_, Dictionary<string, JsonData> changeListDict_) {
            string _currentPath = null;
            for (int _idx = 0; _idx < jsonData_.Count; _idx++) {
                JsonData _item = jsonData_[_idx];
                if (String.CompareOrdinal (currentPath_, "") == 0) {
                    _currentPath = "[" + _idx + "]";
                } else {
                    _currentPath = currentPath_ + "[" + _idx + "]";
                }
                JsonData _value = _item;
                if (_value.IsObject) {
                    convertDictToKeyValueDict (_value, _currentPath, changeListDict_);
                } else if (_value.IsArray) {
                    convertListToKeyValueDict (_value, _currentPath, changeListDict_);
                } else if (_value.IsInt) {
                    changeListDict_.Add (_currentPath, _value);
                } else if (_value.IsBoolean) {
                    changeListDict_.Add (_currentPath, _value);
                } else if (_value.IsDouble) {
                    changeListDict_.Add (_currentPath, _value);
                } else if (_value.IsLong) {
                    changeListDict_.Add (_currentPath, _value);
                } else if (_value.IsString) {
                    changeListDict_.Add (_currentPath, _value);
                }
                _idx = _idx++;
            }
        }
    }
}