using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSphereAtVertex : MonoBehaviour
{
    //public GameObject cube; // Assign the cube GameObject in the Inspector
    public GameObject vertexSphere;
    private Quaternion originalRotation;
    public int vertexCount;
    //public GameObject[] objects = GameObject.FindGameObjectsWithTag("something");
    void Start()
    {
        // Example: Create a sphere at vertex index 0 of the cube
        //int vertexIndex = 0;

        GameObject[] objects = GameObject.FindGameObjectsWithTag("something");
        
        // Ensure the cube GameObject is assigned
        if (objects != null)
        {
            foreach(GameObject obj in objects) 
            {
                // Get the mesh filter of the cube
                MeshFilter cubeMeshFilter = obj.GetComponent<MeshFilter>();
                Dictionary<Vector3, int> posIndex = new Dictionary<Vector3, int>();
                // Ensure the mesh filter is not null and has a mesh
                if (cubeMeshFilter != null && cubeMeshFilter.sharedMesh != null)
                {
                    // Get the cube's mesh
                    Mesh cubeMesh = cubeMeshFilter.sharedMesh;
                    int indexNumber = 0;
                    vertexCount = cubeMesh.vertices.Length;
                    for(int i = 0; i<cubeMesh.vertices.Length; i++) {
                        // Get the world position of the specified vertex
                        Vector3 vertexPosition = obj.transform.TransformPoint(cubeMesh.vertices[i]);
                        if(posIndex.ContainsKey(vertexPosition)){
                            continue;
                        }
                        else{
                            posIndex.Add(vertexPosition, indexNumber);
                            indexNumber++;
                        }
                        // Create a sphere at the vertex position
                        //GameObject vertexSphere = new GameObject("vertex" + indexNumber);
                        
                        //vertexSphere.transform.position = vertexPosition;
                        //vertexSphere.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                        // GameObject somethingElse = new GameObject("vertex" + indexNumber);
                        // somethingElse.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                        
                        //vertex가 가시적으로 
                        Instantiate(vertexSphere, vertexPosition, Quaternion.identity);
                        vertexSphere.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                        vertexSphere.name = "vertex" + indexNumber;
                        
                    }
                    UnityEngine.Object[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                    foreach (GameObject vertexSphere in allObjects){
                        //Debug.Log(somethingElse + "is an active object" + somethingElse.GetInstanceID());
                        //vertex 위치 출력
                        Debug.Log(vertexSphere.transform.position);
                    }
                }
                               // Optional: Attach the sphere to a parent object for organization
                // sphere.transform.parent = cube.transform;

            else
            {
                Debug.LogError("Mesh filter or mesh not found on the cube.");
            }
            }
        }
        
        else
        {
            Debug.LogError("Cube GameObject not assigned. Please assign the cube GameObject in the Inspector.");
        }
    }

    void Update()
    {
        originalRotation = transform.rotation;
    }
}