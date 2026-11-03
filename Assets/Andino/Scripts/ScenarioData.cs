using UnityEngine;
[CreateAssetMenu(menuName="Andino/ScenarioData")]
public class ScenarioData : ScriptableObject {
 public string title="Casa de cambio y casino";
 public float[] operations={300,300,400};
 public float rate=50, alertThreshold=900;
 [Header("Orígenes ficticios de cada operación")]
 public string[] origins={"ORIGEN-A","ORIGEN-A","ORIGEN-A"};
 public string OriginAt(int index){return origins!=null&&index<origins.Length?origins[index]:"ORIGEN-A";}
 public string Ledger(){var text=new System.Text.StringBuilder("LIBRO DE OPERACIONES (ficticio)\n");for(int i=0;i<operations.Length;i++)text.Append(OriginAt(i)+": USD "+operations[i].ToString("N0")+"\n");return text.ToString();}
 [TextArea(3,8)] public string historicalContext="Según el expediente del curso: 5–13 febrero de 2016. El 9 de febrero se retiraron US$58 millones; unos US$30 millones se repartieron en efectivo. Estas cifras no se suman como partidas independientes. La práctica usa importes ficticios a escala.";
 public string disclaimer="Valores ficticios didácticos: USD 1.000; 50 pesos/USD. No son cifras históricas.";
}
