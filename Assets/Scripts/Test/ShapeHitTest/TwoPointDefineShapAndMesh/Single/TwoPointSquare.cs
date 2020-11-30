using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;
using Utils;
using Game;

public class TwoPointSquare : TwoPointBase {

    public float rectPosX;
    public float rectPosY;
    public float rectWidth;
    public float rectHeight;
    public override void CreateMeshAndShape () { //根据子类所表示的类型创建 Shape 和 Mesh
        //承载显示对象的东西
        GameObject _meshSquareGO = new GameObject ();
        //创建一个Mesh
        _meshRingOrContainer = (MeshBySquare) _meshSquareGO.AddComponent<MeshBySquare> ();
        _meshSquareGO.name = "MeshBySquare";
        //将它作为子对象加载到自己身上。
        DisplayUtils.addBToA (gameObject, _meshSquareGO);
        //初始化坐标,同步x坐标
        beginGO.transform.position = new Vector3 (
            endGO.transform.position.x,
            beginGO.transform.position.y,
            beginGO.transform.position.z
        );
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

        //高度像素差大于10 ，就
        if (_width > 0.1f) {
            if (lastBeginGoX != beginGO.transform.position.x) { //begin 在移动，end去补位置
                endGO.transform.position = new Vector3 (
                    beginGO.transform.position.x,
                    endGO.transform.position.y,
                    endGO.transform.position.z
                );
            } else if (lastEndGoX != endGO.transform.position.x) { //end 在移动，begin补位置
                beginGO.transform.position = new Vector3 (
                    endGO.transform.position.x,
                    beginGO.transform.position.y,
                    beginGO.transform.position.z
                );
            }
            _width = 0.0f;
        }

        float _posx = _smallX + _width * 0.5f;
        float _posy = _smallY + _height * 0.5f;

        if (isInit_) {
            //创建一个Shape
            _shape = new ShapeSquare (
                new Vector2 (_posx, _posy),
                _height
            );
        } else {
            //重置Shape
            ((ShapeSquare) _shape).reset (_posx, _posy, _height);
        }

        rectPosX = ((ShapeSquare)_shape).currentRect.x;
        rectPosY = ((ShapeSquare)_shape).currentRect.y;
        rectWidth = ((ShapeSquare)_shape).currentRect.width;
        rectHeight = ((ShapeSquare)_shape).currentRect.height;

        //通过Shape刷新自己
        ((MeshBySquare) _meshRingOrContainer).resetByShape ((ShapeSquare) _shape);
        //摆放自己的位置，在两点之间
        _meshRingOrContainer.transform.localPosition = new Vector3 (_posx, _posy, 0);
    }

    public void Update () { //子类根据自己的 Shape 和 Mesh 进行形状 -> 显示的同步。
        if (isNeedReset ()) {
            reset (false);
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