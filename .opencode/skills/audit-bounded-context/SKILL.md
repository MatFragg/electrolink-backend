---
name: audit-bounded-context
description: Use ONLY when the user asks to audit, review, or analyze a bounded context in this ElectroLink DDD/CQRS monolith. Triggers on phrases like "audita el BC iam", "audit profiles", "review operation BC", "revisar planning", "analiza assets", or any combination of audit/audita/revisar/review/analiza + one of the 8 bounded contexts (iam, profiles, assets, subscriptions, planning, monitoring, analytics, processing). Runs a 4-phase audit (documentation pre-check, code quality + CQRS, external integrations, RESTful API) and updates docs/tacticals/TD-{BC}.md plus creates docs/output/{BC}/api-restful.md and docs/output/{BC}/integraciones.md. Does NOT modify source code under any BC folder. Applies only to the 8 known BCs of this repo; do not invoke for unrelated audits.
---

# Skill: audit-bounded-context

Audita un bounded context (BC) del monolito ElectroLink en 4 fases,
parametrizado por el nombre del BC extraído del prompt del usuario.

---

## §1. Extracción del BC

### 1.1 Whitelist cerrada (8 BCs del repo)

| Slug (lowercase canónico) | Carpeta código | TD propio                            | Reference en `docs/examples/` |
|---|---|--------------------------------------|---|
| `iam` | `IAM/` | `docs/tacticals/TD-IAM.md`           | `docs/examples/Tactical Design IAM 2026.md` |
| `profiles` | `Profiles/` | `docs/tacticals/TD-Profiles.md`      | `docs/examples/Tactical Design Profiles 2026.md` |
| `assets` | `Assets/` | `docs/tacticals/TD-Assets.md`        | (no existe) |
| `subscriptions` | `Subscriptions/` | `docs/tacticals/TD-Subscriptions.md` | `docs/examples/Tactical Design Subscriptions.md` |
| `planning` | `Planning/` | `docs/tacticals/TD-Planning.md`      | (no existe) |
| `monitoring` | `Monitoring/` | `docs/tacticals/TD-Monitoring.md`    | (no existe) |
| `analytics` | (no existe, FUTURO) | `docs/tacticals/TD-Analytics.md`     | `docs/examples/Tactical Design Analytics BC.md` |
| `processing` | (no existe, FUTURO) | `docs/tacticals/TD-Processing.md`     | (no existe) |

**No existe** un BC llamado "operation" u "operación" en este repo. Si el
usuario lo pide, NO mapear a otro BC. Indicar:

> "Este repo no tiene un BC `operation`. ¿Quisiste decir `planning`
> (ejecuta el lado operacional de las solicitudes de servicio)?
> Los 8 BCs auditables son: iam, profiles, assets, subscriptions,
> planning, monitoring, analytics, processing."

**No confundir** `monitoring` (BC de ServiceExecution) con `processing`
(BC de IoT/Edge). Ver §3-bis para desambiguación.

### 1.2 Sinónimos en español → BC

```text
"identidad", "autenticación", "usuarios", "login", "registro"  → iam
"perfiles", "técnicos", "homeowners", "técnico", "perfil"        → profiles
"inventario", "componentes", "propiedades", "catálogo", "assets" → assets
"suscripciones", "pagos", "stripe", "planes", "billing", "sub"  → subscriptions
"planificación", "matching", "asignación", "solicitud", "planning" → planning
"monitoreo", "alertas", "observabilidad", "métricas en vivo"     → monitoring
"analítica", "analytics", "reportes", "dashboards", "BI", "kpi"  → analytics
"procesamiento", "edge", "iot", "edge-processing"                → processing
```

> ⚠️ **No mapear** "monitoreo" / "monitoring" / "alarmas" → `processing`.
> Esos términos van al BC `monitoring` (existente, BC de
> ServiceExecution). `processing` es estrictamente IoT/edge.
> Ver §3-bis para reglas de desambiguación cuando el usuario use
> frases ambiguas como "monitoreo IoT" o "monitoreo de sensores".

### 1.3 Procedimiento de extracción

