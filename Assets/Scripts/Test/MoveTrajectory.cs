using System;
using System.Collections.Generic;
using Dis;
using Game;
using Objs;
using UnityEngine;
using Utils;
public class ArrowMove : IUpdateAbleObj {
    public FixedRouteLoopObj _fixedRouteLoopObj;
    ArrowTrajectory _arrowTrajectory = null;

    ColorfulTrajectory _colorfulTrajectory = null;

    public ArrowMove () {

    }
    //添加到什么上，位置预先变更百分比，旋转多少角度。
    public void init (
        GameObject containerGO_, //所在容器
        float bufferNextPect_ = 0.0f, //从轨迹数组的百分之多少开始
        float beginRotation_ = 0.0f, //开始的旋转角度
        float scale_ = 1.0f //轨迹放缩
    ) {
        _fixedRouteLoopObj = new FixedRouteLoopObj (MoveControlObj.PosType.GAME);
        Vector2[] _paths_1 = CircleUtils.getCirclePointsVec2 (
            new Vector2 (0, 0), //圆心
            (int) ((float) 100 * scale_), //半径
            (int) ((float) 30 * scale_), //用多少个点来切圆，如果0 标示，采用默认的算法运算出一个值。
            (int) ((float) 50 * scale_), //等距螺旋距离
            1 //圆周半分比
        );
        Vector2[] _paths_2 = CircleUtils.getCirclePointsVec2 (
            new Vector2 (0, 0), //圆心
            (int) ((float) 150 * scale_), //半径
            (int) ((float) 60 * scale_), //用多少个点来切圆，如果0 标示，采用默认的算法运算出一个值。
            (int) ((float) 50 * scale_), //等距螺旋距离
            1 //圆周半分比
        );

        //先翻转X，然后倒叙
        Vector2[] _paths_3 = V2Utils.reverse (V2Utils.flipX (_paths_2));
        Vector2[] _paths_4 = V2Utils.reverse (V2Utils.flipX (_paths_1));

        //等距螺旋出去，再回来
        Vector2[] _paths = V2Utils.merge (new Vector2[][] { _paths_1, _paths_2, _paths_3, _paths_4 });

        //旋转多少
        _paths = V2Utils.rotate (beginRotation_, _paths);

        //平移
        _paths = V2Utils.trans (new Vector2 (5.68f, 3.2f), _paths);

        _paths = V2Utils.flipX (_paths);

        //设置路径
        ((FixedRouteLoopObj) _fixedRouteLoopObj).resetPaths (_paths);

        //偏移多少
        int _nextCount = (int) (bufferNextPect_ * (float) _paths.Length);
        _fixedRouteLoopObj.bufferNext (_nextCount);

        //添加到场景上
        _arrowTrajectory = CreateUtils.getComponentAndAddTo (typeof (ArrowTrajectory), containerGO_) as ArrowTrajectory;
        _arrowTrajectory.initDefaultStyle ();
        //绑定关系
        _arrowTrajectory.resetMoveControl (_fixedRouteLoopObj);

        // //添加到场景上
        // _colorfulTrajectory = CreateUtils.getComponentAndAddTo (typeof (ColorfulTrajectory), containerGO_) as ColorfulTrajectory;
        // //绑定关系
        // _colorfulTrajectory.resetLeg (_fixedRouteLoopObj);
    }
    public void updateF () {
        _fixedRouteLoopObj.updateF (); //先更新控制器
        //再更新显示
        if (_arrowTrajectory != null) { _arrowTrajectory.updateF (); }
        if (_colorfulTrajectory != null) { _colorfulTrajectory.updateF (); }
    }
}
public class MoveTrajectory : IUpdateAbleObj, IDisposable {

    public static MoveTrajectory createTrajectory (
        GameObject goContainer_, //容器
        Vector2 pos_, //位置
        float direction_, //角度
        float emitR_, //距离中心点半径，
        float speedPlus_, //速度
        string name_ = ""
    ) {
        MoveTrajectory _moveTrajectory = new MoveTrajectory ();
        _moveTrajectory.init (goContainer_);

        float _radians = CircleUtils.DegreetoRadians (direction_); //方向角度
        float _disCenter = emitR_; //距离中心距离
        Vector2 _pos = new Vector2 (pos_.x + Mathf.Cos (_radians) * _disCenter, pos_.y + Mathf.Sin (_radians) * _disCenter); //朝那个方向偏移1距离。

        if (_moveTrajectory._legObj != null) { //1
            Vector3 _expSeed = SpeedUtils.explosionSpeedByPos (Vector2.zero, speedPlus_, _pos, 1f); //离的越近力越大，设置影响范围就可以了。
            // _creature._legObj.G = -1.5f;
            // _creature._legObj.bounce = 0.7f;
            _moveTrajectory._legObj.resetToPos (_pos.x, _pos.y); //放到沿着方向，距离中心点 _disCenter 远 的地方。
            _moveTrajectory._legObj.speed += _expSeed; //获取当前的速度叠加
        }

        if (_moveTrajectory._appAngleInterval != null) { //2
            _moveTrajectory._appAngleInterval.resetAsInterval (_pos, direction_, UnityEngine.Random.Range (5f, 10f), UnityEngine.Random.Range (5f, 10f), true);
        }
        if (_moveTrajectory._appAngleEasing != null) { //3
            _moveTrajectory._appAngleEasing.resetAsEasing (_pos, direction_, UnityEngine.Random.Range (5f, 15f), UnityEngine.Random.Range (0.01f, 0.3f), true);
        }

        if (_moveTrajectory._appInertance != null) { //4
            _moveTrajectory._appInertance.reset (_pos, direction_, 15f);
        }

        if (_moveTrajectory._fixedObj != null) { //5
            _moveTrajectory._fixedObj.reset (_pos, direction_, emitR_, 0.5f, speedPlus_);
        }
        if (_moveTrajectory._app != null) { //6
            _moveTrajectory._app.reset (_pos, 30f);
        }
        if (name_ != "") {
            _moveTrajectory.prefabTrajectory.gameObject.name = name_;
        }
        return _moveTrajectory;
    }
    public LegObj _legObj = null;
    public FixedAngleBaseObj _fixedObj = null;
    public ApproachingAngleObj _appAngleInterval = null;
    public ApproachingAngleObj _appAngleEasing = null;
    public ApproachingInertanceObj _appInertance = null;
    public ApproachingObj _app = null;


