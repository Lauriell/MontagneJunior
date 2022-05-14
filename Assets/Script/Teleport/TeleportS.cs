using UnityEngine;

public class TeleportS: MonoBehaviour
{
    public Transform PointTele;
    public GameObject Joueur;
    public GameObject Cam;
    public AudioClip audiosource;


    public void Start()
    {
        GetComponent<AudioSource>().clip = audiosource;
    }
    private void OnTriggerEnter(Collider other)
    {
        Joueur.transform.position = PointTele.transform.position;
        Cam.SetActive(true);//cam sur le mesh
        GetComponent<AudioSource>().Play();
    }
}