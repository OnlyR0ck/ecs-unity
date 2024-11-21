using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace VS.Editor
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class ExtendedScriptableObjectDrawer : PropertyDrawer
    {
        private const int ButtonWidth = 66;

        private static readonly List<string> _ignoreClassFullNames = new() { "TMPro.TMP_FontAsset" };

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;
            if (property.objectReferenceValue == null || !AreAnySubPropertiesVisible(property) ||
                !property.isExpanded)
                return totalHeight;

            ScriptableObject data = property.objectReferenceValue as ScriptableObject;

            if (data == null)
                return EditorGUIUtility.singleLineHeight;

            SerializedObject serializedObject = new(data);
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    if (prop.name == "m_Script")
                        continue;

                    SerializedProperty subProp = serializedObject.FindProperty(prop.name);
                    float height = EditorGUI.GetPropertyHeight(subProp, null, true) +
                                   EditorGUIUtility.standardVerticalSpacing;
                    totalHeight += height;
                } while (prop.NextVisible(false));
            }

            totalHeight += EditorGUIUtility.standardVerticalSpacing;
            serializedObject.Dispose();

            return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Type type = GetFieldType();

            if (type == null || _ignoreClassFullNames.Contains(type.FullName))
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            ScriptableObject propertySO = null;
            if (!property.hasMultipleDifferentValues && property.serializedObject.targetObject != null &&
                property.serializedObject.targetObject is ScriptableObject targetObject)
            {
                propertySO = targetObject;
            }

            Rect propertyRect = Rect.zero;
            GUIContent guiContent = new(property.displayName);
            Rect foldoutRect = new(position.x, position.y, EditorGUIUtility.labelWidth,
                EditorGUIUtility.singleLineHeight);

            if (property.objectReferenceValue != null && AreAnySubPropertiesVisible(property))
                property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true);
            else
            {
                foldoutRect.x += 12;
                EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true, EditorStyles.label);
            }

            Rect indentedPosition = EditorGUI.IndentedRect(position);
            float indentOffset = indentedPosition.x - position.x;
            propertyRect = new Rect(position.x + (EditorGUIUtility.labelWidth - indentOffset), position.y,
                position.width - (EditorGUIUtility.labelWidth - indentOffset), EditorGUIUtility.singleLineHeight);

            if (propertySO != null || property.objectReferenceValue == null)
                propertyRect.width -= ButtonWidth;

            EditorGUI.ObjectField(propertyRect, property, type, GUIContent.none);
            if (GUI.changed) property.serializedObject.ApplyModifiedProperties();

            Rect buttonRect = new(position.x + position.width - ButtonWidth, position.y, ButtonWidth,
                EditorGUIUtility.singleLineHeight);

            if (property.propertyType == SerializedPropertyType.ObjectReference &&
                property.objectReferenceValue != null)
            {
                ScriptableObject data = (ScriptableObject)property.objectReferenceValue;

                if (property.isExpanded)
                {
                    GUI.Box(
                        new Rect(0,
                            position.y + EditorGUIUtility.singleLineHeight +
                            EditorGUIUtility.standardVerticalSpacing -
                            1, Screen.width,
                            position.height - EditorGUIUtility.singleLineHeight -
                            EditorGUIUtility.standardVerticalSpacing), "");

                    EditorGUI.indentLevel++;
                    SerializedObject serializedObject = new(data);

                    SerializedProperty prop = serializedObject.GetIterator();
                    float y = position.y + EditorGUIUtility.singleLineHeight +
                              EditorGUIUtility.standardVerticalSpacing;
                    if (prop.NextVisible(true))
                    {
                        do
                        {
                            if (prop.name == "m_Script")
                                continue;

                            float height =
                                EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                            EditorGUI.PropertyField(new Rect(position.x, y, position.width - ButtonWidth, height),
                                prop,
                                true);
                            y += height + EditorGUIUtility.standardVerticalSpacing;
                        } while (prop.NextVisible(false));
                    }

                    if (GUI.changed)
                        serializedObject.ApplyModifiedProperties();

                    serializedObject.Dispose();
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (GUI.Button(buttonRect, "Create"))
                {
                    string selectedAssetPath = "Assets";
                    if (property.serializedObject.targetObject is MonoBehaviour behaviour)
                    {
                        MonoScript ms =
                            MonoScript.FromMonoBehaviour(behaviour);
                        selectedAssetPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(ms));
                    }

                    property.objectReferenceValue = CreateAssetWithSavePrompt(type, selectedAssetPath);
                }
            }

            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        private static ScriptableObject CreateAssetWithSavePrompt(Type type, string path)
        {
            path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", type.Name + ".asset", "asset",
                "Enter a file name for the ScriptableObject.", path);
            if (path == "")
                return null;

            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            EditorGUIUtility.PingObject(asset);
            return asset;
        }

        private Type GetFieldType()
        {
            if (fieldInfo == null)
                return null;

            Type type = fieldInfo.FieldType;

            if (type.IsArray)
                type = type.GetElementType();
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                type = type.GetGenericArguments()[0];

            return type;
        }

        private static bool AreAnySubPropertiesVisible(SerializedProperty property)
        {
            ScriptableObject data = (ScriptableObject)property.objectReferenceValue;
            SerializedObject serializedObject = new(data);
            SerializedProperty prop = serializedObject.GetIterator();

            while (prop.NextVisible(true))
            {
                if (prop.name == "m_Script")
                    continue;

                return true;
            }

            serializedObject.Dispose();
            return false;
        }
    }
}