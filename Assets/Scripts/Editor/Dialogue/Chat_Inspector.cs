using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Dialogue
{
    [CustomEditor(typeof(DialogueData))]
    public class Chat_Inspector : Editor
    {
        private ReorderableList talkList;
        private Dictionary<int, ReorderableList> textLists = new Dictionary<int, ReorderableList>();

        private void OnEnable()
        {
            // Talk 리스트 ReorderableList
            talkList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("talk"),
                true, true, true, true);

            // Header 라벨
            talkList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Dialogue", EditorStyles.boldLabel);
            };

            // 각 Talk element 그리기
            talkList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = talkList.serializedProperty.GetArrayElementAtIndex(index);
                var talkerProp = element.FindPropertyRelative("talker");
                var textList = GetTextList(element, index);

                var spacing = 4f;
                var lineHeight = EditorGUIUtility.singleLineHeight;

                // Talk 인덱스 라벨
                var indexRect = new Rect(rect.x, rect.y, rect.width, lineHeight);
                EditorGUI.LabelField(indexRect, $"Dialogue {index}", EditorStyles.boldLabel);

                // Talker 리스트
                var talkerRect = new Rect(rect.x, indexRect.yMax + spacing, rect.width,
                    EditorGUI.GetPropertyHeight(talkerProp, true));
                EditorGUI.PropertyField(talkerRect, talkerProp, new GUIContent("Talker"), true);
                
                // Text 리스트 (Foldout + ReorderableList)
                if (textList.serializedProperty.isExpanded)
                {
                    var textRect = new Rect(rect.x, talkerRect.yMax + spacing, rect.width, textList.GetHeight());
                    textList.DoList(textRect);
                }
                else
                {
                    var textProp = element.FindPropertyRelative("text");
                    EditorGUI.PropertyField(new Rect(rect.x, talkerRect.yMax + spacing, rect.width, lineHeight),
                        textProp, new GUIContent("Text"), true);
                }
            };

            talkList.elementHeightCallback = (index) =>
            {
                var element = talkList.serializedProperty.GetArrayElementAtIndex(index);
                var talkerProp = element.FindPropertyRelative("talker");
                var textProp = element.FindPropertyRelative("text");

                float height = 2f; // padding
                height += EditorGUIUtility.singleLineHeight * 2 + 2f; // 인덱스, Text 라벨
                height += EditorGUI.GetPropertyHeight(talkerProp, true) + 2f; // Talker 리스트

                if (textProp.isExpanded) // Text 리스트 열었을 때
                {
                    var textList = GetTextList(element, index);
                    height += textList.GetHeight() + 2f;
                }

                height += EditorGUIUtility.singleLineHeight; // element 거리 주기
                return height;
            };
        }

        private ReorderableList GetTextList(SerializedProperty element, int talkIndex)
        {
            if (textLists.ContainsKey(talkIndex)) return textLists[talkIndex];

            var talkerProp = element.FindPropertyRelative("talker");
            var textProp = element.FindPropertyRelative("text");
            var enumProp = element.FindPropertyRelative("enumValue");
            
            var list = new ReorderableList(element.serializedObject, textProp, true, true, true, true);

            // Foldout으로 접기/펼치기
            list.drawHeaderCallback = (rect) =>
            {
                textProp.isExpanded = EditorGUI.Foldout(new Rect(rect.x + 15f, rect.y, rect.width, rect.height), textProp.isExpanded, "Text", true);
            };

            // Text element 그리기
            list.drawElementCallback = (rect, index, active, focused) =>
            {
                enumProp.arraySize = textProp.arraySize;
                var textElement = textProp.GetArrayElementAtIndex(index);
                var enumElement = enumProp.GetArrayElementAtIndex(index);
                
                string[] displayedOptions = new string[talkerProp.arraySize];
                int[] optionValues = new int[talkerProp.arraySize];
                for (int i = 0; i < talkerProp.arraySize; i++)
                {
                    string talkerName = $"Talker {i}";
                    if (talkerProp.GetArrayElementAtIndex(i).objectReferenceValue != null)
                        talkerName = talkerProp.GetArrayElementAtIndex(i).objectReferenceValue.name;
                    displayedOptions[i] = talkerName;
                    optionValues[i] = i;
                }
                
                float enumFieldWidth = rect.width / 4f;
                float padding = 10f;
                enumElement.intValue = EditorGUI.IntPopup(new Rect(rect.x, rect.y, enumFieldWidth, rect.height), enumElement.intValue, displayedOptions, optionValues);

                rect.height = EditorGUIUtility.singleLineHeight * 3f; // TextArea 높이
                textElement.stringValue = EditorGUI.TextArea(new Rect(rect.x + enumFieldWidth + padding, rect.y, rect.width - enumFieldWidth - padding, rect.height), textElement.stringValue);
            };

            // Text element 높이
            list.elementHeightCallback = (i) =>
            {
                return EditorGUIUtility.singleLineHeight * 3f + 2f;
            };

            textLists[talkIndex] = list;
            return list;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            talkList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
