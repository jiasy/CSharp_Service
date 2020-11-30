using System;
using System.Collections.Generic;
using Dis;
using Game;
using Objs;
using UnityEngine;
using Utils;

class EmitBaseTest : EmitBaseObj
{
    private GameObject _goContainer;

    private float speedPlus;
    private float emitR;

    public void init(GameObject containerGO_, float direction_, float emitR_, float speedPlus_)
    {
        emitR = emitR_;
        speedPlus = speedPlus_;
        _goContainer = containerGO_;
        isAutoEmit = true; //自动发射
        resetInterval(5, true); //5帧一个
        resetDirection(direction_); //定方向
    }

    public override void emit()
    {
        EmitTest _emitTest = _goContainer.GetComponent<EmitTest>();
        _emitTest.mtList.Add(MoveTrajectory.createTrajectory(_goContainer, Vector2.zero, direction, emitR,
            speedPlus)); //循环中带上，生成的对象
        //基础刷新逻辑
        base.emit();
    }
}

class EmitRotateAverTest : EmitRotateAverObj
{
    private GameObject _goContainer;
    private float emitR;
    private float speedPlus;

    public void init(GameObject containerGO_, float direction_, float emitR_, float speedPlus_)
    {
        emitR = emitR_;
        speedPlus = speedPlus_;
        _goContainer = containerGO_;
        isAutoEmit = true; //自动发射
        reset(5, direction_, 60, 1f, -1); //扫射设置
    }

    public override void emit()
    {
        EmitTest _emitTest = _goContainer.GetComponent<EmitTest>();
        _emitTest.mtList.Add(MoveTrajectory.createTrajectory(
            _goContainer, //容器
            Vector2.zero, //起点
            direction, //方向
            emitR, //转角能力
            speedPlus //速度
        )); //循环中带上，生成的对象
        base.emit(); //基础刷新逻辑
    }
}

class EmitMulAverCutterTest : EmitMulAverCutterObj
{
    private GameObject _goContainer;
    private float emitR;
    private float speedPlus;

    public void init(GameObject containerGO_, float direction_, float emitR_, float speedPlus_)
    {
        emitR = emitR_;
        speedPlus = speedPlus_;
        _goContainer = containerGO_;
        isAutoEmit = true; //自动发射
        reset(
            5, //间隔
            direction_, //中点值
            30, //范围
            10 //个数
        );
    }

    public override void emit()
    {
        EmitTest _emitTest = _goContainer.GetComponent<EmitTest>();
        for (int _idx = 0; _idx < _rangeAverObj.valuesInRange.Length; _idx++)
        {
            float _angle = _rangeAverObj.valuesInRange[_idx];
            _emitTest.mtList.Add(MoveTrajectory.createTrajectory(_goContainer, Vector2.zero, _angle, emitR,
                speedPlus)); //循环中带上，生成的对象
        }

        base.emit(); //基础刷新逻辑
    }
}

public class EmitTest : MonoBehaviour
{
    public List<MoveTrajectory> mtList = new List<MoveTrajectory>();
    public List<MoveTrajectory> currentRemoveList = new List<MoveTrajectory>();
    private EmitBaseTest[] _emits = null;
    private EmitRotateAverTest[] _emitRotateAver = null;
    private EmitMulAverCutterTest[] _emitMulAverCutters = null;

    private ArrowMove _am = null;

    private float _rotationPerFrame = 0f;
    private int _frameCount = 0;

    public void Start()
    {
        //中间转圈
        _am = new ArrowMove();
        _am.init(gameObject, 0.0f, 0, 1f);

        // _rotationPerFrame = 1f;
        // roundEmit (1);

        _rotationPerFrame = 0f;
        roundEmitOneRotate(1);

        // _rotationPerFrame = 2f;
        // roundEmitMultiple (1);
    }

    public void roundEmitMultiple(int num_)
    {
        _emitMulAverCutters = new EmitMulAverCutterTest[num_];
        int _count = 0;
        float _perAngle = 360f / num_;
        while (_count < num_)
        {
            _emitMulAverCutters[_count] = new EmitMulAverCutterTest();
            _emitMulAverCutters[_count].init(gameObject, _count * _perAngle + 60, 5, 25);
            _count++;
        }
    }

