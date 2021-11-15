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

        GameManager.instance.VisualizeUnits(true);
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
                        //Debug.Log("Playable unit");
                        if(Input.GetMouseButtonDown(0))
                        {
                            tmp.GetComponent<PlayerInput>().enabled = true;//Enable the player input
                            cam_unit.Follow = tmp.transform.GetChild(0);
                            currentUnit = tmp;

                            CameraSwitcher.SwitchCamera(cam_unit);
                            UImanager.instance.Tunnels(1f);
                            StartCoroutine(StartCountdown());
                        }
                    }
                    else
                    {
                        //Debug.Log("Not playable unit");
                    }
                }
            }
        }
    }

    public IEnumerator StartCountdown(float countdownValue = 10)
    {
        float currentTime;
        currentTime = countdownValue;
        GameManager.instance.SpawnPowerup();
        yield return new WaitForSeconds(0.3f);
        GameManager.instance.VisualizeUnits(false);
        yield return new WaitForSeconds(2f);
        UImanager.instance.progressBar.SetActive(true);
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UImanager.instance.loadingBar.fillAmount = currentTime / countdownValue;
            UImanager.instance.durationText.text = currentTime.ToString("F0");
            yield return null;
        }

        UImanager.instance.progressBar.SetActive(false);
        UImanager.instance.Tunnels(0.5f);
        CameraSwitcher.SwitchCamera(cam_overview);
        GameManager.instance.CheckUnits();//Kill all units that dont have a roof
        currentUnit.GetComponent<PlayerInput>().enabled = false;
        GameManager.instance.NextPlayer();
        GameManager.instance.VisualizeUnits(true);
    }
}
