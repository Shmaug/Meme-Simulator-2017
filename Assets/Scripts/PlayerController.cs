using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    Animator anim;
    Rigidbody2D rbody;

    public float moveSpeed = 4;
    
    int uplayer, downlayer, rightlayer, leftlayer;
	void Start () {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();

        uplayer = anim.GetLayerIndex("Up");
        downlayer = anim.GetLayerIndex("Down");
        leftlayer = anim.GetLayerIndex("Left");
        rightlayer = anim.GetLayerIndex("Right");
    }

    void setLayerActive(int layer) {
        anim.SetLayerWeight(uplayer, 0f);
        anim.SetLayerWeight(downlayer, 0f);
        anim.SetLayerWeight(leftlayer, 0f);
        anim.SetLayerWeight(rightlayer, 0f);

        anim.SetLayerWeight(layer, 1f);
    }
    
	void Update () {
        if (Input.GetKey(KeyCode.Mouse0))
            anim.SetTrigger("Shoot");

        Vector2 move = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) {
            setLayerActive(uplayer);
            move += Vector2.up;
        } else if (Input.GetKey(KeyCode.S)) {
            setLayerActive(downlayer);
            move += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A)) {
            setLayerActive(leftlayer);
            move += Vector2.left;
        } else if (Input.GetKey(KeyCode.D)) {
            setLayerActive(rightlayer);
            move += Vector2.right;
        }
        
        if (move.sqrMagnitude > 0) {
            move.Normalize();
            move *= moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
                move *= 1.5f;
        }
        rbody.velocity = Vector2.Lerp(rbody.velocity, move, Time.deltaTime * 10);
    }

    void LateUpdate() {
        anim.SetFloat("WalkSpeed", rbody.velocity.magnitude / 7.5f);
        anim.SetBool("Walking", rbody.velocity.magnitude > 1f);
    }
}
