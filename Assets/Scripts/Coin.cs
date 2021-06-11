using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private AudioClip _audio_PickUp;

    private float _audioDuraton;

    private void Start()
    {
        if (_audio_PickUp == null)
        {
            Debug.LogError("Coin could not locate Pick Up Audio Clip.");
        }        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Return))
        {
            Player player = other.GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("Coin could not locate Player component on Collision.");
            }
            else
            {
                player.PickUpCoin();
                AudioSource.PlayClipAtPoint(_audio_PickUp, transform.position,0.8f);
                Destroy(this.gameObject);
            }
        }
    }    
}
