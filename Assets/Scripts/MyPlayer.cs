using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour, IPunObservable {
    [SerializeField] private PhotonView photonView;

	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	Vector3 smoothMove;
	bool flip;
	bool toFlip;
	float move = 0f;
	float moveReceived = 0f;
	bool isDead = false;

	[SerializeField] GameObject playerCamera;
    [SerializeField] TMPro.TMP_Text nameText;
    [SerializeField] Canvas canvas;
	Animator anim;
    private void Awake() {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}
    private void Start() {
        photonView = GetComponent<PhotonView>();
        playerCamera.SetActive(photonView.IsMine);
        nameText.text = photonView.Owner.NickName;

		Vector3 temp = canvas.transform.localScale;
		temp.x *= -1;
		canvas.transform.localScale = temp;
	}
    void Update() {
        if (photonView.IsMine) {
            ProcessInputs();
        } else {
            transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 3);
			if (flip) {
				Vector3 theScale = transform.localScale;
				theScale.x *= -1;
				transform.localScale = theScale;

				Vector3 temp = canvas.transform.localScale;
				temp.x *= -1;
				canvas.transform.localScale = temp;
				flip = false;
			}
			gameObject.GetComponent<Animator>().SetFloat("Movement", Mathf.Abs(moveReceived));
        }
    }
	private void FixedUpdate() {
		m_Grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				m_Grounded = true;
			}
		}
	}
	private void ProcessInputs() {
        move = isDead ? 0 : Input.GetAxisRaw("Horizontal");
		anim.SetFloat("Movement", Mathf.Abs(move));
		Move(move, false);
	}
	public void Move(float move, bool jump) {
		if (m_Grounded || m_AirControl) {
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			if (move > 0 && !m_FacingRight) {
				Flip();
			}
			else if (move < 0 && m_FacingRight) {
				Flip();
			}
		}
		if (m_Grounded && jump) {
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}
	private void Flip() {
		m_FacingRight = !m_FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

		Vector3 temp = canvas.transform.localScale;
		temp.x *= -1;
		canvas.transform.localScale = temp;
		toFlip = true;
	}
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(transform.position);
			stream.SendNext(toFlip);
			stream.SendNext(move);
			toFlip = false;
		} else if (stream.IsReading) {
            smoothMove = (Vector3)stream.ReceiveNext();
			flip = (bool)stream.ReceiveNext();
			moveReceived = (float)stream.ReceiveNext();
		}
    }
    public void SetName(string name) {
        nameText.text = name;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ghost")) {
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			nameText.text = "DEAD";
			isDead = true;
		}
    }
}
