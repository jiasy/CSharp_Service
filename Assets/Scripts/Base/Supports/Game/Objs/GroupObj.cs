using System;
using System.Collections;
using System.Collections.Generic;
using Dis;
using LitJson;
using UnityEngine;
using Utils;
using Objs;

namespace Game {
    //阵营，分组
    public class GroupObj : BaseObj {
        public int id;
        //阵营队员
        public List<CreatureObj> creatures = new List<CreatureObj> ();
        //阵营附加队员,不计入阵营胜负判断的对象，但是参与阵营械斗。
        public List<CreatureObj> additionalCreatures = new List<CreatureObj> ();
        public GroupObj (int id_) : base () {
            id = id_; //阵营ID
        }
        public override void Dispose () {

            base.Dispose ();
        }

    }
}