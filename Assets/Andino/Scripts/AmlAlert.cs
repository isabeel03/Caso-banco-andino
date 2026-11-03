using UnityEngine;
public class AmlAlert : MonoBehaviour {
 [Header("Alerta compartida simulada")] public bool sent,acknowledged;
 public string recipient="Casino Las Palmeras (ficticio)";
 public void ResetAlert(){sent=acknowledged=false;}
 public void Report(GameManager manager){sent=true;acknowledged=true;manager.ApplyDefense("ROS");manager.defenses.Add("Detección de fraccionamiento");manager.defenses.Add("Alerta compartida con "+recipient);manager.message="ROS SIMULADO: operaciones del mismo origen agregadas.\nFondos retenidos en casa de cambio.\nAlerta recibida por el casino: no aceptar fondos.";}
}
