 #  Sistema de Gestión de Incidentes - FinalAnalisis

Este proyecto es una Web API robusta desarrollada en **.NET 9** diseñada para la gestión, asignación y automatización de incidentes de TI, siguiendo reglas de negocio estrictas de Acuerdos de Nivel de Servicio (SLA) y control de carga de trabajo.

---

##  1. Estructura y Componentes del Proyecto

El espacio de trabajo está compuesto por dos proyectos principales y los archivos de configuración para despliegue:

* **`FinalAnalisiss/` (Web API):** Núcleo del sistema que expone los endpoints HTTP. Se comunica con una base de datos **SQLite** para persistir la información.
    * `Program.cs`: Configuraciones de inicio, inyección de dependencias y activación de documentación interactiva.
    * `Data/FinalAnalisisContext.cs`: Contexto de Entity Framework Core para el mapeo con la base de datos.
    * `Models/`: Entidades de datos (`Incidente`, `EstadoIncidente`, `SeveridadIncidente`) y Objetos de Transferencia de Datos (DTOs).
    * `Services/IncidenteService.cs`: Capa lógica del negocio donde se ejecutan y validan las reglas del sistema.
* **`FinalAnalisis.Tests/` (Pruebas Unitarias):** Suite de control de calidad desarrollada con **xUnit** que valida el comportamiento aislado de las reglas de negocio utilizando una base de datos **InMemory** (en memoria volatil).
* **`Dockerfile`:** Instrucciones de empaquetamiento utilizadas por **Render** para compilar y desplegar la aplicación en la nube mediante contenedores.

---

##  2. Cobertura de Reglas de Negocio (Pruebas Unitarias)

El proyecto cuenta con un blindaje técnico mediante pruebas unitarias automáticas en `IncidenteServiceTests.cs`. Cada prueba valida un requerimiento crítico:

1.  **`CrearAsync_...`:** Garantiza que todo incidente nuevo inicie con el estado `Registrado`, la bandera de escalación en `false` y la fecha actual del sistema.
2.  **`AsignarOReasignarAsync_...`:** Valida que el sistema bloquee la asignación si un técnico ya posee **3 incidentes activos** (estados *Asignado* o *EnProgreso*), disparando una excepción `InvalidOperationException`.
3.  **`ActualizarEstadoAsync_DebeRespetarFlujoUnidireccionalEstricto`:** Verifica que no se puedan saltar estados de forma ilegal (por ejemplo, pasar de *Registrado* directo a *EnProgreso*).
4.  **`VerificarYActivarEscalacionesAsync_...`:** Simula tickets con severidad *Crítica/Urgente* con más de 2 horas de inactividad y comprueba que el sistema los marque automáticamente como `Escalado = true`.
5.  **`ActualizarEstadoAsync_DebeCalcularMinutosDeSLA_AlResolver`:** Al cambiar el estado a *Resuelto*, verifica que se calcule y guarde automáticamente la duración del ciclo de vida en minutos (`DuracionResolucionMinutos`).

---

##  3. Instrucciones de Uso en Entorno Local (VS Code)

### Ejecutar la API paso a paso (Sin PowerShell)
1. Abra la raíz del proyecto (`C:\finalAnalisis`) en **Visual Studio Code**.
2. Abra la terminal integrada con el atajo de teclado `Ctrl + Ñ` o desde el menú *Ver -> Terminal*.
3. Acceda a la carpeta del proyecto principal:
   ```bash
   cd FinalAnalisiss
