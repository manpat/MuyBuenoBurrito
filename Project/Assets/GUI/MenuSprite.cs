using UnityEngine;
using System.Collections;

public class MenuSprite : MonoBehaviour {
	public string animationName;

	private Animator animator;

	void Start () {
		animator = gameObject.GetComponent<Animator>();
		animator.Play(animationName);
	}

	void Update(){
		AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);

		if(!asi.loop && Mathf.Repeat(asi.normalizedTime, 1f) >= 0.95f){
			animator.Play(animationName, -1, 0f);
		}
	}
}
