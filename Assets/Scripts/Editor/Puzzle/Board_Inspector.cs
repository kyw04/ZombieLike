using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    [CustomEditor(typeof(Board))]
    public class Board_Inspector : Editor
    {
        private ReorderableList reorderableList;

        private void OnEnable()
        {
            reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("piecePosition"), true, true, true, true);
            
            reorderableList.drawElementCallback = (rect, index, active, focused) =>
            {
                rect.y += 2;
                var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

                var indexLabelWidth = 20f;
                var spacing = 10f;
                var fieldWidth = (rect.width - indexLabelWidth - spacing) / 2f;

                // 인덱스 번호
                EditorGUI.LabelField(
                    new Rect(rect.x, rect.y, indexLabelWidth, EditorGUIUtility.singleLineHeight),
                    index.ToString(),
                    EditorStyles.boldLabel
                );

                float prevLabelWidth = EditorGUIUtility.labelWidth;

                // pos 필드
                EditorGUIUtility.labelWidth = 50f;
                SerializedProperty posProp = element.FindPropertyRelative("pos");
                posProp.objectReferenceValue = EditorGUI.ObjectField(
                    new Rect(rect.x + indexLabelWidth, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight),
                    new GUIContent("Position"),
                    posProp.objectReferenceValue,
                    typeof(Transform),
                    true
                );
               
                // piece 필드
                EditorGUIUtility.labelWidth = 35f;
                SerializedProperty pieceProp = element.FindPropertyRelative("piece");
                var pieceRect = new Rect(rect.x + indexLabelWidth + fieldWidth + spacing, rect.y, fieldWidth - 40f, EditorGUIUtility.singleLineHeight);
                pieceProp.objectReferenceValue = EditorGUI.ObjectField(
                    pieceRect,
                    new GUIContent("Piece"),
                    pieceProp.objectReferenceValue,
                    typeof(Transform),
                    true
                );
                
                

                // 썸네일 그리기
                if (pieceProp.objectReferenceValue != null)
                {
                    Image image = pieceProp.objectReferenceValue.GameObject().GetComponent<Image>();
                    Texture preview = image.mainTexture;
                    if (preview == null)
                    {
                        preview = AssetPreview.GetMiniThumbnail(pieceProp.objectReferenceValue);
                    }
                    else
                    {
                        var previewRect = new Rect(pieceRect.xMax + 5, rect.y, 35f, EditorGUIUtility.singleLineHeight);
                        Color prevColor = GUI.color;
                        GUI.color = image.color;
                        GUI.DrawTexture(previewRect, preview, ScaleMode.ScaleToFit);
                        GUI.color = prevColor;
                    }
                }
                
                EditorGUIUtility.labelWidth = prevLabelWidth;
            };

            reorderableList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Piece Position");
            };

            reorderableList.onAddCallback = list =>
            {
                ReorderableList.defaultBehaviours.DoAddButton(list);

                int index = list.serializedProperty.arraySize - 1;
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

                
                element.FindPropertyRelative("pos").objectReferenceValue = null;
                element.FindPropertyRelative("piece").objectReferenceValue = null;
            };
            reorderableList.elementHeight = 20.0f;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("radius"));
            EditorGUILayout.Space();
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

