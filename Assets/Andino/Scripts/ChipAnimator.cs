using UnityEngine;
public class ChipAnimator : MonoBehaviour {
 public Animator animator;
 public void SetDealt(bool value){if(animator)animator.SetBool("Repartidas",value);}
}
