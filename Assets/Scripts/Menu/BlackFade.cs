using UnityEngine;
using UnityEngine.UI;
// Script for UI fade out
public class BlackFade : MonoBehaviour
{
    public float Speed;

    private float _alpha = 1;
    private Image _image;

    private void Start()
    {
        _image = this.GetComponent<Image>();
    }
    /// <summary>
    /// back fade alpha decrease per frame
    /// </summary>
    void Update()
    {
        _image.color = new Color(0, 0, 0, _alpha);
        _alpha -= Time.deltaTime * Speed;
        if (_alpha <= 0)
        {
            _image.color = new Color(0, 0, 0, 0);
            this.enabled = false;
            this.gameObject.SetActive(false);
        }
    }
}
