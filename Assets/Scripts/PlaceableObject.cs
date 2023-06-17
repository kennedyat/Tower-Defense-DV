using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
  
    public bool Placed { get; private set; }
    public Vector3Int Size { get; private set; }
    private Vector3[] _Vertices;

    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider box = gameObject.GetComponent<BoxCollider>();
        _Vertices = new Vector3[4];
        _Vertices[0] = box.center + new Vector3(-box.size.x, -box.size.y, -box.size.z) * .5f;
        _Vertices[1] = box.center + new Vector3(-box.size.x, -box.size.y, box.size.z) * .5f;
        _Vertices[2] = box.center + new Vector3(-box.size.x, -box.size.y, box.size.z) * .5f;
        _Vertices[3] = box.center + new Vector3(box.size.x, -box.size.y, box.size.z) * .5f;


    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[_Vertices.Length];

        for( int i=0; i<_Vertices.Length; i++ )
        {
            Vector3 worldPos = transform.TransformPoint(_Vertices[i]);
            vertices[i] = TileController.current.gridLayout.WorldToCell(worldPos);

        }

        Size = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x),
            Mathf.Abs((vertices[0] - vertices[3]).y), 1);
    }

    private Vector3 GetStartPosition()
    {
        return transform.TransformPoint(_Vertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
    }

    public virtual void Place()
    {
        //Override?
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        Placed = true;

        //invoke events of placement 

    }
    /*Utilize vertices to check whether object has been placed there or not 
     * If object has been placed there, delete previous gameObject and add the current one. 
     * Can help with rotation, rotate the vertices by swapping the vertices clockwise
     * Create a seperate script that checks if the path is valid or not.
     * See if you can utilize grid for elevated grounds as well.
     * When going over a size, either get rid of it immediately or dont allow player to placed new object 
     */


}
