using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;
using Utils;
using Game;

public class ThreePointSector : TwoPointBase {
    public GameObject currentBegin = null;
    public GameObject currentEnd = null;
    public float lastPX;
    public float lastPY;

    public GameObject P = null;

    public LineRendererSegment _lineRender;

    public override void Awake () {
        base.Awake ();
        if (P == null) {
            Debug.LogError ("ERROR ThreePointSector 必须指定 P 点，确定 angleRange");
        }
        if (
            P.transform.parent != transform
        ) {
            Debug.LogError ("ERROR ThreePointSector P 点，必须在当前 transform 下。");
        }
        _lineRender = gameObject.AddComponent<LineRendererSegment> ();
    }
    public override void CreateMeshAndShape () { //根据子类所表示的类型创建 Shape 和 Mesh
        //承载显示对象的东西
        GameObject _meshSectorGO = new GameObject ();
        //创建一个Mesh
        _meshRingOrContainer = (MeshBySector) _meshSectorGO.AddComponent<MeshBySector> ();
        _meshSectorGO.name = "MeshBySector";
        //将它作为子对象加载到自己身上。
        DisplayUtils.addBToA (gameObject, _meshSectorGO);
        //初始化
        reset (true);
    }

    public void reset (bool isInit_) {
        // //计算方向向量
        Vector2 directionV2 = endGO.transform.position - beginGO.transform.position;
        // //获取角度
        // float direction = LineUtils.getAngle2D(directionV2);
        float direction = LineUtils.getAngle2D (beginGO.transform.position, endGO.transform.position);
        //获取边向量
        Vector2 edgeV2 = P.transform.position - beginGO.transform.position;
        //-180 到 180 之间
        float angleRange2 = Vector2.SignedAngle (edgeV2, directionV2);
        bool _isRangeMax = false;
        if (angleRange2 > 90.0f) {
            angleRange2 = 90.0f;
            _isRangeMax = true;
        }
        if (angleRange2 < -90.0f) {
            angleRange2 = -90.0f;
            _isRangeMax = true;
        }

        float angleRange = Math.Abs (angleRange2) * 2.0f;

        float _posx = beginGO.transform.position.x;
        float _posy = beginGO.transform.position.y;

        //限制第三点
        if (edgeV2.magnitude != directionV2.magnitude) {
            edgeV2.Normalize ();
            edgeV2 = edgeV2 * directionV2.magnitude;
            P.transform.position = new Vector3 (
                beginGO.transform.position.x + edgeV2.x,
                beginGO.transform.position.y + edgeV2.y,
                P.transform.position.z
            );
        }

        if (isInit_) {
            //创建一个Shape
            _shape = new ShapeSector (
                new Vector2 (_posx, _posy),
                direction,
                angleRange,
                directionV2.magnitude
            );
        } else {
            if (base.isNeedReset ()) {
                //begin和end两个点的移动，就是没调整角度，所以，p做的角度决定要忽略
                angleRange = ((ShapeSector) _shape).angleRange;
                //P点也要重新摆放位置
                Vector2 _newP = LineUtils.rotation (directionV2, angleRange * 0.5f);
                P.transform.position = new Vector3 (
                    beginGO.transform.position.x + _newP.x,
                    beginGO.transform.position.y + _newP.y,
                    P.transform.position.z
                );
            }
            //重置Shape
            ((ShapeSector) _shape).reset (
                _posx, _posy,
                direction,
                angleRange,
                directionV2.magnitude
            );
        }

        //通过Shape刷新自己
        ((MeshBySector) _meshRingOrContainer).resetByShape ((ShapeSector) _shape);
        //摆放自己的位置，在两点之间
        _meshRingOrContainer.transform.localPosition = new Vector3 (_posx, _posy, 0);

        beginGO.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, direction + 90));
        endGO.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, direction - 90));

        if (_isRangeMax) {
            Vector2 _newP = LineUtils.rotation (directionV2, angleRange * 0.5f);
            P.transform.position = new Vector3 (
                beginGO.transform.position.x + _newP.x,
                beginGO.transform.position.y + _newP.y,
                P.transform.position.z
            );
        }
    }

    public override bool isNeedReset () {
        if (base.isNeedReset () ||
            P.transform.position.x != lastPX ||
            P.transform.position.y != lastPY
        ) {
            return true;
        }
        return false;
    }
    public void Update () { //子类根据自己的 Shape 和 Mesh 进行形状 -> 显示的同步。
        if (isNeedReset ()) {
            reset (false);
            //画近似边
            Vector2[] _polygonPos = ((ShapeSector) _shape).polygonPos;
            int _polygonPosCount = ((ShapeSector) _shape).polygonPosCount;
            _lineRender.reDraw (V2Utils.getCount (_polygonPos,_polygonPosCount ),false);
        }
    }
    public override void LateUpdate () {
        if (isNeedReset ()) {
            if (currentBegin != null) {
                currentBegin.transform.position = new Vector3(
                    _shape.pos.x,
                    _shape.pos.y,
                    0
                );
            }
            bool _isHit = false;
            foreach (TwoPointBase _tp in twoPointDict.Values) {
                if (_tp != this) {
                    if(_tp._shape.type == Shape2DObj.ShapeType.Circle){
                        if (currentEnd != null) {
                            currentEnd.transform.position = new Vector3(
                                _tp._shape.pos.x,
                                _tp._shape.pos.y,
                                0
                            );
                        }
                    }
                    if (_shape.hitOther (_tp._shape)) {
                        _isHit = true;
                    }
                }
            }
            isHitBoo (_isHit);
        }
        base.LateUpdate ();
        lastPX = P.transform.position.x;
        lastPY = P.transform.position.y;
    }
}