using UnityEngine;
public class AndinoInteractable : MonoBehaviour {
 public string action; public Vector3 destination; public bool portal;
 Color original; Renderer visual;
 bool isTeleportMarker;
 void Awake(){visual=GetComponent<Renderer>();original=visual.material.color;isTeleportMarker=gameObject.name.StartsWith("Teleport_");}
 public void OnPointerEnter(){if(visual)visual.material.color=isTeleportMarker?new Color(.15f,.9f,1f):Color.yellow;}
 public void OnPointerExit(){if(visual)visual.material.color=original;}
 public void OnPointerClick(){
 var g=GameManager.I; if(!g)return; AndinoWorld.I.Beep();
 if(action=="attack"||action=="defense"){g.Begin(action=="attack"?SimMode.Ataque:SimMode.Defensa);AndinoWorld.I.Move(new Vector3(0,1.6f,0));return;}
 if(portal){if(destination.x>10&&(g.stage<2||g.frozen)){g.message="Acceso al casino: convierte los fondos primero. En defensa quedan retenidos.";return;}AndinoWorld.I.Move(destination);return;}
 if(action=="book"){var evidence=GetComponent<EvidenceItem>();if(evidence)evidence.Collect();}
 if(action=="ros"){var control=GetComponent<DefenseControl>();if(control){control.Activate();return;}}
 g.Act(action);
 }
}
