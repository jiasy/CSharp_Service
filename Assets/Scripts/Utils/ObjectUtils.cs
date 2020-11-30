using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

namespace Utils {
    public class ObjectUtils {
        public static void doSample () {
            //获取属性值
            var _obj = new { A = "a", B = "b" };
            var _valueOfA = getPropertyInObject (_obj, "A");
            Debug.Log ("_valueOfA : " + _valueOfA.ToString ());

        }

        public static string getPropertyInObject (object obj_, string propertyName_) {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties (obj_);
            PropertyDescriptor _property = pdc.Find (propertyName_, true);
            return _property.GetValue (obj_).ToString ();
        }
    }
}