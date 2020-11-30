using System;
using UnityEngine;

namespace Utils {
    public class DisplayUtils {
        public static GameObject _mainContainer;

        //添加对象到对象
        public static void addBToA (GameObject A, GameObject B) {
            B.transform.parent = A.transform;
        }
        public static void addBToA (Transform A, GameObject B) {
            B.transform.parent = A;
        }
        public static void addBToA (GameObject A, Transform B) {
            B.parent = A.transform;
        }
        public static void addBToA (Transform A, Transform B) {
            B.parent = A;
        }

        public static float AFaceToBDegree(Transform A, Transform B) {
            return CircleUtils.RadianstoDegrees(Mathf.Atan2((B.position.y-A.position.y), (B.position.x-A.position.x)));
        }

    }
}