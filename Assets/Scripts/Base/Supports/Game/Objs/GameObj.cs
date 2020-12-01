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

        public static void doSample () {
            //游戏对象对数据监听的封装，这样可以监听到所有想监听的数据路径。
            GameObj _gameObj = new GameObj (null);
            _gameObj.addDataPathListenByList (new string[]{"prop.iHp","prop.sName"}); //增加监听的数据路径
            _gameObj.setValueToPath("prop.iHp",1);
            _gameObj.setValueToPath("prop.sName","xx");
            _gameObj.removeDataPathListen("prop.iHp");
            _gameObj.removeDataPathListen("prop.sName");
        }

        public GameWorldBase gameWorld;
        public ValueObj valueObject = null;
        private List<string> _dataPathListenList = new List<string>();//监听的数据列表
        public GameObj (GameWorldBase gameWorld_) : base () {
            gameWorld = gameWorld_;
            valueObject = new ValueObj ();//数据封装对象
        }
        public override void Dispose () {
            //移除ValueObj的各种监听
            if (_dataPathListenList != null) {
                for (int _idx = 0; _idx < _dataPathListenList.Count; _idx++) {
                    removeDataPathListen (_dataPathListenList[_idx]);
                }
            }
            valueObject.Dispose ();
            valueObject = null;
            
            base.Dispose ();
        }

        //重新根据给定的数据初始化
        public void setValueToPath(string path_,JsonData jsonData_, bool dispatch_ = true) {
            valueObject.setValueToPath(path_, jsonData_, dispatch_); //设置值对象，初始化，设置对象，可以不分发事件给监听。
        }
        public void setValueToPath(string path_,object dataObject_, bool dispatch_ = true) {
            valueObject.setValueToPath(path_, dataObject_, dispatch_); //设置值对象，初始化，设置对象，可以不分发事件给监听。
        }

        //对给定的数据进行监听，数据名称的头一个字符表示数据类型。
        public void addDataPathListen (string dataPath_) {
            if (_dataPathListenList.IndexOf (dataPath_) >= 0) { //监听过了
                return;
            }
            valueObject.AddListener<string, JsonData> (dataPath_, dataChangeHandle);
        }
        public void addDataPathListenByList(string[] targetDataPathList_){
            addDataPathListenByList(new List<string>(targetDataPathList_));
        }
        public void addDataPathListenByList(List<string> targetDataPathList_) {
            //当前对象没有监听过的(取 _dataPathListenList 对 targetDataPathList_ 的差集)
            List<string> _newDataPathListenList = ListUtils.except (targetDataPathList_, _dataPathListenList);
            for (int _idx = 0; _idx < _newDataPathListenList.Count; _idx++) {
                string _dataPath = _newDataPathListenList[_idx]; //这个路径添加过的话也要再添加。因为数据中心可能不是为一个单一对象准备的。
                addDataPathListen(_dataPath); //通过差集算出来的，不用校验是否已经存在，肯定不存在
            }
            //在当前的数据路径监听队列中加上这个新对象(取并集)
            _dataPathListenList = ListUtils.union (_dataPathListenList, _newDataPathListenList);
        }
        public void removeDataPathListen (string dataPath_) {
            valueObject.RemoveListener<string, JsonData> (dataPath_, dataChangeHandle);
        }

        public virtual void dataChangeHandle (string dataPath_, JsonData value_) {
            if (StringUtils.fastEqual (dataPath_, "prop.iHp")) { //比较字符串是否一致，快速判断
                if(!value_.IsInt){
                    Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                        dataPath_ + ": 数据类型不是期待类型。"
                    );
                }
                Debug.Log(String.Format("{0} : {1}",dataPath_,(value_ as IJsonWrapper).GetInt()));
            }else if (StringUtils.fastEqual (dataPath_, "prop.sName")) { //比较字符串是否一致，快速判断
                if(!value_.IsString){
                    Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                        dataPath_ + ": 数据类型不是期待类型。"
                    );
                }
                Debug.Log(String.Format("{0} : {1}",dataPath_,(value_ as IJsonWrapper).GetString()));
            }
        }
    }
}