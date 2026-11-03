using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-1000)]
public class AndinoWorld : MonoBehaviour {
 public static AndinoWorld I;
 [HideInInspector] public bool layoutReady;
 [Header("Referencias de la escena")]
 public Transform player;
 public Camera cam;
 public TextMesh status;
 public Transform[] animated=new Transform[8];
 [HideInInspector] public Vector3[] origins=new Vector3[8];
 [Header("Animación de fichas") ]
 public Animator fichasAnimator;
 public ChipAnimator chipAnimator;public MoneyCounter moneyCounter;
 [Header("Prueba en computadora")]
 [Range(10,90)] public float velocidadMirada=45;
 AudioSource sound;AudioClip tone;float yaw,pitch;
 AndinoInteractable editorFocus;
 void Awake(){
 #if UNITY_EDITOR
 // Cardboard native APIs require a mobile XR loader. Use mouse preview in Editor.
 foreach(var behaviour in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None)){
 var type=behaviour.GetType().Name;
 if(type=="CardboardStartup" || type=="VrModeController" || type=="CardboardReticlePointer" || type=="TrackedPoseDriver"){
 behaviour.enabled=false;
 if(type=="CardboardReticlePointer"){var renderer=behaviour.GetComponent<Renderer>();if(renderer)renderer.enabled=false;}
 }
 }
 #endif
 }
 void Start(){
 I=this;chipAnimator=GetComponent<ChipAnimator>();moneyCounter=GetComponent<MoneyCounter>();if(chipAnimator)chipAnimator.animator=fichasAnimator;
 if(!layoutReady){Debug.LogError("Prepara la escena desde Banco Andino > Preparar escena editable, fuera de Play.");enabled=false;return;}
 #if UNITY_EDITOR
 var pointer=new GameObject("Puntero de prueba");pointer.transform.SetParent(cam.transform,false);pointer.transform.localPosition=new Vector3(0,0,1);
 var text=pointer.AddComponent<TextMesh>();text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.GetComponent<Renderer>().sharedMaterial=text.font.material;
 text.text="+";text.fontSize=96;text.characterSize=.0144f;text.anchor=TextAnchor.MiddleCenter;
 #endif
 sound=gameObject.AddComponent<AudioSource>();tone=AudioClip.Create("Confirmación sintetizada",4410,1,44100,false);
 float[] samples=new float[4410];for(int i=0;i<samples.Length;i++)samples[i]=Mathf.Sin(i*2*Mathf.PI*660/44100)*.08f*(1-i/(float)samples.Length);tone.SetData(samples,0);

 Move(new Vector3(0,1.6f,-6));GetComponent<GameManager>().Begin(SimMode.Ataque);
 }
 static string Wrap(string text,int columns){
 var result=new System.Text.StringBuilder();int length=0;
 foreach(var line in text.Split('\n')){if(result.Length>0)result.Append('\n');length=0;
 foreach(var word in line.Split(' ')){if(length+word.Length+1>columns){result.Append('\n');length=0;}if(length>0){result.Append(' ');length++;}result.Append(word);length+=word.Length;}}
 return result.ToString();
 }
 public void Beep(){sound.PlayOneShot(tone);}
 public void Move(Vector3 target){player.position+=target-cam.transform.position;}
 void Update(){if(!GameManager.I)return;var g=GameManager.I;status.text=moneyCounter.Summary(g)+"\nEvidencias: "+g.evidence.Count+" | "+(g.frozen?"RETENIDO":"SIMULACIÓN")+"\n"+Wrap(g.message,64);
 if(chipAnimator)chipAnimator.SetDealt(g.stage>=4);
 #if UNITY_EDITOR
 var mouse=Mouse.current;
 var keyboard=Keyboard.current;
 bool activate=false;
 if(mouse!=null){
 if(mouse.rightButton.isPressed){yaw+=mouse.delta.ReadValue().x*.12f;pitch-=mouse.delta.ReadValue().y*.12f;}
 activate=mouse.leftButton.wasPressedThisFrame;
 }
 if(keyboard!=null){
 float step=velocidadMirada*Time.unscaledDeltaTime;
 if(keyboard.leftArrowKey.isPressed)yaw-=step;
 if(keyboard.rightArrowKey.isPressed)yaw+=step;
 if(keyboard.upArrowKey.isPressed)pitch-=step;
 if(keyboard.downArrowKey.isPressed)pitch+=step;
 activate|=keyboard.spaceKey.wasPressedThisFrame;
 }
 pitch=Mathf.Clamp(pitch,-80,80);
 cam.transform.localRotation=Quaternion.Euler(pitch,yaw,0);
 AndinoInteractable item=null;
 if(Physics.Raycast(cam.transform.position,cam.transform.forward,out var hit,20))item=hit.collider.GetComponent<AndinoInteractable>();
 if(item!=editorFocus){if(editorFocus)editorFocus.OnPointerExit();editorFocus=item;if(editorFocus)editorFocus.OnPointerEnter();}
 if(activate&&item)item.OnPointerClick();

 #endif
 }
}
