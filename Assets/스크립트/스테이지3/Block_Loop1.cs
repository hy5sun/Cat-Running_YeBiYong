using UnityEngine;
using System.Collections;

public class Block_Loop1 : MonoBehaviour {

	public float Speed = 3;	//블럭들이 움직일 스피드
	public GameObject[] Block;	//계속해서 만들 블럭
	public GameObject A_Zone;	//가운데에 있는 블럭
	public GameObject B_Zone;	//화면의 오른쪽에 있는 블럭

	void Update(){
		Move();
	}
	void Move(){
		A_Zone.transform.Translate (Vector3.left * Speed * Time.deltaTime);
		B_Zone.transform.Translate (Vector3.left * Speed * Time.deltaTime);

		if (B_Zone.transform.position.x <= 0) {
			Destroy (A_Zone);
			A_Zone=B_Zone;
			Make();
			}
	}
	void Make(){
		int A = Random.Range (0, Block.Length);
		B_Zone = Instantiate(Block[A], new Vector3 (A_Zone.transform.position.x+30, -5, 0), transform.rotation) as GameObject;
	}
}
