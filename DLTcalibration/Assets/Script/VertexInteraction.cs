using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexInteraction : MonoBehaviour
{
    public GameObject newMeshPrefab;
    //새로 생성된 Vertex의 screenCoord를 지속적으로 저장 (마우스 위치가 아니라 sphere의 위치로 저장해야 함)
    public Dictionary<int, Vector2> screenCoord = new Dictionary<int, Vector2>();

    private GameObject createdMesh;
    private GameObject LVManger;
    private Color originalColor;
    private new Renderer renderer;
    private static int meshCounter = 0;
    private int meshIndex = 0;
    private bool copied = false;
    void Start()
    {
        
        // Get the renderer component to access the material color
        renderer = GetComponent<Renderer>();

        // Store the original color
        originalColor = renderer.material.color;
        LVManger = GameObject.Find("LevelManager");
    }
    private void OnMouseDown()
    {
        renderer.material.color = renderer.material.color == originalColor ? Color.red : originalColor;
        Debug.Log(this.transform.position);
        if (!copied){
            createdMesh = Instantiate(newMeshPrefab, this.transform.position, Quaternion.identity);
            Vector3 newPos = createdMesh.transform.position;
            meshIndex = LVManger.GetComponent<VertexClickTest>().arrayIndex;
            LVManger.GetComponent<VertexClickTest>().verticesStruct[meshIndex].worldCoordinate = newPos;
            newPos.z += 1000f; // Change this value as needed
            createdMesh.transform.position = newPos;
            // GameObject copy = Instantiate(gameObject);
            // copy.transform.Translate(0f,0f,-10f);
            // copy.transform.Position()
            createdMesh.name = "2D_Vertex_" + meshCounter.ToString();
            meshCounter++;
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