    //当前支持的 MoveControl 的种类数量
    public static int moveControlTypeMax = 6;
    //当前使用的 那个种类 创建轨迹
    public static int currentMoveControlType = 1;

    public static int specifiedMoveControlType = 0;

    public PrefabTrajectory prefabTrajectory = null;
    
    public ShadowTrajectory shadowTrajectory = null;

    public MoveTrajectory () {
        //想具体看那个，就设置那个的值
        specifiedMoveControlType = 0;
    }
    //添加到什么上，位置预先变更百分比，旋转多少角度。
    public void init (GameObject containerGO_) {
        MoveControlObj _mc = new MoveControlObj (MoveControlObj.PosType.GAME); //随便给个初始值，反正后面都得改

        if (specifiedMoveControlType == 0) { //不指定就循环使用。
            currentMoveControlType++;
            if (currentMoveControlType > moveControlTypeMax) {
                currentMoveControlType = 1;
            }
        } else { //指定哪一种
            currentMoveControlType = specifiedMoveControlType;
        }

        if (currentMoveControlType == 1) { //重力系，弹跳
            _mc = new LegObj ();
            _legObj = (_mc as LegObj);
        } else if (currentMoveControlType == 2) { //跟随，旋转角度固定
            _mc = new ApproachingAngleObj (MoveControlObj.PosType.GAME);
            _appAngleInterval = (_mc as ApproachingAngleObj);
        } else if (currentMoveControlType == 3) { //跟随，旋转角度缓动到目标
            _mc = new ApproachingAngleObj (MoveControlObj.PosType.GAME);
            _appAngleEasing = (_mc as ApproachingAngleObj);
        } else if (currentMoveControlType == 4) { //跟随，根据距离和当前速度，算朝向速度。
            _mc = new ApproachingInertanceObj (MoveControlObj.PosType.GAME);
            _appInertance = (_mc as ApproachingInertanceObj);
        } else if (currentMoveControlType == 5) { //沿着固定的方向进行移动。
            _mc = new FixedAngleBaseObj (MoveControlObj.PosType.GAME);
            _fixedObj = (_mc as FixedAngleBaseObj);
        } else if (currentMoveControlType == 6) { //沿着固定的方向进行移动。
            _mc = new ApproachingObj (MoveControlObj.PosType.GAME);
            _app = (_mc as ApproachingObj);
        }

        //添加到场景上//绑定关系
        prefabTrajectory = CreateUtils.getComponentAndAddTo (typeof (PrefabTrajectory), containerGO_) as PrefabTrajectory;
        prefabTrajectory.resetMoveControl (_mc);

        if (_legObj != null) {
            //添加到场景上//绑定关系
            shadowTrajectory = CreateUtils.getComponentAndAddTo (typeof (ShadowTrajectory), containerGO_) as ShadowTrajectory;
            shadowTrajectory.resetMoveControl (_mc);

        }
    }
    public void updateF () {
        if (_legObj != null) _legObj.updateF (); //先更新控制器
        if (_fixedObj != null) _fixedObj.updateF (); //先更新控制器
        if (_appAngleInterval != null) _appAngleInterval.updateF (); //先更新控制器
        if (_appAngleEasing != null) _appAngleEasing.updateF (); //先更新控制器
        if (_appInertance != null) _appInertance.updateF (); //先更新控制器
        if (_app != null) _app.updateF (); //先更新控制器
        //再更新显示
        if (shadowTrajectory != null) shadowTrajectory.updateF ();
        if (prefabTrajectory != null) prefabTrajectory.updateF ();
    }

    public void Dispose () {
        if (_legObj != null) _legObj.Dispose ();
        if (_fixedObj != null) _fixedObj.Dispose ();
        if (_appAngleInterval != null) _appAngleInterval.Dispose ();
        if (_appAngleEasing != null) _appAngleEasing.Dispose ();
        if (_appInertance != null) _appInertance.Dispose ();
        if (_app != null) _app.Dispose ();
        if (shadowTrajectory != null) GameObject.Destroy (shadowTrajectory.gameObject);
        if (prefabTrajectory != null) GameObject.Destroy (prefabTrajectory.gameObject);
    }
}