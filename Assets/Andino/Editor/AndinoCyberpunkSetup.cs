using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;
[InitializeOnLoad]
public static class AndinoCyberpunkSetup {
 static AndinoCyberpunkSetup(){EditorApplication.delayCall+=Auto;}
 static void Auto(){if(EditorApplication.isPlayingOrWillChangePlaymode)return;var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();if(scene.path=="Assets/Scenes/Casino_VR.unity"&&!scene.isDirty&&!GameObject.Find("Estilo_Ciberpunk"))Apply();}
 static Material CreateMaterial(string name,Color color,bool hologram=false){var path="Assets/Andino/Materiales/"+name+".mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(path);if(mat)return mat;
 var shader=Shader.Find(hologram?"Andino/HologramaMovil":"Unlit/Color");if(!shader)throw new System.Exception("Espera a que se importe el shader HologramaMovil.");mat=new Material(shader){color=color};AssetDatabase.CreateAsset(mat,path);return mat;}
 static GameObject Box(Transform parent,string name,Vector3 pos,Vector3 size,Material material){var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=name;o.transform.SetParent(parent,false);o.transform.position=pos;o.transform.localScale=size;o.GetComponent<Renderer>().sharedMaterial=material;o.GetComponent<Renderer>().shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;Object.DestroyImmediate(o.GetComponent<Collider>());return o;}
 static void Frame(Transform root,Vector3 center,Vector2 size,Material material,List<Renderer> accents){
 foreach(float sign in new[]{-1f,1f}){accents.Add(Box(root,"Marco LED",center+Vector3.right*size.x*.5f*sign,new Vector3(.025f,size.y,.025f),material).GetComponent<Renderer>());accents.Add(Box(root,"Marco LED",center+Vector3.up*size.y*.5f*sign,new Vector3(size.x,.025f,.025f),material).GetComponent<Renderer>());}}
 [MenuItem("Banco Andino/Aplicar estilo ciberpunk")]
 public static void Apply(){
 if(EditorApplication.isPlayingOrWillChangePlaymode)return;
 var w=Object.FindFirstObjectByType<AndinoWorld>();if(!w||!w.layoutReady||w.gameObject.scene.path!="Assets/Scenes/Casino_VR.unity")return;
 if(GameObject.Find("Estilo_Ciberpunk"))return;
 if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())return;
 Directory.CreateDirectory("Andino_Backups");File.Copy(w.gameObject.scene.path,"Andino_Backups/Casino_VR_antes_cyberpunk.unity",true);
 var root=new GameObject("Estilo_Ciberpunk").transform;root.SetParent(w.transform,false);
 var cyan=CreateMaterial("Neon_Cian",new Color(.02f,.8f,1));var magenta=CreateMaterial("Neon_Magenta",new Color(.8f,.05f,.5f));
 var holo=CreateMaterial("Holograma_Cian",new Color(.03f,.8f,1,.24f),true);
 var accents=new List<Renderer>();
 foreach(var item in Object.FindObjectsByType<AndinoInteractable>(FindObjectsSortMode.None)){
 if(!item.transform.IsChildOf(w.transform))continue;
 var r=item.GetComponent<Renderer>();if(!r)continue;
 var bounds=r.bounds;
 Frame(root,new Vector3(bounds.center.x,bounds.center.y,bounds.min.z-.02f),new Vector2(bounds.size.x,bounds.size.y),cyan,accents);
 // Keep text on opaque panels for readability; add limited translucent projections above the tables.
 }
 foreach(float x in new[]{0f,20f}){
 foreach(float side in new[]{-1f,1f}){
 accents.Add(Box(root,"Guía de suelo",new Vector3(x+4.5f*side,.015f,1),new Vector3(.035f,.025f,9),cyan).GetComponent<Renderer>());
 Box(root,"Línea de pared",new Vector3(x+4.85f*side,2.8f,1),new Vector3(.025f,.035f,9),magenta);
 }
 Frame(root,new Vector3(x,3.9f,5.65f),new Vector2(8.8f,.85f),magenta,accents);
 }
 // Four small translucent objects instead of a wall of transparency.
 for(int i=0;i<4;i++)accents.Add(Box(root,"Proyección holográfica",new Vector3(18.4f+i*1.05f,1.25f,3.5f),new Vector3(.55f,.45f,.025f),holo).GetComponent<Renderer>());
 Frame(root,new Vector3(0,1.1f,2.52f),new Vector2(7,.15f),cyan,accents);
 Frame(root,new Vector3(20,.85f,1.99f),new Vector2(6,.12f),magenta,accents);
 var label=new GameObject("Estado AML holográfico");label.transform.SetParent(root,false);label.transform.position=new Vector3(0,3.15f,5.3f);
 var text=label.AddComponent<TextMesh>();text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.GetComponent<Renderer>().sharedMaterial=text.font.material;text.fontSize=96;text.characterSize=.006f;text.anchor=TextAnchor.MiddleCenter;text.alignment=TextAlignment.Center;text.color=Color.cyan;text.text="ANDINO EXCHANGE / SIMULACIÓN";
 var environment=root.gameObject.AddComponent<CyberpunkEnvironment>();environment.accents=accents.ToArray();environment.signal=text;
 RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Flat;RenderSettings.ambientLight=new Color(.3f,.33f,.4f);RenderSettings.reflectionIntensity=0;
 EditorSceneManager.MarkSceneDirty(w.gameObject.scene);EditorSceneManager.SaveScene(w.gameObject.scene);AssetDatabase.SaveAssets();Selection.activeGameObject=root.gameObject;
 Debug.Log("Estilo ciberpunk aplicado: materiales editables, cuatro hologramas, marcos LED y estado AML. Validar rendimiento en Android.");
 }
}
