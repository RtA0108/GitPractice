using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VertexClickTest : MonoBehaviour
{
    // private Color originalColor;
    // private Renderer renderer;
    // private bool copied = false;

    public GameObject[] clickedObjects; // Array to store clicked objects
    public Vector3[] worldCoordinates; // Array to store world coordinates
    public Vector2[] screenCoordinates; // Array to store screen coordinates
    public double[] worldPoints;
    public double[] imagePoints;
    public int arrayIndex;
    public Camera projectCam;

    public struct VertexStruct
    {
        public int uniqIndex;
        public Vector3 worldCoordinate;
        public Vector2 screenCoordinate;
        public VertexStruct(int vertexIndex, Vector3 worldCoord, Vector2 screenCoord)
        {
            this.uniqIndex = vertexIndex;
            this.worldCoordinate = worldCoord;
            screenCoordinate = screenCoord;
        }
    }
    public VertexStruct[] verticesStruct;

  

    private void Start()
    {
        // // Get the renderer component to access the material color
        // renderer = GetComponent<Renderer>();

        // // Store the original color
        // originalColor = renderer.material.color;
        Debug.Log(projectCam.pixelHeight);
        clickedObjects = new GameObject[10]; // Initializing arrays with size 10
        worldCoordinates = new Vector3[10];
        screenCoordinates = new Vector2[10];
        worldPoints = new double[18];
        imagePoints = new double[12];
        verticesStruct = new VertexStruct[12];
        arrayIndex = 0;
    }

    private void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Shoot a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits an object
            if (Physics.Raycast(ray, out hit))
            {
                // Store clicked object
                GameObject clickedObject = hit.collider.gameObject;

                // Check if the clicked object is not already in the array
                if (!ArrayContains(clickedObjects, clickedObject))
                {
                    // Find first empty slot
                    int index = System.Array.IndexOf(clickedObjects, null);
                    
                    if (index != -1 && clickedObject.tag == "SphereIn2D")
                    {
                       
                        clickedObjects[index] = clickedObject;
                        worldCoordinates[index] = clickedObject.transform.position;
                        //screenCoordinates[index] = Input.mousePosition;
                        //screenCoordinates[index] = Camera.
                        // -1을 곱하는게 아닌 해상도의 최댓값에서 y좌표를 빼주어야 함
                        //screenCoordinates[index].y = screenCoordinates[index].y * -1;
                         
                        verticesStruct[index].uniqIndex = index;
                        //verticesStruct[index].worldCoordinate = clickedObject.transform.position;
                        verticesStruct[index].screenCoordinate = new Vector2(projectCam.WorldToScreenPoint(clickedObject.transform.position).x, projectCam.pixelHeight - projectCam.WorldToScreenPoint(clickedObject.transform.position).y);
                        worldPoints[index * 3] = worldCoordinates[index].x;
                        worldPoints[index * 3 + 1] = worldCoordinates[index].y;
                        worldPoints[index * 3 + 2] = -worldCoordinates[index].z;
                        imagePoints[index * 2] = screenCoordinates[index].x;
                        imagePoints[index * 2 + 1] = projectCam.WorldToScreenPoint(clickedObject.transform.position).y;
                        arrayIndex++;
                        Debug.Log(verticesStruct[index].uniqIndex);
                        Debug.Log(verticesStruct[index].worldCoordinate);
                        Debug.Log(verticesStruct[index].screenCoordinate);
                        //Debug.Log(imagePoints[index * 2 + 1]);
                    }
                    else
                    {
                        Debug.LogWarning("Clicked objects array is full. Increase array size if needed.");
                    }

                    // You can do something with the clicked object here
                    //Debug.Log("Clicked object: " + clickedObject.name);
                    //Debug.Log("World Position: " + clickedObject.transform.position);
                    //Debug.Log("Screen Position: " + screenCoordinates[index]);
                }
                else
                {
                    Debug.Log("Object already clicked: " + clickedObject.name);
                }
            }
        }
    }
    // private void OnMouseDown()
    // {
    //     renderer.material.color = renderer.material.color == originalColor ? Color.red : originalColor;
    //     //Debug.Log(this.transform.position);
    //     if (!copied){
    //         GameObject copy = Instantiate(gameObject);
    //         copy.transform.Translate(0f,0f,-10f);
    //         copied = true;
    //     }

    // }
    // Function to check if an array contains a specific object
    private bool ArrayContains(GameObject[] array, GameObject obj)
    {
        foreach (GameObject item in array)
        {
            if (item == obj)
                return true;
        }
        return false;
    }
}