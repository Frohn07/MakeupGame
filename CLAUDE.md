# MakeupGame — Project Context for Claude

## Project
Unity тестовое задание — игра-макияж (крем, тени, помада, румяна).
Демонстрация SOLID + Zenject архитектуры для Junior Unity позиции.

## Branches
- `feature/application-of-makeup-tools` — основная механика
- `feature/visual-polish` — визуал, Canvas, анимации
- `main` — для PR

## Plugins
- **Zenject** — Dependency Injection
- **DOTween** — анимации движения руки

## Architecture

```
Assets/Scripts/
├── Core/
│   ├── Hand.cs                — движение руки (DOTween), SetParent инструмента
│   ├── BaseTool.cs            — abstract, ITool, ShelfPosition/DipPosition/ToolTransform
│   ├── FaceZone.cs            — триггер зоны лица, вызывает Apply() + ReturnToShelf()
│   └── Interfaces/
│       ├── ITool.cs           — ShelfPosition, DipPosition, ToolTransform, Apply()
│       └── IDraggable.cs      — OnDragStart/OnDrag/OnDragEnd
├── Data/
│   ├── Enums/MakeupCategory.cs — Lipstick=0, Eyeshadow=1, Blush=2, Powder=3
│   ├── MakeupItemData.cs       — ScriptableObject: Icon, TintColor, ResultOverlay
│   └── MakeupConfig.cs         — ScriptableObject: списки по категориям
├── Services/
│   ├── IMakeupService.cs       — SelectItem, ResetAll, события
│   ├── MakeupService.cs        — Dictionary<category, item>
│   ├── IProgressService.cs
│   └── ProgressService.cs      — PlayerPrefs
├── Controllers/
│   ├── MakeupController.cs     — event OnItemChosen(MakeupItemData, Vector3)
│   │                             event OnCategoryChanged(MakeupCategory)
│   └── DragController.cs       — роутит Input → IDraggable
├── Tools/
│   ├── LipstickTool.cs         — подписывается на свой ColorItemView (GetComponent)
│   ├── BlushTool.cs            — динамический DipPosition, управляет BrushTip
│   ├── EyeshadowTool.cs        — то же что BlushTool
│   ├── CreamTool.cs            — кнопка → OnTapped() → PickUp
│   └── PowderTool.cs
├── UI/
│   ├── MakeupTabsView.cs       — вкладки, инициализирует ColorPaletteView по категории
│   ├── ColorPaletteView.cs     — строит палитру ОДИН РАЗ в Start через Init(category)
│   ├── ColorItemView.cs        — кнопка цвета, event OnClicked(ColorItemView)
│   ├── FaceApplier.cs          — слушает MakeupService, показывает overlays
│   └── SpongeButton.cs         — ResetAll() без анимаций
└── Installers/
    └── GameInstaller.cs        — все Zenject биндинги
```

## Ключевые решения

### Hand
- `_waitAnchor`, `_defaultAnchor` — Transform якоря на сцене (не в инструментах)
- `brushPoint` — дочерний Transform, куда прикрепляется инструмент визуально
- `OnDipReached`, `OnReturnedToShelf` — события для brush tools

### LipstickTool
- Каждый элемент палитры — отдельный экземпляр (префаб Lipstick = ColorItemView + LipstickTool)
- Подписывается на свой ColorItemView через `GetComponent` в Start — не через глобальный OnItemChosen
- Не биндится в GameInstaller (несколько экземпляров)

### ColorPaletteView
- Категорию получает от MakeupTabsView автоматически — в Inspector не выставлять
- GridLayoutGroup удаляется после Build() — позиции фиксируются

### Zenject — когда использовать
- Связи между разными объектами/слоями — Inject
- Компоненты на одном GameObject — GetComponent
- Простые ссылки в одном объекте — SerializeField

## Zenject Bindings (GameInstaller)
- `IMakeupService` → `MakeupService` AsSingle
- `IProgressService` → `ProgressService` AsSingle
- `MakeupController` → BindInterfacesAndSelfTo AsSingle
- `DragController` → AsSingle
- `MakeupConfig` → BindInstance
- `Hand`, `BlushTool`, `EyeshadowTool`, `CreamTool`, `PowderTool` → FromComponentInHierarchy AsSingle
- `LipstickTool` — НЕ биндится (несколько экземпляров в сцене)

## MCP Unity
- Порт 8090, AutoStartServer: true
- Unity Editor должен быть открыт до запуска Claude Code
- При запуске нажать Approve на запрос доверия серверу mcp-unity

## TODO
- Анимации: HandAnimator.cs + Animator + AnimationEvent → ReturnToShelf()
- Scale инструмента при взятии (увеличить) и возврате (вернуть) через DOTween
- GridLayout + помады: при SetParent элемент уходит из контейнера — grid пересчитывает позиции. Решение: `enabled = false` вместо Destroy, или зафиксировать позиции через LayoutRebuilder перед удалением
- Анимировать подбор инструмента рукой — решить позже
