using UnityEngine;
using System.Collections;

public class CreditsChangePage : MonoBehaviour
{
    private GameObject teamCredits;
    private GameObject assetCredits;

    private void Start()
    {
        teamCredits = transform.parent.FindChild("Team").gameObject;
        assetCredits = transform.parent.FindChild("Assets").gameObject;
    }

    private void OnClick()
    {
        if (teamCredits.activeSelf)
        {
            teamCredits.SetActive(false);
            assetCredits.SetActive(true);
        }
        else
        {
            teamCredits.SetActive(true);
            assetCredits.SetActive(false);
        }
    }
}
