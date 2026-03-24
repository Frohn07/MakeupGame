using UnityEngine;

namespace MakeupGame.Core.Interfaces
{
    /// <summary>
    /// Contract for any makeup tool that the Hand can pick up and apply.
    ///
    /// SOLID notes:
    ///   ISP — only the three position properties and Apply(); selection/drag
    ///         behaviour lives in Hand, not here.
    ///   OCP — new tools implement this interface without changing Hand or FaceZone.
    /// </summary>
    public interface ITool
    {
        /// <summary>Where the Hand animates to in order to pick up this tool.</summary>
        Vector3 ShelfPosition { get; }

        /// <summary>
        /// Optional intermediate stop before WaitPosition.
        /// Brush tools use this to "dip" into the selected palette colour.
        /// Null for cream and lipstick.
        /// </summary>
        Vector3? DipPosition { get; }

        /// <summary>
        /// The Transform of the tool GameObject.
        /// Hand uses this to dynamically reparent the tool during pickup/return.
        /// </summary>
        Transform ToolTransform { get; }

        /// <summary>
        /// Called by FaceZone when the Hand is released inside the face area.
        /// Each tool defines what "applying" means for its category.
        /// </summary>
        void Apply();
    }
}
