using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    private Image _img_Crosshair;
    private Text _txt_Ammo;
    private GameObject _coin;

    void Start()
    {
        FindObjects();

        CrosshairColor(false);
        _coin.SetActive(false);
    }      
    
    private void FindObjects()
    {
        Transform child;
        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            if (child == null)
            {
                Debug.LogError("UI Maanger could not find its child.");
            }
            else
            {
                switch (child.name)
                {
                    case "Crosshair Image":
                        _img_Crosshair = child.GetComponent<Image>();
                        break;
                    case "Ammo Text":
                        _txt_Ammo = child.GetComponent<Text>();
                        break;
                    case "Inventory Image":
                        Transform child2 = child.GetChild(0);
                        if (child2 == null)
                        {
                            Debug.LogError("UI Manager could not find child of Inventory Image.");
                        }
                        else
                        {
                            _coin = child2.gameObject;
                        }
                        break;
                    default:
                        Debug.LogWarning("There is an unrecognized child of Canvas.");
                        break;
                }
            }
        }

        if (_img_Crosshair == null)
        {
            Debug.LogError("UI Manager could not locate Crosshair Image.");
        }
        if (_coin == null)
        {
            Debug.LogError("UI Manager could not locate Coin.");
        }
        if (_txt_Ammo == null)
        {
            Debug.LogError("UI Manager could not locate Ammo Text.");
        }
    }

    public void CrosshairColor(bool canShoot)
    {
        if (canShoot)
        {
            _img_Crosshair.color = new Color(0.9056604f, 0.3631186f, 0.4069262f);
        }
        else
        {
            _img_Crosshair.color = new Color(1f, 1f, 1f);
        }
    }

    public void UpdateAmmo(int ammo)
    {
        _txt_Ammo.text = ammo.ToString();
    }

    public void DisplayCoin()
    {
        _coin.SetActive(true);
    }

    public void HideCoin()
    {
        _coin.SetActive(false);
    }
}
