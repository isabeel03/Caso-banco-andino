using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build;
using UnityEngine.Rendering;
using System.IO;
[InitializeOnLoad]
public static class AndinoDeliverySetup {
 static AndinoDeliverySetup(){EditorApplication.delayCall+=Auto;}
 static void Auto(){if(EditorApplication.isPlayingOrWillChangePlaymode)return;var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();if(scene.path=="Assets/Scenes/Casino_VR.unity"&&!scene.isDirty&&!GameObject.Find("Contenido_Entrega"))Complete();}
 static Material Material(Color color){var m=new Material(Shader.Find("Standard")){color=color};m.SetFloat("_Glossiness",.1f);AssetDatabase.CreateAsset(m,AssetDatabase.GenerateUniqueAssetPath("Assets/Andino/Materiales/Entrega.mat"));return m;}
 static GameObject Box(Transform parent,string name,Vector3 pos,Vector3 scale,Color color){var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=name;o.transform.SetParent(parent,false);o.transform.position=pos;o.transform.localScale=scale;o.GetComponent<Renderer>().sharedMaterial=Material(color);return o;}
 static void Label(Transform parent,string text,Vector3 pos,float size){var o=new GameObject("Texto");o.transform.position=pos;o.transform.SetParent(parent,true);var t=o.AddComponent<TextMesh>();t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");t.GetComponent<Renderer>().sharedMaterial=t.font.material;t.text=text;t.fontSize=96;t.characterSize=size;t.anchor=TextAnchor.MiddleCenter;t.alignment=TextAlignment.Center;}
 static void Button(Transform root,string label,string action,Vector3 pos,bool portal=false,Vector3 dest=default){var o=Box(root,label,pos,new Vector3(1.8f,.45f,.12f),new Color(.08f,.25f,.28f));o.layer=8;var a=o.AddComponent<AndinoInteractable>();a.action=action;a.portal=portal;a.destination=dest;Label(o.transform,label,pos+Vector3.back*.07f,.008f);}
 [MenuItem("Banco Andino/Completar contenido y configurar Android")]
 public static void Complete(){
 if(EditorApplication.isPlayingOrWillChangePlaymode)return;
 var world=Object.FindFirstObjectByType<AndinoWorld>();if(!world||!world.layoutReady)return;
 if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())return;
 if(!GameObject.Find("Contenido_Entrega")){
 var root=new GameObject("Contenido_Entrega").transform;root.SetParent(world.transform,false);
 Button(root,"Contexto histórico","history",new Vector3(-3,2.9f,4.4f));
 Button(root,"Análisis y defensas","analysis",new Vector3(-3,2.35f,4.4f));
 Button(root,"Créditos / simulación","credits",new Vector3(3.3f,3.15f,4.5f));
 // Entry portal and physical door frame; all objects remain editable.
 Box(root,"Portal_Entrada_izquierda",new Vector3(-1.2f,1.5f,-4),new Vector3(.16f,3,.2f),Color.cyan);
 Box(root,"Portal_Entrada_derecha",new Vector3(1.2f,1.5f,-4),new Vector3(.16f,3,.2f),Color.cyan);
 Box(root,"Portal_Entrada_dintel",new Vector3(0,3,-4),new Vector3(2.55f,.16f,.2f),Color.cyan);
 Button(root,"ENTRAR AL CASO","",new Vector3(0,1.6f,-4),true,new Vector3(0,1.6f,0));
 Box(root,"Suelo de acceso",new Vector3(0,-.12f,-6),new Vector3(6,.2f,5),new Color(.08f,.1f,.15f));
 Label(root,"ANDES-SIM / ESCENARIO 8\nSimulación defensiva. Datos ficticios.\nMira el portal y activa para entrar.",new Vector3(0,2.5f,-4.1f),.01f);
 // Furniture detail, no downloaded third-party models.
 for(int x=-3;x<=3;x+=2){Box(root,"Pata mostrador",new Vector3(x,.3f,3),new Vector3(.15f,.6f,.6f),new Color(.15f,.12f,.1f));}
 var book=Box(root,"Libro_Operaciones",new Vector3(-2,1.2f,3),new Vector3(.7f,.08f,.45f),new Color(.7f,.65f,.45f));
 book.layer=8;book.AddComponent<EvidenceItem>();book.AddComponent<AndinoInteractable>().action="book";
 for(int i=0;i<4;i++)Box(root,"Billetes ficticios",new Vector3(1+i*.15f,1.16f,3),new Vector3(.35f,.03f,.2f),new Color(.3f,.5f,.3f));
 for(int i=0;i<4;i++){float x=18.2f+(i%2)*3.6f;float z=2.4f+(i/2)*1.2f;Box(root,"Pata mesa bacará",new Vector3(x,.3f,z),new Vector3(.15f,.6f,.15f),new Color(.2f,.12f,.08f));}
 Label(root,"BANCA",new Vector3(19,1.08f,3.9f),.012f);Label(root,"JUGADOR",new Vector3(21,1.08f,3.9f),.012f);
 Button(root,"Por qué es sospechoso","analysis",new Vector3(20,2.6f,4));
 }
 PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android,ScriptingImplementation.IL2CPP);
 PlayerSettings.Android.targetArchitectures=AndroidArchitecture.ARM64;PlayerSettings.Android.minSdkVersion=AndroidSdkVersions.AndroidApiLevel26;
 PlayerSettings.defaultInterfaceOrientation=UIOrientation.LandscapeLeft;
 PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android,false);PlayerSettings.SetGraphicsAPIs(BuildTarget.Android,new[]{GraphicsDeviceType.OpenGLES3});
 PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android,"com.andinosim.casino");PlayerSettings.productName="ANDES-SIM: Casa de cambio y casino";
 EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene("Assets/Scenes/Casino_VR.unity",true)};
 EditorSceneManager.MarkSceneDirty(world.gameObject.scene);EditorSceneManager.SaveScene(world.gameObject.scene);AssetDatabase.SaveAssets();
 Debug.Log("Contenido final añadido y configuración Android preparada.");
 }
 [MenuItem("Banco Andino/Generar APK")]
 public static void BuildApk(){if(EditorApplication.isPlayingOrWillChangePlaymode)return;
 if(!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android,BuildTarget.Android)){EditorUtility.DisplayDialog("Falta módulo Android","Instala Android Build Support, SDK/NDK y OpenJDK en Unity Hub para 6000.1.17f1.","Aceptar");return;}
 Complete();Directory.CreateDirectory("Builds");var result=BuildPipeline.BuildPlayer(new[]{"Assets/Scenes/Casino_VR.unity"},"Builds/AndesSim.apk",BuildTarget.Android,BuildOptions.None);Debug.Log("Resultado APK: "+result.summary.result);}
}
