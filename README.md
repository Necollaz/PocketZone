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
    - Уметь добавлять/удалять предметы.
  - **InventoryView** + **InventoryController**:
    - Toggle панели инвентаря кнопкой, динамически строит ячейки (`InventorySlotView`) при `InventoryChanged`.
    - Возможность удалить предмет из конкретного слота.
  - **Ammo** (модель) и **AmmoService** (сохранение/загрузка JSON).
    - Событие `AmmoCountChanged`, методы `Add(int)` и `Use(int)`.
  - **AmmoController** - при добавлении/использовании патронов синхронизирует Inventory и AmmoService.
  - **InventoryAmmoSyncService** - единоразовая синхронизация между JSON и UI при старте и динамическая очистка.
  - **ItemPickup** - добавление в инвентарь или патронов при пересечении триггера.

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

  - DI (Zenject, MonoInstaller): привязка моделей, сервисов, контроллеров, view-компонентов
  - Models: **LivingEntity**, **Player**, **Enemy**, **Inventory**, **Ammo**, **Bullet**
  - Services: **InputService**, **InventoryService**, **AmmoService**, **EnemyAIService**, **FilePersistenceService**, **InventoryAmmoSyncService**
  - Controllers:
      - **PlayerController** (**Movement**, **Shooting**, **Health**, **InventoryAmmo**)
      - **InventoryController**, **AmmoController**
      - **EnemySpawner**, **DeathHandler**
  - Views: **PlayerView**, **EnemyView**, **WeaponView**, **HealthView**, **InventoryView**, **SimpleJoystick**
  - ScriptableObject **PlayerConfig** хранит все параметры (скорость, здоровье, пул, урон и т.д.)