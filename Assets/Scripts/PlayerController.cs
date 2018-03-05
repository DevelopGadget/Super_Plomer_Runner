using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {
	
	private Rigidbody2D Player, Dis;
	private Animator Animacion;
	public bool Suelo, Agachado = false;
	public GameObject DisparoPrefab, Game;
	public AudioClip Salto, Muerte, Coin;
	private bool Jump = false, Llave = false;
	private float Disp = 0.5f;
	private ArrayList disparos = new ArrayList ();
	private AudioSource AudioController;
	public Camera Camara;

	// Use this for initialization
	void Start () {
		Player = GetComponent<Rigidbody2D> ();
		Player.isKinematic = false;
		AudioController = GetComponent<AudioSource> ();
		Animacion = GetComponent<Animator> ();
		transform.parent = null;
		if (transform.localScale.x > 0) transform.localScale = new Vector3 (5f, 5f, 1f);
		else transform.localScale = new Vector3 (-5f, 5f, 1f);
	}

	// Update is called once per frame
	void Update () {
		if (Animacion.GetBool ("IsAlive")  && !Player.isKinematic) {
			Animacion.SetFloat ("Speed", Mathf.Abs (Player.velocity.x));
			Animacion.SetBool ("Suelo", Suelo);
			if (Input.GetKeyDown (KeyCode.UpArrow) && Suelo && !Animacion.GetBool ("Agachado") && !Jump) Jump = true;
			Animacion.SetBool ("Agachado", false);
			if ((Input.GetKey (KeyCode.DownArrow) ||  Agachado) && Suelo)Animacion.SetBool ("Agachado", true);
			if(Input.GetKeyDown (KeyCode.Space) && Suelo && !Animacion.GetBool ("Agachado") && !Jump) Animacion.SetBool ("Disparar", true);
		}
	}

	void FixedUpdate(){
		if (Animacion.GetBool ("IsAlive") && !Player.isKinematic) {
			Vector3 fix = Player.velocity;
			fix.x *= 0.8f;
			if (Suelo)Player.velocity = fix; 
			if (!Animacion.GetBool ("Agachado")) {
				#if UNITY_EDITOR || UNITY_EDITOR_WIN
					Player.AddForce (Vector2.right * 90f * Input.GetAxis ("Horizontal"));
				#endif
				Player.AddForce (Vector2.right * 50f * CrossPlatformInputManager.GetAxis("Horizontal"));
				Player.velocity = new Vector2 (Mathf.Clamp (Player.velocity.x, -5f, 5f), Player.velocity.y);
				if ((Input.GetAxis ("Horizontal") > 0.1f) || CrossPlatformInputManager.GetAxis("Horizontal") > 0.1f)transform.localScale = new Vector3 (5f, 5f, 1f);
				if ((Input.GetAxis ("Horizontal") < -0.1f)||  CrossPlatformInputManager.GetAxis("Horizontal") < -0.1f)transform.localScale = new Vector3 (-5f, 5f, 1f);
				if (Jump) {
					Musica (Salto, 0.2f);
					Player.AddForce (Vector2.up * 9.5f, ForceMode2D.Impulse);
					Jump = false;
				}
			}
		}
	}

	private void Aparecer(){
		GetComponent<CapsuleCollider2D> ().enabled = true;
		Animacion.SetBool ("IsAlive", true);
		Player.velocity = Vector2.zero;
		transform.position = new Vector3 (0f, -1.3f, 1f);
		Player.isKinematic = false;
	}

	void OnBecameInvisible(){
		if (Animacion.GetBool ("IsAlive"))
			Game.SendMessage ("Vidas_Inc");
		if (Game.GetComponent<GameController> ().GetVida () >= 3)
			Escena ();
		else
			Player.velocity = Vector2.zero;
			Player.isKinematic = true;
			Invoke ("Aparecer", 1f);
	}
		
	public void EnemyJump(){
		Player.AddForce (Vector2.up * 8f, ForceMode2D.Impulse);
	}

	public void EnemyKnockBack(){
		if(Animacion.GetBool ("IsAlive")) Game.SendMessage ("Vidas_Inc");
		if (Game.GetComponent<GameController> ().GetVida() >= 3) {
			GetComponent<CapsuleCollider2D> ().enabled = false;
			Animacion.SetBool ("IsAlive", false);
			Camara.GetComponent<AudioSource> ().Stop ();
			Musica (Muerte, 0.4f);
			Invoke ("Escena", 3f);
		} else {
			Player.velocity = Vector2.zero;
			GetComponent<CapsuleCollider2D> ().enabled = false;
			Animacion.SetBool ("IsAlive", false);
			Player.isKinematic = true;
			Invoke ("Aparecer", 1f);
		}
	}

	private void Disparo(){
		Disp = (transform.localScale.x >= 0f) ? 0.7f : -0.7f;
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

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Estrella") {
			Destroy (col.gameObject);
			Game.SendMessage ("Estrella_Puntos", col.gameObject);
			Musica (Coin, 0.2f);
		} 
		if (col.gameObject.tag == "Puerta") {
			if (Llave) {
				Player.velocity = Vector2.zero;
				Animacion.SetBool ("Final", true);
				col.gameObject.GetComponent<Animator> ().SetBool ("Abierta", true);
				Invoke ("Level_Pass", 2f);
			}
		} 
		if (col.gameObject.tag == "Llave") {
			Musica (Coin, 0.2f);
			Llave = true;
			Game.SendMessage ("Llave", col.gameObject);
		}
	}

	public void BotonSaltar(){
		if (Suelo && !Animacion.GetBool ("Agachado") && !Jump)Jump = true; Animacion.SetBool ("Agachado", false); Agachado = false;
	}

	public void BotonFire(){
		if(Suelo && !Animacion.GetBool ("Agachado") && !Jump)Animacion.SetBool ("Disparar", true);
	}

	public void BotonAgachado(){
		if (Suelo) Agachado = true; 
	}

	public void Dejar(){
		Agachado = false; 
	}

	private void Musica(AudioClip clip , float vol){
		AudioController.clip = clip;
		AudioController.volume = vol;
		AudioController.Play ();
	}

	public void EnemigoPuntos(int Point){
		Game.SendMessage ("Enemigo_Puntos", Point);
	}

	private void Escena(){
		Game.SendMessage ("EscenaManager", "Game Over");
	}

	private void Level_Pass(){
		Game.SendMessage ("EscenaManager", "Passes");
	}
}