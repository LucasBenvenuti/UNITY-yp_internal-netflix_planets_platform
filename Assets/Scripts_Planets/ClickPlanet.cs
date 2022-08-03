using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPlanet : MonoBehaviour
{
    [SerializeField] LayerMask layer;

    Planet planet;
    
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, layer))
        {
            if(planet == null)
            {
                planet = hit.transform.GetComponentInChildren<Planet>();
            }
            else if(planet.transform != hit.transform)
            {
                planet = hit.transform.GetComponentInChildren<Planet>();
            }

            if(planet == null)
            {
                planet = hit.transform.GetComponentInParent<Planet>();
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (planet)
                {
                    planet.SelectPlanet();
                    StartSceneBttn.instance.hasOpenedPlanet = true;
                }
            }

            if(planet != null)
            {
                planet.planetHover.ShowLogo();
            }
        }
        else
        {
            if(planet != null)
            {
                planet.planetHover.HideLogo();
                planet = null;
            }
        }
    }
}
