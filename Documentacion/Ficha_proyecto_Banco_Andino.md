# Ficha del proyecto — Banco Andino
**NRC:** 30188 · **Escenario:** 8, Casa de cambio y casino · **Modalidad:** VR Cardboard
**Integrantes:** completar nombres reales de los dos autores antes de entregar.
**Estado:** borrador de contenido; agregar capturas y exportar con un máximo de cuatro páginas.

## 1. Escenario y objetivo
ANDES-SIM es una simulación educativa del flujo de fondos de la casa de cambio al casino. Una sola escena Casino_VR contiene ambos ambientes, conectados mediante portal y teletransporte. El jugador examina evidencias y compara un modo ataque con un modo defensa.
La lectura base es el Expediente Bangladesh y el Caso Banco Andino entregados por el docente. El escenario se sitúa del 5 al 13 de febrero de 2016. El material menciona US$58 millones retirados el 9 de febrero y unos US$30 millones distribuidos en efectivo; no deben sumarse como partidas independientes. Las entidades del simulador son ficticias.
La práctica utiliza operaciones de USD 300, 300 y 400 y una conversión didáctica de 50 pesos por dólar. No son cifras ni cotizaciones históricas. Se muestran separadas del contexto del expediente.
**Captura pendiente:** casa de cambio y libro de operaciones en la app.

## 2. Vulnerabilidad y contramedida
Las operaciones pequeñas del mismo origen pueden ocultar un monto agregado inusual si se revisan aisladamente. El riesgo se amplifica cuando las entidades no intercambian alertas. El expediente describe además la exclusión de los casinos de las obligaciones antilavado de ese periodo; se presenta como contexto histórico, no como regla vigente.
StructuringDetector agrupa por origen y compara el total con un umbral ficticio configurable. El panel ROS registra el reporte y AmlAlert simula su recepción por el casino. En defensa, se congelan los fondos en la casa de cambio antes de continuar.
Emitir un ROS y retener fondos son dos acciones distintas que esta simulación enlaza como decisión pedagógica. No se envían reportes ni se conectan servicios financieros reales.
En ataque se representa el cambio de dinero a fichas, la distribución a banca y jugador y el canje. El resultado parece legítimo documentalmente, pero el origen ilícito permanece. La demostración no reproduce probabilidades, comisiones ni reglas completas de bacará y no implica que las apuestas garanticen conservar el dinero.
**Captura pendiente:** alerta y contador bloqueado en modo defensa.

## 3. Implementación y conceptos
Unity 6, Cardboard XR Plugin, escena editable con primitivas, materiales y TextMesh. Animator controla los clips Reposo y Repartir mediante el parámetro Repartidas.
GameManager administra estado, modo, evidencias y defensas. ScenarioData mantiene datos configurables. Se utilizan RegisterEvidence, ApplyDefense y EndScenario. EvidenceItem registra el libro y DefenseControl activa el panel. ChipAnimator enlaza estado y Animator; MoneyCounter presenta los importes.
Interacciones por retícula y toque en Android. Flechas y Espacio sirven solo para pruebas en el editor. El desplazamiento entre ambientes es por teletransporte.
Conceptos: agregación de operaciones, detección de anomalías, trazabilidad, evidencia, respuesta coordinada y separación entre datos históricos y simulados.
**Captura pendiente:** casino con fichas y estados del Animator.

## 4. Roles, validación y créditos
Reparto previsto: integrante 1, casa de cambio y detección inicial; integrante 2, casino y fichas; ambos, investigación, integración, pruebas y entrega. Completar nombres y aportes reales; no confundir el reparto previsto con evidencia de trabajo realizado.
Validación realizada: compilación C# contra bibliotecas de Unity y pruebas aisladas de lógica de agregación, secuencia, ataque, defensa, alerta compartida y reinicio. Pendientes: validación visual de la última ampliación, compilación APK, prueba Android real y video desde el dispositivo. No se garantiza una nota sin esas comprobaciones.
Créditos: geometría y materiales creados con herramientas de Unity; clips creados con Animation/Animator mediante configuración asistida; base Google Cardboard, Google LLC, Apache License 2.0. OpenAI Codex asistió en programación, configuración y documentación. No se usó Blender ni se descargaron modelos de terceros para este prototipo. Conservar los avisos de licencia incluidos en Cardboard.
Fuentes de contenido: Semana10_VR_AR_CASO_BANCO_ANDINO.html y Semana10_VR_AR_Expediente_Bangladesh.html, material proporcionado por el docente. Las cifras históricas se atribuyen a ese material; no se declara una nueva investigación independiente de fuentes externas.
