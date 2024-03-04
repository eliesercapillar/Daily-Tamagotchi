using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IAudioManager
{
    public float GetVolumeLevel();
    public void SetVolumeLevel(float volume);
    public void FlipMute();
}
