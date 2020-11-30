using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using Objs;
using UnityEngine;
using Utils;
using Game;

public class TwoPointBase : MonoBehaviour {

    public float lastBeginGoX;
    public float lastEndGoX;

    public float lastBeginGoY;
    public float lastEndGoY;
    public GameObject beginGO = null;
    public GameObject endGO = null;

    protected MonoBehaviour _meshRingOrContainer = null; //显示碰撞区域

    public Shape2DObj _shape = null; //定义图形样式 + 碰撞检测

    public static Dictionary<string, TwoPointBase> twoPointDict = new Dictionary<string, TwoPointBase> ();

    public virtual void Awake () {
        if (beginGO == null || endGO == null) {
            Debug.LogError ("ERROR TwoPointBase 必须指定 begin 和 end 两点");
        }
        if (
            beginGO.transform.parent != transform ||
            endGO.transform.parent != transform
        ) {
            Debug.LogError ("ERROR TwoPointBase begin 和 end 两点，必须在当前 transform 下。");
        }
        if (twoPointDict.ContainsKey (name)) {
            Debug.LogError ("ERROR TwoPointBase 创建时，注意，每一个名称都不能想同，用来区别碰撞 : " + name);
        }
        twoPointDict[name] = this;

        if (
            transform.position.x != 0.0f ||
            transform.position.y != 0.0f ||
            transform.position.z != 0.0f
        ) {
            Debug.LogError ("ERROR TwoPointBase 创建时，x,y,z 值不为零 : " + transform.position.x.ToString () + "," + transform.position.y.ToString ());
        }

        //用当前的 begin 和 end 创建形状和
        CreateMeshAndShape ();
    }

    public virtual void CreateMeshAndShape () { //根据子类所表示的类型创建 Shape 和 Mesh
        throw new NotImplementedException ();
    }

    public virtual void LateUpdate () { //子类根据自己的 Shape 和 Mesh 进行形状 -> 显示的同步。
        lastBeginGoX = beginGO.transform.position.x;
        lastBeginGoY = beginGO.transform.position.y;
        lastEndGoX = endGO.transform.position.x;
        lastEndGoY = endGO.transform.position.y;
    }

    public void isHitBoo (bool isHit_) {
        if (isHit_) {
            beginGO.GetComponent<SpriteRenderer> ().material.color = Color.red;
            endGO.GetComponent<SpriteRenderer> ().material.color = Color.red;
        } else {
            beginGO.GetComponent<SpriteRenderer> ().material.color = Color.green;
            endGO.GetComponent<SpriteRenderer> ().material.color = Color.green;
        }
    }

    public virtual bool isNeedReset () {
        if (
            lastBeginGoX != beginGO.transform.position.x ||
            lastBeginGoY != beginGO.transform.position.y ||
            lastEndGoX != endGO.transform.position.x ||
            lastEndGoY != endGO.transform.position.y
        ) {
            return true;
        } else {
            return false;
        }
    }
}