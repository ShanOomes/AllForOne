using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigUnit : MonoBehaviour
{
    public static ConfigUnit instance { get; set; }
    public GameObject unit;

    private Camera cam;
    private Ray ray;

    private int placeAbleMask;

    private bool isPlacing = false;

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
        cam = Camera.main;
        placeAbleMask = LayerMask.GetMask("PlaceAble");

        UImanager.instance.selectionPanel.SetActive(true);

        //Set UI for start of game
        UImanager.instance.UpdateUI();
        RandomizeSliders();
    }

    void Update()
    {
        if (isPlacing)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, placeAbleMask))
                {
                    if (hit.collider != null)
                    {
                        if (SetUnit(hit.point))
                        {
                            isPlacing = false;
                            GameManager.instance.NextPlayer();
                            if (GameManager.instance.GetCurrentPlayer().HasEnough())
                            {
                                StartCoroutine(ResetSetup());//Restart for the next player
                            }
                            else
                            {
                                Debug.Log("Nobody has money left, start next phase of game");
                            }
                        }
                    }
                }
            }
        }
    }

    bool SetUnit(Vector3 pos)
    {
        GameObject tmp = Instantiate(unit, pos, Quaternion.identity);
        List<Slider> sliders = UImanager.instance.GetValues();

        tmp.GetComponent<Unit>().SetValues(sliders[0].value, sliders[1].value, sliders[2].value, sliders[3].value, GameManager.instance.GetCurrentPlayer().Name);//Set stats of created unit
        tmp.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        GameManager.instance.AddUnit(tmp);//Add created unit to global list of units
        return true;
    }

    public void ButtonClick()
    {
        UImanager UImanager = UImanager.instance;
        GameManager gameManager = GameManager.instance;

        if (gameManager.GetCurrentPlayer().ReduceBalance(UImanager.GetTotalCost()))
        {
            UImanager.selectionPanel.SetActive(false);
            isPlacing = true;
        }
    }

    public IEnumerator ResetSetup()
    {
        yield return new WaitForSeconds(1f);
        UImanager UImanager = UImanager.instance;
        UImanager.selectionPanel.SetActive(true);
        UImanager.textCost.color = new Color(255, 255, 255, 1);

        UImanager.UpdateUI();
        RandomizeSliders();
    }

    public void RandomizeSliders(){
        UImanager manager = UImanager.instance;
        for (int i = 0; i < manager.listSliders.Count; i++)
        {
            manager.listSliders[i].value = Random.Range(1, 100);
        }
    } 
}
