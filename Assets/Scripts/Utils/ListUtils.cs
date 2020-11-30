using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dis;
using Objs;
using UnityEngine;

//https://docs.microsoft.com/zh-cn/dotnet/api/system.linq.enumerable.where?view=netframework-4.8

namespace Utils {
    public class ListUtils {
        // int _randomInt = NumUtils.randomInt(1,100);
        public static void doSample () {
            List<string> _aList = new List<string> () { "_a", "_b", "_c" };
            List<string> _bList = new List<string> () { "_c", "_d", "_e" };
            List<string> _cList = union (_aList, _bList);
            printList (_cList);
            _cList = intersect (_aList, _bList);
            printList (_cList);
            _cList = except (_aList, _bList);
            printList (_cList);
            _cList = except (_bList, _aList);
            printList (_cList);
            //通过拉姆达表达式排序
            _cList = _cList.OrderBy (item => item.ToString ()).ToList ();
            //查找第一个
            _cList.First (item => string.Equals (item, "_c"));
            //任意一个满足条件
            bool _haveAnyBool = _cList.Any (item => item.ToString ().IndexOf ("_") == 0 && item.ToString ().IndexOf ("a") > 0);

        }

        //去重
        public static List<int> distinct (List<int> aList_) {
            IEnumerable<int> _listEn = aList_.Distinct ();
            return _listEn.ToList ();
        }

        //并集
        public static List<int> union (List<int> aList_, List<int> bList_) {
            IEnumerable<int> _listEn = aList_.Union (bList_);
            return _listEn.ToList ();
        }

        //交集
        public static List<int> intersect (List<int> aList_, List<int> bList_) {
            IEnumerable<int> _listEn = aList_.Intersect (bList_);
            return _listEn.ToList ();
        }

        //差集 在 a 不在 b
        public static List<int> except (List<int> aList_, List<int> bList_) {
            IEnumerable<int> _listEn = aList_.Except (bList_);
            return _listEn.ToList ();
        }

        //输出数组
        public static void printList (List<int> _list) {
            string _logStr = "List : ";
            for (int _idx = 0; _idx < _list.Count; _idx++) {
                int _obj = _list[_idx];
                _logStr += "\n" + "    " + _obj.ToString ();
            }
            Debug.Log (_logStr);
        }
        //去重
        public static List<string> distinct (List<string> aList_) {
            IEnumerable<string> _listEn = aList_.Distinct ();
            return _listEn.ToList ();
        }

        //并集
        public static List<string> union (List<string> aList_, List<string> bList_) {
            IEnumerable<string> _listEn = aList_.Union (bList_);
            return _listEn.ToList ();
        }

        //交集
        public static List<string> intersect (List<string> aList_, List<string> bList_) {
            IEnumerable<string> _listEn = aList_.Intersect (bList_);
            return _listEn.ToList ();
        }

        //差集 在 a 不在 b
        public static List<string> except (List<string> aList_, List<string> bList_) {
            IEnumerable<string> _listEn = aList_.Except (bList_);
            return _listEn.ToList ();
        }

        //输出数组
        public static void printList (List<string> _list) {
            string _logStr = "List : ";
            for (int _idx = 0; _idx < _list.Count; _idx++) {
                string _obj = _list[_idx];
                _logStr += "\n" + "    " + _obj.ToString ();
            }
            Debug.Log (_logStr);
        }
    }
}