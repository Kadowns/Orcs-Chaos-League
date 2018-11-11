using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class fonteSpritesCode : MonoBehaviour
{
    public Sprite mapS_rockS, mapS_burningS, mapS_chaosS, mapS_kingS, mapS_runnerS;

    public Sprite p1_cpuS, p2_cpuS, p3_cpuS, p4_cpuS;

    public Sprite p1_offS, p2_offS, p3_offS, p4_offS;

    public Sprite p1_onS, p2_onS, p3_onS, p4_onS;

    public static Sprite p1_cpu, p2_cpu, p3_cpu, p4_cpu, p1_off, p2_off, p3_off, p4_off, p1_on, p2_on, p3_on, p4_on;

    public static Sprite mapS_rock, mapS_burning, mapS_chaos, mapS_king, mapS_runner;

    // Use this for initialization
    void Start()
    {
        p1_cpu = p1_cpuS;
        p2_cpu = p2_cpuS;
        p3_cpu = p3_cpuS;
        p4_cpu = p4_cpuS;

        p1_off = p1_offS;
        p2_off = p2_offS;
        p3_off = p3_offS;
        p4_off = p4_offS;

        p1_on = p1_onS;
        p2_on = p2_onS;
        p3_on = p3_onS;
        p4_on = p4_onS;

        mapS_rock = mapS_rockS;
        mapS_burning = mapS_burningS;
        mapS_chaos = mapS_chaosS;
        mapS_king = mapS_kingS;
        mapS_runner = mapS_runnerS;
    }
    // Update is called once per frame
    void Update()
    {

    }
}

/* OLD PLAYER SELECTION
  public void ButtonVerify()
  {
      startButtonEntradaPlayers = FindObjectOfType<Button>();

      if (menuToggle1.isOn == true || menuToggle2.isOn == true && menuToggle3.isOn == true && startButtonEntradaPlayers.interactable == false)
      {
          startButtonEntradaPlayers.interactable = true;
      }

      if (menuToggle1.isOn == false && menuToggle2.isOn == false && menuToggle3.isOn == false)
      {
          startButtonEntradaPlayers.interactable = false;
      }

      if (menuToggle1.isOn == true)
      {
          ButtonSelector1 = GameObject.Find("SelectedPlayers");
      }
      if (menuToggle2.isOn == true)
      {
          ButtonSelector2 = GameObject.Find("SelectedPlayers2");
      }
      if (menuToggle3.isOn == true)
      {
          ButtonSelector3 = GameObject.Find("SelectedPlayers3");
      }

      if (ButtonSelector1.activeSelf == true && menuToggle1.isOn == false)
      {
          menuToggle1.isOn = true;
          startButtonEntradaPlayers.interactable = true;
      }

      if (ButtonSelector2.activeSelf == true && menuToggle2.isOn == false)
      {
          menuToggle2.isOn = true;
          startButtonEntradaPlayers.interactable = true;
      }

      if (ButtonSelector3.activeSelf == true && menuToggle3.isOn == false)
      {
          menuToggle3.isOn = true;
          startButtonEntradaPlayers.interactable = true;
      }
  }
  */

