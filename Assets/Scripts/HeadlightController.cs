using UnityEngine;
using UnityEngine.UI;

public class HeadlightController : MonoBehaviour
{
    [Header("UI Controls")]
    [SerializeField]
    private Button _headlightButton;

    [Header("Headlights Objects")]
    [SerializeField]
    private GameObject _headlights;

    private bool _areOn = false;

    private void SwitchHeadlights()
    {
        _headlights.SetActive(!_areOn);
        _areOn = !_areOn;
    }

    private void OnEnable()
    {
        _headlightButton.onClick.AddListener(SwitchHeadlights);
    }

    private void OnDisable()
    {
        _headlightButton.onClick.RemoveListener(SwitchHeadlights);
    }
}
