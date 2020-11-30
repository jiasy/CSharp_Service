using System;
using UnityEngine;
using Utils;


namespace Utils {
    public class UIAnalyseUtils {
        public static void analyseUI (GameObject targetNode_,GameObject nodeMain_) {
            Transform _containerTrans = targetNode_.transform;
            string _nodeName;
            bool _needRecord;
            bool _isSubUi = true;
            string _compareMode;
            foreach (Transform _childTrans in _containerTrans) {
                _nodeName = _childTrans.name;
                _needRecord =false;
                _compareMode = null;
                if(_nodeName.Contains("pass_")){
                    continue;
                }

                //判断当前节点是不是一个数据比较
                if (_nodeName.Contains("()")) {
                    _compareMode = "()";
                } else if (_nodeName.Contains("(]")) {
                    _compareMode = "(]";
                } else if (_nodeName.Contains("[)")) {
                    _compareMode = "[)";
                } else if (_nodeName.Contains("[]")) {
                    _compareMode = "[]";
                } else if (_nodeName.Contains(">=")) {
                    _compareMode = ">=";
                } else if (_nodeName.Contains("<=")) {
                    _compareMode = "<=";
                } else if (_nodeName.Contains(">")) {
                    _compareMode = ">";
                } else if (_nodeName.Contains("<")) {
                    _compareMode = "<";
                } else if (_nodeName.Contains("==")) {
                    _compareMode = "==";
                } else if (_nodeName.Contains("!=")) {
                    _compareMode = "!=";
                }

                if (_compareMode != null) {
                    _needRecord = false;
                    String[] _compareArr = StringUtils.splitAWithB(_nodeName,_compareMode);
                    // var _dc = node_.addComponent("DataCompare");
                    // _dc.dataPath = _compareArr[0];
                    // _dc.compareType = _compareMode;
                    // _dc.compareValue = _compareArr[1];
                    // if (_dc.dataPath.Contains("this.") {
                    //     _dc.uiNode = nodeMain_;
                    // }
                }


                if(_nodeName.Contains("btn_")){

                }else if (_nodeName.Contains(".")){
                    
                }
            }
        }
        public static void analyseUI (GameObject container_) {

        }
    }
}