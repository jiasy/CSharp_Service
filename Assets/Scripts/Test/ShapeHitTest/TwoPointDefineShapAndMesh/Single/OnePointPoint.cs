using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;
using Utils;
using Game;

public class OnePointPoint : TwoPointBase {
    public override void CreateMeshAndShape () { //根据子类所表示的类型创建 Shape 和 Mesh
        //承载显示对象的东西
        GameObject _meshPointGO = new GameObject ();
        //创建一个Mesh
        _meshRingOrContainer = (MeshPoint) _meshPointGO.AddComponent<MeshPoint> ();
        _meshPointGO.name = "MeshPoint";
        //将它作为子对象加载到自己身上。
        DisplayUtils.addBToA (gameObject, _meshPointGO);
        //初始化坐标,同步x坐标
        beginGO.transform.position = new Vector3 (
            endGO.transform.position.x,
            endGO.transform.position.y,
            endGO.transform.position.z
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
        if (
            _width > 0.01f
            ||
            _height > 0.01f
        ) {
            if (
                lastBeginGoX != beginGO.transform.position.x
                ||
                lastBeginGoY != beginGO.transform.position.y
            ) { //begin 在移动，end去补位置
                endGO.transform.position = new Vector3 (
                    beginGO.transform.position.x,
                    beginGO.transform.position.y,
                    beginGO.transform.position.z
                );
            } else if (
                lastEndGoX != endGO.transform.position.x
                ||
                lastEndGoY != endGO.transform.position.y
            ) { //end 在移动，begin补位置
                beginGO.transform.position = new Vector3 (
                    endGO.transform.position.x,
                    endGO.transform.position.y,
                    endGO.transform.position.z
                );
            }
            _width = 0.0f;
            _height = 0.0f;
        }

        float _posx = _smallX;
        float _posy = _smallY;

        if (isInit_) {
            //创建一个Shape
            _shape = new ShapePoint (
                new Vector2 (_posx, _posy)
            );
        } else {
            //重置Shape
            ((ShapePoint) _shape).reset (_posx, _posy);
        }

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