1. Normalizar el prompt del usuario: lowercase, sin acentos, trim.
2. Buscar match exacto contra la whitelist (1.1).
3. Si no hay match exacto, buscar contra los sinónimos (1.2).
4. Si aún no hay match, usar la herramienta `question` para listar
   los 8 BCs disponibles y pedir selección.
5. Asignar `BC = slug` (lowercase, sin espacios, sin caracteres
   especiales). Ejemplos: `iam`, `profiles`, `subscriptions`.

---

## §2. Resolución de paths

Una vez extraído `BC`, resolver los paths con `bash` (`Test-Path`) y
`glob`. **No asumir paths**; siempre verificar empíricamente.

### 2.1 Carpetas de código

```text
1. ./{BC-PascalCase}/         (patrón normal: IAM/, Profiles/, …)
2. ./src/{BC}/                (fallback si no existe 1)
3. ./modules/{BC}/            (fallback si no existen 1 y 2)
4. ./services/{BC}/           (fallback final)
```

Si NINGUNO existe → STATUS = `DOCS-ONLY` (casos `analytics` y
`processing`). No detener; continuar la auditoría con secciones
marcadas "N/A — BC no implementado" en FASE 1.

### 2.2 Tactical design propio del BC

```text
1. docs/tacticals/TD-{BC-PascalCase}.md       (patrón normal)
2. docs/tacticals/TD-{BC-UPPERCASE}.md        (variante todo-mayúsculas)
3. glob: docs/tacticals/TD-*{BC}*.md          (fallback flexible)
```

Si no existe → **crear** el archivo desde cero con la estructura
estándar de §5.1. Si existe → **actualizarlo** preservando contenido
del usuario y agregando las secciones §10–§12.

### 2.3 Tactical reference (otro proyecto, conceptual)

```text
glob: docs/examples/Tactical Design*{BC}*.md
```

Si no hay match → WARN (no detener). Marcar §11 del TD como
"Contraste no evaluable: no existe tactical reference comparable".

### 2.4 Documentos fijos del repo

```text
docs/external-dependencies-architecture.md   (SIEMPRE debe existir)
docs/ProjectArchitecture.md                  (SIEMPRE debe existir)
```

Si alguno falta → **DETENER** la ejecución y reportar a Hampcoders.

---

## §3. Conciencia de stack (anti-alucinación técnica)

Las references en `docs/examples/` provienen de OTRO proyecto en otro
stack (típicamente Java/Spring). **NO copiar nombres, prefijos, sufijos
ni paths del reference**. Extraer únicamente el QUÉ (qué agregados,
qué eventos, qué ACLs, qué casos de uso).

