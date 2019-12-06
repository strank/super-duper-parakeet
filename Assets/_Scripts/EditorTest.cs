using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// Help from:
// https://answers.unity.com/questions/192895/hideshow-properties-dynamically-in-inspector.html
// 2019 December 5th
public class EditorTest : MonoBehaviour {

    public bool flag = false;
    public int intNumber = 0;
    public float floatNumber = 1.0f;
}

[CustomEditor(typeof(EditorTest))]
public class MyEditorCustomizer : Editor
{
    public override void OnInspectorGUI()
    {
        var myScript = target as EditorTest;

        myScript.flag = EditorGUILayout.Toggle("Flag", myScript.flag);

        if(myScript.flag == true)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PrefixLabel("First Number");
            myScript.intNumber = EditorGUILayout.IntSlider(myScript.intNumber, 0, 5);
            EditorGUILayout.PrefixLabel("Second Number");
            myScript.floatNumber = EditorGUILayout.FloatField(myScript.floatNumber);
        }
    }
}
