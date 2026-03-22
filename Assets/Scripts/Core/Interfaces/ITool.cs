namespace MakeupGame.Core.Interfaces
{
    /// <summary>
    /// Defines a selectable makeup tool. (ISP: separated from IDraggable)
    /// </summary>
    public interface ITool
    {
        bool CanInteract { get; }
        void OnToolSelected();
        void OnToolDeselected();
    }
}