    public void roundEmitOneRotate(int num_)
    {
        _emitRotateAver = new EmitRotateAverTest[num_];
        int _count = 0;
        float _perAngle = 360f / num_;
        while (_count < num_)
        {
            //创建发射装置
            _emitRotateAver[_count] = new EmitRotateAverTest();
            _emitRotateAver[_count].init(gameObject, _count * _perAngle, -10, 50);
            _count++;
        }
    }

    public void roundEmit(int num_)
    {
        _emits = new EmitBaseTest[num_];
        int _count = 0;
        float _perAngle = 360f / num_;
        while (_count < num_)
        {
            _emits[_count] = new EmitBaseTest();
            _emits[_count].init(gameObject, _count * _perAngle, 25, 50);
            _count++;
        }
    }

    public void Update()
    {
        _frameCount++;
        // if (_frameCount % 5 != 0) {
        //     return;
        // }

        int _length;
        int _idx;

        _am.updateF();

        MoveTrajectory _mt;
        _length = mtList.Count;
        for (_idx = 0; _idx < _length; _idx++)
        {
            _mt = mtList[_idx];

            if (_mt._appAngleInterval != null)
            {
                if (_mt._appAngleInterval.targetMC == null)
                {
                    //设置跟踪目标
                    _mt._appAngleInterval.resetTargetMC(_am._fixedRouteLoopObj);
                }
            }

            if (_mt._appAngleEasing != null)
            {
                if (_mt._appAngleEasing.targetMC == null)
                {
                    //设置跟踪目标
                    _mt._appAngleEasing.resetTargetMC(_am._fixedRouteLoopObj);
                }
            }

            if (_mt._appInertance != null)
            {
                if (_mt._appInertance.targetMC == null)
                {
                    //设置跟踪目标
                    _mt._appInertance.resetTargetMC(_am._fixedRouteLoopObj);
                }
            }

            if (_mt._app != null)
            {
                if (_mt._app.targetMC == null)
                {
                    //设置跟踪目标
                    _mt._app.resetTargetMC(_am._fixedRouteLoopObj);
                }
            }

            _mt.updateF();
            if (
                _mt._legObj != null && _mt._legObj.noMoveFrameCount > _mt.prefabTrajectory.maxPointNum //不动超过拖尾长，再消除
                ||
                _mt._fixedObj != null && _mt._fixedObj.moveFrameCount > 300 //移动超过帧数在消除
                ||
                _mt._appAngleInterval != null && _mt._appAngleInterval.moveFrameCount > 300 //移动超过帧数在消除
                ||
                _mt._appAngleEasing != null && _mt._appAngleEasing.moveFrameCount > 300 //移动超过帧数在消除
                ||
                _mt._appInertance != null && _mt._appInertance.moveFrameCount > 300 //移动超过帧数在消除
                ||
                _mt._app != null && _mt._app.moveFrameCount > 300 //移动超过帧数在消除
            )
            {
                currentRemoveList.Add(_mt);
            }
        }

        _length = currentRemoveList.Count;
        for (_idx = 0; _idx < _length; _idx++)
        {
            _mt = currentRemoveList[_idx];
            mtList.Remove(_mt);
            _mt.Dispose();
        }

        currentRemoveList.Clear();

        if (_emits != null)
        {
            _length = _emits.Length;
            EmitBaseTest _emit;
            for (_idx = 0; _idx < _length; _idx++)
            {
                _emit = _emits[_idx];
                _emit.resetDirection(_emit.direction + _rotationPerFrame);
                _emit.updateF();
            }
        }

        if (_emitRotateAver != null)
        {
            _length = _emitRotateAver.Length;
            EmitRotateAverTest emitRotateAver;
            for (_idx = 0; _idx < _length; _idx++)
            {
                emitRotateAver = _emitRotateAver[_idx];
                //自带旋转的发射器，他的朝向是faceDirection，具体发射的那个方向才是 direction。
                emitRotateAver.resetFaceDirection(emitRotateAver.faceDirection + _rotationPerFrame); //旋转
                emitRotateAver.updateF();
            }
        }

        if (_emitMulAverCutters != null)
        {
            _length = _emitMulAverCutters.Length;
            EmitMulAverCutterTest emitMulAverCutter;
            for (_idx = 0; _idx < _length; _idx++)
            {
                emitMulAverCutter = _emitMulAverCutters[_idx];
                emitMulAverCutter.resetDirection(emitMulAverCutter.direction + _rotationPerFrame);
                emitMulAverCutter.updateF();
            }
        }
    }
}