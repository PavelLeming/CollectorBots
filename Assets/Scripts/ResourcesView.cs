using TMPro;
using UnityEngine;

public class ResourcesView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Base _base;

    private void OnEnable()
    {
        _base.NewResource += ReDraw;
    }

    private void OnDisable()
    {
        _base.NewResource -= ReDraw;
    }

    private void ReDraw()
    {
        _text.text = _base.Resources.ToString();
    }
}
