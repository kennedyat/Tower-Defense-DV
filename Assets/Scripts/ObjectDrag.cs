using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 _offset;

    private void OnMouseDown()
    {
        _offset = transform.position - TileController.GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        //Check logic for the drag
        Vector3 pos = TileController.GetMouseWorldPosition() + _offset;
        transform.position = TileController.current.SnapCoordinateToGrid(pos); 
    }

}
