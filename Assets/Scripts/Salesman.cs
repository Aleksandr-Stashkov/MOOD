using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salesman : MonoBehaviour
{
    private AudioSource _audio_Sale;

    void Start()
    {
        _audio_Sale = GetComponent<AudioSource>();
        if (_audio_Sale == null)
        {
            Debug.LogError("Salesman could not locate Audio Source.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Return))
        {
            Player player = other.GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("Salesman could not locate Player component on collision.");
            }
            else
            {
                if (player.SellWeapon())
                {
                    _audio_Sale.Play();
                }
            }
        }
    }
}
