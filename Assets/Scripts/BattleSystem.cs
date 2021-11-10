using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instance { get; set; }

    private bool isActive = false;
    private Camera cam;
    private Ray ray;
    private int unitMask;

    public CinemachineVirtualCamera virtualCamera;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartConfig()
    {
        isActive = true;
        cam = Camera.main;
        unitMask = LayerMask.GetMask("Unit");
    }

    private void Update()
    {
        if(isActive)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, unitMask))//Raycasting for placing the unit
            {
                if (hit.collider != null)
                {
                    if(hit.collider.gameObject.GetComponent<Unit>().Team == GameManager.instance.GetCurrentPlayer().Name)
                    {
                        Debug.Log("Playable unit");
                        if(Input.GetMouseButtonDown(0))
                        {
                            hit.collider.gameObject.GetComponent<PlayerInput>().enabled = true;
                            virtualCamera.Follow = hit.collider.gameObject.transform.GetChild(0);
                        }
                    }
                    else
                    {
                        Debug.Log("Not playable unit");
                    }
                }
            }
        }
    }

}
