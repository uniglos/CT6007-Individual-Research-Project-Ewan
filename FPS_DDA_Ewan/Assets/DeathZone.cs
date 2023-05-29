using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
        private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.Die(this.gameObject,"Out of Bounds");
        }
    }
}
