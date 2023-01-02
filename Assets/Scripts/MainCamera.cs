using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;

    [Range(1, 10)]
    public float smoothFactor;
    public GameObject Canvas;
    private float width,
        height;

    private void Start()
    {
        width = Canvas.GetComponent<SpriteRenderer>().bounds.size.x;
        height = Canvas.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector2 targetPosition = target.position;
        Vector2 canvasPosition = Canvas.transform.position;
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        float y = height / 2 - cameraHeight;
        float x = width / 2 - cameraWidth;
        Vector2 boundPosition = new Vector2(
     Mathf.Clamp(targetPosition.x, canvasPosition.x - x, canvasPosition.x + x),
     Mathf.Clamp(targetPosition.y, canvasPosition.y - y, canvasPosition.y + y)
 );
        transform.position = boundPosition;
    }

    private void Update()
    {
        Vector2 targetPosition = target.position;
        Vector2 canvasPosition = Canvas.transform.position;
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        float y = height / 2 - cameraHeight;
        float x = width / 2 - cameraWidth;
        Vector2 boundPosition = new Vector2(
            Mathf.Clamp(targetPosition.x, canvasPosition.x - x, canvasPosition.x + x),
            Mathf.Clamp(targetPosition.y, canvasPosition.y - y, canvasPosition.y + y)
        );
        Vector3 smoothPosition = Vector2.Lerp(
            transform.position,
            boundPosition,
            smoothFactor * Time.deltaTime
        );
        smoothPosition.z = -10;
        transform.position = smoothPosition;
    }

    public void ChangeCanvas(GameObject canvas)
    {
        Canvas = canvas;
        width = Canvas.GetComponent<SpriteRenderer>().bounds.size.x;
        height = Canvas.GetComponent<SpriteRenderer>().bounds.size.y;
    }
}
