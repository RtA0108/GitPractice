using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVertex : MonoBehaviour
{
    // Material for the default color of the mesh
    public Color defaultColor = Color.white;

    // Material for the selected color of the mesh 
    public Color selectedColor = Color.red;

    // Variable to hold the reference to the selected mesh
    private GameObject selectedMesh;

    // Flag to indicate if a mesh is currently selected
    private bool meshSelected = false;

    // Flag to indicate if the mesh is being dragged
    private bool isDragging = false;

    // Offset between mouse click position and object center
    private Vector3 offset;
    public Camera projectCamera;
    void OnMouseDown()
    {
        // Check if a mesh is currently selected
        if (!meshSelected)
        {
            // Select the clicked mesh
            SelectMesh();
        }
        else if (selectedMesh == gameObject)
        {
            // Deselect the selected mesh
            DeselectMesh();
        }
    }

    void OnMouseDrag()
    {
        if (meshSelected && selectedMesh == gameObject)
        {
            //Debug.Log("Go");
            // Perform dragging of the selected mesh
            DragSelectedMesh();
        }
    }

    void SelectMesh()
    {
        // Change color of the selected mesh
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = selectedColor;
        }

        // Set the selected mesh
        selectedMesh = gameObject;
        meshSelected = true;

        // Calculate offset for dragging
        offset = transform.position - GetWorldPositionFromMouse();
    }

    void DeselectMesh()
    {
        // Change color of the deselected mesh
        Renderer renderer = selectedMesh.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = defaultColor;
        }

        // Deselect the mesh
        selectedMesh = null;
        meshSelected = false;
    }

    void DragSelectedMesh()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Convert screen coordinates to world coordinates using the camera targeting display 2
        mousePosition.z = -10.0f; // Set the z-coordinate to initialZ
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Update the position of the selected mesh while maintaining the z-coordinate
        transform.position = worldPosition + offset;

        // Vector3 mousePosition = GetWorldPositionFromMouse();
        // mousePosition.z = -10.0f; // Maintain z-coordinate
        // transform.position = mousePosition + offset;
    }
    Vector3 GetWorldPositionFromMouse()
    {
        // Find the Project Camera targeting display 2
        Camera projectCamera = GameObject.Find("Project Camera").GetComponent<Camera>();
        if (projectCamera == null)
        {
            Debug.LogError("Project Camera not found in the scene.");
            return transform.position;
        }

        // Cast a ray from the Project Camera and return the world position
        Ray ray = projectCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return transform.position;
    }
}