using MakeupGame.Controllers;
using MakeupGame.Data;
using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.Installers
{
    /// <summary>
    /// Scene-level Zenject installer.
    /// Attach this to the SceneContext GameObject and assign MakeupConfig in the Inspector.
    ///
    /// Binding strategy:
    ///   • Services are pure C# → AsSingle (one instance per scene lifetime)
    ///   • MakeupController implements IInitializable → BindInterfacesAndSelfTo
    ///     so Zenject calls Initialize() automatically
    ///   • MakeupConfig ScriptableObject → BindInstance (data-only, no DI needed internally)
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private MakeupConfig _makeupConfig;

        public override void InstallBindings()
        {
            // ── Data ────────────────────────────────────────────────────────────
            Container.BindInstance(_makeupConfig).AsSingle();

            // ── Services ────────────────────────────────────────────────────────
            Container.Bind<IMakeupService>()
                     .To<MakeupService>()
                     .AsSingle();

            Container.Bind<IProgressService>()
                     .To<ProgressService>()
                     .AsSingle();

            // ── Controllers ─────────────────────────────────────────────────────
            // BindInterfacesAndSelfTo → binds IInitializable + IDisposable + MakeupController
            Container.BindInterfacesAndSelfTo<MakeupController>()
                     .AsSingle();

            Container.Bind<DragController>()
                     .AsSingle();
        }
    }
}
