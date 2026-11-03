using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using System.IO;

[InitializeOnLoad]
public static class AndinoAnimatorSetup {
 const string Folder="Assets/Andino/Animaciones";
 static AndinoAnimatorSetup(){EditorApplication.delayCall+=TryAutomatic;}
 static void TryAutomatic(){
 if(EditorApplication.isPlayingOrWillChangePlaymode)return;
 var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
 if(scene.path!="Assets/Scenes/Casino_VR.unity"||scene.isDirty)return;
 var world=Object.FindFirstObjectByType<AndinoWorld>();
 if(world&&world.layoutReady&&!world.fichasAnimator&&!AssetDatabase.LoadAssetAtPath<AnimatorController>(Folder+"/Fichas.controller"))Install();
 }
 [MenuItem("Banco Andino/Configurar Animator de fichas")]
 public static void Install(){
 if(EditorApplication.isPlayingOrWillChangePlaymode){EditorUtility.DisplayDialog("Detén Play","Detén Play antes de configurar Animator.","Aceptar");return;}
 var world=Object.FindFirstObjectByType<AndinoWorld>();
 if(!world||!world.layoutReady||world.gameObject.scene.path!="Assets/Scenes/Casino_VR.unity"){
 EditorUtility.DisplayDialog("Abre Casino_VR","Se necesita la escena editable Casino_VR abierta.","Aceptar");return;}
 if(world.fichasAnimator){Selection.activeObject=world.fichasAnimator.runtimeAnimatorController;return;}
 if(world.animated==null||world.animated.Length!=8){Debug.LogError("Se esperaban ocho fichas.");return;}
 foreach(var chip in world.animated)if(!chip){Debug.LogError("Falta una ficha asignada.");return;}
 if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())return;
 Directory.CreateDirectory("Andino_Backups");
 File.Copy(world.gameObject.scene.path,"Andino_Backups/Casino_VR_antes_Animator.unity",true);
 Directory.CreateDirectory(Folder);AssetDatabase.Refresh();
 var group=new GameObject("Fichas_Animator");group.transform.SetParent(world.animated[0].parent,false);
 var idle=new AnimationClip{name="Fichas_en_reposo",frameRate=30};
 var move=new AnimationClip{name="Repartir_banca_y_jugador",frameRate=30};
 for(int i=0;i<world.animated.Length;i++){
 var chip=world.animated[i];chip.name="Ficha_"+(i+1).ToString("00");chip.SetParent(group.transform,true);
 Vector3 origin=chip.localPosition;float endX=origin.x+(i%2==0?-1f:1f);
 for(int axis=0;axis<3;axis++){
 string property="m_LocalPosition."+"xyz"[axis];
 var binding=EditorCurveBinding.FloatCurve(chip.name,typeof(Transform),property);
 AnimationUtility.SetEditorCurve(idle,binding,AnimationCurve.Constant(0,1,origin[axis]));
 var curve=axis==0?AnimationCurve.EaseInOut(0,origin.x,1.2f,endX):AnimationCurve.Constant(0,1.2f,origin[axis]);
 AnimationUtility.SetEditorCurve(move,binding,curve);
 }
 }
 AssetDatabase.CreateAsset(idle,AssetDatabase.GenerateUniqueAssetPath(Folder+"/Fichas_en_reposo.anim"));
 AssetDatabase.CreateAsset(move,AssetDatabase.GenerateUniqueAssetPath(Folder+"/Repartir_banca_y_jugador.anim"));
 var controller=AnimatorController.CreateAnimatorControllerAtPath(AssetDatabase.GenerateUniqueAssetPath(Folder+"/Fichas.controller"));
 controller.AddParameter("Repartidas",AnimatorControllerParameterType.Bool);
 var machine=controller.layers[0].stateMachine;
 var rest=machine.AddState("Reposo",new Vector3(250,70));rest.motion=idle;rest.writeDefaultValues=false;
 var deal=machine.AddState("Repartir",new Vector3(500,70));deal.motion=move;deal.writeDefaultValues=false;
 machine.defaultState=rest;
 var go=rest.AddTransition(deal);go.hasExitTime=false;go.duration=0;go.AddCondition(AnimatorConditionMode.If,0,"Repartidas");
 var back=deal.AddTransition(rest);back.hasExitTime=false;back.duration=.15f;back.AddCondition(AnimatorConditionMode.IfNot,0,"Repartidas");
 var animator=group.AddComponent<Animator>();animator.runtimeAnimatorController=controller;animator.applyRootMotion=false;animator.cullingMode=AnimatorCullingMode.AlwaysAnimate;
 world.fichasAnimator=animator;EditorUtility.SetDirty(world);
 AssetDatabase.SaveAssets();EditorSceneManager.MarkSceneDirty(world.gameObject.scene);EditorSceneManager.SaveScene(world.gameObject.scene);
 Selection.activeGameObject=group;
 Debug.Log("Animator de fichas instalado: Reposo → Repartir. Al reiniciar vuelve a Reposo. Edita los clips en Assets/Andino/Animaciones.");
 }
}
