using UnityEngine;

public class BoidWrapAround : MonoBehaviour
{
    private float screenWidth;
    private float screenHeight;
    private float objectWidth;
    private float objectHeight;

    private void Start()
    {
        // 1. Get screen size in world units
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * Camera.main.aspect;

        // 2. Add an offset based on the object's sprite/renderer size
        if (TryGetComponent<Renderer>(out Renderer rend))
        {
            objectWidth = rend.bounds.size.x / 2f;
            objectHeight = rend.bounds.size.y / 2f;
        }
    }

    private void Update()
    {
        Vector3 position = transform.position;
        // Account for camera position in case the camera moves
        Vector3 camPos = Camera.main.transform.position;

        // Horizontal boundaries (with object width offset)
        float rightEdge = camPos.x + (screenWidth / 2f) + objectWidth;
        float leftEdge = camPos.x - (screenWidth / 2f) - objectWidth;

        // Vertical boundaries (with object height offset)
        float topEdge = camPos.y + (screenHeight / 2f) + objectHeight;
        float bottomEdge = camPos.y - (screenHeight / 2f) - objectHeight;

        // Wrap horizontally
        if (position.x > rightEdge)
        {
            position.x = leftEdge;
        }
        else if (position.x < leftEdge)
        {
            position.x = rightEdge;
        }

        // Wrap vertically
        if (position.y > topEdge)
        {
            position.y = bottomEdge;
        }
        else if (position.y < bottomEdge)
        {
            position.y = topEdge;
        }

        transform.position = position;
    }

}
