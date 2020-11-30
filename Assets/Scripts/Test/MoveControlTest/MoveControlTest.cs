using System;
using System.Collections.Generic;
using Dis;
using Game;
using Objs;
using UnityEngine;
using Utils;
class CircleMove : IUpdateAbleObj {
    public FixedRouteLoopObj _fixedRouteLoopObj;
    public ArrowTrajectory _arrowTrajectory = null;
    public int radians;

    public CircleMove (int radians_) {
        radians = radians_;
    }

    public void init (GameObject containerGO_) {
        //移动跟随者
        _fixedRouteLoopObj = new FixedRouteLoopObj (MoveControlObj.PosType.GAME);
        //路径序列
        Vector2[] _paths = CircleUtils.getCirclePointsVec2 (
            new Vector2 (0, 0), //圆心
            radians, //半径
            20, //用多少个点来切圆，如果0 标示，采用默认的算法运算出一个值。
            0, //等距螺旋距离
            1 //圆周半分比
        );
        _paths = V2Utils.scaleY (MoveControlObj.sightY, _paths);
        ((FixedRouteLoopObj) _fixedRouteLoopObj).resetPaths (_paths);
        //添加到场景上
        _arrowTrajectory = CreateUtils.getComponentAndAddTo (typeof (ArrowTrajectory), containerGO_) as ArrowTrajectory;
        _arrowTrajectory.initStyle ("Sprites/Default", Color.yellow, Color.red, 0.01f, 0.01f);
        _arrowTrajectory.maxPointNum = 10;
        //绑定关系
        _arrowTrajectory.resetMoveControl (_fixedRouteLoopObj);
    }
    public void updateF () {
        _fixedRouteLoopObj.updateF (); //先更新控制器
        //再更新显示
        if (_arrowTrajectory != null) {
            _arrowTrajectory.updateF ();
        }
    }
}
class CreatureMove : IUpdateAbleObj {
    public LegObj _legObj;
    public ArrowTrajectory arrowTrajectory = null;

    public PrefabTrajectory prefabTrajectory = null;

    public ShadowTrajectory shadowTrajectory = null;

    public CreatureMove () {

    }
    //添加到什么上，位置预先变更百分比，旋转多少角度。
    public void init (GameObject containerGO_) {
        _legObj = new LegObj ();
        // //添加到场景上//绑定关系
        // arrowTrajectory = CreateUtils.getComponentAndAddTo (typeof (ArrowTrajectory), containerGO_) as ArrowTrajectory;
        // arrowTrajectory.initDefaultStyle ();
        // arrowTrajectory.resetMoveControl (_legObj);

        prefabTrajectory = CreateUtils.getComponentAndAddTo (typeof (PrefabTrajectory), containerGO_) as PrefabTrajectory;
        prefabTrajectory.resetMoveControl (_legObj);

        //添加到场景上//绑定关系
        shadowTrajectory = CreateUtils.getComponentAndAddTo (typeof (ShadowTrajectory), containerGO_) as ShadowTrajectory;
        shadowTrajectory.resetMoveControl (_legObj);
    }
    public void updateF () {
        _legObj.updateF (); //先更新控制器
        //再更新显示
        if (shadowTrajectory != null) shadowTrajectory.updateF ();
        if (arrowTrajectory != null) arrowTrajectory.updateF ();
        if (prefabTrajectory != null) prefabTrajectory.updateF ();
    }
}

class LineMove : IUpdateAbleObj {
    public FixedAngleBaseObj _moveObj;
    public ArrowTrajectory arrowTrajectory = null;

    public LineMove () {

    }
    //添加到什么上，位置预先变更百分比，旋转多少角度。
    public void init (GameObject containerGO_, float angle_) {
        _moveObj = new FixedAngleBaseObj (MoveControlObj.PosType.GAME);
        resetAcc ();
        //添加到场景上//绑定关系
        arrowTrajectory = CreateUtils.getComponentAndAddTo (typeof (ArrowTrajectory), containerGO_) as ArrowTrajectory;
        arrowTrajectory.initDefaultStyle ();
        arrowTrajectory.resetMoveControl (_moveObj);
    }
    public void resetAcc () {
        _moveObj.reset (Vector2.zero, UnityEngine.Random.Range (0f, 360f), 0f, 1f, 20);
    }
    public void resetNoAcc () {
        _moveObj.reset (Vector2.zero, UnityEngine.Random.Range (0f, 360f), 10f);
    }
    public void updateF () {
        _moveObj.updateF (); //先更新控制器
        if (arrowTrajectory != null) arrowTrajectory.updateF ();
    }
}

//坐标轴
public class MoveControlTest : MonoBehaviour {
    private ArrowMove[] arrowMoves = null;
    private CreatureMove[] _creatures = null;
    private LineMove[] _lines = null;
    private CircleMove _centerCircle = null;

    private PrefabTrajectory _poShow = null;

