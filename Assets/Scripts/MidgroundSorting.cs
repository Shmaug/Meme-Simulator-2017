using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpriteRenderer))]
public class MidgroundSortingEditor : Editor {
    public override void OnInspectorGUI() {
        SpriteRenderer t = target as SpriteRenderer;
        // Sprite
        // Color
        // Flip [x] [y]
        // Material
        // Sorting Layer  [dropdown]
        // Order in Layer

        t.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", t.sprite, typeof(Sprite), true, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        t.color = EditorGUILayout.ColorField("Color", t.color);
        EditorGUILayout.BeginHorizontal(new GUIStyle() { padding = new RectOffset() });
        GUILayout.Label("Flip", GUILayout.MinWidth(EditorGUIUtility.labelWidth));
        t.flipX = EditorGUILayout.Toggle("X", t.flipX);
        t.flipY = EditorGUILayout.Toggle("Y", t.flipY);
        EditorGUILayout.EndHorizontal();
        t.sharedMaterial = (Material)EditorGUILayout.ObjectField("Material", t.sharedMaterial, typeof(Material), true);

        string[] layers = new string[SortingLayer.layers.Length];
        int index = 0;
        for (int i = 0; i < layers.Length; i++) {
            layers[i] = SortingLayer.layers[i].name;
            if (SortingLayer.layers[i].id == t.sortingLayerID)
                index = i;
        }
        t.sortingLayerName = layers[EditorGUILayout.Popup("Sorting Layer", index, layers)];
        t.sortingOrder = EditorGUILayout.IntField("Order in Layer", t.sortingOrder);

        if (t.sortingLayerName == "Midground")
            if (!t.GetComponent<MidgroundSorting>())
                if (GUILayout.Button("Set sorting layer from position")) {
                    float offset = 0f;
                    Collider2D c = t.GetComponent<Collider2D>();
                    if (c != null)
                        offset = c.bounds.center.y + c.bounds.extents.y;
                    
                    t.sortingOrder = Mathf.RoundToInt(-(t.transform.position.y + offset) * 2f);
                }
    }

    public void OnSceneGUI() {
        SpriteRenderer t = target as SpriteRenderer;
        if (t.sortingLayerName == "Midground") {
            Handles.color = Color.yellow;
            if (t.transform.GetComponent<MidgroundSorting>()) {
                float offset = t.transform.GetComponent<MidgroundSorting>().offset;
                Handles.DrawLine(t.transform.position + new Vector3(-2, offset, 0), t.transform.position + new Vector3(2, offset, 0));
            } else {
                Handles.DrawLine(new Vector3(t.transform.position.x - 2, t.sortingOrder * 2f, 0), new Vector3(t.transform.position.x + 2, t.sortingOrder * 2f, 0));
            }
        }
    }
}

public class MidgroundSorting : MonoBehaviour {
    SpriteRenderer sr;
    
    public float offset;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        sr.sortingOrder = Mathf.RoundToInt(-(transform.position.y + offset) * 2f);
	}
}
