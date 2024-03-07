using UnityEditor;

[CustomEditor(typeof(Item_SO))]
class Item_SO_Editor : Editor
{
    //직렬화된 속성형식의 필드 선언
    SerializedProperty type_Prop;
    SerializedProperty equipType_Prop;
    SerializedProperty effectType_Prop;
    SerializedProperty grade_Prop;
    SerializedProperty itemPrefab_Prop;
    SerializedProperty itemImage_Prop;
    SerializedProperty itemName_Prop;
    SerializedProperty itemClass_Prop;
    SerializedProperty price_Prop;
    SerializedProperty canUseLevel_Prop;
    SerializedProperty itemAbillity_Prop;
    SerializedProperty itemDescription_Prop;
    SerializedProperty goldAmount_Prop;

    private void Awake()
    {
        type_Prop = serializedObject.FindProperty("type");
        equipType_Prop = serializedObject.FindProperty("equipType");
        effectType_Prop = serializedObject.FindProperty("effectType");
        grade_Prop = serializedObject.FindProperty("grade");
        itemPrefab_Prop = serializedObject.FindProperty("itemPrefab");
        itemImage_Prop = serializedObject.FindProperty("itemImage");
        itemName_Prop = serializedObject.FindProperty("itemName");
        itemClass_Prop = serializedObject.FindProperty("itemClass");
        price_Prop = serializedObject.FindProperty("price");
        canUseLevel_Prop = serializedObject.FindProperty("canUseLevel");
        itemAbillity_Prop = serializedObject.FindProperty("itemAbillity");
        itemDescription_Prop = serializedObject.FindProperty("itemDescription");
        goldAmount_Prop = serializedObject.FindProperty("goldAmount");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(type_Prop);

        switch (type_Prop.enumValueIndex)
        {
            case (int)ItemType.Equipment:
                EditorGUILayout.PropertyField(equipType_Prop);
                EditorGUILayout.PropertyField(effectType_Prop);
                EditorGUILayout.PropertyField(grade_Prop);
                EditorGUILayout.PropertyField(itemPrefab_Prop);
                EditorGUILayout.PropertyField(itemImage_Prop);
                EditorGUILayout.PropertyField(itemName_Prop);
                EditorGUILayout.PropertyField(itemClass_Prop);
                EditorGUILayout.PropertyField(price_Prop);
                EditorGUILayout.PropertyField(canUseLevel_Prop);
                EditorGUILayout.PropertyField(itemAbillity_Prop);
                EditorGUILayout.PropertyField(itemDescription_Prop);
                break;
            case (int)ItemType.Used:
                EditorGUILayout.PropertyField(effectType_Prop);
                EditorGUILayout.PropertyField(grade_Prop);
                EditorGUILayout.PropertyField(itemPrefab_Prop);
                EditorGUILayout.PropertyField(itemImage_Prop);
                EditorGUILayout.PropertyField(itemName_Prop);
                EditorGUILayout.PropertyField(price_Prop);
                EditorGUILayout.PropertyField(canUseLevel_Prop);
                EditorGUILayout.PropertyField(itemAbillity_Prop);
                EditorGUILayout.PropertyField(itemDescription_Prop);
                break;
            case (int)ItemType.Gold:
                EditorGUILayout.PropertyField(itemName_Prop);
                EditorGUILayout.PropertyField(itemPrefab_Prop);
                EditorGUILayout.PropertyField(goldAmount_Prop);
                break;
        }

        serializedObject.ApplyModifiedProperties(); //변경사항 프로퍼티를 적용하는 메서드
    }
}