using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    Animator anim;
    Rigidbody2D rbody;

    public float moveSpeed = 7;

	void Start () {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        Vector2 move = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
            move += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            move += Vector2.right;
        if (Input.GetKey(KeyCode.W))
            move += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            move += Vector2.down;

        if (move.sqrMagnitude > 0) {
            move.Normalize();
            move *= moveSpeed;
        }
        rbody.velocity = move;

        if (Input.GetKey(KeyCode.Mouse0))
            anim.Play("Shoot");
    }
}
