using UnityEngine;
public class DefenseControl : MonoBehaviour {
 public void Activate(){if(GameManager.I)GameManager.I.Act("ros");}
}
