using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyDropPrize))]
class EnemyDropPrize_Editor : Editor
{
    SerializedProperty items_Prop;
    SerializedProperty expPoint_Prop;
    SerializedProperty dropGold_Prop;
    SerializedProperty minGold_Prop;
    SerializedProperty maxGold_Prop;

    private void OnEnable()
    {
        items_Prop = serializedObject.FindProperty("items");
        expPoint_Prop = serializedObject.FindProperty("expPoint");
        dropGold_Prop = serializedObject.FindProperty("dropGold");
        minGold_Prop = serializedObject.FindProperty("minGold");
        maxGold_Prop = serializedObject.FindProperty("maxGold");
        //Debug.Log(items_Prop.serializedObject.ToString());
    }

    public override void OnInspectorGUI()
    {
        //PropertyField(property, includeChildren)
        //-> includeChildren : 참이면 하위 항목을 포함한 속성이 그려지고, 그렇지 않으면 컨트롤 자체(예: 접힌 부분만 그 아래에 없음)만 그려집니다.
        EditorGUILayout.PropertyField(items_Prop, true);
        EditorGUILayout.PropertyField(dropGold_Prop);
        EditorGUILayout.PropertyField(expPoint_Prop);

        if (dropGold_Prop.boolValue == true)
        {
            EditorGUILayout.PropertyField(minGold_Prop);
            EditorGUILayout.PropertyField(maxGold_Prop);
        }

        serializedObject.ApplyModifiedProperties();
    }
}