---
name: implement-bounded-context
description: Use ONLY when the user asks to implement, generate, scaffold, build, or create the C# code of a bounded context in this ElectroLink DDD/CQRS monolith from a tactical design. Triggers on phrases like "implementa el BC iam", "genera el código de profiles", "scaffold de analytics", "construye monitoring desde el TD", "crea el BC processing", or any combination of implementa/genera/scaffold/construye/crea + one of the 8 bounded contexts (iam, profiles, assets, subscriptions, planning, monitoring, analytics, processing). Reads docs/tacticals/TD-{BC}.md as the authoritative contract, plus docs/ResumenElectrolink-IOT.md (cross-BC events), docs/ProjectArchitecture.md (conventions), and docs/examples/Tactical Design {BC}*.md (conceptual reference, Java/Spring — extract WHAT only). Generates the 4 layers (Domain, Application, Infrastructure, Interfaces) under {BC-PascalCase}/ and wires csproj, Program.cs, AppDbContext, and Shared Kernel IDs. Does NOT modify audit-bounded-context behavior; does NOT modify docs/ or docs/examples/; does NOT generate tests in this version.
---

# Skill: implement-bounded-context

Genera el código C# de un bounded context (BC) del monolito ElectroLink
a partir de su Tactical Design y los documentos de contexto del repo.

Es la **inversa** de `audit-bounded-context`: esa skill LEE código y
produce análisis; esta LEE diseños y produce código.

---

## §1. Extracción del BC

### 1.1 Whitelist cerrada (8 BCs del repo)

Reutilizar EXACTAMENTE la whitelist de `audit-bounded-context` §1.1:

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

> "Este repo no tiene un BC `operation`. ¿Quisiste decir `monitoring`
> (ejecuta el lado operacional de los servicios)?
> Los 8 BCs implementables son: iam, profiles, assets, subscriptions,
> planning, monitoring, analytics, processing."

**No confundir** `monitoring` (BC de ServiceExecution) con `processing`
(BC de IoT/Edge). Reutilizar la tabla de desambiguación de
`audit-bounded-context` §3-bis (mismas reglas, mismo resultado).

### 1.2 Sinónimos en español → BC

Reutilizar exactamente la tabla de sinónimos de `audit-bounded-context`
§1.2. Si el prompt del usuario no matchea whitelist ni sinónimos, usar
la herramienta `question` para listar los 8 BCs y pedir selección.

### 1.3 Procedimiento de extracción

1. Normalizar el prompt: lowercase, sin acentos, trim.
2. Match exacto contra whitelist.
3. Match contra sinónimos.
4. Si nada aplica → `question` con los 8 BCs.
5. Asignar `BC = slug`. Derivar:
   - `BC-PascalCase` (ej: `iam` → `IAM`, `profiles` → `Profiles`).
   - `BC-camel` (ej: `iam` → `iam`, `profiles` → `profiles`) para `docs/output/`.
   - `BC-upper` (ej: `IAM`, `PROFILES`) para namespaces.
   - `BC-pascal` (ej: `Iam`, `Profiles`) para nombres de clase.

### 1.4 No invocar para

- Auditar / revisar un BC → usar `audit-bounded-context`.
- Modificar un solo archivo de un BC existente → no es alcance de esta skill.
- Generar tests → fuera de alcance de esta versión.
- Implementar features puntuales (ej: "añade endpoint X") → fuera de alcance.

---

## §2. Lectura de los 4 documentos de contexto

Orden obligatorio. Verificar empíricamente con `Test-Path` y `glob` antes
de leer. NUNCA asumir que un archivo existe por su nombre.

### 2.1 Tactical Design del BC (contrato autoritativo)

```text
1. docs/tacticals/TD-{BC-PascalCase}.md       (patrón normal)
2. docs/tacticals/TD-{BC-UPPERCASE}.md        (variante todo-mayúsculas)
3. glob: docs/tacticals/TD-*{BC}*.md          (fallback flexible)
```

**Si el TD falta** → DETENER y reportar:

> "No existe `docs/tacticals/TD-{BC}.md`. Esta skill NO diseña: el
> equipo de Hampcoders debe crearlo primero con la estructura de
> `audit-bounded-context` §5.1 (o seguir la plantilla de TD-IAM.md /
> TD-Profiles.md como referencia)."

El TD es la **fuente única de verdad** para la generación. Secciones
usadas del TD:

| Sección TD | Genera |
|---|---|
| §1 Estructura | Lista de archivos a crear |
| §2 Domain | Domain layer completo |
| §3 Application | Application layer completo |
| §4 Infrastructure | Infrastructure layer completo |
| §5 Interfaces | Interfaces layer completo |
| §6 Esquema DB | EFC Configurations + nota SQL |
| §7 Flujos CQRS | Validación cruzada (no se genera código) |
| §8+ Integración | Eventos cross-BC (ver §6) |

### 2.2 Resumen del sistema (eventos cross-BC)

```text
docs/ResumenElectrolink-IOT.md
```

Usar **§4 Tabla de Eventos** para extraer:

- Eventos que el BC **publica** → handlers en otros BCs (informativo;
  verificar que el `IEvent` publicado tenga los mismos campos en
  producción y en el TD).
- Eventos que el BC **consume** → clases handler en
  `Application/Internal/EventHandlers/` (uno por evento, implementando
  `Shared.Application.Internal.EventHandler.IEventHandler<TEvent>`).
- §5 **Integraciones entre BCs** → también referencia, en particular
  los `*ContextFacade` que este BC debe consumir (puerto outbound).

### 2.3 Arquitectura del proyecto (convenciones)

```text
docs/ProjectArchitecture.md
```

