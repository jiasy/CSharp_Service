using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;
using Utils;
using Game;

public class TwoPointRect : TwoPointBase {
    public float rectPosX;
    public float rectPosY;
    public float rectWidth;
    public float rectHeight;
    public override void CreateMeshAndShape () { //根据子类所表示的类型创建 Shape 和 Mesh
        //承载显示对象的东西
        GameObject _meshRectGO = new GameObject ();
        //创建一个Mesh
        _meshRingOrContainer = (MeshByRect) _meshRectGO.AddComponent<MeshByRect> ();
        _meshRectGO.name = "MeshByRect";
        //将它作为子对象加载到自己身上。
        DisplayUtils.addBToA (gameObject, _meshRectGO);
        //初始化
        reset (true);
    }

    public void reset (bool isInit_) {
        float _smallX = Math.Min (beginGO.transform.position.x, endGO.transform.position.x);
        float _smallY = Math.Min (beginGO.transform.position.y, endGO.transform.position.y);
        float _bigX = Math.Max (beginGO.transform.position.x, endGO.transform.position.x);
        float _bigY = Math.Max (beginGO.transform.position.y, endGO.transform.position.y);
        float _width = _bigX - _smallX;
        float _height = _bigY - _smallY;
        float _posx = _smallX + _width * 0.5f;
        float _posy = _smallY + _height * 0.5f;

        if (isInit_) {
            //创建一个Shape
            _shape = new ShapeRect (
                new Vector2 (_posx, _posy),
                _width,
                _height
            );
        } else {
            //重置Shape
            ((ShapeRect) _shape).reset (_posx, _posy, _width, _height);
        }

        rectPosX = ((ShapeRect)_shape).currentRect.x;
        rectPosY = ((ShapeRect)_shape).currentRect.y;
        rectWidth = ((ShapeRect)_shape).currentRect.width;
        rectHeight = ((ShapeRect)_shape).currentRect.height;

        //通过Shape刷新自己
        ((MeshByRect) _meshRingOrContainer).resetByShape ((ShapeRect) _shape);
        //摆放自己的位置，在两点之间
        _meshRingOrContainer.transform.localPosition = new Vector3 (_posx, _posy, 0);
    }

    public void Update () { //子类根据自己的 Shape 和 Mesh 进行形状 -> 显示的同步。
        if (isNeedReset ()) {

            reset (false);

            float _angle = DisplayUtils.AFaceToBDegree (beginGO.transform, endGO.transform);
            beginGO.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, _angle));
            endGO.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, _angle));
        }
    }
    public override void LateUpdate () {
        if (isNeedReset ()) {
            bool _isHit = false;
            foreach (TwoPointBase _tp in twoPointDict.Values) {
                if (_tp != this) {
                    if (this._shape.hitOther (_tp._shape)) {
                        _isHit = true;
                    }
                }
            }
            isHitBoo (_isHit);
        }
        base.LateUpdate ();
    }
}