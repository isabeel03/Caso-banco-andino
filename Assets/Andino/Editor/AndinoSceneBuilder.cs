using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
public class AndinoSceneBuilder {
 Transform transform,player; GameObject gameObject; Camera cam; TextMesh status;
 Transform[] animated=new Transform[8]; Vector3[] origins=new Vector3[8];
 [MenuItem("Banco Andino/Preparar escena editable")]
 public static void Prepare(){
 if(EditorApplication.isPlayingOrWillChangePlaymode){EditorUtility.DisplayDialog("Detén Play","Primero detén Play para preparar la escena.","Entendido");return;}
 if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())return;
 var scene=EditorSceneManager.OpenScene("Assets/Scenes/Casino_VR.unity");
 var world=Object.FindFirstObjectByType<AndinoWorld>();
 if(!world){EditorUtility.DisplayDialog("Escena no disponible","No se encontró ANDINO_Prototipo.","Aceptar");return;}
 if(world.layoutReady){Selection.activeGameObject=world.gameObject;EditorUtility.DisplayDialog("Ya preparada","Los objetos ya son editables. Expande ANDINO_Prototipo en Hierarchy.","Aceptar");return;}
 System.IO.Directory.CreateDirectory("Assets/Andino/Materiales");AssetDatabase.Refresh();
 new AndinoSceneBuilder().Generate(world);
 EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
 Selection.activeGameObject=world.gameObject;
 EditorUtility.DisplayDialog("Escena preparada","Expande ANDINO_Prototipo: CasaDeCambio y Casino. Puedes mover objetos, editar textos y cambiar materiales sin Play.","Aceptar");
 }
 Material Mat(Color c){var m=new Material(Shader.Find("Standard"));m.color=c;m.SetFloat("_Glossiness",0.1f);AssetDatabase.CreateAsset(m,AssetDatabase.GenerateUniqueAssetPath("Assets/Andino/Materiales/Material.mat"));return m;}
 GameObject Box(string n,Vector3 pos,Vector3 size,Color c){var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=n;o.transform.SetParent(transform);o.transform.position=pos;o.transform.localScale=size;o.GetComponent<Renderer>().sharedMaterial=Mat(c);return o;}
 TextMesh Text(string value,Vector3 pos,float size=0.09f){var o=new GameObject("Texto");o.transform.SetParent(transform);o.transform.position=pos;var t=o.AddComponent<TextMesh>();t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");t.GetComponent<Renderer>().sharedMaterial=t.font.material;t.text=value;t.fontSize=96;t.characterSize=size*.16f;t.anchor=TextAnchor.MiddleCenter;t.alignment=TextAlignment.Center;t.color=Color.white;return t;}
 void Button(string label,string action,Vector3 pos,Color color,bool portal=false,Vector3 dest=default){var o=Box(label,pos,new Vector3(1.7f,.55f,.15f),color);o.layer=8;var a=o.AddComponent<AndinoInteractable>();a.action=action;a.portal=portal;a.destination=dest;var labelText=Text(label,pos+Vector3.back*.09f,.065f);
 var bounds=labelText.GetComponent<Renderer>().bounds;
 if(bounds.size.x>1.45f)labelText.transform.localScale*=1.45f/bounds.size.x;labelText.transform.SetParent(o.transform,true);}
 void Room(float x,string title,Color accent){Box("Suelo",new Vector3(x,-.12f,1),new Vector3(10,.2f,10),new Color(.08f,.1f,.15f));Box("Pared fondo",new Vector3(x,2.2f,5.9f),new Vector3(10,4.5f,.2f),new Color(.12f,.16f,.22f));Box("Pared izquierda",new Vector3(x-5,2.2f,1),new Vector3(.2f,4.5f,10),accent);Box("Pared derecha",new Vector3(x+5,2.2f,1),new Vector3(.2f,4.5f,10),accent);Text(title,new Vector3(x,4.1f,5.7f),.16f);var l=new GameObject("Luz ambiente");l.transform.SetParent(transform);l.transform.position=new Vector3(x,3,1);var light=l.AddComponent<Light>();light.type=LightType.Point;light.range=15;light.intensity=.8f;}

 void Generate(AndinoWorld world){
 gameObject=world.gameObject;transform=world.transform;cam=Camera.main;player=GameObject.Find("Player").transform;
 foreach(var n in new[]{"Treasure","CubeRoom","GraphicsAPIText"}){var o=GameObject.Find(n);if(o)o.SetActive(false);}
 // Remove the demo room renderer (including prefab instances) but retain the VR player.
 foreach(var r in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))if(!r.transform.IsChildOf(player)&&!r.transform.IsChildOf(transform))r.gameObject.SetActive(false);
 var g=gameObject.GetComponent<GameManager>();if(!g)g=gameObject.AddComponent<GameManager>();
 g.data=AssetDatabase.LoadAssetAtPath<ScenarioData>("Assets/Andino/Configuracion.asset");
 if(!g.data){g.data=ScriptableObject.CreateInstance<ScenarioData>();AssetDatabase.CreateAsset(g.data,"Assets/Andino/Configuracion.asset");}
 foreach(var oldLight in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))oldLight.enabled=false;
 RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Flat;
 RenderSettings.ambientLight=new Color(.48f,.48f,.48f);
 RenderSettings.reflectionIntensity=0;
 LightmapSettings.lightmaps=new LightmapData[0];
 cam.allowHDR=false;
 Room(0,"BANCO ANDINO / CASA DE CAMBIO",new Color(.04f,.25f,.28f));Room(20,"CASINO / SIMULACIÓN EDUCATIVA",new Color(.25f,.08f,.13f));
 Box("Mostrador",new Vector3(0,.55f,3),new Vector3(7,1.1f,.8f),new Color(.28f,.18f,.1f));
 Box("Computadora",new Vector3(2,1.5f,3),new Vector3(.9f,.7f,.12f),Color.black);
 Button("ATAQUE / reiniciar","attack",new Vector3(-1,2.65f,4),new Color(.55f,.13f,.1f));Button("DEFENSA / reiniciar","defense",new Vector3(1,2.65f,4),new Color(.05f,.4f,.25f));
 Button("1. Libro de operaciones","book",new Vector3(-2,1.6f,2.5f),new Color(.15f,.3f,.45f));Button("Panel ROS / alerta","ros",new Vector3(0,1.6f,2.5f),new Color(.4f,.2f,.1f));Button("2. Convertir USD a pesos","convert",new Vector3(2,1.6f,2.5f),new Color(.1f,.4f,.3f));
 Button("PORTAL AL CASINO","",new Vector3(3.3f,2.5f,4.5f),Color.blue,true,new Vector3(20,1.6f,0));
 // The ledger details appear in the status panel when inspected.
 Text("PRÁCTICA FICTICIA: USD 1.000 / 50 pesos por USD",new Vector3(0,3.65f,5.5f),.065f);
 Box("Mesa de bacará",new Vector3(20,.7f,3),new Vector3(6,.3f,2),new Color(.03f,.3f,.17f));
 Text("BANCA                 JUGADOR",new Vector3(20,1.05f,3),.09f);
 Button("3. Cambiar por fichas","chips",new Vector3(18,1.7f,2.5f),new Color(.1f,.3f,.45f));Button("4. Observar apuesta doble","bet",new Vector3(20,1.7f,2.5f),new Color(.4f,.2f,.1f));Button("5. Canjear / resultado","cash",new Vector3(22,1.7f,2.5f),new Color(.3f,.1f,.3f));
 Button("VOLVER A CASA DE CAMBIO","",new Vector3(17,2.6f,4),Color.blue,true,new Vector3(0,1.6f,0));Button("Ver conclusión","result",new Vector3(23,2.6f,4),new Color(.15f,.35f,.3f));
 Text("5–13 febrero 2016 / Adaptación ficticia\nLos controles conectan entidades y detectan fraccionamiento.\nCréditos: geometría y audio propios; código asistido por IA.\nBase Google Cardboard: Google LLC / Apache 2.0.",new Vector3(20,3,5.5f),.065f);
 for(int i=0;i<8;i++){var o=GameObject.CreatePrimitive(PrimitiveType.Cylinder);o.name="Ficha animada";o.transform.SetParent(transform);o.transform.localScale=new Vector3(.22f,.035f,.22f);o.transform.position=new Vector3(19.7f+i%2*.4f,.95f+i/2*.08f,3);o.GetComponent<Renderer>().sharedMaterial=Mat(i%2==0?Color.red:Color.blue);animated[i]=o.transform;origins[i]=o.transform.position;}
 var hud=new GameObject("Estado y resultado");hud.transform.SetParent(cam.transform,false);hud.transform.localPosition=new Vector3(0,-.60f,2.0f);status=hud.AddComponent<TextMesh>();status.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");status.GetComponent<Renderer>().sharedMaterial=status.font.material;status.fontSize=80;status.characterSize=.009f;status.anchor=TextAnchor.MiddleCenter;status.alignment=TextAlignment.Center;
 var background=Box("Fondo del panel de estado",Vector3.zero,Vector3.one,new Color(.025f,.035f,.055f));
 Object.DestroyImmediate(background.GetComponent<Collider>());
 background.layer=2;
 background.transform.SetParent(cam.transform,false);
 background.transform.localPosition=new Vector3(0,-.60f,2.02f);
 background.transform.localScale=new Vector3(2.15f,.52f,.01f);
 var hudMaterial=new Material(Shader.Find("Unlit/Color")){color=new Color(.025f,.035f,.055f)};
 AssetDatabase.CreateAsset(hudMaterial,AssetDatabase.GenerateUniqueAssetPath("Assets/Andino/Materiales/Panel.mat"));
 background.GetComponent<Renderer>().sharedMaterial=hudMaterial;

 var house=new GameObject("CasaDeCambio").transform;house.SetParent(transform,false);
 var casino=new GameObject("Casino").transform;casino.SetParent(transform,false);
 foreach(var child in transform.Cast<Transform>().ToArray()){
 if(child==house||child==casino)continue;
 child.SetParent(child.position.x>10?casino:house,true);
 }
 world.player=player;world.cam=cam;world.status=status;world.animated=animated;world.origins=origins;world.layoutReady=true;
 status.text="Pulsa Play para comenzar. Flechas: mirar | Espacio: activar.";
 EditorUtility.SetDirty(world);
 }
}
