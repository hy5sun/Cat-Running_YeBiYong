using UnityEngine;
using System.Collections;

public enum PlayerState{
	Run,
	Jump,
	D_Jump,
	Sliding,
	Death
}

public class Player_Ctrl : MonoBehaviour {
	
	public CapsuleCollider collider;
	
	public PlayerState PS;
	public float Jump_Power = 0.05f;
	public float doubleJump_Power = 0.04f;
	
	public Rigidbody rig;
	public float slideSpeed = 10f;
	
	public AudioClip[] Sound;
	public Animator animator;
	
	public GameObject AnotherSpeaker;
	public GameManager GM;
	void Start()
	{
		collider = GetComponent<CapsuleCollider>();
		rig = GetComponent<Rigidbody>();
	}
	//------------------------------------------------------------
	void Update(){
		rigidbody.WakeUp(); 
		if(Input.GetKeyDown (KeyCode.Space) && PS != PlayerState.Death){ //점프키가 눌리고, 플레이어가 죽은 상태가 아니라면
			if(PS == PlayerState.Jump){  //플레이어 상태가 점프라면 
				D_Jump ();//더블점프 
			}
			if(PS == PlayerState.Run){//플레이어 상태가 달리기라면
				Jump (); //점프 
			}
			if(PS == PlayerState.Sliding){
				GoUp();
				Jump ();
			}
		}
		if (Input.GetKey (KeyCode.LeftShift))//점프 도중에 슬라이드를 눌러도 캐릭터가 바닿에 내려가면 바로 슬라이드가 가능하도록 함(GetKey)
			if (PS == PlayerState.Run)
				Sliding ();
		
		if (Input.GetKeyUp (KeyCode.LeftShift))
			if (PS == PlayerState.Sliding)
				GoUp ();
	}
	//---------------------------------------------------------------
	void Jump(){
		PS = PlayerState.Jump;
		rigidbody.AddForce (Vector3.up *Jump_Power, ForceMode.Impulse);
		//SoundPlay (2);
		AnotherSpeaker.SendMessage ("SoundPlay");
		
		animator.SetTrigger("Jump");
		animator.SetBool("Ground",false);
	}
	
	void D_Jump(){
		PS = PlayerState.D_Jump;
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce (Vector3.up *doubleJump_Power, ForceMode.Impulse);
		//SoundPlay (2);
		AnotherSpeaker.SendMessage ("SoundPlay");
		
		animator.SetTrigger("D_Jump");
		animator.SetBool("Ground",false);
	}
	void Run(){
		PS = PlayerState.Run;
		
		animator.SetBool("Ground",true);
	}
	void OnCollisionEnter(Collision collision){
		if(PS != PlayerState.Run && PS != PlayerState.Death && PS !=PlayerState.Sliding){
			Run();
		}
	}
	
	void Sliding()
	{
		PS = PlayerState.Sliding;
		transform.rotation = Quaternion.Euler(0,0,-90);
		rig.MovePosition(transform.position + Vector3.down); //캐릭터가 누울때 내려가는 움직임을 없애줌 
		animator.SetTrigger("Sliding");
		animator.SetBool("Ground",false);
	}
	
	void GoUp()
	{
		transform.transform.rotation = Quaternion.Euler(0,0,0);
		rig.MovePosition (transform.position + Vector3.up);
		Run();

		animator.SetBool("Ground",false);
	}
	//---------------------------------------------------------------------------------------------
	
	void CoinGet(){
		SoundPlay (0);
		GM.CoinGet ();
	}
	
	void GameOver(){
		PS = PlayerState.Death;
		SoundPlay (1);
		GM.GameOver ();
	}
	
	void SoundPlay(int Num){
		audio.clip = Sound [Num];
		audio.Play ();
	}
	
	void OnTriggerEnter(Collider other){
		rigidbody.WakeUp();
		if (other.gameObject.name == "Coin") {
			Destroy (other.gameObject);
			CoinGet ();
		}
		else if (other.gameObject.tag == "Coin_R"){ //무지개 붕어빵이랑 닿으면
			OnStrong(); //OnStrong 함수 실행
			Destroy (other.gameObject);
			CoinGet ();
		}
		
		if(other.gameObject.name == "DeathZone" && PS != PlayerState.Death && this.gameObject.layer != 9){ //player layer가 9번이 아니라면
			GameOver();
		}
	}
	
	void OnStrong()
	{
		this.gameObject.layer = 9; //player layer이 9번 (Strong)으로 바뀜
		Invoke ("OffStrong", 5); //5초 후에는 OffStrong 함수 실행
	}
	
	void OffStrong()
	{
		gameObject.layer = 0; //player layer이 Default로 바뀜
	}
}