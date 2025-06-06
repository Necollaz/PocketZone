# PocketZone

## Что реализовано

- **Персонаж (Player)**
  - Движение 2D (клавиатура и виртуальный джойстик).
  - Анимации покоя, ходьбы и атаки.
  - Стрельба:
    - Если в радиусе атаки игрока нет врагов — пуля летит строго горизонтали.
    - Если враги в радиусе — пуля целится в ближайшего врага.
  - Полоса здоровья над персонажем. При смерти – перезагрузка сцены.
  - Сбор и использование патронов: при подборе предмета увеличиваются патроны, при стрельбе уменьшаются.

- **Враги (Enemy)**
  - Спавн в пределах заданной зоны: случайная точка внутри.
  - Полоса здоровья над врагом.
  - AI, разбитый на сервисы:
    1. **EnemyRotation** – разворот лицом к игроку.
    2. **EnemyMovement** – преследование игрока в пределах _detectionRadius_ и возврат на спавн.
    3. **EnemyAttack** – проверка кулдауна и дистанции (_attackDistance_), запуск анимации атаки и нанесение урона.
    4. **EnemyAIService** – объединяет всё вместе.

- **Инвентарь и патроны**
  - **Inventory** (модель слотов заданного размера) и **InventoryService** (сохранение/загрузка в JSON через `FilePersistenceService`).
    - Уметь добавлять/удалять предметы, генерировать событие `InventoryChanged`.
  - **InventoryView** + **InventoryController**:
    - Toggle панели инвентаря кнопкой, динамически строит ячейки (`InventorySlotView`) при `InventoryChanged`.
    - Возможность удалить предмет из конкретного слота.
  - **Ammo** (модель) и **AmmoService** (сохранение/загрузка JSON).
    - Событие `AmmoCountChanged`, методы `Add(int)` и `Use(int)`.
  - **AmmoController** – связывает AmmoService ↔ InventoryService ↔ InventoryView:
    - При добавлении патронов в модель сразу создаёт или обновляет слот `"Ammo"` в инвентаре.
    - При использовании патронов уменьшает их число в обоих хранилищах.
    - Инициализация: синхронизирует модель и инвентарь при старте (разность между JSON и текущим Inventory).

- **Пулл пуль (Object Pooling)**
  - **BulletPool** – заранее создаёт нужное количество объектов `Bullet` (Queue).
  - **Bullet**:
    - Кинематический `Rigidbody2D` + `Collider2D` (IsTrigger = true).
    - Движение в направлении `SetDirection(dir)`, скорость `_speed`, урон `_damage`, время жизни `_lifetime`.
    - При столкновении с `IDamageable` наносит урон и возвращается в пул.
    - При столкновении со слоем “Obstacle” или по истечении `_lifetime` – возвращается в пул.

- **Сохранение и загрузка (Persistence)**
  - **IPersistenceService** ↔ **FilePersistenceService** (JSON-файлы в `Application.persistentDataPath`).
  - **InventorySaveData** и **AmmoSaveData** – DTO для сериализации C# → JSON.
  - При старте `InventoryService.Load()` и `AmmoService.Load()` восстанавливают предыдущее состояние из JSON.

- **Вспомогательные элементы UI**
  - **HealthView** — обновляет `Slider` (World Space для врагов и для игрока).
  - **InventorySlotView** — одна ячейка инвентаря (Icon, CountText, RemoveButton).
  - **InventoryView** — панель инвентаря, кнопка открыть/закрыть, выстраивание всех `InventorySlotView`.
  - **WeaponView** — инициализация `BulletPool` и метод `Shoot(dir)` (брать пулл → установить позицию → задать направление → активировать).
  - **SimpleJoystick** — виртуальный джойстик (UI + Canvas), реализует `IJoystick` для мобильного ввода.

---

## Архитектура

