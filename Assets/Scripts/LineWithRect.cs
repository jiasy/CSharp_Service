using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

/// 
/// 只要线与矩形有一条线有相交，则线与矩形相交
/// 
public class LineWithRect : MonoBehaviour {
    public Rect rect = new Rect (263 , -31, 169, 169);
    public Transform LineStart;
    public Transform lineEnd;
    Vector3 VecLineStart;
    Vector3 vecLineEnd;

    public Transform PointTrans;

    Vector3 leftDown {
        get {
            return new Vector2 (rect.xMin, rect.yMin);
        }
    }
    Vector3 leftUp {
        get {
            return new Vector2 (rect.xMin, rect.yMax);
        }
    }
    Vector3 RigtDown {
        get {
            return new Vector2 (rect.xMax, rect.yMin);
        }
    }
    Vector3 RightUp {
        get {
            return new Vector2 (rect.xMax, rect.yMax);
        }
    }

    private void OnDrawGizmos () {
        if (LineStart == null || lineEnd == null)
            return;

        VecLineStart = new Vector2 (LineStart.position.x, LineStart.position.y);
        vecLineEnd = new Vector2 (lineEnd.position.x, lineEnd.position.y);

        Gizmos.DrawLine (VecLineStart, vecLineEnd);

        Gizmos.DrawLine (leftDown, leftUp);
        Gizmos.DrawLine (leftUp, RightUp);
        Gizmos.DrawLine (RightUp, RigtDown);
        Gizmos.DrawLine (RigtDown, leftDown);
    }

    private void OnGUI () {
        string content = "不在矩形内";
        
        if (rect.Contains(Input.mousePosition))
            content = "在矩形内";

        GUILayout.Label (" - - - - - - - - - - - - - - - - - - - - - - ");
        GUILayout.Label (content);
        GUILayout.Label (" - - - - - - - - - - - - - - - - - - - - - - ");
        GUILayout.Label ("mouseX: "+Input.mousePosition.x.ToString());
        GUILayout.Label ("mouseY : "+Input.mousePosition.y.ToString());
        GUILayout.Label (" - - - - - - - - - - - - - - - - - - - - - - ");
        GUILayout.Label ("rect.x : "+rect.x.ToString());
        GUILayout.Label ("rect.y : "+rect.y.ToString());
        GUILayout.Label ("rect.xMin : "+rect.xMin.ToString());
        GUILayout.Label ("rect.xMax : "+rect.xMax.ToString());
        GUILayout.Label ("rect.yMin : "+rect.yMin.ToString());
        GUILayout.Label ("rect.yMax : "+rect.yMax.ToString());
        GUILayout.Label ("rect.width : "+rect.width.ToString());
        GUILayout.Label ("rect.height : "+rect.height.ToString());
        GUILayout.Label ("rect.center.x : "+rect.center.x.ToString());
        GUILayout.Label ("rect.center.y : "+rect.center.y.ToString());
        GUILayout.Label (" - - - - - - - - - - - - - - - - - - - - - - ");

        PointTrans.position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,0);
    }
}