using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Game;
using LitJson;
using UnityEngine;
using Utils;
using Objs;

namespace Game {
    public class GameObj : BaseObj {
        public GameWorldBase gameWorld;
        public EventObserverObj eventObserver = null;
        public ValueObj vo = null;
        private List<string> _dataPathListenList = null;//监听的数据列表
        public GameObj (GameWorldBase gameWorld_) : base () {
            gameWorld = gameWorld_;
            //对象内的监听分发。
            vo = new ValueObj ();
            eventObserver = new EventObserverObj ();
        }
        public override void Dispose () {
            //事件中心销毁
            eventObserver.Dispose ();
            eventObserver =null;

            //移除ValueObj的各种监听
            if (_dataPathListenList != null) {
                for (int _idx = 0; _idx < _dataPathListenList.Count; _idx++) {
                    removeDataPathListen (_dataPathListenList[_idx]);
                }
            }
            vo.Dispose ();
            vo = null;
            
            base.Dispose ();
        }

        //重新根据给定的数据初始化
        public void reInitVo (JsonData jsonData_, bool dispatch_) {
            vo.sv ("", jsonData_, dispatch_); //设置值对象，初始化，设置对象，可以不分发事件给监听。
        }
        public void reInitVo (object dataObject_, bool dispatch_) {
            vo.sv ("", dataObject_, dispatch_); //设置值对象，初始化，设置对象，可以不分发事件给监听。
        }

        //对给定的数据进行监听，数据名称的头一个字符表示数据类型。
        public void addDataPathListen (string dataPath_, bool noNeedCheck_ = false) {
            if (noNeedCheck_ == false) { //不越过检测
                if (_dataPathListenList == null) { //没创建过监听列表，就创建一个
                    _dataPathListenList = new List<string> ();
                } else { //创建过监听列表的，判断是否已经监听了这个数据路径
                    if (_dataPathListenList.IndexOf (dataPath_) >= 0) { //监听过了
                        return;
                    }
                }
            }
            string[] _dataPathArr = StringUtils.splitAWithB (dataPath_, ".");
            string _lastPath = _dataPathArr[_dataPathArr.Length - 1];
            if (_lastPath.IndexOf ("i") == 0) { //int
                vo.eventObserver.AddListener<string, int> (dataPath_, DataChanged);
            } else if (_lastPath.IndexOf ("s") == 0) { //string
                vo.eventObserver.AddListener<string, string> (dataPath_, DataChanged);
            } else if (_lastPath.IndexOf ("b") == 0) { //bool
                vo.eventObserver.AddListener<string, bool> (dataPath_, DataChanged);
            } else if (_lastPath.IndexOf ("a") == 0) { //array
                vo.eventObserver.AddListener<string, JsonData> (dataPath_, DataChangedArr);
            } else if (_lastPath.IndexOf ("d") == 0) { //dict
                vo.eventObserver.AddListener<string, JsonData> (dataPath_, DataChangedDict);
            } else if (_lastPath.IndexOf ("f") == 0) { //float
                vo.eventObserver.AddListener<string, float> (dataPath_, DataChanged);
            } else {
                vo.eventObserver.AddListener<string, JsonData> (dataPath_, DataChangedDict);
                Debug.LogWarning ("数据路径不命名默认按照Dict处理 : " + _lastPath);
            }
        }
        public void addDataPathListenByList (List<string> targetDataPathList_) {
            if (_dataPathListenList == null) { //没创建过监听列表，就创建一个
                _dataPathListenList = new List<string> ();
            }
            //当前对象没有监听过的
            List<string> _newDataPathListenList = ListUtils.except (targetDataPathList_, _dataPathListenList);
            for (int _idx = 0; _idx < _newDataPathListenList.Count; _idx++) {
                string _dataPath = _newDataPathListenList[_idx]; //这个路径添加过的话也要再添加。因为数据中心可能不是为一个单一对象准备的。
                addDataPathListen (_dataPath, true); //通过差集算出来的，不用校验是否已经存在，肯定不存在
            }
            //在当前的数据路径监听队列中加上这个新对象
            _dataPathListenList = ListUtils.union (_dataPathListenList, _newDataPathListenList);
        }
        public void removeDataPathListen (string dataPath_) {
            string[] _dataPathArr = StringUtils.splitAWithB (dataPath_, ".");
            string _lastPath = _dataPathArr[_dataPathArr.Length - 1];
            if (_lastPath.IndexOf ("i") == 0) { //int
                vo.eventObserver.RemoveListener<string, int> (dataPath_, DataChanged);
            } else if (_lastPath.IndexOf ("s") == 0) { //string
                vo.eventObserver.RemoveListener<string, string> (dataPath_, DataChanged);
            } else if (_lastPath.IndexOf ("b") == 0) { //bool
                vo.eventObserver.RemoveListener<string, bool> (dataPath_, DataChanged);
            } else if (_lastPath.IndexOf ("a") == 0) { //array
                vo.eventObserver.RemoveListener<string, JsonData> (dataPath_, DataChangedArr);
            } else if (_lastPath.IndexOf ("d") == 0) { //dict
                vo.eventObserver.RemoveListener<string, JsonData> (dataPath_, DataChangedDict);
            } else if (_lastPath.IndexOf ("f") == 0) { //float
                vo.eventObserver.RemoveListener<string, float> (dataPath_, DataChanged);
            } else {
                vo.eventObserver.RemoveListener<string, JsonData> (dataPath_, DataChangedDict);
            }
        }

        public virtual void DataChanged (string dataPath_, int int_) {
            if (StringUtils.fastEqual (dataPath_, "prop.iHp")) { //比较字符串是否一致，快速判断
                Debug.Log (dataPath_ + " : " + int_.ToString ());
            }
        }
        public virtual void DataChanged (string dataPath_, float float_) {

        }
        public virtual void DataChanged (string dataPath_, string str_) {

        }
        public virtual void DataChanged (string dataPath_, bool boo_) {

        }
        public virtual void DataChangedArr (string dataPath_, JsonData arr_) {

        }
        public virtual void DataChangedDict (string dataPath_, JsonData dict_) {

        }
    }
}