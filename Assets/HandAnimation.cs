using System;
using MakeupGame.Core;
using UnityEngine;

/// <summary>
/// Controls the Animator on the Hand GameObject.
///
/// Contract:
///   • External code calls PlayDip / PlayApply with an optional onFinished callback.
///   • HandAnimation plays the animation, then:
///       — fires onFinished callback
///       — returns to Idle automatically (via AnimationEvent)
///   • Idle is played automatically after any animation ends — no manual calls needed.
///   • Dip is subscribed to Hand.OnDipReached internally — no external wiring required.
///
/// animationIndex values (must match your Animator parameter):
///   0 = Idle
///   1 = Dip   (brush picks up colour)
///   2 = Apply (tool applied to face)
///
/// SRP: drives animation state only. Game logic runs through callbacks, not inside this class.
/// GetComponent is correct — Hand and HandAnimation share the same GameObject.
/// </summary>
public class HandAnimation : MonoBehaviour
{
    private static readonly int AnimIndex = Animator.StringToHash("animationIndex");

    private const int IdleIndex  = 0;
    private const int DipIndex   = 1;
    private const int ApplyIndex = 2;

    private Animator _animator;
    private Hand     _hand;

    private Action _onApplyFinished;
    private Action _onDipFinished;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _hand     = GetComponent<Hand>();
    }


    // ── Public API ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Play the "dip" animation.
    /// onFinished fires after the animation ends (AnimationEvent), then Idle plays.
    /// Pass null if no follow-up is needed.
    /// </summary>
    public void PlayDip(Action onFinished = null)
    {
        _onDipFinished = onFinished;
        _animator.SetInteger(AnimIndex, DipIndex);
    }

    /// <summary>
    /// Play the "apply to face" animation.
    /// onFinished fires after the animation ends (AnimationEvent), then Idle plays.
    /// Pass null if no follow-up is needed.
    /// </summary>
    public void PlayApply(Action onFinished = null)
    {
        _onApplyFinished = onFinished;
        _animator.SetInteger(AnimIndex, ApplyIndex);
    }

    /// <summary>Force Idle immediately (e.g. on reset).</summary>
    public void PlayIdle() => _animator.SetInteger(AnimIndex, IdleIndex);

    // ── AnimationEvents (set these on the animation clips in the Animator) ─────

    /// <summary>
    /// Add this as AnimationEvent at the END of the Dip clip.
    /// </summary>
    public void OnDipAnimationFinished()
    {
        var callback = _onDipFinished;
        _onDipFinished = null;

        PlayIdle();
        callback?.Invoke();
    }

    /// <summary>
    /// Add this as AnimationEvent at the END of the Apply clip.
    /// </summary>
    public void OnApplyAnimationFinished()
    {
        var callback = _onApplyFinished;
        _onApplyFinished = null;

        PlayIdle();
        callback?.Invoke();
    }

}
