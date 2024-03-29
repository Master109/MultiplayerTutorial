using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    /*  This file is part of the "Ultimate Unity networking project" by M2H (http://www.M2H.nl)
     *  This project is available on the Unity Store. You are only allowed to use these
     *  resources if you've bought them from the Unity Assets Store.
     */

    public GameObject ammoGO;
    public GameObject powerupGO;
    public GameObject hpGO;

    public GUIText powerupText;
    public int respawnTimePickup = 30;
    private bool isActive = false;
    private ArrayList powerUps = new ArrayList();
    private int currentItem = 0;
    private GameObject currentGO;

    void Start()
    {
        powerupText = GameObject.Find("powerupGUIText").GetComponent<GUIText>() as GUIText;
        powerupText.text = "";

        powerUps.Add("Invincible");
        powerUps.Add("hp");
        powerUps.Add("ammo");

        hpGO.SetActiveRecursively(false);
        ammoGO.SetActiveRecursively(false);
        powerupGO.SetActiveRecursively(false);

        StartCoroutine(NewPickup(0, Random.Range(0, powerUps.Count)));
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        PlayerScript playerScript = obj.GetComponent<PlayerScript>() as PlayerScript;

        if (isActive)
        {
            DoPickup(obj, playerScript);
        }
    }

    void DoPickup(GameObject obj, PlayerScript playerScript)
    {
        if (!playerScript.localPlayer)
        {
            //Only the local player can pickup his own item
            return;
        }

        string pickedUp = powerUps[currentItem] as string;

        NetworkView playersNetworkView = obj.GetComponent<NetworkView>() as NetworkView;

        if (pickedUp == "Invincible")
        {
            StartCoroutine(DisplayMessage("INVINCIBLE!", 10));
            playersNetworkView.RPC("StartInvincibility", RPCMode.All);
        }
        else if (pickedUp == "hp")
        {
            int newHP = playerScript.hp + 50;
            if (newHP > 100)
            {
                newHP = 100;
            }
            playersNetworkView.RPC("setHP", RPCMode.All, newHP);

            StartCoroutine(DisplayMessage("GOT EXTRA HP!", 2));

        }
        else if (pickedUp == "ammo")
        {
            Machinegun gun = obj.transform.Find("CrateCamera/Weapon").GetComponent<Machinegun>() as Machinegun;
            gun.clips += 3;
            gun.GetBulletsLeft();

            StartCoroutine(DisplayMessage("GOT EXTRA AMMO!", 2));

        }
        else
        {
            Debug.LogError("ERROR: UNKNOWN POWERUP: " + pickedUp);
        }

        //deactivates current pickup and will spawn a new one
        networkView.RPC("NewPickup", RPCMode.All, respawnTimePickup, Random.Range(0, powerUps.Count));

    }

    [RPC]
    IEnumerator NewPickup(int seconds, int newItem)
    {
        //Disable the pickup, reactive it after X seconds
        if (currentGO)
        {
            currentGO.SetActiveRecursively(false);
        }
        isActive = false;

        yield return new WaitForSeconds(seconds);

        currentItem = newItem;
        if (powerUps[currentItem]+"" == "hp")
        {
            currentGO = hpGO;
        }
        else if (powerUps[currentItem] + "" == "ammo")
        {
            currentGO = ammoGO;
        }
        else
        {
            currentGO = powerupGO;
        }
        isActive = true;
        currentGO.SetActiveRecursively(true);
    }

    IEnumerator DisplayMessage(string message, int seconds)
    {
        powerupText.text = message;
        yield return new WaitForSeconds(seconds);
        if (powerupText.text == message)
        {
            powerupText.text = "";
        }
    }


}