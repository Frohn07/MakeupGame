using MakeupGame.Controllers;
using MakeupGame.Core;
using MakeupGame.Data;
using MakeupGame.Services;
using MakeupGame.Tools;
using UnityEngine;
using Zenject;

namespace MakeupGame.Installers
{
    /// <summary>
    /// Scene-level Zenject installer.
    /// Attach to the SceneContext GameObject.
    ///
    /// Binding notes:
    ///   • Services (pure C#)          → AsSingle
    ///   • MakeupController            → BindInterfacesAndSelfTo (IInitializable + IDisposable + self)
    ///   • Hand / FaceZone / Tools     → FromComponentInHierarchy (MonoBehaviours already in scene)
    ///   • MakeupConfig (SO)           → BindInstance
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private MakeupConfig _makeupConfig;

        public override void InstallBindings()
        {
            // ── Data ─────────────────────────────────────────────────────────────
            Container.BindInstance(_makeupConfig).AsSingle();

            // ── Services ──────────────────────────────────────────────────────────
            Container.Bind<IMakeupService>()
                     .To<MakeupService>()
                     .AsSingle();

            Container.Bind<IProgressService>()
                     .To<ProgressService>()
                     .AsSingle();

            // ── Controllers ───────────────────────────────────────────────────────
            Container.BindInterfacesAndSelfTo<MakeupController>()
                     .AsSingle();

            Container.Bind<DragController>()
                     .AsSingle();

            // ── Core scene objects ────────────────────────────────────────────────
            Container.Bind<Hand>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            // ── Tools (MonoBehaviours present in the scene) ───────────────────────
            Container.Bind<LipstickTool>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<BlushTool>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<EyeshadowTool>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<PowderTool>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<CreamTool>()
                     .FromComponentInHierarchy()
                     .AsSingle();
        }
    }
}
