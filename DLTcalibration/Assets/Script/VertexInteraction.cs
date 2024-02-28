using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexInteraction : MonoBehaviour
{
    private Color originalColor;
    private Renderer renderer;
    private bool copied = false;
    void Start()
    {
        // Get the renderer component to access the material color
        renderer = GetComponent<Renderer>();

        // Store the original color
        originalColor = renderer.material.color;

    }
    private void OnMouseDown()
    {
        renderer.material.color = renderer.material.color == originalColor ? Color.red : originalColor;
        Debug.Log(this.transform.position);
        if (!copied){
            GameObject copy = Instantiate(gameObject);
            copy.transform.Translate(0f,0f,-10f);
            copied = true;
        }
        
    }
    void Update()
    {
        // Cast a ray from the mouse position
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;

        // // Check if the ray hits a collider
        // if (Physics.Raycast(ray, out hit))
        // {
        //     // Check if the collider is attached to the same GameObject
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         // Change the color when the mouse hovers over the vertex
        //        // renderer.material.color = Color.red;

        //         // Check for mouse click
        //         if (hit.collider.gameObject == gameObject)
        //         {
        //             // Change the color when the mouse is clicked
        //             renderer.material.color = Color.green;

        //             // Do something with the selected vertex
        //             Debug.Log("Clicked on vertex at: " + hit.point);
        //         }
        //     }
        //     else
        //     {
        //         // Restore the original color when not hovering over the vertex
        //         //renderer.material.color = originalColor;
        //     }
        // }
        // else
        // {
        //     // Restore the original color when not hitting any collider
        //     renderer.material.color = originalColor;
        // }
    }

   
}