Usar para:

- **Namespace raíz**: `Hampcoders.Electrolink.API.{BC}.{Layer}.{Sub}`
  (ver `Program.cs:3-21` y todos los `.cs` existentes; ej:
  `Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates`).
- **Estructura de carpetas** canónica: `Domain/Model/Aggregates`,
  `Domain/Model/ValueObjects`, `Application/Internal/CommandServices`,
  `Application/Internal/QueryServices`, `Application/Internal/EventHandlers`,
  `Application/Internal/OutboundServices`,
  `Infrastructure/Persistence/EFC/Configurations`,
  `Infrastructure/Persistence/EFC/Configuration/Extensions`,
  `Infrastructure/Persistence/EFC/Repositories`,
  `Interfaces/REST/Resources`, `Interfaces/REST/Transform`,
  `Interfaces/REST/{Entity}Controller.cs`,
  `Interfaces/ACL/{I{BC}ContextFacade.cs, Services/{BC}ContextFacade.cs}`,
  `Infrastructure/Interfaces.ASP/Configuration/Extensions`.
- **Patrones**: CQRS segregado, Aggregates que heredan
  `Shared.Domain.Model.Aggregates.BaseAggregateRoot`, eventos que
  implementan `Shared.Domain.Model.Events.IEvent`, repos que heredan
  `Shared.Infrastructure.Persistence.EFC.Repositories.BaseRepository<T, TId>`,
  Unit of Work vía `Shared.Domain.Repositories.IUnitOfWork`.
- **Stack confirmado**: .NET 9, EF Core 9, Npgsql, MediatR, Cortex.Mediator
  (ver `Hampcoders.Electrolink.API.csproj:1-58`).
- **Snake case + pluralización** en columnas/tablas vía
  `EFCore.NamingConventions` (en `csproj:13`).

**NUNCA** evaluar conceptos no documentados aquí (HATEOAS,
content negotiation `application/hal+json`, gRPC, etc.). Si
`ProjectArchitecture.md` no los menciona, NO agregarlos al checklist.

### 2.4 Tactical reference (otro proyecto, conceptual)

```text
glob: docs/examples/Tactical Design*{BC}*.md
```

**SÍ existe** solo para: `iam`, `profiles`, `subscriptions`, `analytics`.

Reglas duras al usar la reference:

- **Extraer SOLO el QUÉ** (qué agregados, qué eventos, qué ACLs, qué
  casos de uso).
- **PROHIBIDO** copiar: sufijos `*Impl`, `*ServiceImpl`, prefijos
  `*FacadeImpl`, namespaces `com.hampcoders.*`, anotaciones Java/Spring
  (`@Service`, `@RestController`, `@Transactional`, etc.), archivos
  `pom.xml` o `build.gradle`, rutas `src/main/java/...`.
- **PROHIBIDO** usar nombres de clases que NO aparezcan en el TD propio
  de este repo. Si la reference tiene `SubscriptionPlanService` y el
  TD dice `SubscriptionCommandService` → gana el TD.
- Tabla de traducción Java/Spring → .NET (de `audit-bounded-context`
  §3) aplica idéntica.

Si la reference contradice al TD, **siempre gana el TD** (es el contrato
autoritativo, §2.1).

---

## §3. Detección de estado del BC y política de choque

Antes de generar, escanear la carpeta del BC con `Get-ChildItem
-LiteralPath "./{BC-PascalCase}" -Recurse` (o `bash`/`glob` equivalente).

### 3.1 Tres estados posibles

| Estado | BCs del repo (al 2026-06) | Política |
|---|---|---|
| `NO_EXISTE` | `Analytics/`, `Processing/` | Flujo greenfield §4 (generación completa, sin preguntas) |
| `EXISTE_PARCIAL` | `Assets/`, `Planning/`, `Monitoring/`, `Subscriptions/` | **SIEMPRE** preguntar con `question` antes de escribir |
| `EXISTE_COMPLETO` | `IAM/`, `Profiles/` | **SIEMPRE** preguntar; recomendar opción (b) o (c) |

### 3.2 Pregunta al usuario cuando `EXISTE_PARCIAL` o `EXISTE_COMPLETO`

Mostrar:

- Conteo de archivos `.cs` existentes en `./{BC-PascalCase}/`.
- Conteo de archivos `.cs` que el TD generaría (estimado a partir de §1).
- Diferencia.

Opciones (con la primera como recomendada solo en `EXISTE_PARCIAL`):

1. **(a) Sobrescribir TODO** — borrar todo bajo `{BC}/` y regenerar
   desde el TD. Destructivo. **NO recomendada** en `EXISTE_COMPLETO`.
2. **(b) Generar solo lo que falta (diff mode)** — comparar el TD con
   el disco. Producir:
   - `A_GENERAR` (en TD, no en disco) → se crean.
   - `A_SOBRESCRIBIR` (en ambos) → se piden confirmación individual.
   - `HUERFANOS` (en disco, no en TD) → NO se borran automáticamente;
     se listan para que el equipo decida.
3. **(c) Abortar** — útil si el equipo quiere revisar primero.

Esperar respuesta. Si el usuario elige (b), proceder con §3.3.

### 3.3 Diff mode — procedimiento detallado

1. Parsear la sección §1 del TD (árbol de carpetas con emoji 📁) y
   extraer **todos los paths relativos** que se listan.
2. Listar **todos los paths relativos** en `./{BC-PascalCase}/` con
   `Get-ChildItem -Recurse -File` y relativizar.
3. Calcular:
   - `A_GENERAR = TD_paths − Disk_paths`
   - `A_SOBRESCRIBIR = TD_paths ∩ Disk_paths`
   - `HUERFANOS = Disk_paths − TD_paths`