    public void Start () {
        LineMove (100);
        creatureMove (100);
    }
    public void LineMove (int num_) {
        _lines = new LineMove[num_];
        int _count = 0;
        float _perAngle = 360f / num_;
        while (_count < num_) {
            _lines[_count] = new LineMove ();
            _lines[_count].init (gameObject, _count * _perAngle);
            _count++;
        }
    }
    public void creatureMove (int num_) {

        _centerCircle = new CircleMove (50);
        _centerCircle.init (gameObject);

        _creatures = new CreatureMove[num_];
        int _count = 0;
        while (_count < num_) {
            _creatures[_count] = new CreatureMove ();
            _creatures[_count].init (gameObject);
            _count++;
        }

        //随机点标示
        _poShow = CreateUtils.getComponentAndAddTo (typeof (PrefabTrajectory), gameObject) as PrefabTrajectory;
    }
    public void arrowTestAngle (int arrowCount_) {
        arrowMoves = new ArrowMove[arrowCount_];
        int _count = 0;
        //创建
        while (_count < arrowCount_) {
            arrowMoves[_count] = new ArrowMove ();
            _count++;
        }
        //重置一下角度
        float _perAngle = 360.0f / (float) _count;
        float _scaleBuffer = 3.0f / (float) arrowCount_;
        //重置一下
        _count = 0;
        while (_count < arrowCount_) {
            arrowMoves[_count].init (gameObject, 0.0f, _perAngle * _count, 0.3f + _scaleBuffer * _count);
            _count++;
        }
    }
    public void arrowTestBuffer (int arrowCount_) {
        arrowMoves = new ArrowMove[arrowCount_];
        int _count = 0;
        //创建
        while (_count < arrowCount_) {
            arrowMoves[_count] = new ArrowMove ();
            _count++;
        }

        //路径偏移百分比
        float _perBuffer = 1.0f / (float) _count;
        float _scaleBuffer = 2.0f / (float) arrowCount_;
        float _perAngle = 360.0f / (float) _count;
        //重置一下
        _count = 0;
        while (_count < arrowCount_) {
            arrowMoves[_count].init (gameObject, _perBuffer * _count, _perAngle * _count, 0.35f + _scaleBuffer * _count);
            _count++;
        }
    }
    void Update () {
        if (arrowMoves != null) {
            int _length = arrowMoves.Length;
            for (int _idx = 0; _idx < _length; _idx++) {
                arrowMoves[_idx].updateF ();
            }
        }
        if (_lines != null) {
            int _length = _lines.Length;
            LineMove _line;
            for (int _idx = 0; _idx < _length; _idx++) {
                _line = _lines[_idx];
                if (
                    _line._moveObj.accType == FixedAngleBaseObj.SpeedChangeType.None_ACC && _line._moveObj.moveFrameCount > 50
                ) { //匀速运动超过时间，变成加速运动
                    _line._moveObj.moveFrameCount = 0;
                    _line.resetAcc ();
                    _line.arrowTrajectory.resetAllV2ToMoveControl ();
                }
                if (
                    _line._moveObj.accType != FixedAngleBaseObj.SpeedChangeType.None_ACC && _line._moveObj.isAccEnd
                ) { //加速运动结束，变成匀速运动

                    _line.resetNoAcc ();
                }
                _line.updateF ();
            }
        }
        if (_creatures != null) {
            int _length = _creatures.Length;
            CreatureMove _creature;
            Vector2 _roundCenterPos;
            Vector3 _expSeed;
            for (int _idx = 0; _idx < _length; _idx++) {
                _creature = _creatures[_idx];
                if (
                    _creature.arrowTrajectory != null &&
                    _creature._legObj.noMoveFrameCount > _creature.arrowTrajectory.maxPointNum //不动的帧数大于线段节点总数，也就是轨迹全部掉落在一点。
                    ||
                    _creature.prefabTrajectory != null &&
                    _creature._legObj.noMoveFrameCount > _creature.prefabTrajectory.maxPointNum ||
                    (
                        _creature.arrowTrajectory == null &&
                        _creature.prefabTrajectory == null &&
                        _creature.shadowTrajectory != null &&
                        _creature._legObj.noMoveFrameCount > _creature.shadowTrajectory.maxPointNum //只有影子的时候，判断影子
                    )

                ) { //轨迹长，也就是轨迹完全行进结束时
                    float _range = _centerCircle.radians; //指定爆炸范围
                    _roundCenterPos = RandomUtils.randomPosInRange (Vector2.zero, _range * 0.5f); //范围内随机一个爆炸点,向内侧聚集一下，这样随机出来的点离中点近一些，受力大一些
                    _expSeed = SpeedUtils.explosionSpeedByPos (Vector2.zero, _range, _roundCenterPos, 1f); //获取这个爆炸点，按照这个范围爆炸，对当前点的影响速度

                    //确定当前的位置，附加当前的速度初始值。
                    _creature._legObj.resetToPos (_roundCenterPos.x, _roundCenterPos.y); //更新下它的坐标。放置到这个随机点上,重置显示位置
                    _creature._legObj.speed += _expSeed; //获取当前的速度叠加

                    //将整个轨迹都移动到当前leg所在位置。
                    int _count = 0;

                    if (_creature.arrowTrajectory != null) {
                        _creature.arrowTrajectory.resetAllV2ToMoveControl ();
                    }

                    _count = 0;
                    if (_creature.prefabTrajectory != null) {
                        _creature.prefabTrajectory.resetAllV2ToMoveControl ();
                    }

                    _count = 0;
                    if (_creature.shadowTrajectory != null) {
                        _creature.shadowTrajectory.resetAllV2ToMoveControl ();
                    }
                    if (_poShow != null) {
                        //绘画随机点
                        _poShow.addV2 (new Vector2 (_roundCenterPos.x * 0.01f, _roundCenterPos.y * 0.01f), false);
                    }
                }
                _creature.updateF ();
            }
        }
        if (_centerCircle != null) {
            _centerCircle.updateF ();
        }
    }
    private void OnGUI () {

    }
}