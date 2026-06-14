1.
desarrolla un prototipo de api rest que permita gestionar incidentes de red, implementando las reglas de negocio, timepo maximo, no deben haber mas de 3 incidentes activos, los estados deben avanzar en; registrado, asignado en progreso, resulto, cerrado, incidentes criticos o uegentes, debe existir un historia, , debe ser con C#, docker, y una bd de sqlite  tambien se requiren pruebas unitarias


en base a estas historias de  usuarios dame los codigos necesarios para las api rest, debe ser con lenguaje c#,.net9,  usando docker, render, usando sqlite, HI-01 Registro e Inicializacion 
Como analista de NestGuard GT, quiero registrar un nuevo
incidente de red ingresado el titulo, descripción, severidad y especialidad,
para digitalizar el reporte mensual de mas de 80 incidentes. 
Criterios de aceptación 
               El incidente
debe crearse obligatoriamente con el estado Registrado
               Debe guardarse
de forma automática 
HI-02  Transicion de
estados 
Como desarrollador quiero restringir que el ciclo de vida
del incidente o avance, para evitar saltos de estados inválidos o no
autorizados 
Criterios de aceptación 
               Las
transiciones permitidas deben ser únicamente: Registrado Asignado En progreso Resuelto
Cerrado.
HI-03 Cierre de incidentes 
Como supervisor quiero cambiar el estado de un incidente
suelto a cerrado, para dar por concluido de soporte de ticket o congelar sus
modificaciones 
Criterios de aceptación 
               Solo los
incidentes en estado resuelto pueden pasar al estado cerrado 
               Un incidente
no pude cambiar cuando esta en Cerrado 
HU-04: Límite Máximo de Carga de Trabajo de Técnicos
Como coordinador de soporte, quiero restringir que un mismo
técnico tenga más de 3 incidentes activos simultáneamente, para balancear la
carga operativa y evitar sobrecargados mientras otros tienen poco trabajo.
Criterios de Aceptación:
Se consideran incidentes activos
aquellos que estén en estado Asignado o En progreso.
 
Si el técnico ya posee 3 tickets
activos, la API debe denegar la asignación de un cuarto incidente.          
 
HU-05: Reasignación y Liberación
Eficiente de Incidentes
Como técnico de soporte en campo,
quiero liberar un incidente asignado o permitir que sea reasignado a otro
técnico especialista en cualquier momento, para agilizar la transferencia del
ticket cuando se requiera apoyo o un relevo de turno.
Criterios de Aceptación:
Al reasignar el incidente a un
nuevo técnico, el contador de incidentes activos del técnico anterior debe
disminuir en 1.
HU-06: Monitoreo y Escalación de
Incidentes Críticos o Urgentes
Como administrador de operaciones
de red, quiero que el sistema marque automáticamente como "Escalado"
un incidente Crítico o Urgente si supera las 2 horas sin atención, para mitigar
retrasos graves y alertar de inmediato a la gerencia
Criterios de Aceptación:
Aplica únicamente si el ticket se
encuentra en estado Registrado por más de 2 horas.
Al cumplirse la condición, el
campo boolean IsEscalated debe cambiar a verdadero de forma autónoma.
 
HU-07: Medición de Tiempos
Máximos de Resolución por Severidad
Como analista de cumplimiento de
SLA, quiero registrar la duración transcurrida entre el inicio y la solución de
un incidente para comparar el tiempo real contra el tiempo máximo estipulado en
los acuerdos de servicio.
Criterios de Aceptación:
El sistema debe calcular la
diferencia de tiempo neta desde CreatedAt hasta el cambio de estado a Resuelto.
 
 
 
HU-08: Historial Cronológico de
Cambios de Estado
Como Auditor de Operaciones de
NetGuard GT, quiero consultar una bitácora detallada de todos los movimientos
de estados de un incidente específico, para realizar un seguimiento forense de
la atención de la falla y verificar qué técnico intervino en cada fase.
Criterios de Aceptación:
Cada transición debe inyectar una
fila inmutable en la tabla de historial con el estado anterior, el estado
nuevo, la fecha UTC y notas aclaratorias.
 