4. **Siempre** pedir confirmación por cada categoría antes de actuar.
   No asumir "sobrescribir todo lo que coincida".
5. Generar **solo** los archivos de `A_GENERAR` y los confirmados de
   `A_SOBRESCRIBIR`. Los `HUERFANOS` se reportan en el resumen final
   (§8) para que el equipo decida manualmente (puede ser código de
   infraestructura adicional, tests, etc.).

### 3.4 Política de archivos compartidos

`csproj`, `Program.cs`, `AppDbContext.cs`, `Shared/Domain/Model/ValueObjects/`
sean editados, se aplican estas reglas (sin importar el estado del BC):

- **csproj**: agregar `<Folder Include="{BC}\..." />` solo para carpetas
  NUEVAS. Verificar primero con `Select-String` que no estén.
- **Program.cs**: agregar `using` + `builder.Add{Bc}ContextServices();`
  solo si NO existe ya. Si existe, NO duplicar.
- **AppDbContext.cs**: agregar `using` + `modelBuilder.Apply{Bc}Configuration();`
  solo si NO existe ya.
- **Shared ValueObjects**: agregar un `record` solo si el path
  `Shared/Domain/Model/ValueObjects/{Name}.cs` NO existe.

En todos los casos: **agregar, nunca borrar ni renombrar**.

---

## §4. Flujo "Greenfield" — generación de las 4 capas

Si §3 dijo `NO_EXISTE` o el usuario eligió (a)/(b) en §3.2, generar
en este orden estricto, **una capa a la vez, validando que compile
conceptualmente antes de pasar a la siguiente**. Si una sección del TD
está vacía (ej: §2.5 Commands vacío) → omitir la carpeta, no inventar
archivos.

### 4.1 Domain Layer (§2 del TD)

#### 4.1.1 Aggregates (`Domain/Model/Aggregates/`)

Para cada Aggregate Root listado en §2.1 del TD:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Domain.Model.Aggregates`
- Clase: `public class {Name} : BaseAggregateRoot`
- **SIEMPRE** constructor privado sin parámetros (para EF Core).
- Propiedades `public {Type} {Name} { get; private set; }` (no setters
  públicos; ver patrón en `IAM/Domain/Model/Aggregates/User.cs`).
- Factory methods `public static {Name} Create(...)` que validan
  invariantes y disparan `RaiseDomainEvent(new {Name}{Action}Event(...))`.
- Métodos de negocio `public void {Action}(...)` que mutan estado y
  disparan eventos.
- Copiar **EXACTAMENTE** el código del bloque §2.1 del TD. Si el TD
  muestra snippets incompletos, completar con el patrón del BC más
  maduro del repo (típicamente `iam` o `profiles`) **citándolo
  explícitamente** en el comentario o en el resumen final.
- Para entidades internas (no aggregate root, ej: `TechnicianProfile`
  en `TD-Profiles.md:2.2`): misma estructura pero **sin heredar** de
  `BaseAggregateRoot`, y método `internal static` en vez de `public`.

#### 4.1.2 Value Objects (`Domain/Model/ValueObjects/`)

Para cada VO listado en §2.X del TD (típicamente 2.2, 2.4, 2.5 según BC):

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Domain.Model.ValueObjects`
- **Convención del repo**: `public record {Name}` con:
  - Propiedad `public string Value { get; }` (o múltiples props para
    VOs compuestos, ej: `PersonalData`, `Address`).
  - Constructor `private` (o vacío para `record` puro).
  - `public static {Name} Create(...)` con validaciones (`ArgumentException`).
  - Para VOs con TTL: agregar `DateTime ExpiresAt { get; }` y método
    `bool IsValid()` (ver `VerificationToken` en `IAM/Domain/Model/ValueObjects/`).
- IDs (`{Entity}Id`): prefijo `Value = "{slug}-{Guid.NewGuid()}"` salvo
  que el TD indique otra cosa (ver `UserId.NewUserId()` →
  `usr-{Guid.NewGuid()}`).
- Enums: `public enum {Name} { Value1, Value2, ... }` con valores
  `PascalCase` (ver `EProfileStatus`, `ESpecialty` en
  `Profiles/Domain/Model/ValueObjects/`). **Convención observada**: el
  repo usa prefijo `E` solo en `Profiles`; **NO asumir prefijo `E`
  universalmente**. Verificar la convención del BC objetivo en su
  carpeta `ValueObjects/` si existe.

#### 4.1.3 Commands (`Domain/Model/Commands/`)

Para cada comando de §2.X del TD:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Domain.Model.Commands`
- `public record {Name}Command({params}...)` — sin prefijo, sin sufijo.
- Sin lógica; solo datos. Validaciones en el handler del Application
  Layer o en el factory method del aggregate.

#### 4.1.4 Queries (`Domain/Model/Queries/`)

Idéntica convención que Commands: `public record {Name}Query({params}...)`.
Ubicación: `Domain/Model/Queries/`.

#### 4.1.5 Domain Events (`Domain/Model/Events/`)

Para cada evento de §2.X del TD:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Domain.Model.Events`
- `public record {Name}Event({params}...) : IEvent;` —
  **SIEMPRE** hereda de `Shared.Domain.Model.Events.IEvent`
  (ver `UserRegisteredEvent` en `IAM/Domain/Model/Events/`).
- **Validar payload**:
  - NO incluir passwords, hashes, tokens JWT, API keys, secretos.
  - Para tokens de verificación/reset, evaluar: si otros BCs lo
    necesitan → incluir; si solo lo consume este BC → NO incluir
    (mantener en el aggregate, no exponer).
  - Fechas como `DateTime OccurredAt` (UTC).

