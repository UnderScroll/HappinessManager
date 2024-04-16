using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(BlockTemplate), true)]
public class BlockEditor : EditorWindow
{
    AnimBool showFacesEditor;

    Color m_Color;
    string m_String;
    int m_Number = 0;

    [MenuItem("Window/BlockTemplateEditor")]
    static void Init()
    {
        BlockEditor window = (BlockEditor)GetWindow(typeof(BlockEditor));
    }
    private void OnEnable()
    {
        showFacesEditor = new AnimBool(true);
        showFacesEditor.valueChanged.AddListener(Repaint);
    }

    public void OnGUI()
    {

        Rect referenceRect = EditorGUILayout.GetControlRect();
        float paddingLeft = referenceRect.x + EditorGUI.indentLevel * 10 + 8;
        float paddingRight = 10;
        float paddingTop = 10;

        float squareSideSize = (referenceRect.width - paddingLeft - paddingRight) / 4;

        float width = squareSideSize * 4;
        float height = squareSideSize * 3;

        Rect zone = new Rect(referenceRect.x + paddingLeft, referenceRect.y + paddingTop, width, height);
        EditorGUI.DrawRect(zone, Color.red);

        float space = referenceRect.y + paddingTop + height;
        
        GUILayout.Space(space);

        GUILayout.Button("Click me");
        GUILayout.Space(20);
        GUILayout.Button("Or me");

        Repaint();
    }
}
