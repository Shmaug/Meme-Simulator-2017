using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Usable))]
public class UsableEditor : Editor {
    void OnSceneGUI() {
        Usable obj = target as Usable;

        Handles.color = Color.yellow;
        obj.range = Handles.RadiusHandle(Quaternion.identity, obj.transform.position, obj.range);
    }
}
#endif

public class Usable : MonoBehaviour {
    [HideInInspector]
    public bool selected { get; private set; }
    public Color baseColor = Color.white;
    public Color selectionColor = Color.white;
    public float range = 10f;
    public bool singleUse;
    public KeyCode useKey;
    bool used;
    
    SpriteRenderer sr;
    MaterialPropertyBlock block;
    float selectionTime;
    float c;
    
    Collider2D col;

    public EventTrigger.Entry trigger;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        block = new MaterialPropertyBlock();
        sr.GetPropertyBlock(block);
	}

	void Update () {
        if (singleUse && used) return;

        selected = col.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) &&
            Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < range;
        
        if (selected) {
            selectionTime += Time.deltaTime;

            // pulse
            float s = Mathf.Sin(5f * selectionTime);
            c = s * s;
            // no pulse
            //c = Mathf.Lerp(c, 1, selectionTime * 5f);

            if (Input.GetKeyDown(useKey)) {
                trigger.callback.Invoke(null);
                used = true;
                if (singleUse) {
                    block.SetColor("_Color", baseColor);
                    sr.SetPropertyBlock(block);
                    return;
                }
            }
        } else {
            selectionTime = 0;
            c = Mathf.Lerp(c, 0, Time.deltaTime * 10f);
        }

        block.SetColor("_Color", Color.Lerp(baseColor, selectionColor, c));
        sr.SetPropertyBlock(block);
    }
}
