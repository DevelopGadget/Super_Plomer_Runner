using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour {
	
	private Rigidbody2D Player, Dis;
	private Animator Animacion;
	public bool Suelo;
	public GameObject DisparoPrefab;
	public AudioClip Salto, Muerte;
	private bool Jump = false;
	private float Disp = 0.5f;
	private ArrayList disparos = new ArrayList ();
	private AudioSource AudioController;
	public Camera Camara;

	// Use this for initialization
	void Start () {
		Player = GetComponent<Rigidbody2D> ();
		AudioController = GetComponent<AudioSource> ();
		Animacion = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Animacion.GetBool ("IsAlive")) {
			Animacion.SetFloat ("Speed", Mathf.Abs (Player.velocity.x));
			Animacion.SetBool ("Suelo", Suelo);
			if (Input.GetKeyDown (KeyCode.UpArrow) && Suelo && !Animacion.GetBool ("Agachado") && !Jump) Jump = true;
			Animacion.SetBool ("Agachado", false);
			if (Input.GetKey (KeyCode.DownArrow) && Suelo)Animacion.SetBool ("Agachado", true);
			if(Input.GetKeyDown (KeyCode.Space) && Suelo && !Animacion.GetBool ("Agachado") && !Jump) Animacion.SetBool ("Disparar", true);
		}
	}

	void FixedUpdate(){
		if (Animacion.GetBool ("IsAlive")) {
			Vector3 fix = Player.velocity;
			fix.x *= 0.8f;
			if (Suelo)Player.velocity = fix; 
			if (!Animacion.GetBool ("Agachado")) {Player.AddForce (Vector2.right * 90f * Input.GetAxis ("Horizontal"));Player.velocity = new Vector2 (Mathf.Clamp (Player.velocity.x, -5f, 5f), Player.velocity.y);
				if ((Input.GetAxis ("Horizontal") > 0.1f))transform.localScale = new Vector3 (5f, 5f, 1f);
				if ((Input.GetAxis ("Horizontal") < -0.1f))transform.localScale = new Vector3 (-5f, 5f, 1f);
				if (Jump) {
					Musica (Salto, 0.2f);
					Player.AddForce (Vector2.up * 9.5f, ForceMode2D.Impulse);
					Jump = false;
				}
			}
		}
	}
	void OnBecameInvisible(){
		transform.position = new Vector3 (0f, 1f, 1f);
	}
		
	public void EnemyJump(){
		Jump = true;
	}

	public void EnemyKnockBack(float enemyPosX){
		Jump = true;
		GetComponent<CapsuleCollider2D>().enabled = false;
		Player.AddForce (Vector2.left * Mathf.Sign (enemyPosX - transform.position.x) * 9.5f, ForceMode2D.Impulse);
		Animacion.SetBool ("IsAlive", false);
		Camara.GetComponent<AudioSource> ().Stop ();
		Musica (Muerte, 0.4f);
	}

	private void Disparo(){
		Disp = (transform.localScale.x == 5f) ? 0.7f : -0.7f;
		GameObject clone = (GameObject) Instantiate (DisparoPrefab, new Vector3 (Disp + transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
		disparos.Add (clone);
		Dis = clone.GetComponent<Rigidbody2D> ();
		Vector2 dir = (Disp == 0.7f) ? Vector2.right : Vector2.left;
		Dis.AddForce (dir * 350f);
		Dis.velocity = new Vector2 (Mathf.Clamp (Dis.velocity.x, -20f, 20f), Dis.velocity.y);
		Invoke ("die", 3f);
		Animacion.SetBool ("Disparar", false);
	}

	private void die(){
		Destroy ((GameObject) disparos[0]);
		disparos.RemoveAt (0);
	}

	public void BotonSaltar(){
		if (Suelo && !Animacion.GetBool ("Agachado") && !Jump)Jump = true;
	}

	public void BotonFire(){
		if(Suelo && !Animacion.GetBool ("Agachado") && !Jump)Animacion.SetBool ("Disparar", true);
	}

	private void Musica(AudioClip clip , float vol){
		AudioController.clip = clip;
		AudioController.volume = vol;
		AudioController.Play ();
	}

}