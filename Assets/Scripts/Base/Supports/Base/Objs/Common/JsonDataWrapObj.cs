using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Utils;
using System.Text;

//json格式数据的包装
namespace Objs {
    public class JsonDataWrapObj : BaseObj {
        #region 样例
        public static void doSample () {
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
            };
            JsonDataWrapObj _jsonDataWrap = new JsonDataWrapObj ();
            _jsonDataWrap.addKeyValueToPath(null,"dict",_dict);
            _jsonDataWrap.printPathValueDict();
            Debug.Log(_jsonDataWrap.toJsonString());
        }
        #endregion
        #region 静态方法
        private static string _jsonListMark = "<LIST_MARK>";
        //数据节点是否是数组转化而来的判断。
        private static bool isList(JsonData jsonDict_){
            if(jsonDict_.ContainsKey("[0]")){
                return true;
            }
            return false;
        }
        //将数组字典变换成数组
        private static JsonData convertDictToList(JsonData jsonDict_){
            if(isList(jsonDict_)){
                JsonData _jsonList = new JsonData();
                _jsonList.SetJsonType (JsonType.Array);
                int _idx = 0;
                StringBuilder _sb = new StringBuilder (20);
                string _idxKey = null;
                int _length = ((IJsonWrapper)jsonDict_["length"]).GetInt();
                while(_idx < _length){
                    _sb.Append ("[");
                    _sb.Append ((_idx + 1).ToString());
                    _sb.Append ("]");
                    _idxKey = _sb.ToString();
                    _sb.Clear ();
                    _jsonList.Add(jsonDict_[_idxKey]);
                    _idx++;
                }
                return _jsonList;
            }else{
                return jsonDict_;
            }
        }
        //遍历转换获得一个正常的字典对象，用来转换回json
        public static JsonData convertToNormalJsonData(JsonData jsonDict_){
            if(!jsonDict_.IsObject){
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "只有字典才能转换");
            }
            if(isList(jsonDict_)){
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "数组转换的字典不能转换");
            }
            JsonData _jsonDict = new JsonData();
            _jsonDict.SetJsonType (JsonType.Object);
            foreach (string _tempKey in jsonDict_.Keys) {
                var _tempValue = jsonDict_[_tempKey];
                if (_tempValue.IsInt ||_tempValue.IsDouble ||_tempValue.IsLong ||_tempValue.IsBoolean ||_tempValue.IsString) {
                    _jsonDict[_tempKey] = _tempValue;
                }else if(_tempValue.IsObject){
                    if(isList(_tempValue)){
                        _jsonDict[_tempKey] = convertDictToList(_tempValue);
                    }else{
                        _jsonDict[_tempKey] = convertToNormalJsonData(_tempValue);
                    }
                }
            }
            return _jsonDict;
        }
        //数据节点 JsonData 节点转换成 json 字符串
        public static string toJsonString(JsonData jsonDict_){
            return convertToNormalJsonData(jsonDict_).ToJson();
        }
        #endregion

        public JsonData dataSet = null;
        public Dictionary<string, JsonData> pathValueDict = new Dictionary<string,JsonData>();
        //刚刚变化的数据路径，原有值 -> 现在值。
        public Dictionary<string, JsonData> justChangeDict = new Dictionary<string,JsonData>();

        #region 创建销毁
        public JsonDataWrapObj () : base () {
            dataSet = new JsonData ();
            dataSet.SetJsonType (JsonType.Object);
        }
        //递归清理字典对象
        public void clearJsonDict(JsonData jsonDict_){
            foreach (string _tempKey in jsonDict_.Keys) {
                JsonData _tempValue = jsonDict_[_tempKey];
                if(_tempValue.IsObject){//先递归这个对象
                    clearJsonDict(_tempValue);
                }else if(_tempValue.IsArray){
                    //不会有数组对象
                }
                _tempValue.Clear();//后清理
                jsonDict_.Remove(_tempKey);//再后，移除键值关系
            }
        }
        public override void Dispose () {
            clearJsonDict(dataSet);
            dataSet.Clear();
            dataSet = null;
            pathValueDict.Clear();//清理键值关系，值并没有清理
            pathValueDict = null;
            justChangeDict.Clear();//清理键值关系，值并没有清理
            justChangeDict = null;
            base.Dispose ();
        }
        #endregion

        #region 打印以及获取路径值
        public void printPathValueDict(){
            string _logStr = "pathValueDict : \n";
            foreach(string _tempPath in pathValueDict.Keys) {//以这个路径开头的所有缓存都全部清理
                _logStr += "    " + _tempPath + " : " + pathValueDict[_tempPath].ToString() + "\n";
            }
            Debug.Log(_logStr);
        }

        public void printJustChangeDict(){
            string _logStr = "justChangeDict : \n";
            foreach(string _tempPath in justChangeDict.Keys) {//以这个路径开头的所有缓存都全部清理
                _logStr += "    " + _tempPath + " : " + justChangeDict[_tempPath].ToString() + "\n";
            }
            Debug.Log(_logStr);
        }

        //获取路径上的数据
        public string getStringValueOnPath(string path_){
            if (pathValueDict.ContainsKey(path_)){
                return pathValueDict[path_].ToString();
            }else{
                ParentDictInfo _parentDictInfo = getParentDictInfo(path_);
                if(_parentDictInfo.parentDict != null){
                    if(_parentDictInfo.parentDict.ContainsKey(_parentDictInfo.key)){
                        Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                            "\""+_parentDictInfo.parentPath +"\" 键值缓存中遗漏"
                        );
                        return _parentDictInfo.parentDict[_parentDictInfo.key].ToString();   
                    }
                }
            }
            return null;
        }

        public string toJsonString(){
            return toJsonString(dataSet);
        }

        //路径拆分成前后两部分
        public static string[] separatePath(string path_){
            if (path_.Contains(".")) {//多
                ArrayList _dataPathList = new ArrayList (path_.Split('.'));
                string _key = (string)_dataPathList[_dataPathList.Count - 1];
                _dataPathList.RemoveAt(_dataPathList.Count - 1);//退出最后一个
                string _dictPath = string.Join(".",_dataPathList);//合并出所在的字典节点路径
                return new string[]{_dictPath,_key};
            }else{
                return new string[]{"",path_};
            }
        }
        #endregion

        private struct ParentDictInfo{
            public string key;
            public string parentPath;
            public JsonData parentDict;
        }; 
        private ParentDictInfo getParentDictInfo(string path_){
            ParentDictInfo _parentDictInfo;
            string[] _separatePathAndKey = separatePath(path_);
            _parentDictInfo.parentPath = _separatePathAndKey[0];
            _parentDictInfo.key = _separatePathAndKey[1];
            if (path_.Contains(".")) {//多层
                _parentDictInfo.parentDict = getDictOnPath(_parentDictInfo.parentPath,false);//获取父数据节点，不确保其存在
            } else { //根路径下的第一层
                _parentDictInfo.parentDict = dataSet;
            }
            return _parentDictInfo;
        }

        //清理数据路径上的字典
        public void clearDictOnPath(string path_){
            ParentDictInfo _parentDictInfo = getParentDictInfo(path_);
            if(_parentDictInfo.parentDict != null){//从父节点上移除这个键值
                if(_parentDictInfo.parentDict.ContainsKey(_parentDictInfo.key)){
                    _parentDictInfo.parentDict[_parentDictInfo.key].Clear();
                    _parentDictInfo.parentDict.Remove(_parentDictInfo.key);
                    foreach(string _tempPath in pathValueDict.Keys) {//以这个路径开头的所有缓存都全部清理
                        if(_tempPath.IndexOf(path_)==0){//路径和路径下的节点全部清理
                            pathValueDict[_tempPath].Clear();
                            pathValueDict.Remove(_tempPath);
                        }
                    }
                }else{
                    Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                        "\""+_parentDictInfo.parentPath + "\" 不包含键 "+_parentDictInfo.key
                    );
                }
            }else{
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    "\""+_parentDictInfo.parentPath +"\" 不存在这个路径。"
                );
            }
        }
        //获得数据
        public JsonData getJsonDataOnPath(string path_,bool makeSureExists_ = false){
            JsonData _dataOnPath = null;
            if (pathValueDict.ContainsKey(path_)){
                _dataOnPath = pathValueDict[path_];
            }else{
                ArrayList _dataPathList = null;
                if (path_.Contains (".")) {//多层
                    _dataPathList = new ArrayList (path_.Split ('.'));
                } else { //根路径下的第一层
                    _dataPathList = new ArrayList ();
                    _dataPathList.Add (path_);
                }
                _dataOnPath = dataSet;//数据枝干节点
                StringBuilder _sb = new StringBuilder (20);//当前路径拼接
                while (_dataPathList.Count > 0) {//逐层级，直至目标对象
                    string _currentKey = (string) _dataPathList[0];//逐层向下获取节点
                    _dataPathList.RemoveAt (0);
                    _sb.Append(_currentKey);
                    if (_dataOnPath.ContainsKey(_currentKey) == false) {//没有这个键对应的对象，就创建
                        if (makeSureExists_){//确保其一定存在
                            _dataOnPath[_currentKey] = new JsonData ();
                            _dataOnPath[_currentKey].SetJsonType(JsonType.Object);
                            pathValueDict[_sb.ToString()] = _dataOnPath[_currentKey];//缓存到路径和值的键值对中
                        }else{//不确保存在，遇到不存在就直接返回空。
                            _sb.Clear();
                            return null;
                        }
                    }
                    _dataOnPath = _dataOnPath[_currentKey];
                    _sb.Append(".");
                }
                _sb.Clear();
            }
            return _dataOnPath;
        }

        //获取路径上的对象
        public JsonData getDictOnPath(string path_,bool makeSureExists_ = false){
            JsonData _dictOnPath = getJsonDataOnPath(path_,makeSureExists_);
            if (_dictOnPath != null && !_dictOnPath.IsObject) {//不是字典就返回空
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    " 数据路径不是字典 "+path_
                );
                return null;
            }
            return _dictOnPath;
        }
        
        // 所在路径上的字典 和 当前路径 + 键 构成的目标路径
        private struct DictAndPathInfo{
            // path 指定路径上的字典对象
            public JsonData currentDictOnPath;
            // path . key 目标路径
            public string currentPath;
        }
        //获取当前的路径上的字典和拼接键后的目标路径
        private DictAndPathInfo getDictAndPathInfo(
            string path_,
            string key_,
            bool isMain_ = true
        ){
            DictAndPathInfo _dictAndPathInfo;
            if (string.IsNullOrEmpty (key_)) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    " key_ 不能为空"
                );
                _dictAndPathInfo.currentDictOnPath = null;
                _dictAndPathInfo.currentPath = null;
            }else{
                StringBuilder _sb = new StringBuilder (20);
                if (string.IsNullOrEmpty(path_) || path_ == "") {//空 或者 空字符串 都是直接在数据对象上操作
                    _dictAndPathInfo.currentDictOnPath = dataSet;
                    _sb.Append (key_);
                }else{
                    _sb.Append (path_);
                    _sb.Append (".");
                    _sb.Append (key_);
                    _dictAndPathInfo.currentDictOnPath = getDictOnPath(path_,true);
                }
                if (isMain_){//外部调用
                    justChangeDict.Clear();//清理键值
                }
                _dictAndPathInfo.currentPath = _sb.ToString ();//当前路径
                _sb.Clear ();
            }
            return _dictAndPathInfo;
        }
        //纯值时，检验原有值和当前值是不是一个类型并提示
        private void checkIsValueTypeChange(JsonData original_,JsonData current_,string currentPath_){
            if(original_.GetJsonType() != current_.GetJsonType()){//新值和原有类型不一样
                if(original_.IsBoolean || original_.IsString || current_.IsBoolean || current_.IsString){
                    Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                        "原有值和现有值中有非数字类型，且变更前后类型发生变化，应当是 数字、布尔、字符串 之间的变化。\n"+currentPath_);
                }
                if(original_.IsObject){//字典和数组都是字典
                    Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                        "原有值是数组或字典，变换成纯值\n"+currentPath_);
                }
            }
        }
        //直接向路径赋值
        public void setValueToDataPath(string path_,JsonData value_){
            string[] _separatePathAndKey = separatePath(path_);
            addKeyValueToPath(_separatePathAndKey[0],_separatePathAndKey[1],value_);
        }
        public void setValueToDataPath(string path_,object value_){
            string[] _separatePathAndKey = separatePath(path_);
            addKeyValueToPath(_separatePathAndKey[0],_separatePathAndKey[1],value_);
        }
        //向路径上添加一个键值
        public void addKeyValueToPath(
            string path_,
            string key_,
            JsonData value_,
            bool isMain_ = true
        ){
            DictAndPathInfo _dictAndPathInfo = getDictAndPathInfo(path_,key_,isMain_);
            JsonData _currentDictOnPath = _dictAndPathInfo.currentDictOnPath;
            string _currentPath = _dictAndPathInfo.currentPath;
            JsonData _originalValue = null;
            if (value_.IsInt ||value_.IsDouble ||value_.IsLong ||value_.IsBoolean ||value_.IsString) {
                if (pathValueDict.ContainsKey(_currentPath)) {// 原来这个位置有值
                    _originalValue = pathValueDict[_currentPath];//获取原有值
                    checkIsValueTypeChange(_originalValue,value_,_currentPath);//校验当前值类型是否变化
                    _originalValue.Clear();
                }
                pathValueDict[_currentPath] = value_;//缓存新值
                justChangeDict[_currentPath] = value_;//缓存变化
                _currentDictOnPath[key_] = value_;//附新值
            } else if (value_.IsArray) { //数组替换
                JsonData _oldDictOnPath = getDictOnPath(_currentPath,false);//获取旧的。
                if(_oldDictOnPath != null){
                    if (_oldDictOnPath.IsInt ||_oldDictOnPath.IsDouble ||_oldDictOnPath.IsLong ||_oldDictOnPath.IsBoolean ||_oldDictOnPath.IsString) {
                        Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                            "原有值是纯值，变换成数组字典\n"+_currentPath);
                    }
                    _oldDictOnPath.Clear();//调用清理方法，清理字典
                    clearDictOnPath(_currentPath);//清理对象
                }
                //记录当前的数据长度
                addKeyValueToPath(_currentPath,"length",new JsonData(value_.Count),false);
                //明确其为数组，键 [0] 主要是应对 0 长的时候也能明确它维数组。(length有可能是json原有，不能作为数组标示)
                addKeyValueToPath(_currentPath,"[0]",new JsonData(_jsonListMark),false);
                StringBuilder _sb = new StringBuilder (20);
                //清理原有数组。需要清理整个字典
                for (int _idx = 0; _idx < value_.Count; _idx++) {
                     var _tempValue = value_[_idx];
                    _sb.Append ("[");
                    _sb.Append ((_idx + 1).ToString());
                    _sb.Append ("]");
                    addKeyValueToPath(
                        _currentPath,
                        _sb.ToString(),
                        _tempValue,
                        false
                    );
                    _sb.Clear();
                }
                //变更记录，刚放上的承载数据元素的新字典
                justChangeDict[_currentPath] = getDictOnPath(_currentPath,false);
            } else if (value_.IsObject) { //字典覆盖
                //字典覆盖。不需要清理字典上与当前无关的值
                foreach (string _tempKey in value_.Keys) {
                    var _tempValue = value_[_tempKey];
                    addKeyValueToPath(
                        _currentPath,
                        _tempKey,
                        _tempValue,
                        false
                    );
                }
                //变更记录，刚改变的字典(也可能是刚添加的字典)
                justChangeDict[_currentPath] = getDictOnPath(_currentPath,false);
            }
        }

        #region object
        //对象包装
        public JsonData convertObjectToJsonDict(object obj_){
            JsonData _jsonDict = new JsonData();
            _jsonDict.SetJsonType (JsonType.Object);
            foreach (System.Reflection.PropertyInfo _kvObj in obj_.GetType ().GetProperties ()) {
                if (_kvObj.DeclaringType != _kvObj.ReflectedType) { //非自定义方法
                    continue;
                }
                string _key = _kvObj.Name;
                var _value = _kvObj.GetValue (obj_);
                if (_value is bool ||_value is string ||_value is int ||_value is double) {
                    _jsonDict[_key] = new JsonData(_value);
                } else if (_value is Array) {
                    _jsonDict[_key] = convertArrayToJsonList((Array)_value);
                } else if (_value is object) {//最后包对象，因为对象是基类...上面的条件都满足
                    _jsonDict[_key] = convertObjectToJsonDict(_value);
                }
            }
            return _jsonDict;
        }
        //数组包装
        public JsonData convertArrayToJsonList(Array list_){
            JsonData _jsonArr = new JsonData();
            _jsonArr.SetJsonType (JsonType.Array);
            for (int _idx = 0; _idx < list_.Length; _idx++) {
                var _item = list_.GetValue (_idx);
                if (_item is bool ||_item is string ||_item is int ||_item is double) {
                    _jsonArr.Add(new JsonData(_item));
                }else if(_item is Array) {
                    _jsonArr.Add(convertArrayToJsonList((Array)_item));
                } else if (_item is object) {//最后包对象，因为对象是基类...上面的条件都满足
                    _jsonArr.Add(convertObjectToJsonDict(_item));
                }
            }
            return _jsonArr;
        }
        //C# 对象直接赋值
        public void addKeyValueToPath(string path_,string key_,object obj_,bool isMain_ = true){
            addKeyValueToPath(path_,key_,convertObjectToJsonDict(obj_),isMain_);
        }
        #endregion

    }
}