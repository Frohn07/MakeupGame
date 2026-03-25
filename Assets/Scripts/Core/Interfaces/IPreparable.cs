using UnityEngine;

namespace MakeupGame.Core.Interfaces
{
    /// <summary>
    /// Optional contract for tools that require an extra preparation step
    /// before the player can drag (e.g. brush dipping into a colour swatch).
    ///
    /// Hand checks: if tool is IPreparable → fly to PreparePosition → OnPrepared() → MoveToWait.
    /// New tools that need a prep step implement this; tools that don't — don't.
    ///
    /// SOLID:
    ///   ISP — preparation logic is segregated from the base ITool contract.
    ///   OCP — Hand never needs to change when new preparable tools are added.
    /// </summary>
    public interface IPreparable
    {
        /// <summary>World position the Hand flies to for the preparation step.</summary>
        Vector3 PreparePosition { get; }

        /// <summary>Called by Hand when it reaches PreparePosition.</summary>
        void OnPrepared();

        /// <summary>Called by Hand after the tool has been returned to the shelf.</summary>
        void OnReturned();
    }
}
