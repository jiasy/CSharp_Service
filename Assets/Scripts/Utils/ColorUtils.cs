using UnityEngine;

namespace Utils {
    public class ColorUtils {
        public static Color randomColor () {
            return new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);
        }
    }
}