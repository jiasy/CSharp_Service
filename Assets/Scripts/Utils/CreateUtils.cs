using System;
using UnityEngine;

namespace Utils {
    public class CreateUtils {
        //第一个参数使用  typeof(SphereCollider) 来获得类型
        public static Component getComponentAndAddTo (Type t_, MonoBehaviour component_) {
            return getComponentAndAddTo (t_, component_.gameObject);
        }
        public static Component getComponentAndAddTo (Type t_, GameObject parent_) {
            return getComponentAndAddTo (t_, parent_.transform);
        }
        public static Component getComponentAndAddTo (Type t_, Transform parentTrans_) {
            Component _component = getComponent (t_);
            _component.transform.parent = parentTrans_;
            return _component;
        }
        public static Component getComponent (Type t_, object[] parameters_ = null) {
            return (new GameObject ()).AddComponent (t_);
        }

    }
}