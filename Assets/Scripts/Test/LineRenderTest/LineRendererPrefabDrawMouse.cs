using System;
using System.Collections.Generic;
using Objs;
using UnityEngine;
using Utils;

namespace Dis {
    //坐标轴
    public class LineRendererPrefabDrawMouse : LineRendererPrefab {
        public void Start () {
            initByPrefab ("Prefabs/Lines/ColorLineRenderer", 10);
        }
        protected void Update () {
            if (Input.GetMouseButton (0)) { //鼠标移动
                addV2 (Input.mousePosition);
            }
        }
    }
}