using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundManager : MonoBehaviour
{
    [SerializeField]
    private CarController _carController;
    [SerializeField]
    private AudioSource _engineAudioSource;
    [SerializeField]
    private AudioSource _tiresAudioSource;

    private float _enginePitch;

    private const float _minPitch = 0.4f;
    private const float _maxPitch = 3f;

    void Update()
    {
        _enginePitch = _carController.CarSpeed / 50;
        _engineAudioSource.pitch = Mathf.Clamp(_enginePitch, _minPitch, _maxPitch);
        if (_carController.IsBraking && !_tiresAudioSource.isPlaying)
        {
            _tiresAudioSource.Play();
        }
        else if (!_carController.IsBraking && _tiresAudioSource.isPlaying)
        {
            _tiresAudioSource.Stop();
        }
    }
}
