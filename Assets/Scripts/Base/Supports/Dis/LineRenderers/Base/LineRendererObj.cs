using System.Collections;
using UnityEngine;
using Utils;

namespace Dis {
    public class LineRendererObj : MonoBehaviour {
        public LineRenderer lineRenderer;

        public bool isInited = false; //是否初始化过

        public LineRendererObj () {

        }

        //通过代码设置的单一过度色的线段
        //initStyle("Particles/Additive",Color.white,Color.black,0.5f,0.1f);//内置粒子使用的，叠加效果
        // initStyle("Sprites/Default",Color.yellow,Color.red,0.02f,0.01f);//内置粒子使用的，图片默认
        // initStyle("Unlit/Texture",Color.white,Color.white,0.04f,0.01f);//无光照Shader,更节省性能
        public void initStyle (string shader_, Color beginColor_, Color endColor_, float beginWidth_, float endWidth_) {
            if (isInited == false) {
                isInited = true;
                lineRenderer = gameObject.AddComponent<LineRenderer> (); //添加一个绘画线
                Shader _shader = Shader.Find (shader_);
                Material _material = new Material (_shader);
                lineRenderer.material = _material; //设置材质
                lineRenderer.startColor = endColor_;//前后倒置，为运动的哪一头是数组末尾
                lineRenderer.endColor = beginColor_;
                lineRenderer.startWidth = endWidth_;
                lineRenderer.endWidth = beginWidth_;
                lineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
                lineRenderer.useWorldSpace = false;
            }
        }
        public void initByPic (string picPath_) {
            // if (isInited == false) {
            //     isInited = true;
            //     lineRenderer = gameObject.AddComponent<LineRenderer> (); //添加一个绘画线
            //     Shader _shader = Shader.Find (shader_);
            //     Material _material = new Material (_shader);
            //     lineRenderer.material = _material; //设置材质
            //     resetLineDefault ();
            //     resetColorWhite ();
            //     lineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
            //     lineRenderer.useWorldSpace = false;
            // }
        }

        protected Vector3 convertToWorldV3 (Vector2 v2_) {
            // z 必须是 1 ，才会绘画到场景。
            return Camera.main.ScreenToWorldPoint (new Vector3 (v2_.x, v2_.y, 1));
        }
        protected void resetColorFade (float beginWidth_, Color beginColor_) {
            lineRenderer.startColor = Color.black; //设置为黑色，叠加之后就没颜色了
            lineRenderer.endColor = beginColor_;
            lineRenderer.startWidth = 0.001f; //终点的宽几乎没有，这样就是渐隐的效果了
            lineRenderer.endWidth = beginWidth_;
        }
    }
}