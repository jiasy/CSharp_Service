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
    public class GameObj : ValueObj {
        public static void doSample () {
            //游戏对象对数据监听的封装，这样可以监听到所有想监听的数据路径。
            List<string> _tempListenPath = new List<string> {
                "prop.iHp",
                "prop.sName"
            };
            GameObj _gameObj = new GameObj (null);
            _gameObj.addDataPathListenByList (_tempListenPath); //增加监听的数据路径
            _gameObj.setValueToPath("prop.iHp",1);
            _gameObj.setValueToPath("prop.sName","xx");
            _gameObj.removeDataPathListen("prop.iHp");
            _gameObj.removeDataPathListen("prop.sName");
        }

        public GameWorldBase gameWorld;
        
        public GameObj (GameWorldBase gameWorld_) : base () {
            gameWorld = gameWorld_;
        }
        public override void Dispose () {
            base.Dispose ();
        }

        public override void dataChangeHandle (string dataPath_, JsonData value_) {
            if (StringUtils.fastEqual(dataPath_, "prop.iHp")) {
                if(!value_.IsInt){
                    Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                        dataPath_ + ": 数据类型不是期待类型。"
                    );
                }
                Debug.Log(String.Format("dataChanged {0} : {1}",dataPath_,(value_ as IJsonWrapper).GetInt()));
            }else if (StringUtils.fastEqual(dataPath_, "prop.sName")) {
                if(!value_.IsString){
                    Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                        dataPath_ + ": 数据类型不是期待类型。"
                    );
                }
                Debug.Log(String.Format("dataChanged {0} : {1}",dataPath_,(value_ as IJsonWrapper).GetString()));
            }
        }
    }
}