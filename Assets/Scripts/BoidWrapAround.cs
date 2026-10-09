using UnityEngine;
//This is a script that Wraps the boids around the screen when they go off the edge of the camera view.
public class BoidWrapAround : MonoBehaviour
{
    private float screenWidth;
    private float screenHeight;
    private float objectWidth;
    private float objectHeight;

    private void Start()
    {
        //Gets the screen hight and width based on the camera size and aspect ratio.
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * Camera.main.aspect;

        if (TryGetComponent<Renderer>(out Renderer rend))//Adds offset to the wrap around so it looks better when the boids leave the screen.
        {
            objectWidth = rend.bounds.size.x / 2f;
            objectHeight = rend.bounds.size.y / 2f;
        }
    }

    private void Update()
    {
        Vector3 position = transform.position;
        Vector3 camPos = Camera.main.transform.position;

        float rightEdge = camPos.x + (screenWidth / 2f) + objectWidth;
        float leftEdge = camPos.x - (screenWidth / 2f) - objectWidth;//calculates the horizontal width of the screen, and makes it a edge.

        float topEdge = camPos.y + (screenHeight / 2f) + objectHeight;
        float bottomEdge = camPos.y - (screenHeight / 2f) - objectHeight;//calculates the verticle hight of the screen, and makes it a edge.

        if (position.x > rightEdge)//Horizontal Wrap Around
        {
            position.x = leftEdge;
        }
        else if (position.x < leftEdge)
        {
            position.x = rightEdge;
        }

        if (position.y > topEdge)//Vertical Wrap Around
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
