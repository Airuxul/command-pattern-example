using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CellMaker))]
public class CellMakerEditor : Editor
{
    private CellMaker cellMaker;

    public override void OnInspectorGUI()
    {
        cellMaker = (CellMaker)this.target;
        base.OnInspectorGUI();
        if (GUILayout.Button("CreatCells"))
        {
            cellMaker.CreatCells();
        }
    }
}
