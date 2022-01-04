using DB.Utils;
using DB.War.Stack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DB.War;

public class HelperUI : MonoBehaviour
{
    public void OnReach70Ammox()
    {
        if(tutorialIndex == 0)
        {
            GotoNextStage();
        }
    }

    public void OnUpgradeStage()
    {
        if(tutorialIndex == 1)
        {
            GotoNextStage();
        }
    }

    public void OnUpgradeTower()
    {
        if(tutorialIndex == 2)
        {
            GotoNextStage();
        }
    }

    [SerializeField] private GameObject touchUIObj;
    [SerializeField] private Transform canvasT, playerT, baseT, towerT, bT;
    [SerializeField] private GameObject indicatorPrefab, dashLinePrefab, t0, t1, t2;
    [SerializeField] private List<GameObject> dots;

    /// <summary>
    /// 0 - find enemy base and go there
    /// 1 - go back to base once you have 70 ammox (upgrade base)
    /// 2 - upgrade a tower
    /// 3 and more - done
    /// </summary>
    int tutorialIndex = 0;

    private void BuildingDied(Building b)
    {
        if (tutorialIndex == 0)
        {
            GotoNextStage();
        }
    }

    GameObject indicator;
    private void Stage0()
    {
        t0.SetActive(true);
        StartCoroutine(TickStage0());

        GameObject go = Instantiate(indicatorPrefab);
        go.transform.parent = canvasT;
        Indicator ind = go.GetComponent<Indicator>();
        ind.SetTarget(bT.transform);
        ind.Activate();
        indicator = go;
    }

    private IEnumerator TickStage0()
    {
        while(tutorialIndex == 0)
        {
            NavMeshPath path = new NavMeshPath();
            Vector3 sp = playerT.position;
            sp.y = 0;
            Vector3 tp = bT.transform.position;
            tp.y = 0;
            tp = tp + (sp - tp).normalized * 10;
            tp.y = 0;

            if(NavMesh.CalculatePath(sp, tp, NavMesh.AllAreas, path))
            {
                ShowPath(path.corners);
            }
            else
            {
                print("pathfind err");
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void Stage1()
    {
        t0.SetActive(false);
        t1.SetActive(true);
        indicator.SetActive(false);
        StartCoroutine(TickStage1());

        GameObject go = Instantiate(indicatorPrefab);
        go.transform.parent = canvasT;
        Indicator ind = go.GetComponent<Indicator>();
        ind.SetTarget(baseT);
        ind.Activate();
        indicator = go;
    }

    private IEnumerator TickStage1()
    {
        while (tutorialIndex == 1)
        {
            NavMeshPath path = new NavMeshPath();
            Vector3 sp = playerT.position;
            sp.y = 0;
            Vector3 tp = baseT.transform.position;
            tp.y = 0;
            tp.y = 0;

            if (NavMesh.CalculatePath(sp, tp, NavMesh.AllAreas, path))
            {
                ShowPath(path.corners);
            }
            else
            {
                print("pathfind err");
            }

            yield return new WaitForEndOfFrame();
        }

        if (tutorialIndex > 1)
        {

        }
    }

    private void Stage2()
    {
        t1.SetActive(false);
        t2.SetActive(true);
        indicator.SetActive(false);
        StartCoroutine(TickStage2());

        GameObject go = Instantiate(indicatorPrefab);
        go.transform.parent = canvasT;
        Indicator ind = go.GetComponent<Indicator>();
        ind.SetTarget(towerT);
        ind.Activate();
        indicator = go;
    }

    private IEnumerator TickStage2()
    {
        while (tutorialIndex == 2)
        {
            NavMeshPath path = new NavMeshPath();
            Vector3 sp = playerT.position;
            sp.y = 0;
            Vector3 tp = towerT.transform.position;
            tp.y = 0;
            tp = tp + (sp - tp).normalized * 10;
            tp.y = 0;

            if (NavMesh.CalculatePath(sp, tp, NavMesh.AllAreas, path))
            {
                ShowPath(path.corners);
            }
            else
            {
                print("pathfind err");
            }

            yield return new WaitForEndOfFrame();
        }

        if (tutorialIndex > 2)
        {
            t2.SetActive(false);
            foreach(GameObject d in dots)
            {
                d.SetActive(false);
            }
        }
    }

    [SerializeField] private float betweenDots = 0.5f;
    private void ShowPath(Vector3[] corners)
    {
        int di = 0;
        float distance = 0;
        for(int i = 0; i < corners.Length - 1 && di < dots.Count; i++)
        {
            float mag = (corners[i + 1] - corners[i]).magnitude;
            while (true)
            {
                GameObject dot = dots[di++];
                dot.SetActive(true);
                Vector3 pos = Vector3.MoveTowards(corners[i], corners[i + 1], distance);
                dot.transform.position = pos;
                distance += betweenDots;

                // break
                if (distance >= mag)
                {
                    distance = 0;
                    break;
                }
            }
        }

        for(int i = di; i < dots.Count; i++)
        {
            dots[i].SetActive(false);
        }
    }

    private void GotoNextStage()
    {
        tutorialIndex++;

        switch (tutorialIndex)
        {
            case 0:
                Stage0();
                break;
            case 1:
                Stage1();
                break;
            case 2:
                Stage2();
                break;
            case 3:
                indicator.SetActive(false);
                PlayerPrefs.SetInt("done_tut", 1);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchUIObj.SetActive(false);
        }
    }

    private void Start()
    {
        t0.SetActive(false);
        t1.SetActive(false);
        t2.SetActive(false);

        GameObject dgo = new GameObject();
        for(int i = 0; i < 100; i++)
        {
            GameObject go = Instantiate(dashLinePrefab);
            go.SetActive(false);
            go.transform.parent = dgo.transform;
            dots.Add(go);
        }

        if(PlayerPrefs.GetInt("done_tut") == 0)
            Stage0();
    }

    /*private void OnDrawGizmos()
    {
        Vector3 sp = playerT.position;
        sp.y = 0;
        Vector3 tp = bT.transform.position;
        tp.y = 0;
        tp = tp + (sp - tp).normalized * 10;
        tp.y = 0;

        Gizmos.DrawSphere(sp, 0.4f);
        Gizmos.DrawSphere(tp, 0.4f);
    }*/
}
