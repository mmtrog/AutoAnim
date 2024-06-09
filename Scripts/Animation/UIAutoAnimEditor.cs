using UnityEditor;
using UnityEngine;

namespace Minh_Trong.Scripts.UIAutoAnimation
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(UIAutoAnim), true), CanEditMultipleObjects]
    public class UIAutoAnimationEditor : Editor
    {
        #region  Serialized Property

        private SerializedProperty useAppearAnim;
        private SerializedProperty useDisAppearAnim;
        private SerializedProperty useLoopAnim;
        private SerializedProperty customTransform;
        private SerializedProperty manualSetUp;
        private SerializedProperty L_Type;
        
        #endregion
        
        void OnEnable()
        {
            useAppearAnim = serializedObject.FindProperty("useAppearAnim");
            useDisAppearAnim = serializedObject.FindProperty("useDisAppearAnim");
            useLoopAnim = serializedObject.FindProperty("useLoopAnim");
            customTransform = serializedObject.FindProperty("customTransform");
            manualSetUp = serializedObject.FindProperty("manualSetUp");
            L_Type = serializedObject.FindProperty("L_Type");
        }
        public override void OnInspectorGUI()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                normal =
                {
                    textColor = new Color(0.1f,0.5f,0.8f,1),
                },
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 5, 5),
            };
            
            GUIStyle descriptionStyle = new GUIStyle(GUI.skin.label)
            {
                normal =
                {
                    textColor = new Color(0.4f,0.7f,0f,0.8f),
                },
                fontSize = 10,
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 3, 0),
            };
            
            EditorGUILayout.BeginVertical(labelStyle);
            
            GUI.Label(GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight * 1.5f), "AUTO ANIMATION", labelStyle);
            
            GUI.Label(GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight * 0.7f), "Required DOTween package", descriptionStyle);
            
            //GUILayout.Label ("REQUIRED DOTween PACKAGE", GUILayout.Width(EditorGUIUtility.labelWidth));

            EditorGUILayout.EndVertical();
            
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
            {
                normal =
                {
                    textColor = Color.white,
                    background = MakeTex(600, 1, new Color(0.2f, 0.2f, 0.2f, 1f))
                },
                margin = new RectOffset(15, 15, 10, 10),
                padding = new RectOffset(15, 15, 10, 10),
                border = new RectOffset(1,1,1,1),
            };
            
            GUIStyle loopBoxStyle = new GUIStyle(GUI.skin.box)
            {
                normal =
                {
                    textColor = Color.white,
                    background = MakeTex(500, 1, new Color(0.15f, 0.15f, 0.15f, 1f))
                },
                margin = new RectOffset(0, 0, 5, 5),
                padding = new RectOffset(0, 0, 5, 5),
            };
            

            serializedObject.Update();
            
            SerializedProperty iterator = serializedObject.GetIterator();
            while (iterator.NextVisible(true))
            {
                switch (iterator.name)
                {
                    case "A_Type":
                        if (useAppearAnim.boolValue)
                        {
                            EditorGUILayout.BeginVertical(boxStyle);
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Type"), true); 
                        }  
                        break;
                    case "A_Duration":
                        if (useAppearAnim.boolValue)
                        {
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Duration"), true); 
                        }  
                        break;
                    case "A_Ease":
                        if (useAppearAnim.boolValue)
                        {
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Ease"), true); 
                            EditorGUILayout.EndVertical();
                        }  
                        break;
                    case "DA_Type":
                        if (useDisAppearAnim.boolValue)
                        {
                            EditorGUILayout.BeginVertical(boxStyle);
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Type"), true); 
                        }  
                        break;
                    case "DA_Duration":
                        if (useDisAppearAnim.boolValue)
                        {
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Duration"), true); 
                        }  
                        break;
                    case "DA_Ease":
                        if (useDisAppearAnim.boolValue)
                        {
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Ease"), true); 
                            EditorGUILayout.EndVertical();
                        }  
                        break;
                    case "L_Type":
                        if (useLoopAnim.boolValue)
                        {
                            EditorGUILayout.BeginVertical(boxStyle);
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Type"), true);
                        }  
                        break;
                    case "L_Duration":
                    case "L_LoopPerPhase":
                    case "L_DelayPerPhase":
                        if (useLoopAnim.boolValue)
                        {
                            EditorGUILayout.PropertyField(iterator, new GUIContent(iterator.name.Substring(2, iterator.name.Length - 2)), true); 
                        }  
                        break;
                    case "L_Ease":
                        if (useLoopAnim.boolValue)
                        {
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Ease"), true); 
                            EditorGUILayout.EndVertical();
                        }  
                        break;
                    case "posChange":
                        if (useLoopAnim.boolValue)
                        {
                            if (L_Type.enumValueIndex == 0)
                            {
                                EditorGUILayout.BeginVertical(loopBoxStyle);
                                EditorGUI.indentLevel++;
                                EditorGUILayout.PropertyField(iterator, new GUIContent("Increase position"));
                                EditorGUI.indentLevel--;  
                                EditorGUILayout.EndVertical();
                            }      
                        }
                        break;
                    case "rotChange":
                        if (useLoopAnim.boolValue)
                        {
                            if (L_Type.enumValueIndex == 1)
                            {
                                EditorGUILayout.BeginVertical(loopBoxStyle);
                                EditorGUI.indentLevel++;
                                EditorGUILayout.PropertyField(iterator, new GUIContent("Increase rotation"));
                                EditorGUI.indentLevel--;  
                                EditorGUILayout.EndVertical();
                            }      
                        }
                        break;
                    case "trans":
                        if (manualSetUp.boolValue)
                        {
                            EditorGUILayout.BeginVertical(boxStyle);
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Rect Transform"), true); 
                        }
                        break;
                    case "group":
                        if (manualSetUp.boolValue)
                        {
                            EditorGUILayout.PropertyField(iterator, new GUIContent("Canvas Group"), true); 
                            EditorGUILayout.EndVertical();
                        }  
                        break;
                    case "scale":
                        if (customTransform.boolValue)
                        {
                            EditorGUILayout.BeginVertical(boxStyle);
                            EditorGUILayout.PropertyField(iterator, true); 
                        }
                        break;
                    case "rotation":
                        if (customTransform.boolValue)
                        {
                            EditorGUILayout.PropertyField(iterator, true); 
                            EditorGUILayout.EndVertical();
                        }
                        break;
                    case "customTransform":
                        EditorGUILayout.PropertyField(iterator, new GUIContent("Custom Rect Transform"), true);
                        break;
                    case "useAppearAnim":
                        EditorGUILayout.PropertyField(iterator, new GUIContent("Appear Animation"), true);
                        break;
                    case "useDisAppearAnim":
                        EditorGUILayout.PropertyField(iterator, new GUIContent("Disappear Animation"), true);
                        break;
                    case "useLoopAnim":
                        EditorGUILayout.PropertyField(iterator, new GUIContent("Loop Animation"), true);
                        break;
                    case "manualSetUp":
                        EditorGUILayout.PropertyField(iterator, true);
                        if (!manualSetUp.boolValue)
                        {
                            EditorGUILayout.HelpBox("Manual setup is recommended for performance reasons", MessageType.Info);
                        }
                        break;
                    case "x":
                    case "y":
                    case "z":
                    case "m_Script":
                        break;
                    default:
                        EditorGUILayout.PropertyField(iterator, true);   
                        break;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        private Texture2D MakeTex(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
    #endif
}