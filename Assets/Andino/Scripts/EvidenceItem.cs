using UnityEngine;
public class EvidenceItem : MonoBehaviour {
 public string evidenceId="Libro de operaciones fraccionadas";
 public void Collect(){if(GameManager.I)GameManager.I.RegisterEvidence(evidenceId);}
}
