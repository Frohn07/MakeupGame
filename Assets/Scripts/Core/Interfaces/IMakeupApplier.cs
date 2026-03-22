using MakeupGame.Data;

namespace MakeupGame.Core.Interfaces
{
    /// <summary>
    /// Applies a makeup effect to a face area. (ISP: isolated rendering contract)
    /// </summary>
    public interface IMakeupApplier
    {
        void ApplyEffect(MakeupItemData item);
        void ResetEffect();
    }
}