- **InstallBindings (Zenject / MonoInstaller)**
  - **Models**
    - `LivingEntity`  – базовый класс для здоровья
    - `Player`       – наследник `LivingEntity`
    - `Enemy`        – наследник `LivingEntity`
    - `Inventory`    – модель массива `Item[]`, событие `InventoryChanged`
    - `Ammo`         – модель патронов, событие `AmmoCountChanged`
    - `Bullet`       – MonoBehaviour, представляет пулю, возвращается в пул
    - `BulletPool`   – обёртка для пула объектов `Bullet`
  - **Services**
    - `InputService`           – горизонталь/вертикаль, `Fire1`, поддержка джойстика
    - `InventoryService`        – логика инвентаря + сохранение/загрузка
    - `AmmoService`             – логика патронов + сохранение/загрузка
    - `EnemyAIService`          – управление логикой врага: Rotation/Movement/Attack
    - `FilePersistenceService` – `IPersistenceService` → JSON в `Application.persistentDataPath`
  - **Controllers**
    - `PlayerController`  (ITickable, связывает `Player` ↔ `PlayerView` ↔ `HealthView(UI)` ↔ `WeaponView` ↔ `InventoryView` ↔ `AmmoController`)  
      └─ `HandleMovement`, `HandleShooting`, `UpdateHealth` UI, перезагрузка сцены при смерти  
    - `InventoryController` (связывает `InventoryService` ↔ `InventoryView`: Toggle, Remove, RebuildSlots)  
    - `AmmoController`       (`AmmoService` ↔ `InventoryService` ↔ `InventoryView`: синхронизация, UI-патронов)  
    - `EnemySpawner`        (MonoBehaviour, Zenject-спавн врагов в заданной зоне)
  - **Views**
    - `PlayerView`            (MonoBehaviour, `Rigidbody2D` + `Animator` + методы `SetVelocity`, `SetWalkDirection`, `PlayAttack`)
    - `EnemyView`             (MonoBehaviour, `Collider2D`, `HealthView (World Space)`, `TakeDamage`, `OnDied` → дроп предмета)
    - `WeaponView`            (MonoBehaviour, инициализация пула, метод `Shoot`)
    - `HealthView`            (MonoBehaviour с `Canvas`, обновление `Slider`; RenderMode настраивается через инспектор)
    - `InventoryView`         (MonoBehaviour, UI-панель с кнопкой Toggle, слоты `InventorySlotView`, текст патронов)
    - `InventorySlotView`     (UI-ячейка, Icon + CountText + RemoveButton)
    - `SimpleJoystick`        (MonoBehaviour, UI-джойстик, реализует `IJoystick`)
    - `ItemPickup`            (MonoBehaviour + `Collider2D`, при `OnTriggerEnter2D` добавляет `Item` или `Ammo`)
  - **Configs (ScriptableObject)**
    - `PlayerConfig` – скорость, MaxHealth, `BulletPrefab`, пул, урон, скорость, время жизни, `ShootRange`, `EnemyLayerMask`
---

## Использованные инструменты и подходы

1. **Unity 2D**  
   - Rigidbody2D, Collider2D, Animator, Tilemap (для финальной карты), Canvas (World Space для врагов, Screen Space – Overlay для UI).
   - UI: Slider, Button, TextMeshProUGUI, RectTransform, LayoutRebuilder.

2. **Dependency Injection (Zenject)**  
   - `Container.Bind<…>` и `FromInstance`/`AsSingle`, `BindInterfacesAndSelfTo<…>().NonLazy()`.  
   - Конструкторы с `[Inject]` для всех Controllers, Services, View-компонентов, позволяющие легко заменить зависимости.

3. **ScriptableObject**  
   - `PlayerConfig` хранит параметры: скорость, здоровье, пул пуль (префаб + размеры), урон, скорость пули, время жизни, радиус стрельбы, слой врагов.  

4. **Object Pooling**  
   - `BulletPool` — избежание лишних Instantiate/Destroy при стрельбе.
   - `Bullet.SharedPool` для возврата в пул без прямой ссылки на пулл-менеджер.

5. **Сохранение в JSON**  
   - `IPersistenceService` → `FilePersistenceService` (сохраняет в `Application.persistentDataPath`)  
   - `InventorySaveData` и `AmmoSaveData` — сериализуемые структуры для хранения в JSON  
   - При старте `Load()`, при изменениях `Save()`.

6. **Animation Events**  
   - В клипе “Attack” у врага вызываются `OnDealDamage()` и `OnAttackEnded()` для синхронизации анимации и логики атаки.

7. **Реализация UI-джойстика (Mobile-Friendly)**  
   - `SimpleJoystick` (реализует `IJoystick`) — при нажатии появляется полупрозрачный фон+ручка, при движении рука смещается в пределах радиуса, при отпускании возвращается в ноль.

8. **Разделение ответственности (SRP)**  
   - Модели отвечают за хранение состояния (здоровье, инвентарь, патроны).  
   - Сервисы — за логику (управление инвентарём, патронами, ИИ, вводом, сохранением).  
   - Контроллеры — за связь Model/Service ↔ View  
   - View — за отрисовку: UI-элементы, анимации, Rigidbody2D и коллайдеры.