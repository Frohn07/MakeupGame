# MakeupGame — Project Context for Claude

## Project
Unity тестовое задание — игра-макияж. Демонстрационный проект с акцентом на качество кода.

## Branch
`feature/core-architecture` (main — для PR)

## Plugins
- **Zenject** — Dependency Injection контейнер
- **DOTween** — анимации (пока не используется)

## Architecture (созданные файлы)

```
Assets/Scripts/
├── Core/
│   ├── BaseTool.cs                    — abstract MonoBehaviour + ITool + IDraggable
│   └── Interfaces/
│       ├── ITool.cs                   — выбор/снятие инструмента
│       ├── IDraggable.cs              — drag & drop
│       └── IMakeupApplier.cs          — нанесение эффекта (для будущего FaceZone)
├── Data/
│   ├── Enums/MakeupCategory.cs        — Lipstick=0, Eyeshadow=1, Blush=2, Powder=3
│   ├── MakeupItemData.cs              — ScriptableObject (icon, resultOverlay, tintColor)
│   └── MakeupConfig.cs                — ScriptableObject, хранит списки по категориям
├── Services/
│   ├── IMakeupService.cs              — интерфейс стейта выбора + C# events
│   ├── MakeupService.cs               — реализация, Dictionary<category, item>
│   ├── IProgressService.cs            — разблокировка уровней
│   └── ProgressService.cs             — PlayerPrefs persistence
├── Controllers/
│   ├── MakeupController.cs            — медиатор UI ↔ сервис, IInitializable
│   └── DragController.cs              — роутит drag-события к IDraggable
├── Tools/
│   ├── BaseTool.cs                    — drag движение + ApplyMakeupEffect()
│   ├── LipstickTool.cs
│   ├── BlushTool.cs
│   ├── EyeshadowTool.cs
│   └── PowderTool.cs
├── UI/
│   ├── MakeupTabsView.cs              — вкладки категорий, active/normal спрайты
│   ├── ColorPaletteView.cs            — горизонтальная палитра, InstantiatePrefab
│   ├── ColorItemView.cs               — кнопка одного цвета, event OnClicked
│   └── FaceApplier.cs                 — слушает MakeupService, показывает overlays
├── Installers/
│   └── GameInstaller.cs               — MonoInstaller, все биндинги
```

## Zenject Bindings (GameInstaller)
- `IMakeupService` → `MakeupService` AsSingle (pure C#)
- `IProgressService` → `ProgressService` AsSingle (pure C#)
- `MakeupController` — BindInterfacesAndSelfTo (IInitializable + IDisposable + конкретный тип)
- `DragController` — AsSingle
- `MakeupConfig` — BindInstance (ScriptableObject из инспектора)

## Спрайты (Assets/Sprittes/ТЗ Unity developer/)
- Категории: Помады (6), Румяна (9), Тени (9), Пудра (нет отдельных)
- Инструменты: blush_brush, eye_brush, cream, loofah
- UI: tabs_makeup_*, next_done_button, reset_button, shelf, background_pink

## MCP Unity
- Настроен: порт 8090, AutoStartServer: true
- Конфиг: `ProjectSettings/McpUnitySettings.json`
- Сервер: `Library/PackageCache/com.gamelovers.mcp-unity@72c005fa0a/Server~/build/index.js`
- MCP конфиг для Claude Code: `.mcp.json` в корне проекта (стандартный формат)
  - `.claude/settings.json` также содержит конфиг, но глобальный `~/.claude/settings.json` не поддерживает `mcpServers`
- При запуске Claude Code нужно нажать **Approve** на запрос доверия серверу `mcp-unity`
- Unity Editor должен быть открыт с проектом до запуска Claude Code (WebSocket на порту 8090)

## Known Issues / TODO
- Drag система есть, но нет MonoBehaviour который читает Input и вызывает DragController
- Нет InputHandler (MouseInputHandler / TouchInputHandler)
- ResultOverlay спрайты не назначены в MakeupItemData (тонирование через TintColor)

## SOLID в проекте
- **S**: каждый класс одна ответственность
- **O**: BaseTool — abstract, расширяется без изменений
- **L**: BlushTool / LipstickTool взаимозаменяемы с BaseTool
- **I**: ITool и IDraggable разделены (MonoBehaviour может реализовать только одно)
- **D**: UI зависит от IMakeupService, не от MakeupService

## Команды Git
```bash
git add Assets/Scripts/
git commit -m "feat: ..."
```
