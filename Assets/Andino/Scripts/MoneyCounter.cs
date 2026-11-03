using UnityEngine;
public class MoneyCounter : MonoBehaviour {
 public string Summary(GameManager g){return g.mode+" | Pesos: "+g.pesos.ToString("N0")+" | Fichas: "+g.chips.ToString("N0")+" | Canje aparente: "+g.apparent.ToString("N0");}
}
