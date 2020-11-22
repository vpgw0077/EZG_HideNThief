using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Hand theHand;
    public static bool isActivate = false;
    public bool isReady = false;

    public IEnumerator PrepareCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        isReady = true;
    }

    public Hand getHand()
    {
        return theHand;
    }

    public void HandChange(Hand _hand)
    {
        if (ItemManager.currentWeapon != null)

            ItemManager.currentWeapon.gameObject.SetActive(false);




        theHand = _hand;
        ItemManager.currentWeapon = theHand.GetComponent<Transform>();

        theHand.transform.localPosition = Vector3.zero;
        theHand.gameObject.SetActive(true);
        isActivate = true;

    }

}
