using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float maxSpeed = 1f;
	public float speed = 5f;
	private Animator Animacion;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		Animacion = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Animacion.GetBool ("IsAlive")) {
			rb2d.AddForce (Vector2.right * speed);	
			rb2d.velocity = new Vector2 (Mathf.Clamp (rb2d.velocity.x, -maxSpeed, maxSpeed), rb2d.velocity.y);

			if (rb2d.velocity.x > -0.01f && rb2d.velocity.x < 0.01f) {
				speed = -speed;
				rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
			}

			if (speed < 0) {
				transform.localScale = new Vector3 (7f, 7f, 1f);
			} else if (speed > 0) {
				transform.localScale = new Vector3 (-7f, 7f, 1f);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if ((col.gameObject.tag == "Player" || col.gameObject.tag == "Disparo") && gameObject.tag == "Enemigo Tipo 1") {
			if ((transform.position.y - (col.transform.position.y - transform.position.y) < col.transform.position.y - 0.064f) || col.gameObject.tag == "Disparo") {
				if (col.gameObject.tag != "Disparo")
					col.SendMessage ("EnemyJump");
				Anim ();
			} else {
				col.SendMessage ("EnemyKnockBack", transform.position.x);
			}
		} else if (gameObject.tag == "Enemigo Tipo 2") {
			if (col.gameObject.tag == "Disparo") {
				Anim ();
			} else if (col.gameObject.tag == "Player") {
				col.SendMessage ("EnemyKnockBack", transform.position.x);
			}
		}
	}

	private void die(){
		Destroy(gameObject);
	}
		
	private void Anim(){
		rb2d.isKinematic = true;
		rb2d.velocity = Vector2.zero;
		Destroy (transform.GetChild(0).gameObject);
		Animacion.SetBool ("IsAlive", false);
		Invoke ("die", 1f);
	}

}