Tabla de traducción al stack real de este repo (.NET 8/9 + C# + EFC + Serilog):

| Concepto en reference (típico Java/Spring) | Equivalente en este repo (.NET / C# / EFC) |
|---|---|
| `*ServiceImpl` (sufijo) | `*CommandService` / `*QueryService` (CQRS segregado) |
| `*Repository` (interfaz) | `I*Repository` (interfaz en Domain, prefijo `I`) |
| Repositorio JPA se implementa en runtime | EFC requiere `I*Repository` en Domain + `*Repository` (sin prefijo `I`) en Infrastructure |
| `Enum` plano | Prefijo `E` (ej: `EUserStatus`) — verificar convención del BC auditado, no asumir |
| Interfaz `*Service` | `I*Service` o `I*CommandService` / `I*QueryService` (CQRS) |
| `application.yml` / `application.properties` | `appsettings.json` / `appsettings.Development.json` |
| `pom.xml` / `build.gradle` | `Hampcoders.Electrolink.API.csproj` (único csproj raíz con carpetas de BCs) |
| Carpeta de tests `src/test/...` | `tests/` o `[BC].Tests/` (verificar por BC) |
| `Spring` controllers `@RestController` | `Controllers` en `Interfaces/REST/` con atributo `[ApiController]` |

**Regla dura:** si en el reference aparece una clase/método/evento y
NO existe equivalente claro en el código del BC auditado, reportar
como discrepancia en §11, NO asumir que existe.

---

## §3-bis. Desambiguación de BCs con nombres similares

Para evitar asignar el BC equivocado cuando el usuario usa términos
ambiguos, aplicar esta tabla de resolución:

| Usuario dice | BC asignado | Razón |
|---|---|---|
| "monitoreo", "monitoring", "alarmas operacionales" | `monitoring` | BC existente de ServiceExecution / ServiceCancellationRequest |
| "monitoreo IoT", "monitoreo de sensores", "monitoreo en el edge" | `processing` | Aclara intención: es el BC IoT, no el operacional |
| "procesamiento", "edge", "iot", "edge-processing", "procesamiento en el borde" | `processing` | Sinónimos directos |
| "monitoreo de servicios", "ejecución de servicios", "cancelaciones" | `monitoring` | Aclara intención: es el BC operacional |
| "monitoreo de anomalías eléctricas", "detección de picos" | `processing` | El concepto de anomalía eléctrica es del BC IoT |
| "monitoreo de técnicos en campo" | `monitoring` | Monitoreo de operaciones, no de sensores |
| "lecturas del sensor", "relé", "control de circuito" | `processing` | Vocabulario del BC IoT |

Si después de aplicar la tabla la ambigüedad persiste (ej: "monitoreo
del dispositivo"), usar la herramienta `question` para mostrar las
opciones `monitoring` vs `processing` con una breve descripción de
cada uno (ServiceExecution vs IoT/Edge).

---

## §4. Workflow de 4 fases (los 4 prompts embebidos)

Cada fase ejecuta el contenido del prompt correspondiente con `{BC}`
en lugar de `IAM`. Las fases son **secuenciales y obligatorias**; el
output de cada fase alimenta la siguiente.

---

### FASE 0 — Pre-chequeo documental

**Rol:** Arquitecto de Software experto en DDD, Clean Architecture y CQRS.

**Objetivo:** Validar que la documentación necesaria existe y está
actualizada, antes de cualquier análisis de código.

**Procedimiento:**

1. Verificar la existencia de los siguientes archivos en el repositorio:

   ```text
   - docs/tacticals/TD-{BC-PascalCase}.md
   - docs/examples/Tactical Design {BC} 2026.md   (o variante; resolver por glob §2.3)
   - docs/external-dependencies-architecture.md
   - docs/ProjectArchitecture.md
   ```

2. Si alguno falta, **detener la ejecución** y reportar:
   - El path faltante.
   - Una solicitud clara al equipo de Hampcoders para que lo
     proporcionen.

3. Si todos existen, leerlos y extraer:
   - El tactical design actual de **{BC}** (del primer archivo).
   - El tactical design de referencia (del segundo archivo, si
     existe).
   - La lista de dependencias externas síncronas y asíncronas (del
     tercer archivo).
   - La arquitectura general del proyecto (del cuarto archivo).

4. Generar un resumen estructurado (máximo 500 líneas) con estos
   hallazgos y **guardarlo en memoria** para las fases siguientes.

**Restricciones:** No generar código aún. Solo documentación.

**Salida:** Resumen interno `FASE_0_RESUMEN.md` (temporal, no
persistir en disco).

---

### FASE 1 — Calidad de código + CQRS

**Rol:** Arquitecto de Software + Ingeniero de Performance.

**Tarea:**

1. **Calidad y posibles fallas de rendimiento:**
   - Buscar **N+1 queries**: ciclos sobre colecciones que disparan
     queries por elemento (típicamente dentro de `*CommandService`
     o `*QueryService` accediendo a `I*Repository`).
   - Buscar **loops bloqueantes** sobre operaciones async (falta de
     `await`, uso de `.Result`/`.Wait()`).
   - Buscar **falta de caché** en operaciones de lectura costosas
     repetidas.
   - Buscar **transacciones largas** (múltiples operaciones de
     escritura sin delimitar `UnitOfWork`/`SaveChanges`).
   - Buscar **uso ineficiente de colecciones** (LINQ sobre `List` en
     vez de `IEnumerable`, materialización prematura con `ToList()`).
   - Señalar cualquier **fetch masivo de datos sin paginación/filtros**
     (ej: `GetAll()` retornando toda la tabla, ausencia de `Skip`/`Take`).

2. **Verificación de CQRS en servicios:**
   - Identificar las interfaces en `{BC}/Domain/Services/` (o ruta
     equivalente según §2.1).
   - Confirmar que existe **separación clara entre comandos (write)
     y consultas (read)**.
   - Denunciar cualquier servicio que **mezcle responsabilidades**:
     método que modifica estado y retorna datos complejos; query
     service que ejecuta writes; command service que retorna DTOs
     complejos en vez de identificadores/void.

3. **Restricción anti-alucinación:**
   - Si un concepto (ej: `DomainEvent`, `CommandHandler`,
     `MediatR`, `Outbox`) no aparece explícitamente en el código o
     documentación del BC, **NO asumirlo**. Reportar como "Ausente:
     {concepto}".
   - Si encuentras patrones no documentados en el TD actual,
     pregúntalos en el informe como **"Posible hallazgo no
     especificado"**.

**Salida esperada:** Lista de hallazgos clasificada por:

- **CRÍTICO** (falla de rendimiento o violación de CQRS).
- **MEJORABLE** (código con riesgo).
- **OK** (cumple estándares).

**Restricciones:** No modificar archivos del BC. Solo reportar.

**Salida:** Resumen interno `FASE_1_HALLAZGOS.md` (temporal).

---

### FASE 2 — Integraciones externas (ACL + eventos)

**Rol:** Especialista en Arquitectura de Integraciones.

**Tareas concretas:**

#### A. Dependencias síncronas vía ACL (Anti-Corruption Layer)

- Localizar en el código las clases/adapters que implementen ACL para
  sistemas externos. Patrones de búsqueda:
  ```text
  {BC}/Interfaces/ACL/**/*.cs
  {BC}/Application/Internal/OutboundServices/**/*.cs
  Nombres que terminan en: ACL, Adapter, Gateway, Service (puertos),
  ContextFacade, Client
  ```
- Verificar que cada llamada externa tenga:
  - **Timeout configurado** (en cliente HTTP, en opciones del SDK,
    en atributos del método).
  - **Manejo de errores:** retry, circuit breaker, política de
    reintentos, `try/catch` específico (no genérico `Exception`).
  - **Logging estructurado** (Serilog con template properties, no
    `Console.WriteLine` ni string interpolation).
- Reportar cualquier **llamada directa (sin ACL) a un sistema
  externo** (uso de `HttpClient` directo, SDK de terceros sin
  abstracción, etc.).

#### B. Eventos asíncronos

- Buscar publicadores/suscriptores de eventos internos. Patrones:
  ```text
  {BC}/Application/Internal/EventHandlers/**/*.cs
  {BC}/Domain/Model/Events/**/*.cs
  Nombres que terminan en: EventHandler, EventBus, DomainEvent,
  EventPublisher, IEvent, BaseEvent
  ```
- Confirmar que los eventos **no contengan datos sensibles**
  (passwords en plain o hash, tokens JWT, API keys, secretos, PII
  no necesaria para el consumidor).
- Verificar que los **handlers de eventos sean idempotentes** (o al
  menos manejen duplicados con deduplicación, `if (exists) return`,
  upsert, etc.).

#### C. Contraste con el tactical design de referencia

- Comparar el uso real de eventos/ACL contra el tactical reference
  (`docs/examples/Tactical Design*{BC}*.md`).
- Si **no existe reference** (caso `assets`, `planning`, `monitoring`):
  - NO detener.
  - Aplicar checklist genérico basado en:
    - **SOLID** (SRP, OCP, LSP, ISP, DIP).
    - **CQRS** (separación Command/Query).
    - **DDD** (Aggregate Root, Entity, Value Object, Repository,
      Domain Service, Domain Event).
    - **Clean Architecture** (dependencias inward-only; Domain sin
      refs externas).
    - Patrones extrapolados de los BCs más maduros del repo
      (típicamente `iam` y `profiles`). Citarlos explícitamente.
  - Marcar §11 del TD como "Contraste no evaluable: no existe
    tactical reference comparable en `docs/examples/`".

**Formato de salida por dependencia:**

```markdown
| Dependencia | Tipo | Estado | Evidencia | Recomendación |
|---|---|---|---|---|
| {nombre} | ACL/Evento/Outbound | OK/WARNING/FAIL | {BC}/ruta/archivo.cs:L{linea} | {acción concreta} |
```

---

### FASE 3 — API RESTful + cierre

**Rol:** API Architect.

**Checklist RESTful:**

1. **Nombres de endpoints:** ¿Usan sustantivos en plural?
   - ✓ `/users`, `/profiles`, `/service-requests`
   - ✗ `/getUser`, `/createProfile`, `/listServiceRequests`
2. **Métodos HTTP:** ¿Usan correctamente GET, POST, PUT, PATCH, DELETE?
   - Denunciar cualquier endpoint que use **GET para modificar
     datos** (debería ser POST/PUT/PATCH/DELETE).
3. **Códigos de estado:** ¿Devuelven códigos semánticos?
   - Aceptables: 200, 201, 204, 400, 401, 403, 404, 409, 422, 500.
   - Denunciar: 200 con cuerpo de error, 500 para validaciones de
     negocio (debería ser 400/422).
4. **Versionado:** ¿Existe versión en la URL (`/v1/iam/...`) o
   header (`Accept: application/vnd.api+json; version=1`)?
   - Si NO existe, marcar WARNING con recomendación de versionado.
5. **Endpoints deprecados:** Buscar:
   - Comentarios `// deprecated`, `[Obsolete]`, `[Deprecated]`.
   - Headers `Deprecation` o `Sunset` en responses.
   - Rutas marcadas como obsoletas en la documentación.
   - Listar todos los encontrados.

**Actualización documental:**

- Releer `docs/tacticals/TD-{BC-PascalCase}.md` (o el que se
  generó/actualizó en FASE 0).
- Verificar que **todos los endpoints actuales** estén documentados.
- Si un endpoint existe en el código pero NO en la documentación →
  marcarlo como **"no documentado"**.
- Si la documentación menciona un endpoint que NO existe en el
  código → marcarlo como **"endpoint huérfano"**.

**Restricción anti-alucinación:** Si un concepto (ej: HATEOAS,
content negotiation con `application/hal+json`, OpenAPI 3.1 con
`x-extension`) no está en `docs/ProjectArchitecture.md`, **NO
evaluarlo**.

---

### FASE 4 — Informe consolidado (cierre)

Generar el **informe final de auditoría** (markdown) que consolide los
hallazgos de las fases 0–3. Secciones obligatorias:

1. **Resumen ejecutivo** (1 párrafo): BC auditado, fecha, hallazgos
   críticos totales, recomendación global.
2. **Tabla de hallazgos por categoría:**

   | Categoría | CRÍTICO | MEJORABLE | OK |
   |---|---|---|---|
   | Rendimiento | {n} | {n} | {n} |
   | CQRS | {n} | {n} | {n} |
   | Integraciones | {n} | {n} | {n} |
   | API RESTful | {n} | {n} | {n} |

3. **Discrepancias con el tactical design de referencia** (si
   existe; si no, "Contraste no evaluable" + principios aplicados).
4. **Lista de endpoints deprecados o a deprecar** (de FASE 3).
5. **Próximos pasos recomendados** (ordenados por prioridad,
   máximo 10 items, con responsable sugerido: Hampcoders / equipo de
   BC).

---

## §5. Archivos de salida

La skill **SIEMPRE** produce exactamente 3 archivos (sobrescribiendo
los previos). El código fuente del BC **NUNCA** se modifica.

### 5.1 `docs/tacticals/TD-{BC-PascalCase}.md`

Crear si no existe; actualizar si existe. Estructura estándar:

```markdown
# Tactical Design: {BC} BC
## ElectroLink System — ASP.NET Core | DDD | CQRS | Single Database

## Índice
1. Estructura del Proyecto
2. Domain Layer
3. Application Layer
4. Infrastructure Layer
5. Interfaces Layer
6. Esquema de Base de Datos
7. Flujos CQRS por Caso de Uso
8. Endpoints RESTful (referencia: docs/output/{bc}/api-restful.md)
9. Integraciones Externas (referencia: docs/output/{bc}/integraciones.md)
10. Hallazgos de Auditoría
    10.1 CRÍTICOS
    10.2 MEJORABLES
    10.3 OK
11. Discrepancias con tactical design de referencia
12. Próximos pasos recomendados
```

Al **actualizar** un TD existente: preservar el contenido del usuario
en §1–§9. Solo regenerar §10, §11, §12 (o agregarlas si no existen).

### 5.2 `docs/output/{bc-lowercase}/api-restful.md`

Generar siempre. Crear la carpeta `docs/output/{bc}/` si no existe.

```markdown
# Endpoints RESTful — {BC} BC
**Generado:** {fecha ISO 8601}
**Auditoría:** audit-bounded-context skill

## Tabla de endpoints

| Método | Ruta | Controller | Auth | Status codes | Doc status | Notas |
|---|---|---|---|---|---|---|
| GET | /api/v1/{bc}/users | UsersController | JWT | 200, 401 | OK / No documentado / Huérfano | {comentarios} |
| POST | /api/v1/{bc}/sign-in | AuthenticationController | none | 200, 400, 401 | OK | … |

## Endpoints no documentados
{Lista de endpoints en código que no aparecen en TD-{BC}.md}

## Endpoints huérfanos
{Lista de endpoints en TD-{BC}.md que no aparecen en código}

## Endpoints deprecados
{Lista de endpoints con [Obsolete], [Deprecated], header Deprecation, o comentarios}

## Checklist RESTful
- [x/✗] Sustantivos en plural
- [x/✗] Métodos HTTP correctos
- [x/✗] Códigos de estado semánticos
- [x/✗] Versionado
- [x/✗] Sin endpoints deprecados sin marcar
```

### 5.3 `docs/output/{bc-lowercase}/integraciones.md`

```markdown
# Integraciones — {BC} BC
**Generado:** {fecha ISO 8601}
**Auditoría:** audit-bounded-context skill

## A. Dependencias síncronas (ACL / Outbound)

| Nombre | Tipo | Destino | Timeout | Retry/CB | Logging | Estado | Evidencia |
|---|---|---|---|---|---|---|---|
| IHashingService | Puerto | BCrypt (interno) | N/A | N/A | {✓/✗} | OK | {ruta}:L{linea} |
| ITokenService | Puerto | JWT (interno) | N/A | N/A | {✓/✗} | OK | {ruta}:L{linea} |
| ExternalProfilesService | ACL | Profiles BC | {ms} | {✓/✗} | {✓/✗} | {OK/WARN/FAIL} | {ruta}:L{linea} |

## B. Eventos asíncronos

### Eventos publicados por {BC}
| Evento | Handler en otros BCs | Datos sensibles | Idempotente | Evidencia |
|---|---|---|---|---|
| UserRegisteredEvent | Profiles, Subscriptions, Assets | {✓/✗} | {✓/✗} | {ruta}:L{linea} |

### Eventos consumidos por {BC}
| Evento | Origen | Handler | Idempotente | Evidencia |
|---|---|---|---|---|
| {evento} | {BC-origen} | {handler} | {✓/✗} | {ruta}:L{linea} |

## C. Contraste con tactical reference
{Sección presente solo si existe reference; si no, marcar "Contraste no evaluable"}

## Recomendaciones
{Lista priorizada}
```

---

## §6. Reglas anti-alucinación (transversales)

Aplican a TODAS las fases:

1. **No generar código.** La skill produce análisis y documentación,
   no implementaciones.
2. **No inventar conceptos.** Si `DomainEvent`, `Outbox`,
   `CommandHandler`, `MediatR` u otro patrón no aparece
   explícitamente en el código o en `docs/tacticals/TD-{BC}.md`, NO
   asumir que existe. Reportar como "Ausente" o "No especificado".
3. **No copiar del reference.** Los archivos en `docs/examples/`
   son conceptuales (de otro proyecto/stack). Extraer solo
   conceptos, no nombres de clases ni paths.
4. **No evaluar conceptos no documentados.** Si un patrón (ej:
   HATEOAS, `application/hal+json`, GraphQL, gRPC) no está
   mencionado en `docs/ProjectArchitecture.md`, NO incluirlo en
   el checklist ni en las recomendaciones.
5. **Verificar paths empíricamente.** Usar `Test-Path` y `glob`
   antes de `Read`. Nunca asumir que un archivo existe por su
   nombre.
6. **Cero fetch masivo sin paginación.** Si un endpoint expone
   `GetAll()` sin filtros, denunciarlo en FASE 1.
7. **Cero GET que muta.** Si un endpoint usa GET para modificar
   estado, denunciarlo en FASE 3.
8. **Cero secretos en eventos.** Si un evento contiene passwords,
   tokens, API keys, denunciarlo en FASE 2B.
9. **No inventar la Edge API ni el firmware.** El
   `docs/ResumenElectrolink-IOT.md` menciona que la Edge API está
   en Flask/Python y el firmware en C++/MicroPython en ESP32. Esos
   componentes son **de otro stack** y NO viven en este repo .NET.
   La skill solo audita la documentación del BC `processing` y,
   cuando exista, su código .NET. NO asumir código Python, C++ ni
   microcontroladores.

---

## §7. Modo de operación

### 7.1 Permisos de la skill

- **Read:** permitido en todo el repo (código del BC, docs, examples,
  tests).
- **Write:** permitido **únicamente** en los 3 archivos de salida
  definidos en §5. Cualquier otro `write` o `edit` debe ser
  reportado al usuario antes de ejecutarse.
- **Bash:** permitido para verificación de paths (`Test-Path`,
  `Get-ChildItem`, `glob`) y para el smoke test. NO ejecutar
  `dotnet build`, `dotnet test`, `git commit`, ni comandos
  destructivos.

### 7.2 Idempotencia

Los 3 archivos de salida se **sobrescriben** en cada invocación. No
se acumula historial de auditorías. Si el usuario necesita
historial, debe versionar el repo (git) o pedir explícitamente un
modo `--append` (no soportado en esta versión).

### 7.3 Orden de ejecución

```text
FASE 0 (docs) → FASE 1 (código+CQRS) → FASE 2 (integraciones) →
FASE 3 (RESTful) → FASE 4 (consolidado) → escrituras §5
```

Si una fase falla (ej: falta un archivo base en FASE 0), las fases
siguientes no se ejecutan. Se reporta al usuario.

### 7.4 Idioma y formato

- Toda la salida de la skill (reportes, tablas, recomendaciones) en
  **español**, consistente con los `TD-*.md` existentes.
- Código, paths, identificadores técnicos en su idioma original
  (inglés por convención de C#/.NET).
- Fechas en formato **ISO 8601** (`YYYY-MM-DD`).

### 7.5 Resumen al usuario

Al terminar, la skill reporta en la conversación:

```text
✅ Auditoría de {BC} completada.
   • {N} hallazgos CRÍTICOS, {N} MEJORABLES, {N} OK.
   • Archivos actualizados/creados:
     - docs/tacticals/TD-{BC-PascalCase}.md
     - docs/output/{bc-lowercase}/api-restful.md
     - docs/output/{bc-lowercase}/integraciones.md
   • Próximo paso sugerido: {1 línea accionable}
```

---

## §8. Smoke test recomendado

**Orden sugerido** (de menos a más completo):

1. `processing` — DOCS-ONLY (sin código, sin reference). Valida el
   camino de §2.1, §2.3 y §3-bis. Útil para verificar que la skill
   maneja correctamente la ausencia de código (FASE 1 = N/A) y
   ausencia de reference (FASE 2C = contraste no evaluable).
2. `analytics` — DOCS-ONLY con reference. Valida §2.3 + §2C-bis
   (uso de principios SOLID/CQRS/DDD como fallback).
3. `iam` — código + TD + reference + carpeta completos. Caso
   representativo del flujo completo.

Para cada caso, validar:

1. Los 3 archivos generados existen y tienen contenido no trivial.
2. `TD-{BC}.md` actualizado incluye §10, §11, §12 (o secciones
   "N/A — BC no implementado" si es DOCS-ONLY).
3. `api-restful.md` lista los controllers reales (o lista vacía
   con nota "Sin código fuente" si es DOCS-ONLY).
4. `integraciones.md` lista los servicios/ACL/eventos reales (o
   secciones "Pendiente de implementación" si es DOCS-ONLY).
5. No se modificaron archivos bajo `{BC}/`.

Si algún punto falla, ajustar la skill y re-ejecutar.
