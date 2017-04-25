#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(StackableItem)), CanEditMultipleObjects]
public class StackableItemEditor : Editor {

    public override void OnInspectorGUI()
    {
        StackableItem stackableItem = (StackableItem)target;

        stackableItem.itemName = EditorGUILayout.TextField("Item Name", stackableItem.itemName);
        stackableItem.hasLimit = EditorGUILayout.Toggle("Has Limit", stackableItem.hasLimit);

        if (stackableItem.hasLimit)
        {
            stackableItem.stackLimit = EditorGUILayout.IntField("Stack Limit", stackableItem.stackLimit);
        }

    }
}
#endif

