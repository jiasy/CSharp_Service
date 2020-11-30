using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Utils;
using System.Text;

//json格式数据的包装
namespace Objs {
    public class JsonDataWrap : BaseObj {
        public JsonData dataSet = null;
        public Dictionary<string, JsonData> pathValueDict = new Dictionary<string,JsonData>();
        //刚刚变化的数据路径，原有值 -> 现在值。
        public Dictionary<string, JsonData> justChangeDict = new Dictionary<string,JsonData>();
        public JsonDataWrap () : base () {
            dataSet = new JsonData ();
            dataSet.SetJsonType (JsonType.Object);
        }

        public override void Dispose () {
            dataSet.Clear();
            dataSet = null;
            pathValueDict.Clear();
            pathValueDict = null;
            justChangeDict.Clear();
            justChangeDict = null;
            base.Dispose ();
        }

        public void PrintPathValueDict(){
            string _logStr = "pathValueDict : \n";
            foreach(string _tempPath in pathValueDict.Keys) {//以这个路径开头的所有缓存都全部清理
                _logStr += "    " + _tempPath + " : " + pathValueDict[_tempPath].ToString() + "\n";
            }
            Debug.Log(_logStr);
        }

        public void PrintJustChangeDict(){
            string _logStr = "justChangeDict : \n";
            foreach(string _tempPath in justChangeDict.Keys) {//以这个路径开头的所有缓存都全部清理
                _logStr += "    " + _tempPath + " : " + justChangeDict[_tempPath].ToString() + "\n";
            }
            Debug.Log(_logStr);
        }

        private struct ParentDictInfo{
            public string key;
            public string parentPath;
            public JsonData parentDict;
        }; 

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

        private ParentDictInfo getParentDictInfo(string path_){
            ParentDictInfo _parentDictInfo;
            if (path_.Contains(".")) {//多层
                ArrayList _dataPathList = new ArrayList (path_.Split('.'));
                _parentDictInfo.key = (string)_dataPathList[_dataPathList.Count - 1];
                _dataPathList.RemoveAt(_dataPathList.Count - 1);//退出最后一个
                _parentDictInfo.parentPath = string.Join(".",_dataPathList);//合并出所在的字典节点路径
                _parentDictInfo.parentDict = getDictOnPath(_parentDictInfo.parentPath,false);//获取父数据节点，不确保其存在
            } else { //根路径下的第一层
                _parentDictInfo.key = path_;
                _parentDictInfo.parentPath = "";
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

        //获取路径上的对象
        public JsonData getDictOnPath(string path_,bool makeSureExists_ = true){
            JsonData _dictOnPath = null;
            if (pathValueDict.ContainsKey(path_)){
                _dictOnPath = pathValueDict[path_];
            }else{
                ArrayList _dataPathList = null;
                if (path_.Contains (".")) {//多层
                    _dataPathList = new ArrayList (path_.Split ('.'));
                } else { //根路径下的第一层
                    _dataPathList = new ArrayList ();
                    _dataPathList.Add (path_);
                }
                _dictOnPath = dataSet;//数据枝干节点
                StringBuilder _sb = new StringBuilder (20);//当前路径拼接
                while (_dataPathList.Count > 0) {//逐层级，直至目标对象
                    string _currentKey = (string) _dataPathList[0];//逐层向下获取节点
                    _dataPathList.RemoveAt (0);
                    _sb.Append(_currentKey);
                    if (_dictOnPath.ContainsKey(_currentKey) == false) {//没有这个键对应的对象，就创建
                        if (makeSureExists_){//确保其一定存在
                            _dictOnPath[_currentKey] = new JsonData ();
                            _dictOnPath[_currentKey].SetJsonType(JsonType.Object);
                            pathValueDict[_sb.ToString()] = _dictOnPath[_currentKey];//缓存到路径和值的键值对中
                        }else{//不确保存在，遇到不存在就直接返回空。
                            _sb.Clear();
                            return null;
                        }
                    }
                    _dictOnPath = _dictOnPath[_currentKey];
                    _sb.Append(".");
                }
                _sb.Clear();
            }

            if (!_dictOnPath.IsObject) {//不是字典就返回空
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    " 数据路径不是字典 "+path_
                );
                return null;
            }

            return _dictOnPath;
        }
        
        public void addKeyValueToPath(
            string path_,
            string key_,
            JsonData value_,
            bool isMain_ = true
        ){
            if (string.IsNullOrEmpty (key_)) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    " key_ 不能为空"
                );
                return;
            }

            JsonData _currentDictOnPath = null;
            if (string.IsNullOrEmpty (path_)) {//空 或者 空字符串 都是直接在数据对象上操作
                _currentDictOnPath = dataSet;
            }else{
                _currentDictOnPath = getDictOnPath(path_,true);
            }

            if(_currentDictOnPath == null){
                return;
            }

            if (isMain_){//外部调用
                justChangeDict.Clear();//清理键值
            }

            StringBuilder _sb = new StringBuilder (20);
            _sb.Append (path_);
            _sb.Append (".");
            _sb.Append (key_);
            string _currentPath = _sb.ToString ();//当前路径
            _sb.Clear ();
            
            JsonData _originalValue = null;
            if (value_.IsInt ||value_.IsDouble ||value_.IsLong ||value_.IsBoolean ||value_.IsString) {
                if (pathValueDict.ContainsKey(_currentPath)) {// 原来这个位置有值
                    _originalValue = pathValueDict[_currentPath];//获取原有值
                    if(_originalValue.GetJsonType() != value_.GetJsonType()){//新值和原有类型不一样
                        if(_originalValue.IsBoolean || _originalValue.IsString || value_.IsBoolean || value_.IsString){
                            Debug.LogError("原有值和现有值中有非数字类型，且变更前后类型发生变化，应当是 数字、布尔、字符串 之间的变化。\n"+_currentPath);
                        }
                        if(_originalValue.IsObject){//字典和数组都是字典
                            Debug.LogError("原有值是数组或字典，变换成纯值\n"+_currentPath);
                        }
                    }
                    _originalValue.Clear();//IsArray、IsObject时有效
                }
                pathValueDict[_currentPath] = value_;//缓存新值
                justChangeDict[_currentPath] = value_;//缓存变化
                _currentDictOnPath[key_] = value_;//附新值
            } else if (value_.IsArray) { //数组替换
                JsonData _oldDictOnPath = getDictOnPath(_currentPath,false);//获取旧的。
                if(_oldDictOnPath != null){
                    if (_oldDictOnPath.IsInt ||_oldDictOnPath.IsDouble ||_oldDictOnPath.IsLong ||_oldDictOnPath.IsBoolean ||_oldDictOnPath.IsString) {
                        Debug.LogError("原有值是纯值，变换成数组字典\n"+_currentPath);
                    }
                    _oldDictOnPath.Clear();//调用清理方法，清理字典
                    clearDictOnPath(_currentPath);//清理对象
                }
                //清理原有数组。需要清理整个字典
                for (int _idx = 0; _idx < value_.Count; _idx++) {
                     var _tempValue = value_[_idx];
                    _sb.Append ("[");
                    _sb.Append (_idx.ToString());
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
        public void addKeyValueToPath(
            string path_,
            string key_,
            object obj_
        ){
            addKeyValueToPath(
                path_,
                key_,
                new JsonData(obj_)
            );
        }
    }
}