#### 4.1.6 Exceptions (`Domain/Model/Exceptions/`)

Para cada excepción listada en §2.X del TD (típicamente 2.X con
excepciones de dominio):

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Domain.Model.Exceptions`
- `public class {Name}Exception : Exception` con
  `public {Name}Exception(string message) : base(message) { }`.
- Si hay jerarquía (ej: `InvalidCredentialsException`,
  `UserTemporarilyLockedException` derivadas de una base), respetar
  la del TD.

#### 4.1.7 Repository Interfaces (`Domain/Repositories/`)

Una interfaz por aggregate principal + las adicionales que liste el TD:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Domain.Repositories`
- `public interface I{Aggregate}Repository : IBaseRepository<{Aggregate}, {Aggregate}Id>`
  con métodos específicos del TD: `FindByXxxAsync(...): Task<{Aggregate}?>`,
  `ExistsByXxx(...): bool`, etc.
- **NO** agregar métodos que no estén en el TD.

#### 4.1.8 Service Interfaces (`Domain/Services/`)

- `public interface I{Aggregate}CommandService` con un
  `Task Handle({Command} command);` por cada command de §2.X del TD.
- `public interface I{Aggregate}QueryService` con un
  `Task<{Result}> Handle({Query} query);` por cada query.
- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Domain.Services`
- Ubicación física: `Domain/Services/I{Aggregate}CommandService.cs`
  y `Domain/Services/I{Aggregate}QueryService.cs`.

### 4.2 Application Layer (§3 del TD)

#### 4.2.1 CommandServices (`Application/Internal/CommandServices/`)

Para cada interfaz de §4.1.8, su implementación:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Application.Internal.CommandServices`
- **Constructor primario** (estilo .NET 9):
  ```csharp
  public class {Name}CommandService(
      I{Aggregate}Repository {aggregate}Repository,
      IUnitOfWork unitOfWork,
      IMediator mediator,
      ILogger<{Name}CommandService> logger)
      : I{Aggregate}CommandService
  ```
  (Incluir `IMediator` y `ILogger` aunque el TD no los liste — son
  convención del repo, ver `IAM/Application/Internal/CommandServices/UserCommandService.cs:666-674`).
- Métodos `Handle`:
  1. Validaciones de input (ej: `command.Password != command.PasswordConfirmation`).
  2. Lectura vía repository.
  3. Invocación del aggregate (`{Aggregate}.Create(...)` o
     `{aggregate}.{Method}(...)`).
  4. Persistencia: `await {aggregate}Repository.AddAsync({aggregate});`
     o `.Update(...)`.
  5. `await unitOfWork.CompleteAsync();`
  6. Dispatch de eventos:
     ```csharp
     foreach (var ev in {aggregate}.DomainEvents)
         await mediator.Publish(ev, CancellationToken.None);
     {aggregate}.ClearDomainEvents();
     ```
  7. Side-effects (ej: `emailNotificationService.Send...`) si el TD
     los lista.
  8. Logging con `ILogger.LogInformation("[{BC-upper} BC] {acción}", ...)`.

#### 4.2.2 QueryServices (`Application/Internal/QueryServices/`)

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Application.Internal.QueryServices`
- **NO** usar `IMediator` ni `IUnitOfWork` (las queries no mutan).
- `Task<{Result}?> Handle({Query} query)` que delega al repository.
- Para queries que devuelven listas grandes, recordar: usar
  `AsNoTracking()` en el repository (no en el query service).

#### 4.2.3 EventHandlers (`Application/Internal/EventHandlers/`)

Para cada evento en `ResumenElectrolink-IOT.md` §4 cuyo destino sea
este BC, crear un handler:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Application.Internal.EventHandlers`
- Implementar `Shared.Application.Internal.EventHandler.IEventHandler<TEvent>`.
- Naming: `{EventName}Handler.cs`.
- Lógica: típicamente dispara un `CreateXxxCommand` que se enruta al
  `I{BC-pascal}CommandService.Handle(...)` del BC destino.
- Marcar handlers como **idempotentes** (verificar si el aggregate ya
  existe con `ExistsByXxx` antes de crearlo — patrón observado en
  `Profiles/Application/Internal/EventHandlers/UserRegisteredEventHandler.cs`).

#### 4.2.4 OutboundServices / ACLs outbound (`Application/Internal/OutboundServices/`)

Si el BC consume ACLs de otros BCs (ver `ResumenElectrolink-IOT.md` §5
o §4 columna "BCs que Escuchan" para los eventos que este BC dispara):

- Un archivo de **puerto (interfaz)**: `I{Service}Port.cs` con los
  métodos exactos que el BC necesita.
- Un archivo de **adaptador**: `External{Service}Service.cs` que
  implementa el puerto. Si el sistema externo es interno (otro BC),
  el adaptador **delega** al `I{BC-origen}ContextFacade` del namespace
  de ese BC (NO al facade de este BC).

#### 4.2.5 Anti-Corruption Layer (`Application/ACL/`)

