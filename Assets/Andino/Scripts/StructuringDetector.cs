using UnityEngine;
public class StructuringDetector : MonoBehaviour {
 public float TotalForOrigin(ScenarioData data,string origin){float sum=0;for(int i=0;i<data.operations.Length;i++)if(data.OriginAt(i)==origin)sum+=data.operations[i];return sum;}
 public bool IsSuspicious(ScenarioData data,string origin){int count=0;for(int i=0;i<data.operations.Length;i++)if(data.OriginAt(i)==origin)count++;return count>1&&TotalForOrigin(data,origin)>=data.alertThreshold;}
}
