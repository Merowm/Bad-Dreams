using UnityEngine;
using System.Collections;

public class SwitchToDarkWoods : MonoBehaviour
{
    public Vector3 newCameraLimitMinimum;
    public Vector3 newCameraLimitMaximum;

    private GameObject player;
    private Transition transition;
    private Vector3 playerSpawnPosition;
    private GameObject cameraLimitMinimum, cameraLimitMaximum;
    private GameObject firstSectionBackground, secondSectionBackground;

    private void Start()
    {
        player = GameObject.Find("Player");
        transition = GameObject.Find("Transition").GetComponent<Transition>();
        playerSpawnPosition = GameObject.Find("Dark Woods Player Start Position").transform.position;
        cameraLimitMinimum = GameObject.Find("Camera Limit Min");
        cameraLimitMaximum = GameObject.Find("Camera Limit Max");
        firstSectionBackground = GameObject.Find("First Section Background");
        secondSectionBackground = GameObject.Find("Second Section Background");
        secondSectionBackground.SetActive(false);
        ModifyScene();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        transition.PlayForward(TransitionStyle.Death);
        transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "ModifyScene"));
    }

    private void ModifyScene()
    {
        transition.GetComponent<TweenScale>().RemoveOnFinished(new EventDelegate(this, "ModifyScene"));

        player.transform.position = playerSpawnPosition;
        cameraLimitMinimum.transform.position = newCameraLimitMinimum;
        cameraLimitMaximum.transform.position = newCameraLimitMaximum;
        firstSectionBackground.SetActive(false);
        secondSectionBackground.SetActive(true);

        transition.PlayReverse();
    }
}
