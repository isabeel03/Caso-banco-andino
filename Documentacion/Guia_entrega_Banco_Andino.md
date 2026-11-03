# ANDES-SIM — Escenario 8
NRC 30188 · Casa de cambio y casino · Unity 6000.1.17f1 / Cardboard

## Estado verificable
- Scripts compilados contra las bibliotecas locales de Unity.
- Pruebas aisladas de lógica aprobadas: agregación por origen, separación de orígenes, secuencia, ataque, ROS, recepción de alerta, bloqueo y reinicio.
- Escena editable y Animator presentes en el proyecto.
- La última ampliación visual se aplica cuando Unity recompila con Casino_VR abierta y sin cambios pendientes. Alternativa: Banco Andino > Completar contenido y configurar Android.
- Pendientes: revisión visual de esta ampliación, APK y pruebas reales, capturas, video, publicación del repositorio y aportes de ambos integrantes.
- Bloqueo comprobado: Unity solo tiene MacStandaloneSupport instalado; falta Android Build Support.

## Recorrido de demostración
Ataque: portal de entrada → libro de operaciones → conversión → portal al casino → compra de fichas → apuesta doble → canje y conclusión.
Defensa: reiniciar en Defensa → libro → panel ROS → verificar fondos retenidos y alerta recibida por casino → intentar convertir y pasar al casino (debe impedirse).
No se realizan operaciones financieras reales.

## Video de 2–3 minutos (grabar desde Android real)
0:00–0:15: título, NRC y objetivo educativo.
0:15–0:45: entrada, operaciones del mismo origen y conversión en ataque.
0:45–1:20: portal, fichas, Animator y canje; explicar que el dinero sigue siendo ilícito.
1:20–2:00: reiniciar en defensa, revisar libro y activar ROS; mostrar retención y alerta compartida.
2:00–2:30: comparar resultados, explicar vulnerabilidad y mostrar créditos.
No sustituir la grabación real del celular por una captura del editor.

## Exposición de 10 minutos
0–1: objetivo y contexto de Bangladesh, adaptación ficticia.
1–4: recorrido de ataque.
4–6: recorrido de defensa.
6–7: configuración editable y Animator.
7–9: análisis: fraccionamiento, información aislada, controles y limitaciones.
9–10: hipótesis del cómplice y conclusión.
Hipótesis de discusión: Renzo Aliaga tuvo contacto técnico y acceso remoto; su actualización aprobada y documentada es evidencia en contra de atribuirle complicidad. No hay prueba concluyente. La solución ficticia corresponde al docente; no presentar una acusación como hecho.

## Pruebas antes de entregar
1. Android real con giroscopio: orientación, mirada, retícula y toque.
2. Al menos cinco objetos activables y dos portales utilizables.
3. Conversión y canje no duplicables mediante pulsaciones repetidas.
4. Ataque completo: las evidencias se acumulan y aparece la conclusión.
5. Defensa: dinero retenido antes del casino; contador de canje en cero.
6. Reinicio: limpia dinero, evidencias, alertas y vuelve las fichas a Reposo.
7. Sin errores, texto legible, sonido moderado y sin desplazamiento continuo.
8. Revisar Animator: parámetro Repartidas, estados Reposo y Repartir.
9. Cambiar temporalmente orígenes en Configuracion: no sumar clientes distintos como uno solo. Restaurar valores de entrega.
Registrar modelo de celular, versión Android, fecha, resultado y cualquier incidencia. Estas pruebas no se han declarado realizadas.

## APK y GitHub
Instalar en Unity Hub, para 6000.1.17f1: Android Build Support, Android SDK & NDK Tools y OpenJDK.
Luego Banco Andino > Generar APK. Salida esperada: Builds/AndesSim.apk; no existe hasta que termine una compilación correcta.
Repositorio: incluir Assets, Packages, ProjectSettings y documentación. Excluir Library, Temp, Logs, Obj y archivos locales.
Cada integrante debe realizar y registrar sus propios cambios con su identidad. No atribuirles commits generados por IA.

## Roles acordados
Integrante 1: casa de cambio, conversión, evidencias y detección inicial.
Integrante 2: casino, fichas, Animator y representación de la apuesta doble.
Ambos: investigación, integración, pruebas, ficha, video y exposición.
Los nombres y la evidencia de contribuciones deben completarlos los autores reales.
