using System;
using System.Collections.Generic;
using Objs;

namespace Utils {
    public class PoolUtils {
        public static Dictionary<string, PoolContainerObj> _objDict = new Dictionary<string, PoolContainerObj> ();

        //从池中拿出一个
        public static IPoolObj pullOut (string fullClassName_, object[] parameters_ = null) {
            PoolContainerObj _poolObj = getPool (fullClassName_);
            IPoolObj _obj = _poolObj.pullOut (fullClassName_, parameters_); //对象池创建出来的对象，都有所属池<belongToPool>。
            return _obj;
        }

        //放回池中一个
        public static void pushBack (IPoolObj _obj) {
            _obj.belongToPool.pushBack (_obj);
        }

        //根据类名获取池
        public static PoolContainerObj getPool (string fullClassName_) {
            PoolContainerObj _poolObj = null;
            if (!_objDict.ContainsKey (fullClassName_)) {
                _poolObj = new PoolContainerObj (); //池对象，不在池中。只是通过普通的new创建的
                _objDict[fullClassName_] = _poolObj;
            } else {
                _poolObj = _objDict[fullClassName_];
            }
            return _poolObj;
        }

        public static void clearPoolByName (string fullClassName_) {
            PoolContainerObj _poolObj = getPool (fullClassName_);
            _poolObj.clearAll ();
        }
    }
}