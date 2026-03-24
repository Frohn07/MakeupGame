using UnityEngine;
using Zenject;

namespace MakeupGame.Core
{
    /// <summary>
    /// Collision trigger that represents the face area.
    ///
    /// When the Hand enters this zone:
    ///   1. Asks the Hand for its CurrentTool.
    ///   2. Calls tool.Apply() — the tool knows what visual effect to produce.
    ///   3. Tells the Hand to return the tool to the shelf.
    ///
    /// SRP: FaceZone only detects "hand arrived" and delegates everything else.
    /// Requires a 2D Trigger Collider on this GameObject.
    /// </summary>
    public class FaceZone : MonoBehaviour
    {
        [Inject] private Hand _hand;

        private bool _applied;

        private void OnTriggerEnter2D(Collider2D other) => TryApply(other);
        private void OnTriggerStay2D(Collider2D other)  => TryApply(other);

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Hand>(out _)) _applied = false;
        }

        private void TryApply(Collider2D other)
        {
            if (_applied) return;
            if (!other.TryGetComponent<Hand>(out _)) return;
            if (_hand.CurrentTool == null || !_hand.IsDraggingEnabled) return;

            _applied = true;
            _hand.CurrentTool.Apply();
            _hand.ReturnToShelf();
        }
    }
}
