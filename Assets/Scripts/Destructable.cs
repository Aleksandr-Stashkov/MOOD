using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private GameObject _piecesForReplacement;
    
    void Start()
    {
        if (_piecesForReplacement == null)
        {
            Debug.LogError("Destructable " + transform.name + " could not find its Pieces.");
        }
    }
        
    public void Destruct()
    {
        GameObject replacement = Instantiate(_piecesForReplacement, transform.position, transform.rotation, transform.parent);
        replacement.transform.localScale = transform.localScale*100f;
        Destroy(this.gameObject);
    }
}
