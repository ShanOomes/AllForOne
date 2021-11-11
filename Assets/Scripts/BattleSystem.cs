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

    private GameObject currentUnit;
    [SerializeField] CinemachineVirtualCamera cam_unit;
    [SerializeField] CinemachineVirtualCamera cam_overview;

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

        //Register camera's
        CameraSwitcher.AddCamera(cam_overview);
        CameraSwitcher.AddCamera(cam_unit);
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
                    GameObject tmp = hit.collider.gameObject;
                    if(tmp.GetComponent<Unit>().Team == GameManager.instance.GetCurrentPlayer().Name)
                    {
                        Debug.Log("Playable unit");
                        if(Input.GetMouseButtonDown(0))
                        {
                            tmp.GetComponent<PlayerInput>().enabled = true;//Enable the player input
                            cam_unit.Follow = tmp.transform.GetChild(0);
                            currentUnit = tmp;

                            CameraSwitcher.SwitchCamera(cam_unit);

                            StartCoroutine(StartCountdown());
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

    public IEnumerator StartCountdown(float countdownValue = 10)
    {
        float currCountdownValue;
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }

        CameraSwitcher.SwitchCamera(cam_overview);
        currentUnit.GetComponent<PlayerInput>().enabled = false;
        GameManager.instance.NextPlayer();
    }
}
