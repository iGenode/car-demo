using Cinemachine;
using UnityEngine;

public class CinemachineTouchControls : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook _cinemachine;
    [SerializeField]
    private CameraTouchControls _touchControls;
    [SerializeField] 
    private float _senstivityX = 0.5f;
    [SerializeField]
    private float _senstivityY = 0.5f;

    private int _xSensetivityMutliplier = 200;

    void Update()
    {
        _cinemachine.m_XAxis.Value += _touchControls.TouchDist.x * _xSensetivityMutliplier * _senstivityX * Time.deltaTime;
        _cinemachine.m_YAxis.Value += _touchControls.TouchDist.y * _senstivityY * Time.deltaTime;
    }
}