Si el TD §5 lista una fachada para que **otros BCs** consuman este BC:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Application.ACL`
- Clase: `{BC-pascal}ContextFacade : I{BC-pascal}ContextFacade`.
- Implementación: delega a `I{BC-pascal}CommandService` (writes) y
  `I{BC-pascal}QueryService` (reads). Devuelve DTOs **específicos del
  ACL** (NO los `Resources` de REST; estos son distintos).
- **Convención observada**: `Profiles` tiene la ACL en
  `Profiles/Application/ACL/ProfilesContextFacade.cs`; `IAM` la tiene
  en `IAM/Interfaces/ACL/Services/IamContextFacade.cs`. **Respetar
  la convención del BC objetivo**; verificar si ya existe la
  estructura antes de decidir.

### 4.3 Infrastructure Layer (§4 del TD)

#### 4.3.1 EF Core Configurations (`Infrastructure/Persistence/EFC/Configurations/`)

Una clase por aggregate listado en §4.1.1:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Infrastructure.Persistence.EFC.Configurations`
- `public class {Aggregate}Configuration : IEntityTypeConfiguration<{Aggregate}>`
- `Configure(EntityTypeBuilder<{Aggregate}> builder)`:
  - `builder.ToTable("{bc}_xxx_yyy")` (snake_case, prefijo del BC).
  - PK + conversión desde VO Id:
    `builder.Property(u => u.Id).HasConversion(id => id.Value, value => {Aggregate}Id.From(value)).HasColumnName("id").IsRequired().ValueGeneratedNever();`
  - Conversión de VOs simples (`HasConversion(un => un.Value, value => {VO}.Create(value))`).
  - VOs complejos con TTL: `builder.OwnsOne(u => u.{Token}, vt => { vt.Property(t => t.Value).HasColumnName("..."); vt.Property(t => t.ExpiresAt).HasColumnName("..."); });`
  - Enums: `builder.Property(u => u.Status).HasConversion<string>().HasColumnName("status").IsRequired();`
  - Índices únicos donde aplique (ej: `username` UNIQUE).
  - `builder.Ignore(p => p.DomainEvents);` (los eventos NO se persisten).
- Auditar `BaseAggregateRoot` ya tiene `CreatedDate`/`UpdatedDate` (vía
  `EntityFrameworkCore.CreatedUpdatedDate`, ver csproj:40 y
  `BaseAggregateRoot.cs:23-24`). NO redeclarar.

#### 4.3.2 ModelBuilderExtensions (`Infrastructure/Persistence/EFC/Configuration/Extensions/`)

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Infrastructure.Persistence.EFC.Configuration.Extensions`
- Clase `static` con método:
  ```csharp
  public static class ModelBuilderExtensions
  {
      public static void Apply{Bc-pascal}Configuration(this ModelBuilder modelBuilder)
      {
          modelBuilder.ApplyConfiguration(new {Aggregate1}Configuration());
          modelBuilder.ApplyConfiguration(new {Aggregate2}Configuration());
          // ... uno por aggregate
      }
  }
  ```
- Este método se invoca desde `Shared/Infrastructure/Persistence/EFC/Configuration/AppDbContext.cs`
  en `OnModelCreating` (ver §5.3).

#### 4.3.3 Repositories (`Infrastructure/Persistence/EFC/Repositories/`)

Una implementación por interfaz de §4.1.7:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Infrastructure.Persistence.EFC.Repositories`
- `public class {Aggregate}Repository(AppDbContext context) : BaseRepository<{Aggregate}, {Aggregate}Id>(context), I{Aggregate}Repository`
- Implementar los métodos específicos del TD. Patrón:
  ```csharp
  public async Task<{Aggregate}?> FindByXxxAsync(string xxx) =>
      await Context.Set<{Aggregate}>()
          .FirstOrDefaultAsync(a => a.Xxx == XxxVO.Create(xxx));
  ```
- **NO** incluir lógica de negocio aquí. Solo queries.

#### 4.3.4 Servicios técnicos (`Infrastructure/{Service}/`)

Si el TD §4 lista integraciones técnicas (Hashing, JWT, Cloudinary,
Stripe, OpenAI, etc.):

- Subcarpeta por servicio: `Infrastructure/Hashing/BCrypt/`,
  `Infrastructure/Tokens/JWT/`, `Infrastructure/Cloudinary/`, etc.
- Un `Configuration/{Service}Settings.cs` con la clase de settings
  bindable desde `appsettings.json` (ver patrón en
  `IAM/Infrastructure/Tokens/JWT/Configuration/TokenSettings.cs`).
- Un `Services/{Service}Service.cs` con la implementación concreta.
- Si la integración es **opcional en MVP** (ej: Cloudinary cuando no
  hay fotos): agregar en `Shared/Infrastructure/ExternalProviders/`
  un `Null{Service}Provider.cs` (ver
  `Shared/Infrastructure/ExternalProviders/NullFileStorageProvider.cs`).
  El DI extension method (§6.1) decidirá cuál binding usar.

### 4.4 Interfaces Layer (§5 del TD)

#### 4.4.1 Resources (`Interfaces/REST/Resources/`)

Un DTO por cada endpoint del TD §5:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Interfaces.REST.Resources`
- `public record {Name}Resource({params}...)` (record inmutable,
  misma convención que los `*Resource.cs` de `IAM/Interfaces/REST/Resources/`).
- Naming: input resources terminan en `*Resource` (ej: `SignUpResource`),
  output resources también (ej: `AuthenticatedUserResource`,
  `UserResource`).

#### 4.4.2 Assemblers / Transform (`Interfaces/REST/Transform/`)

Un assembler por cada command/query y por cada response:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Interfaces.REST.Transform`
- `public static class {Name}CommandFromResourceAssembler`
  con `public static {Name}Command ToCommandFromResource({Name}Resource resource) => new(...);`
- `public static class {Name}ResourceFromEntityAssembler`
  con `public static {Name}Resource ToResourceFromEntity({Aggregate} entity) => new(...);`
- **NO** usar AutoMapper (no está en el csproj). Manual siempre.

#### 4.4.3 Controllers (`Interfaces/REST/`)

Uno o varios controllers según §5 del TD:

