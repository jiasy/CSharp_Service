using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Dis {
    //坐标轴
    public class GoldCutCoordinate : MonoBehaviour {
        private List<LineRendererSegment> _lineRenderers = new List<LineRendererSegment> ();
        public int currentShowIdx = -1;
        //绘画黄金分割
        public void Start () {
            LineRendererGoldCut _line = CreateUtils.getComponentAndAddTo (typeof (LineRendererGoldCut), this) as LineRendererGoldCut;
            _line.initStyle ("Sprites/Default",Color.grey,Color.grey,0.05f,0.05f);
            _line.gameObject.name = "line";
            _line.drawDefaultGoldCutLine ();
            _lineRenderers.Add (_line);

            LineRendererGoldCut _rect = CreateUtils.getComponentAndAddTo (typeof (LineRendererGoldCut), this) as LineRendererGoldCut;
            _rect.initStyle ("Sprites/Default",Color.white,Color.white,0.05f,0.05f);
            _rect.gameObject.name = "rect";
            _rect.drawDefaultGoldCutRect ();
            _lineRenderers.Add (_rect);

            LineRendererGoldCut _bezier = CreateUtils.getComponentAndAddTo (typeof (LineRendererGoldCut), this) as LineRendererGoldCut;
            _bezier.initStyle ("Sprites/Default",Color.green,Color.green,0.05f,0.05f);
            _bezier.gameObject.name = "bezier";
            _bezier.drawDefaultGoldCutBezier ();
            _lineRenderers.Add (_bezier);

            LineRendererGoldCut _circle = CreateUtils.getComponentAndAddTo (typeof (LineRendererGoldCut), this) as LineRendererGoldCut;
            _circle.initStyle ("Sprites/Default",Color.gray,Color.gray,0.05f,0.05f);
            _circle.gameObject.name = "circle";
            _circle.drawDefaultGoldCutCircle ();
            _lineRenderers.Add (_circle);
            //添加到自己上
            LineRendererGoldCutReal _rectReal = CreateUtils.getComponentAndAddTo (typeof (LineRendererGoldCutReal), this) as LineRendererGoldCutReal;
            _rectReal.initStyle ("Sprites/Default",Color.red,Color.red,0.1f,0.1f);
            _rectReal.gameObject.name = "rectReal";
            _rectReal.drawDefaultRect ();
            _lineRenderers.Add (_rectReal);

            LineRendererGoldCutReal _rectCircle = CreateUtils.getComponentAndAddTo (typeof (LineRendererGoldCutReal), this) as LineRendererGoldCutReal;
            _rectCircle.initStyle ("Sprites/Default",Color.yellow,Color.yellow,0.1f,0.1f);
            _rectCircle.gameObject.name = "rectCircle";
            _rectCircle.drawDefaultCruve ();
            _lineRenderers.Add (_rectCircle);

        }
        public void switchShow () {
            for (int _idx = 0; _idx < _lineRenderers.Count; _idx++) {
                LineRendererSegment _lineRendererSegment = _lineRenderers[_idx];
                _lineRendererSegment.gameObject.SetActive (false);
            }
            //当前只有一个显示
            currentShowIdx++;
            if (currentShowIdx == _lineRenderers.Count) {
                currentShowIdx = 0;
            }
            _lineRenderers[currentShowIdx].gameObject.SetActive (true);
        }
        private void Update () {
            //0左，1右，2中
            if (Input.GetMouseButtonUp (0)) { //鼠标抬起
                switchShow (); //切换显示
            }
        }
        private void OnGUI () {
            GUILayout.Label (" - - - - - - - - - - - currentShow - - - - - - - - - - ");
            if (currentShowIdx == -1) {
                GUILayout.Label ("all");
            } else {
                GUILayout.Label (currentShowIdx.ToString () + " : " + _lineRenderers[currentShowIdx].gameObject.name + " : " + _lineRenderers[currentShowIdx].GetType ());
            }
        }
    }
}