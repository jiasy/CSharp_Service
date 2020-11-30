using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;
using Utils;
using Game;

public class TwoPointLine : TwoPointBase {
    public GameObject currentBegin = null;
    public GameObject currentEnd = null;

    public float currentBeginX;
    public float currentBeginY;
    public float currentEndX;
    public float currentEndY;
    public override void CreateMeshAndShape () { //根据子类所表示的类型创建 Shape 和 Mesh
        //承载显示对象的东西
        GameObject _meshLineGO = new GameObject ();
        //创建一个Mesh
        _meshRingOrContainer = (MeshByLine) _meshLineGO.AddComponent<MeshByLine> ();
        _meshLineGO.name = "MeshByLine";
        //将它作为子对象加载到自己身上。
        DisplayUtils.addBToA (gameObject, _meshLineGO);
        //初始化
        reset (true);
    }

    public void reset (bool isInit_) {
        Vector3 _begin = beginGO.transform.position;
        Vector3 _end = endGO.transform.position;
        float _smallX = Math.Min (_begin.x, _end.x);
        float _smallY = Math.Min (_begin.y, _end.y);
        float _bigX = Math.Max (_begin.x, _end.x);
        float _bigY = Math.Max (_begin.y, _end.y);
        float _width = _bigX - _smallX;
        float _height = _bigY - _smallY;
        float _posx = _smallX + _width * 0.5f;
        float _posy = _smallY + _height * 0.5f;
        float _direction = CircleUtils.RadianstoDegrees ((float) Math.Atan2 ((_end.y - _begin.y), (_end.x - _begin.x)));
        float _length = (float) Math.Sqrt (_width * _width + _height * _height);

        if (isInit_) {
            //创建一个Shape
            _shape = new ShapeLine (
                new Vector2 (_posx, _posy),
                _direction,
                _length
            );
        } else {
            //重置Shape
            ((ShapeLine) _shape).reset (_posx, _posy, _direction, _length);
        }

        currentBeginX = ((ShapeLine) _shape)._currentBegin.x;
        currentBeginY = ((ShapeLine) _shape)._currentBegin.y;
        currentEndX = ((ShapeLine) _shape)._currentEnd.x;
        currentEndY = ((ShapeLine) _shape)._currentEnd.y;

        //通过Shape刷新自己
        ((MeshByLine) _meshRingOrContainer).resetByShape ((ShapeLine) _shape);
        //摆放自己的位置，在两点之间
        _meshRingOrContainer.transform.localPosition = new Vector3 (_posx, _posy, 0);

    }

    public void Update () { //子类根据自己的 Shape 和 Mesh 进行形状 -> 显示的同步。
        if (isNeedReset ()) {

            reset (false);

            float _angle = DisplayUtils.AFaceToBDegree (beginGO.transform, endGO.transform);
            beginGO.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, _angle + 90));
            endGO.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, _angle - 90));
        }
    }
    public override void LateUpdate () {
        if (isNeedReset ()) {
            bool _isHit = false;
            foreach (TwoPointBase _tp in twoPointDict.Values) {
                if (_tp != this) {
                    if (this._shape.hitOther (_tp._shape)) {
                        _isHit = true;
                        if(_tp._shape.type == Shape2DObj.ShapeType.Circle){
                            //直线和圆的焦点
                            List<Vector2> _foucs = HitUtils.getInsertPointBetweenCircleAndLine (
                                ((ShapeLine) _shape)._currentBegin.x, ((ShapeLine) _shape)._currentBegin.y,
                                ((ShapeLine) _shape)._currentEnd.x, ((ShapeLine) _shape)._currentEnd.y,
                                _tp._shape.pos.x, _tp._shape.pos.y,
                                ((ShapeCircle)_tp._shape).radius
                            );
                            if (currentBegin != null && _foucs.Count >= 1) {
                                currentBegin.transform.position = new Vector3 (
                                    _foucs[0].x,
                                    _foucs[0].y,
                                    currentBegin.transform.position.z
                                );
                            }
                            if (currentEnd != null && _foucs.Count == 2) {
                                currentEnd.transform.position = new Vector3 (
                                    _foucs[1].x,
                                    _foucs[1].y,
                                    currentEnd.transform.position.z
                                );
                            }
                        }
                    }
                }
            }
            isHitBoo (_isHit);
        }
        base.LateUpdate ();
    }
}