- Namespace: `Hampcoders.Electrolink.API.{BC-upper}.Interfaces.REST`
- Atributos obligatorios:
  ```csharp
  [ApiController]
  [Route("api/v1/[controller]")]
  [Produces(MediaTypeNames.Application.Json)]
  [SwaggerTag("{BC-pascal} endpoints")]
  ```
- Constructor primario con servicios inyectados.
- Cada método:
  - `[HttpGet|HttpPost|HttpPut|HttpPatch|HttpDelete("{route}")]`
  - `[SwaggerOperation(Summary = "...", OperationId = "...")]`
  - `[SwaggerResponse(200|201|204|400|401|403|404|409|422, "...")]`
  - `try/catch (Exception ex)` con códigos semánticos:
    - `ArgumentException` o validación → `BadRequest(new { message = ex.Message })`
    - `InvalidCredentialsException` o similar → `Unauthorized(...)`
    - Aggregate not found → `NotFound()`
  - **NUNCA** `GET` para mutar (verificar contra el TD; si el TD lista
    un `GET` que muta → **denunciarlo** en el resumen final §8 como
    discrepancia, no reproducirlo).
  - **NUNCA** devolver `200 OK` con cuerpo de error.
- Llamar a los assemblers, NO construir commands manualmente.

#### 4.4.4 ACL Interface (`Interfaces/ACL/`)

(Ya cubierto conceptualmente en §4.2.5; verificar convención
`Application/ACL/` vs `Interfaces/ACL/` del BC objetivo y respetar.)

- Interfaz: `public interface I{BC-pascal}ContextFacade` con
  `Task<{Dto}> {Method}Async(...)` para cada método que el TD §5
  declara público hacia otros BCs.
- Implementación: misma convención de namespace que el resto de
  fachadas del repo.

---

## §5. Cableado de archivos compartidos

Aplicar ANTES del primer `dotnet build` del equipo. **Siempre agregar,
nunca borrar ni renombrar.**

### 5.1 `Hampcoders.Electrolink.API.csproj`

Agregar `<Folder Include="..." />` por cada subcarpeta de primer nivel
**nueva** del BC (las que el csproj necesita declarar para que el
compilador las incluya; ver patrón en csproj:62-71):

```xml
<Folder Include="{BC-PascalCase}\Application\" />
<Folder Include="{BC-PascalCase}\Domain\" />
<Folder Include="{BC-PascalCase}\Infrastructure\" />
<Folder Include="{BC-PascalCase}\Interfaces\" />
```

Si ya existen en el csproj (verificar con `Select-String` antes de
agregar), no duplicar.

Si el BC incluye archivos de configuración no-C# que deben copiarse al
output (ej: `appsettings`, prompts, JSON schemas), agregar:

```xml
<None Update="{path}">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</None>
```

(Ver ejemplo con `Planning\Infrastructure\ExternalProviders\Prompts\matching-system-prompt.txt`
en csproj:79-81.)

### 5.2 `Program.cs`

Verificar con `Select-String "Add{Bc-pascal}ContextServices"` si ya
existe la llamada (puede que el BC ya esté parcialmente cableado).

Si NO existe:

1. Agregar `using` después de los existentes:
   ```csharp
   using Hampcoders.Electrolink.API.{BC-upper}.Infrastructure.Interfaces.ASP.Configuration.Extensions;
   ```
2. Agregar la llamada a `builder` después de las demás `builder.Add*ContextServices()`:
   ```csharp
   builder.Add{Bc-pascal}ContextServices();
   ```

Si existe → no tocar.

### 5.3 `Shared/Infrastructure/Persistence/EFC/Configuration/AppDbContext.cs`

Verificar con `Select-String "Apply{Bc-pascal}Configuration"` si ya
está aplicado.

Si NO está:

1. Agregar `using`:
   ```csharp
   using Hampcoders.Electrolink.API.{BC-upper}.Infrastructure.Persistence.EFC.Configuration.Extensions;
   ```
2. Agregar dentro de `OnModelCreating` (después de las otras
   `modelBuilder.Apply*Configuration();`):
   ```csharp
   modelBuilder.Apply{Bc-pascal}Configuration();
   ```

### 5.4 `Shared/Domain/Model/ValueObjects/`

Si el TD lista IDs externos (ej: `TD-Profiles.md` lista `UserId` como
referencia a IAM), verificar `Test-Path` en
`Shared/Domain/Model/ValueObjects/{Name}.cs`:

- Si EXISTE → no tocar (común: `Shared/Domain/Model/ValueObjects/UserId.cs`
  ya está y se reutiliza).
- Si NO EXISTE → crear el `record` siguiendo el patrón:
  ```csharp
  namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

  public record {ExternalBc}Id
  {
      public string Value { get; }
      private {ExternalBc}Id(string value) => Value = value;
      public static {ExternalBc}Id From(string value)
      {
          if (string.IsNullOrWhiteSpace(value))
              throw new ArgumentException("{ExternalBc}Id cannot be empty.");
          return new {ExternalBc}Id(value);
      }
  }
  ```
- **PROHIBIDO** agregar un factory `New{ExternalBc}Id()` en IDs que
  representan referencias externas (eso solo aplica a IDs propios del
  BC). Ver patrón en `Shared/Domain/Model/ValueObjects/UserId.cs`:
  solo `From(string)`, no `NewUserId()`.

### 5.5 Conflictos en Shared

Si la skill detecta que un cambio en `Shared/` puede romper a otro
BC (ej: eliminar un `record` que `Profiles` aún usa), **DETENER** y
preguntar al usuario antes de modificar.

---

## §6. Wiring interno del BC

### 6.1 DI extension method

