using Codice.CM.Common;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Manager.AudioManager;

namespace DiscountDelirium
{
    public class ExampleStateUsingAudio : MonoBehaviour
    {
        //private int m_audioSourceIndex;

        //public override void OnEnter()
        //{
        //    Debug.LogWarning("current state: MOVING");

        //    m_audioSourceIndex = _AudioManager.PlaySoundEffectsLoopOnTransform(ESound.CartRolling, m_cartStateMachine.transform);
        //}

        //public override void OnUpdate()
        //{
        //    _AudioManager.ModifySound(
        //        m_audioSourceIndex,
        //        ESoundModification.Pitch,
        //        Mathf.Lerp(0.8f, 2f, m_cartStateMachine.LocalVelocity.magnitude / m_cartStateMachine.MaxSpeed));

        //    _AudioManager.ModifySound(
        //        m_audioSourceIndex,
        //        ESoundModification.Volume,
        //        Mathf.Lerp(0f, 1f, m_cartStateMachine.LocalVelocity.magnitude / m_cartStateMachine.MaxSpeed));
        //}


        //public override void OnExit()
        //{
        //    _AudioManager.StopSoundEffectsLoop(m_audioSourceIndex);
        //}
    }
}
