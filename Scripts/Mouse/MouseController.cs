using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Mouse0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
                Debug.Log("hit this: " + hit.collider.gameObject.name);
            }
        }
    }
}
