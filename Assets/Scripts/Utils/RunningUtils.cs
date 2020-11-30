using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Dis;
using Objs;
using UnityEngine;

namespace Utils {
    public class RunningUtils {
        // //当前所有运行的对象中，可以和池交互的对象
        // public static Dictionary<string,List<IPoolObj>> _runningPoolObjDict = new Dictionary<string,List<IPoolObj>>();
        //当前运行的所有对象[包含 池交互 的一部分对象，不包含不继承 BaseObj 的对象]
        public static Dictionary<string, List<BaseObj>> _runningObjDict = new Dictionary<string, List<BaseObj>> ();

        //运行BaseObj的子类集合--------------------------------------------------------------------------------------------------------------
        //当前类对应的运行时列表
        public static List<BaseObj> getRunningList (string fullClassName_) {
            List<BaseObj> _objList = null;
            if (!_runningObjDict.ContainsKey (fullClassName_)) {
                _objList = new List<BaseObj> ();
                _runningObjDict[fullClassName_] = _objList;
            } else {
                _objList = _runningObjDict[fullClassName_];
            }
            return _objList;
        }
        public static string runningInfo (bool _print = false) {
            string _infoStr = "";
            foreach (string _fullClassName in _runningObjDict.Keys) {
                List<BaseObj> _objList = getRunningList (_fullClassName);
                _infoStr = _infoStr + _fullClassName + " : " + _objList.Count.ToString () + System.Environment.NewLine;
            }
            if (_print) {
                Debug.Log ("当前运行状态如下" + System.Environment.NewLine + _infoStr);
            }
            return _infoStr;
        }

//        //将当前的运行信息对应到一个ValueObject上
//        public static void currentToValueObject (ValueObj vo_) {
//            dynamic _dict = new ExpandoObject ();
//            foreach (string _fullClassName in _runningObjDict.Keys) {
//                _dict[_fullClassName] = getRunningList (_fullClassName).Count.ToString ();
//            }
//            vo_.sv ("RunnigUtils", _dict);
//        }

        // //对象池----------------------------------------------------------------------------------------------------------------------------
        // //重新创建
        // //RunningUtils.reCreate(fullClassName,,new object[]{ parameter1,parameter2 });
        // public static IPoolObj reCreate(string fullClassName_,object[] parameters_ = null) {//对象池中获取对象
        //     IPoolObj _obj = PoolUtils.pullOut(fullClassName_,parameters_);//对象池创建出来的对象，都有所属池 <_obj.belongToPool>。
        //     List<IPoolObj> _objList;//获取对应的运行时列表
        //     if (_obj.belongToRunningList == null) {
        //         _objList  = getRunningPoolList(fullClassName_);
        //         _obj.belongToRunningList = _objList;
        //     } else {
        //         _objList = _obj.belongToRunningList;
        //     }
        //     _objList.Add(_obj);//添加到运行时列表中
        //     return _obj;
        // }
        // //重新销毁
        // public static void reDestory(IPoolObj obj_) {
        //     obj_.belongToPool.pushBack(obj_);//放回池
        //     obj_.belongToRunningList.Remove(obj_);//当前的运行列表中移除这个对象
        // }
        // //当前类对应的运行时列表
        // public static List<IPoolObj> getRunningPoolList(string fullClassName_) {
        //     List<IPoolObj> _objList = null;
        //     if (!_runningPoolObjDict.ContainsKey(fullClassName_)) {
        //         _objList = new List<IPoolObj>();
        //         _runningPoolObjDict[fullClassName_] = _objList;
        //     } else {
        //         _objList = _runningPoolObjDict[fullClassName_];
        //     }
        //     return _objList;
        // }
        // //moudlueName_ 全部返还对象池
        // public static void backAllToPool() {
        //     foreach (string _fullClassName in _runningPoolObjDict.Keys) {
        //         backAllToPoolByClassName(_fullClassName);
        //     }
        // }
        // //moudlueName_ 运行中的类名对应的对象，返还对象池
        // public static void backAllToPoolByClassName(string fullClassName_) {
        //     List<IPoolObj> _objList = getRunningPoolList(fullClassName_);//获取对应的运行时列表
        //     while (_objList.Count > 0) {
        //         reDestory(_objList[0]);
        //     }
        // }

        // //清理所有池中对象
        // public static void clearAllPool() {
        //     foreach (string _fullClassName in _runningPoolObjDict.Keys) {
        //         clearPoolByClassName(_fullClassName);
        //     }
        // }
        // //moudlueName_ 运行中的类名对应的对象，返还对象池
        // public static void clearPoolByClassName(string fullClassName_) {
        //     List<IPoolObj> _objList = getRunningPoolList(fullClassName_);//获取对应的运行时列表
        //     while (_objList.Count > 0) {//调用销毁方法
        //         ((IDisposable)_objList[0]).Dispose();
        //     }
        // }
    }
}