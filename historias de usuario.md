HI-01 Registro e Inicializacion 
Como analista de NestGuard GT, quiero registrar un nuevo incidente de red ingresado el titulo, descripción, severidad y especialidad, para digitalizar el reporte mensual de mas de 80 incidentes. 
Criterios de aceptación 
	El incidente debe crearse obligatoriamente con el estado Registrado
	Debe guardarse de forma automática 
HI-02  Transicion de estados 
Como desarrollador quiero restringir que el ciclo de vida del incidente o avance, para evitar saltos de estados inválidos o no autorizados 
Criterios de aceptación 
	Las transiciones permitidas deben ser únicamente: Registrado Asignado En progreso Resuelto Cerrado.
HI-03 Cierre de incidentes 
Como supervisor quiero cambiar el estado de un incidente suelto a cerrado, para dar por concluido de soporte de ticket o congelar sus modificaciones 
Criterios de aceptación 
	Solo los incidentes en estado resuelto pueden pasar al estado cerrado 
	Un incidente no pude cambiar cuando esta en Cerrado 
HU-04: Límite Máximo de Carga de Trabajo de Técnicos
Como coordinador de soporte, quiero restringir que un mismo técnico tenga más de 3 incidentes activos simultáneamente, para balancear la carga operativa y evitar sobrecargados mientras otros tienen poco trabajo.
Criterios de Aceptación:
Se consideran incidentes activos aquellos que estén en estado Asignado o En progreso.

Si el técnico ya posee 3 tickets activos, la API debe denegar la asignación de un cuarto incidente.	

HU-05: Reasignación y Liberación Eficiente de Incidentes
Como técnico de soporte en campo, quiero liberar un incidente asignado o permitir que sea reasignado a otro técnico especialista en cualquier momento, para agilizar la transferencia del ticket cuando se requiera apoyo o un relevo de turno.
Criterios de Aceptación:
Al reasignar el incidente a un nuevo técnico, el contador de incidentes activos del técnico anterior debe disminuir en 1.
HU-06: Monitoreo y Escalación de Incidentes Críticos o Urgentes
Como administrador de operaciones de red, quiero que el sistema marque automáticamente como "Escalado" un incidente Crítico o Urgente si supera las 2 horas sin atención, para mitigar retrasos graves y alertar de inmediato a la gerencia
Criterios de Aceptación:
Aplica únicamente si el ticket se encuentra en estado Registrado por más de 2 horas.
Al cumplirse la condición, el campo boolean IsEscalated debe cambiar a verdadero de forma autónoma.

HU-07: Medición de Tiempos Máximos de Resolución por Severidad
Como analista de cumplimiento de SLA, quiero registrar la duración transcurrida entre el inicio y la solución de un incidente para comparar el tiempo real contra el tiempo máximo estipulado en los acuerdos de servicio.
Criterios de Aceptación:
El sistema debe calcular la diferencia de tiempo neta desde CreatedAt hasta el cambio de estado a Resuelto.



HU-08: Historial Cronológico de Cambios de Estado
Como Auditor de Operaciones de NetGuard GT, quiero consultar una bitácora detallada de todos los movimientos de estados de un incidente específico, para realizar un seguimiento forense de la atención de la falla y verificar qué técnico intervino en cada fase.
Criterios de Aceptación:
Cada transición debe inyectar una fila inmutable en la tabla de historial con el estado anterior, el estado nuevo, la fecha UTC y notas aclaratorias.

HU-9: Reporte Consolidado de Incidentes Mensuales
Como Gerente de Operaciones de NetGuard GT, quiero extraer un reporte que exponga la totalidad de incidentes reportados, agrupados por estado, severidad y técnico, para evaluar el rendimiento general de los 12 técnicos de la compañía y tomar decisiones informadas.
Criterios de Aceptación:
El API Rest debe proveer un endpoint de lectura estructurada para alimentar cuadros de mando.

HU-10: Trazabilidad por Sitio de Red (Cobertura Nacional)
Como Ingeniero de Infraestructura, quiero asociar cada incidente reportado a uno de los 45 sitios de red distribuidos en todo el país (antenas, nodos, puntos de presencia POP), para identificar geográficamente los puntos críticos con mayor recurrencia de fallas.
Criterios de Aceptación:
Al registrar o visualizar un incidente, este debe mostrar de forma mandatoria el identificador o nombre del sitio de red afectado.

