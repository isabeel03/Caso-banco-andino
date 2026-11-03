using UnityEngine;
public class CyberpunkEnvironment : MonoBehaviour {
 public Renderer[] accents;
 public TextMesh signal;
 public Color normal=new Color(.02f,.8f,1), blocked=new Color(1,.12f,.08f);
 bool previous;bool initialized;
 void Update(){var g=GameManager.I;if(!g)return;bool alarm=g.frozen;
 if(initialized&&alarm==previous)return;initialized=true;previous=alarm;
 foreach(var item in accents)if(item){var material=item.material;var color=alarm?blocked:normal;color.a=material.color.a;material.color=color;}
 if(signal)signal.text=alarm?"OPERACIÓN RETENIDA\nALERTA ANTILAVADO COMPARTIDA":"ANDINO EXCHANGE\nSIMULACIÓN / INSPECCIONA LAS EVIDENCIAS";
 }
}
