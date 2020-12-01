using System;
using System.Collections.Generic;
using LitJson;
using Objs;
using UnityEngine;
using Utils;
using Game;

// public delegate void CallBack ();
public delegate void CallBack<T> (T arg);
public delegate void CallBack<T, X> (T arg1, X arg2);
// public delegate void CallBack<T, X, Y> (T arg1, X arg2, Y arg3);
// public delegate void CallBack<T, X, Y, Z> (T arg1, X arg2, Y arg3, Z arg4);

//任何一个对象，都可以持有一个事件转发对象。
//ValueObj通过 动态的 数据路径，进行数据的监听
//StateObj通过 固定的 状态 变化中 + 变化后，来做事件监听。
public class EventDispatcherObj : BaseObj {
    private Dictionary<string, Delegate> _evtListenerDict = new Dictionary<string, Delegate> ();

    public EventDispatcherObj () : base () {

    }

    public override void Dispose () {
        _evtListenerDict.Clear ();
        base.Dispose ();
    }

    //注册监听事件
    public void AddListener<T> (string evtName_, CallBack<T> callBack_) {
        if (!_evtListenerDict.ContainsKey (evtName_)) {
            _evtListenerDict.Add (evtName_, null);
        }

        Delegate _delegate = _evtListenerDict[evtName_];
        if (_delegate != null && _delegate.GetType () != callBack_.GetType ()) {
            throw new Exception (string.Format ("添加监听错误：当前尝试为事件类型{0}添加不同的委托，原本的委托是{1}，现要添加的委托是{2}", evtName_,
                _delegate.GetType (),
                callBack_.GetType ()));
        }
        _evtListenerDict[evtName_] = (CallBack<T>) _evtListenerDict[evtName_] + callBack_;
    }

    //移除监听事件
    public void RemoveListener<T> (string evtName_, CallBack<T> callBack_) {
        if (_evtListenerDict.ContainsKey (evtName_)) {
            Delegate _delegate = _evtListenerDict[evtName_];
            if (_delegate == null) {
                throw new Exception (string.Format ("移除监听错误：事件{0}不存在委托", evtName_));
            } else if (_delegate.GetType () != callBack_.GetType ()) {
                throw new Exception (string.Format ("移除监听错误：尝试为事件{0}移除不同的委托，原先的委托为{1}，现在要移除的委托为{2}", evtName_, _delegate.GetType (), callBack_.GetType ()));
            }
        } else {
            throw new Exception (string.Format ("移除监听错误：不存在事件{0}", evtName_));
        }
        _evtListenerDict[evtName_] = (CallBack<T>) _evtListenerDict[evtName_] - callBack_;
        if (_evtListenerDict[evtName_] == null) {
            _evtListenerDict.Remove (evtName_);
        }
    }

    //广播事件
    public void DispatchEvent<T> (string evtName_, T arg_) {
        if (_evtListenerDict.ContainsKey (evtName_) == false) {
            return; //没有监听就部不用分发
        }
        Delegate _delegate;
        if (_evtListenerDict.TryGetValue (evtName_, out _delegate)) {
            CallBack<T> _callBack = _delegate as CallBack<T>;
            if (_callBack != null) {
                _callBack (arg_);
            } else {
                throw new Exception (string.Format ("事件广播错误：事件{0}存在不同的委托类型", evtName_));
            }
        }
    }
    //注册监听事件
    public void AddListener<T, X> (string evtName_, CallBack<T, X> callBack_) {
        if (!_evtListenerDict.ContainsKey (evtName_)) {
            _evtListenerDict.Add (evtName_, null);
        }

        Delegate _delegate = _evtListenerDict[evtName_];
        if (_delegate != null && _delegate.GetType () != callBack_.GetType ()) {
            throw new Exception (string.Format ("添加监听错误：当前尝试为事件类型{0}添加不同的委托，原本的委托是{1}，现要添加的委托是{2}", evtName_,
                _delegate.GetType (),
                callBack_.GetType ()));
        }
        _evtListenerDict[evtName_] = (CallBack<T, X>) _evtListenerDict[evtName_] + callBack_;
    }

    //移除监听事件
    public void RemoveListener<T, X> (string evtName_, CallBack<T, X> callBack_) {
        if (_evtListenerDict.ContainsKey (evtName_)) {
            Delegate _delegate = _evtListenerDict[evtName_];
            if (_delegate == null) {
                throw new Exception (string.Format ("移除监听错误：事件{0}不存在委托", evtName_));
            } else if (_delegate.GetType () != callBack_.GetType ()) {
                throw new Exception (string.Format ("移除监听错误：尝试为事件{0}移除不同的委托，原先的委托为{1}，现在要移除的委托为{2}", evtName_, _delegate.GetType (), callBack_.GetType ()));
            }
        } else {
            throw new Exception (string.Format ("移除监听错误：不存在事件{0}", evtName_));
        }
        _evtListenerDict[evtName_] = (CallBack<T, X>) _evtListenerDict[evtName_] - callBack_;
        if (_evtListenerDict[evtName_] == null) {
            _evtListenerDict.Remove (evtName_);
        }
    }

    //广播事件
    public void DispatchEvent<T, X> (string evtName_, T arg1_, X arg2_) {
        if (_evtListenerDict.ContainsKey (evtName_) == false) {
            return; //没有监听就部不用分发
        }
        Delegate _delegate;
        if (_evtListenerDict.TryGetValue (evtName_, out _delegate)) {
            CallBack<T, X> _callBack = _delegate as CallBack<T, X>;
            if (_callBack != null) {
                _callBack (arg1_, arg2_);
            } else {
                throw new Exception (string.Format ("事件广播错误：事件{0}存在不同的委托类型", evtName_));
            }
        }
    }
}