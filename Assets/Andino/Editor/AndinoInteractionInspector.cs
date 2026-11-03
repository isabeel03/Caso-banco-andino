using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(AndinoInteractable))]
public class AndinoInteractionInspector : Editor {
 static readonly string[] names={"Iniciar ataque","Iniciar defensa","Revisar libro","Emitir alerta ROS","Convertir dinero","Comprar fichas","Observar apuesta","Canjear fichas","Mostrar resultado","Contexto histórico","Créditos","Análisis"};
 static readonly string[] ids={"attack","defense","book","ros","convert","chips","bet","cash","result","history","credits","analysis"};
 public override void OnInspectorGUI(){
 serializedObject.Update();var portal=serializedObject.FindProperty("portal");EditorGUILayout.PropertyField(portal,new GUIContent("Es un portal"));
 if(portal.boolValue)EditorGUILayout.PropertyField(serializedObject.FindProperty("destination"),new GUIContent("Destino (posición de los ojos)"));
 else{var action=serializedObject.FindProperty("action");int current=System.Array.IndexOf(ids,action.stringValue);int next=EditorGUILayout.Popup("Acción",Mathf.Max(0,current),names);if(next!=current)action.stringValue=ids[next];}
 EditorGUILayout.HelpBox("Mueve el objeto con Transform. Expande su hijo Texto para cambiar la etiqueta. El collider recibe la mirada.",MessageType.Info);
 serializedObject.ApplyModifiedProperties();
 }
}
