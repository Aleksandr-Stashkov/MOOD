using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform _weapon;
    private ParticleSystem _muzzleFlash;
    private AudioSource _audio_Fire;
    private UI_Manager _UI_Manager;
    [SerializeField]
    private GameObject _hitMarker;    

    private int _maxAmmo = 100;
    private int _ammo;
    private float _reloadDuration = 1.5f;
    private bool _isReloading;
    private float _hitMarkerDuration;

    private bool _hasCoin = false;
    private bool _hasWeapon = false;

    private void Start()
    {
        FindObjects();

        _weapon.gameObject.SetActive(false);
        _ammo = 0;
        _UI_Manager.UpdateAmmo(_ammo);
        _muzzleFlash.Stop();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FindObjects()
    {
        if (_hitMarker == null)
        {
            Debug.LogError("Player could not locate Hit Marker Prefab.");
        }
        else
        {
            ParticleSystem hitMarker = _hitMarker.GetComponent<ParticleSystem>();
            if (hitMarker == null)
            {
                Debug.LogError("Player could not locate Particle System on Hit Marker.");
            }
            else
            {
                _hitMarkerDuration = hitMarker.main.duration;
            }
        }

        Transform child;
        if (_weapon == null)
        {
            Debug.LogError("Player could not locate Weapon.");
        }
        else
        {
            _audio_Fire = _weapon.GetComponent<AudioSource>();

            for (int i = 0; i < _weapon.childCount; i++)
            {
                child = _weapon.GetChild(i);
                if (child == null)
                {
                    Debug.LogError("Player could not locate a child of Weapon.");
                }
                else
                {
                    if (child.CompareTag("Effect"))
                    {
                        _muzzleFlash = child.GetComponent<ParticleSystem>();
                    }
                }
            }
        }

        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Player could not locate Canvas.");
        }
        else
        {
            _UI_Manager = canvas.GetComponent<UI_Manager>();
        }

        CheckObjects();
    }

    private void CheckObjects()
    {
        if (_UI_Manager == null)
        {
            Debug.LogError("Player could not locate UI Manager");
        }
        if (_muzzleFlash == null)
        {
            Debug.LogError("Player could not locate Muzzle Flash.");
        }
        if (_audio_Fire == null)
        {
            Debug.LogError("Player could not locate Audio Source on Weapon.");
        }
    }

    private void Update()
    {  
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (_hasWeapon && !_isReloading && Input.GetKeyDown(KeyCode.Keypad0))
        {
            StartCoroutine(Reload());
        }

        Shoot();        
    }    

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && _ammo > 0 && !_isReloading)
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hitInfo, 100f))
            {
                GameObject newHitMarker = Instantiate(_hitMarker, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(newHitMarker, _hitMarkerDuration);

                Transform crate = hitInfo.transform;
                if (crate.CompareTag("Destructable"))
                {
                    Destructable destructable = crate.GetComponent<Destructable>();
                    if (destructable == null)
                    {
                        Debug.LogError("Player could not locate Destructable component on an object.");
                    }
                    else
                    {
                        destructable.Destruct();
                    }
                }
            }            

            if (!_muzzleFlash.isPlaying)
            {
                _muzzleFlash.Play();
            }
            if (!_audio_Fire.isPlaying)
            {
                _audio_Fire.Play();
            }

            _ammo--;
            _UI_Manager.UpdateAmmo(_ammo);
        }
        else        
        {
            _muzzleFlash.Stop();
            _audio_Fire.Stop();
        }
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        yield return new WaitForSeconds(_reloadDuration);

        _ammo = _maxAmmo;
        _UI_Manager.UpdateAmmo(_ammo);
        _isReloading = false;
    }

    public void PickUpCoin()
    {
        _hasCoin = true;
        _UI_Manager.DisplayCoin();
    }
    
    public bool SellWeapon()
    {
        if (_hasCoin && !_hasWeapon)
        {
            _hasCoin = false;
            _UI_Manager.HideCoin();

            _hasWeapon = true;            
            _weapon.gameObject.SetActive(true);
            _ammo = _maxAmmo;
            _UI_Manager.UpdateAmmo(_ammo);
            return true;
        }
        else
        {
            return false;
        }
    }
}