HU-9: Reporte Consolidado de
Incidentes Mensuales
Como Gerente de Operaciones de
NetGuard GT, quiero extraer un reporte que exponga la totalidad de incidentes
reportados, agrupados por estado, severidad y técnico, para evaluar el
rendimiento general de los 12 técnicos de la compañía y tomar decisiones
informadas.
Criterios de Aceptación:
El API Rest debe proveer un
endpoint de lectura estructurada para alimentar cuadros de mando.
 
HU-10: Trazabilidad por Sitio de
Red (Cobertura Nacional)
Como Ingeniero de
Infraestructura, quiero asociar cada incidente reportado a uno de los 45 sitios
de red distribuidos en todo el país (antenas, nodos, puntos de presencia POP),
para identificar geográficamente los puntos críticos con mayor recurrencia de
fallas.
Criterios de Aceptación:
Al registrar o visualizar un
incidente, este debe mostrar de forma mandatoria el identificador o nombre del
sitio de red afectado.
 



corrige los siguientes errores, 
C:\finalAnalisis\FinalAnalisiss\Data\DbContext.cs(1,17): error CS0234: El tipo o el nombre del espacio de nombres 'EntityFrameworkCore' no existe en el espacio de nombres 'Microsoft' (¿falta alguna referencia de ensamblado?)
C:\finalAnalisis\FinalAnalisiss\Services\IncidenteService.cs(1,17): error CS0234: El tipo o el nombre del espacio de nombres 'EntityFrameworkCore' no existe en el espacio de nombres 'Microsoft' (¿falta alguna referencia de ensamblado?)
C:\finalAnalisis\FinalAnalisiss\Data\DbContext.cs(6,41): error CS0246: El nombre del tipo o del espacio de nombres 'DbContext' no se encontró (¿falta una directiva using o una referencia de ensamblado?)
C:\finalAnalisis\FinalAnalisiss\Data\DbContext.cs(8,37): error CS0246: El nombre del tipo o del espacio de nombres 'DbContextOptions<>' no se encontró (¿falta una directiva using o una referencia de ensamblado?)
C:\finalAnalisis\FinalAnalisiss\Data\DbContext.cs(10,16): error CS0246: El nombre del tipo o del espacio de nombres 'DbSet<>' no se encontró (¿falta una directiva using o una referencia de ensamblado?)
C:\finalAnalisis\FinalAnalisiss\Data\DbContext.cs(11,16): error CS0246: El nombre del tipo o del espacio de nombres 'DbSet<>' no se encontró (¿falta una directiva using o una referencia de ensamblado?)
C:\finalAnalisis\FinalAnalisiss\Data\DbContext.cs(13,49): error CS0246: El nombre del tipo o del espacio de nombres 'ModelBuilder' no se encontró (¿falta una directiva using o una referencia de ensamblado?)

Tengo problemas con el program le agregue la esta linea , using Swashbuckle.AspNetCore;   y me da este error  
C:\finalAnalisis\FinalAnalisiss\Program.cs(4,7): error CS0246: El nombre del tipo o del espacio de nombres 'Swashbuckle' no se encontró (¿falta una directiva using o una referencia de ensamblado?)



dame datos para agregar

en base a los códigos que me diste genera pruebas unitarias 


ahora solo me da esto, PS C:\finalAnalisis\FinalAnalisis.Tests> dotnet test
Restauración completada (1.0s)
  FinalAnalisiss realizado correctamente (1.0s) → C:\finalAnalisis\FinalAnalisiss\bin\Debug\net9.0\FinalAnalisiss.dll
  FinalAnalisis.Tests error con 1 errores (1.1s)
    C:\finalAnalisis\FinalAnalisis.Tests\IncidenteServiceTests.cs(66,21): error CS1912: Inicialización del miembro 'Titulo' duplicada


Compilación error con 1 errores en 4.1s
lo esto subiendo a render pero me dice esto, ==> Root directory "Dockerfile" does not exist. Verify the Root Directory configured in your service settings.


y ahora ,Deploy failed for 8e1a7a8: api con render
Exited with status 1 while building your code. Check your deploy logs for more information.
June 13, 2026 at 8:56 AM

Rollback

First deploy started for 8e1a7a8: api con render
