using UnityEngine;
using System.Collections.Generic;
public enum SimMode { Ataque, Defensa }
public class GameManager : MonoBehaviour {
 public static GameManager I;
 public ScenarioData data;
 public SimMode mode=SimMode.Ataque;
 public StructuringDetector detector; public AmlAlert aml; public bool ended;
 public int stage; public float pesos,chips,apparent; public bool frozen;
 public HashSet<string> evidence=new HashSet<string>(), defenses=new HashSet<string>();
 public string message="Elige ATAQUE o DEFENSA para comenzar.";
 void Awake(){if(I!=null&&I!=this){Destroy(gameObject);return;} I=this;if(!detector)detector=GetComponent<StructuringDetector>();if(!aml)aml=GetComponent<AmlAlert>();}
 void OnDestroy(){if(I==this)I=null;}
 public void Begin(SimMode value){mode=value;ended=false;if(aml)aml.ResetAlert();stage=0;pesos=chips=apparent=0;frozen=false;evidence.Clear();defenses.Clear();message="1. Revisa el libro de operaciones del mismo origen.";}
 public void RegisterEvidence(string id){evidence.Add(id);}
 public void ApplyDefense(string id){defenses.Add(id);frozen=true;message="ROS emitido: fondos retenidos antes del casino. Se comparte la alerta entre entidades.";}
 public void EndScenario(){if(!frozen&&stage<5){message="Completa el recorrido para ver el resultado final.";return;}ended=true;message=frozen?"DEFENSA: flujo detenido. Dinero aparentemente lavado: 0.\nEl reporte y la suma de operaciones evitan el paso al casino.":"ATAQUE: el canje aparenta un origen legítimo, pero NO cambia el origen ilícito.\nLa apuesta doble reduce la exposición; no garantiza ganancias ni elimina pérdidas.";}
 public void Act(string id){
 if(id=="history"){message=data.historicalContext;return;}
 if(id=="credits"){message="Geometría: primitivas de Unity. Fichas: Unity Animator.\nBase: Google Cardboard, Google LLC, Apache 2.0.\nAsistencia: OpenAI Codex para código y configuración.\nSolo simulación: entidades y datos de práctica ficticios.";return;}
 if(id=="analysis"){message="Vulnerabilidades: operaciones fraccionadas sin agregación,\ninformación aislada y casinos fuera de las obligaciones\nantilavado descritas para 2016. Defensa: sumar por origen,\nreportar y compartir la alerta antes de transferir.";return;}

 if(id=="book"){RegisterEvidence("Operaciones fraccionadas");stage=Mathf.Max(stage,1);message=data.Ledger()+"Revisa el total por origen en el panel ROS.";return;}
 if(id=="ros") {if(stage<1){message="Primero revisa el libro.";return;} RegisterEvidence("Total agregado"); if(mode==SimMode.Defensa){bool suspicious=false;for(int i=0;i<data.operations.Length;i++)if(detector.IsSuspicious(data,data.OriginAt(i)))suspicious=true;if(suspicious){aml.Report(this);ended=true;}else message="No se detectó fraccionamiento con el umbral de esta práctica.";}else message="ATAQUE: no se aplica el control. Continúa a convertir.";return;}
 if(frozen){EndScenario();return;}
 if(id=="convert"){if(stage!=1){message="Revisa el libro primero; la conversión solo se realiza una vez.";return;}if(mode==SimMode.Defensa){Act("ros");return;}float total=0;foreach(float n in data.operations)total+=n;pesos=total*data.rate;stage=2;RegisterEvidence("Conversión");message="2. Conversión registrada. Usa el portal al casino.";}
 if(id=="chips"){if(stage!=2){message="Primero convierte el dinero en la casa de cambio.";return;}chips=pesos;pesos=0;stage=3;RegisterEvidence("Compra de fichas");message="3. Observa la simulación de apuestas a banca y jugador.";}
 if(id=="bet"){if(stage!=3){message="Primero cambia el dinero por fichas.";return;}stage=4;RegisterEvidence("Patrón de apuesta doble");message="4. Fichas repartidas en ambos lados. Observa el patrón sospechoso.\nModelo didáctico sin ganancias: no reproduce reglas ni probabilidades reales.";}
 if(id=="cash"){if(stage!=4){message="Primero observa la apuesta doble.";return;}apparent=chips;chips=0;stage=5;RegisterEvidence("Canje");EndScenario();}
 if(id=="result")EndScenario();
 }
}