Crear **un único** archivo:

```text
{BC-PascalCase}/Infrastructure/Interfaces.ASP/Configuration/Extensions/WebApplicationBuilderExtensions.cs
```

Contenido (adaptar a los servicios del BC):

```csharp
using ...;

namespace Hampcoders.Electrolink.API.{BC-upper}.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void Add{Bc-pascal}ContextServices(this WebApplicationBuilder builder)
    {
        // Repositories
        builder.Services.AddScoped<I{Aggregate}Repository, {Aggregate}Repository>();

        // Command/Query Services
        builder.Services.AddScoped<I{Aggregate}CommandService, {Aggregate}CommandService>();
        builder.Services.AddScoped<I{Aggregate}QueryService, {Aggregate}QueryService>();

        // ACL (si el BC expone fachada)
        builder.Services.AddScoped<I{BC-pascal}ContextFacade, {BC-pascal}ContextFacade>();

        // Outbound ports → adaptadores (o Null*Provider si es opcional)
        builder.Services.AddScoped<I{External}Port, External{External}Service>();

        // JWT/Auth pipeline (solo si el BC tiene controllers con [Authorize])
        // builder.Services.AddHttpContextAccessor();
    }
}
```

Si el BC tiene autenticación propia (como `IAM` con
`RequestAuthorizationMiddleware`):

- Copiar el patrón de `IAM/Infrastructure/Pipeline/Middleware/...`
  (Attributes, Components, Extensions) **solo si el TD lo lista** en §4.
- Registrar `app.UseRequestAuthorization()` en `Program.cs` solo si NO
  está ya (ver `Program.cs:189`).

### 6.2 Eventos cross-BC (publishers y handlers)

#### 6.2.1 Decisión: `IMediator` vs `IIntegrationEventPublisher`

- **`IMediator` (MediatR)**: usado en `IAM` y `Profiles` para eventos
  **in-process**. Bajo acoplamiento, sin outbox.
- **`IIntegrationEventPublisher` (Cortex/Outbox)**: usado en
  `Subscriptions` (ver `Shared/Application/Internal/EventPublisher/IntegrationEventPublisher.cs`)
  y en `Shared/Infrastructure/BackgroundServices/OutboxProcessorBackgroundService.cs`.
  Para eventos que deben **persistir antes de publish** (outbox pattern),
  garantizando entrega aunque el broker externo esté caído.

**Regla**: si el BC destino está **dentro del mismo proceso** (todos
los BCs de este repo lo son), cualquiera de los dos funciona. Respetar
la convención ya establecida en el BC destino:

- Si el BC destino YA usa MediatR para sus eventos internos → usar
  `IMediator.Publish(...)` en el publicador.
- Si el BC destino YA usa `IIntegrationEventPublisher` → usar ese y
  guardar el evento en `OutboxMessage` antes de publicar.

Para los BCs nuevos (analytics, processing): por defecto usar
`IIntegrationEventPublisher` (es la dirección que el repo está tomando
para los BCs críticos, ver `auditoria-integracion-sistemas-externos.md`).

#### 6.2.2 Handlers de eventos consumidos

Por cada fila de `ResumenElectrolink-IOT.md` §4 donde el BC destino
es este BC:

1. Crear `{EventName}Handler.cs` en `Application/Internal/EventHandlers/`
2. Implementar `IEventHandler<TEvent>` de
   `Shared.Application.Internal.EventHandler`.
3. En el constructor, inyectar `I{BC-pascal}CommandService` (no el
   repository directamente).
4. En `HandleAsync`, construir el command apropiado y llamar al
   `CommandService.Handle(command)`.
5. **Idempotencia**: el primer check del CommandService debe ser
   `if (repository.ExistsByXxx(...)) return;` (ver patrón en
   `ProfileCommandService.Handle(CreateProfileCommand)` en
   `Profiles/Application/Internal/CommandServices/`).

#### 6.2.3 Registro de handlers

En `Add{Bc-pascal}ContextServices`:

- Si los handlers usan Cortex/MediatR: el escaneo por reflexión del
  `Program.cs:166` (`AddMediatR(typeof(SubscriptionCommandService).Assembly)`)
  los descubre automáticamente.
- Si usan `IEventHandler<T>`: registrar manualmente:
  ```csharp
  builder.Services.AddScoped<IEventHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();
  ```
  (verificar convención del BC objetivo).

---

## §7. Reglas anti-alucinación (transversales)

Aplican a TODAS las fases:

1. **El TD es ley**. Si el TD lista 7 commands, generar 7 commands.
   No agregar un octavo "porque parece útil".
2. **NO inventar código no presente en el TD**. Si el TD no menciona
   un evento, un VO, un método, un endpoint → no se crea.
3. **NO copiar del reference** (`docs/examples/`). Es conceptual, de
   otro stack. Extraer solo el QUÉ.
4. **NO evaluar conceptos no documentados** en `ProjectArchitecture.md`.
   No agregar gRPC, GraphQL, HATEOAS, content negotiation `hal+json`,
   OpenAPI extensions, etc.
5. **NO generar tests**. Esta versión no incluye proyecto de tests;
   el equipo los agrega por su cuenta.
6. **NO inventar la Edge API ni el firmware ESP32**. Esos viven en
   otros stacks. El BC `processing` solo cubre la capa .NET cloud
   (no el firmware ni Flask).
7. **NO correr `dotnet build` ni `dotnet test`**. La skill ENTREGA
   código; compilar y probar es responsabilidad del equipo.
8. **NO modificar** `docs/`, `docs/examples/`, `tactical-docs/`,
   `tactical-docs/output/`, `docs/tacticals/TD-{otro-BC}.md` (solo
   el TD del BC target si el usuario lo pide explícitamente, en cuyo
   caso aplicar la política de §3.4).
