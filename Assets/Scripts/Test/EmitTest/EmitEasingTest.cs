using System;
using System.Collections.Generic;
using Dis;
using Game;
using Objs;
using UnityEngine;
using Utils;

class EmitRotateEasingTest : EmitRotateEasingObj {
    private GameObject _goContainer;
    private float emitR;
    private float speedPlus;
    public void init (GameObject containerGO_, float direction_, float emitR_, float speedPlus_) {
        emitR = emitR_;
        speedPlus = speedPlus_;
        _goContainer = containerGO_;
        isAutoEmit = true; //自动发射
        reset (5, direction_, 60, 1f, -1); //扫射设置
    }

    public override void emit () {
        EmitEasingTest _emitTest = _goContainer.GetComponent<EmitEasingTest> ();
        _emitTest.mtList.Add (MoveTrajectory.createTrajectory (_goContainer, Vector2.zero, direction, emitR, speedPlus)); //循环中带上，生成的对象
        base.emit (); //基础刷新逻辑
    }
}

class EmitMulEasingTest : EmitMulEasingCutterObj {
    private GameObject _goContainer;
    private float emitR;
    private float speedPlus;
    public void init (GameObject containerGO_, float direction_, float emitR_, float speedPlus_) {
        emitR = emitR_;
        speedPlus = speedPlus_;
        _goContainer = containerGO_;
        isAutoEmit = true; //自动发射
        reset (
            5, //间隔
            direction_, //中点值
            30, //范围
            0.2f, //缓动
            10f, //最小成立范围
            false
        );
    }

    public override void emit () {
        EmitEasingTest _emitTest = _goContainer.GetComponent<EmitEasingTest> ();
        for (int _idx = 0; _idx < _rangeEasingObj.positiveValuesInRange.Length; _idx++) {
            float _angle = _rangeEasingObj.positiveValuesInRange[_idx];
            _emitTest.mtList.Add (MoveTrajectory.createTrajectory (_goContainer, Vector2.zero, _angle, emitR, speedPlus, _idx.ToString ())); //循环中带上，生成的对象
        }
        base.emit (); //基础刷新逻辑
    }
}

public class EmitEasingTest : MonoBehaviour {

    public List<MoveTrajectory> mtList = new List<MoveTrajectory> ();
    private EmitRotateEasingTest[] _emitRotates = null;
    private EmitMulEasingTest[] _emitMuls = null;
    
    private ArrowMove  _am = null;

    private float _rotationPerFrame = 1f;
    private int _frameCount = 0;
    public void Start () {
        _am = new ArrowMove ();
        _am.init (gameObject, 0.0f, 0, 1f);
        roundEmitOneRotate (1);
        //roundEmitMultiple (5);
    }
    public void roundEmitMultiple (int num_) {
        _emitMuls = new EmitMulEasingTest[num_];
        int _count = 0;
        float _perAngle = 360f / num_;
        while (_count < num_) {
            _emitMuls[_count] = new EmitMulEasingTest ();
            _emitMuls[_count].init (gameObject, _count * _perAngle + 60, 5, 25);
            _count++;
        }
    }
    public void roundEmitOneRotate (int num_) {
        _emitRotates = new EmitRotateEasingTest[num_];
        int _count = 0;
        float _perAngle = 360f / num_;
        while (_count < num_) {
            _emitRotates[_count] = new EmitRotateEasingTest ();
            _emitRotates[_count].init (gameObject, _count * _perAngle, -10, 50);
            _count++;
        }
    }

    public void Update () {
        _frameCount++;
        // if (_frameCount % 5 != 0) {
        //     return;
        // }

        int _length;
        int _idx;
        
        _am.updateF();

        _length = mtList.Count;

        MoveTrajectory _mt;
        for (_idx = 0; _idx < _length; _idx++) {
            _mt = mtList[_idx];
            if(_mt._appAngleInterval != null){
                if(_mt._appAngleInterval.targetMC == null){
                    _mt._appAngleInterval.resetTargetMC(_am._fixedRouteLoopObj);
                }
            }
            if(_mt._appAngleEasing != null){
                if(_mt._appAngleEasing.targetMC == null){
                    _mt._appAngleEasing.resetTargetMC(_am._fixedRouteLoopObj);
                }
            }
            if(_mt._appInertance != null){
                if(_mt._appInertance.targetMC == null){
                    _mt._appInertance.resetTargetMC(_am._fixedRouteLoopObj);
                }
            }
            if(_mt._app != null){
                if(_mt._app.targetMC == null){
                    _mt._app.resetTargetMC(_am._fixedRouteLoopObj);
                }
            }
            _mt.updateF ();
            if (
                _mt._legObj != null && _mt._legObj.noMoveFrameCount > _mt.prefabTrajectory.maxPointNum //不动超过拖尾长，再消除
                ||
                _mt._fixedObj != null && _mt._fixedObj.moveFrameCount > 50 //移动超过帧数在消除
                ||
                _mt._appAngleInterval != null && _mt._appAngleInterval.moveFrameCount > 50 //移动超过帧数在消除
                ||
                _mt._appAngleEasing != null && _mt._appAngleEasing.moveFrameCount > 50 //移动超过帧数在消除
                ||
                _mt._appInertance != null && _mt._appInertance.moveFrameCount > 50 //移动超过帧数在消除
                ||
                _mt._app != null && _mt._app.moveFrameCount > 50 //移动超过帧数在消除
            ) {
                mtList.RemoveAt (_idx);
                _mt.Dispose ();
                _idx--;
                _length--;
            }
        }

        if (_emitRotates != null) {
            _length = _emitRotates.Length;
            EmitRotateEasingTest _emitRotate;
            for (_idx = 0; _idx < _length; _idx++) {
                _emitRotate = _emitRotates[_idx];
                _emitRotate.updateF ();
            }
        }
        if (_emitMuls != null) {
            _length = _emitMuls.Length;
            EmitMulEasingTest _emitMul;
            for (_idx = 0; _idx < _length; _idx++) {
                _emitMul = _emitMuls[_idx];
                _emitMul.resetDirection (_emitMul.direction + _rotationPerFrame);
                _emitMul.updateF ();
            }
        }
    }
}