9. **NO renombrar ni borrar** archivos existentes. Solo agregar.
10. **NO inventar conexiones a sistemas externos**. Si el TD no
    menciona Stripe/Cloudinary/OpenAI, no se agrega esa integración.
11. **Validar paths empíricamente** con `Test-Path` y `glob` antes
    de `Read` o `Write`. Nunca asumir existencia.
12. **Cero GET que muta**. Si el TD lista un endpoint GET que muta
    estado → denunciarlo en §8, no implementarlo.
13. **Cero secretos en eventos**. Si un evento del TD incluye
    passwords, tokens, API keys → denunciarlo en §8.
14. **Cero 200 con cuerpo de error**. Si el TD sugiere devolver
    `200 OK { error: "..." }` → ajustar a `4xx` con código semántico
    y documentar la desviación en §8.

---

## §8. Operación de la skill

### 8.1 Permisos

- **Read**: permitido en todo el repo (código, docs, examples).
- **Write**: permitido **únicamente** en:
  - Archivos nuevos bajo `{BC-PascalCase}/`.
  - `Hampcoders.Electrolink.API.csproj` (solo agregar `<Folder>`).
  - `Program.cs` (solo agregar `using` + `builder.Add...()`).
  - `Shared/Infrastructure/Persistence/EFC/Configuration/AppDbContext.cs`
    (solo agregar `using` + `modelBuilder.Apply...()`).
  - `Shared/Domain/Model/ValueObjects/{NewId}.cs` (solo crear nuevos).
- Cualquier otro `write` debe ser reportado al usuario antes de ejecutarse.

### 8.2 Bash

Permitido para verificación de paths (`Test-Path`, `Get-ChildItem`,
`glob`, `Select-String`, `Select-String -NotMatch`). NO ejecutar
`dotnet build`, `dotnet test`, `git commit`, ni comandos destructivos.

### 8.3 Idempotencia

La skill es regenerable. Cada invocación:

- En modo greenfield: produce un conjunto completo de archivos.
- En modo diff (§3.3): solo crea `A_GENERAR` y confirmados de `A_SOBRESCRIBIR`.
- En cualquier modo: el resumen final enumera **todo** lo escrito.

### 8.4 Orden de ejecución

```text
§1 Extracción del BC
        ↓
§2 Lectura de los 4 documentos
        ↓
§3 Detección de estado + política de choque
        ↓
[pregunta al usuario si EXISTE_*]
        ↓
§4 Generación de las 4 capas (Domain → Application → Infrastructure → Interfaces)
        ↓
§5 Cableado de archivos compartidos
        ↓
§6 Wiring interno (DI + eventos)
        ↓
§7 Auto-verificación anti-alucinación
        ↓
§8 Resumen al usuario
```

Si una fase falla (ej: falta TD en §2.1), las fases siguientes no se
ejecutan. Se reporta al usuario.

### 8.5 Idioma y formato

- Toda la salida de la skill (resúmenes, reportes, discrepancias) en
  **español**, consistente con los TDs existentes.
- Código, paths, identificadores técnicos en su idioma original
  (inglés por convención de C#/.NET).
- Fechas en formato **ISO 8601** (`YYYY-MM-DD`).

### 8.6 Resumen al usuario

Al terminar, la skill reporta en la conversación:

```text
✅ Implementación de {BC} completada.
   • Estado detectado: {NO_EXISTE | EXISTE_PARCIAL | EXISTE_COMPLETO}
   • Política aplicada: {greenfield | sobrescritura total | diff mode}
   • Archivos creados: {N} (.cs en {BC}/)
   • Archivos modificados (compartidos):
     - {path relativo}
     - {path relativo}
   • Discrepancias con el TD: {N} (ver lista abajo)
   • Próximo paso sugerido: {1 línea accionable, ej: "agregar <Folder> en csproj y correr dotnet build"}
```

Si hubo discrepancias (§7.12, §7.13, §7.14 u otras), listarlas en
bullets con path del TD + path del código generado + acción recomendada.

### 8.7 Smoke test sugerido

Orden de menos a más completo:

1. **`processing`** — `NO_EXISTE`. Sin código, sin carpetas.
   Valida el flujo greenfield §4 completo y el cableado §5 sobre un
   csproj/Program.cs que NO tiene nada de `processing` aún.
2. **`analytics`** — `NO_EXISTE` con reference en `docs/examples/`.
   Valida §2.4 (extracción conceptual sin copiar nombres Java/Spring)
   y §6.2.2 (handlers de eventos IoT Monitoring → Analytics).
3. **`subscriptions`** — `EXISTE_PARCIAL`. Carpeta existe con
   `StripeService.cs` y `StripeWebhookController.cs`. Valida §3.2
   (pregunta al usuario) y §3.3 (diff mode): el `StripeService.cs`
   NO debe regenerarse, pero los controllers y assemblers faltantes SÍ.
4. **`iam`** — `EXISTE_COMPLETO`. Valida §3.2 con respuesta esperada
   (b) o (c) del usuario, y que la skill no intente sobrescribir.

Para cada caso, validar:

1. Los archivos generados existen y tienen contenido no trivial.
2. El cableado a `csproj`/`Program.cs`/`AppDbContext` se aplicó
   (verificar con `Select-String` que los `using` y llamadas están).
3. Ningún archivo de otro BC fue modificado.
4. Si hubo diff mode: los `HUERFANOS` están listados en el resumen §8.6
   pero NO eliminados.

Si algún punto falla, ajustar la skill y re-ejecutar.
