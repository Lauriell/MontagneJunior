using System.Collections;
using Cinemachine;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform endPoint;
    // Prefab du canvas de loadingScreen avec l’animator d’attaché
    public Animator loadingScreen;

    public GameObject player;
    // Le component attaché à la main camera quand on utilise cinemachine
    private CinemachineBrain cameraBrain;

    private void Start()
    {
        // Raccourci vers le component Camera attaché à l’objet taggé MainCamera
        var mainCamera = Camera.main;
        // S’il n’y a pas de camera trouvée
        if (mainCamera == null)
            Debug.LogError("No main camera found! Did you tagged it correctly?");
        else
            cameraBrain = mainCamera.GetComponent<CinemachineBrain>();

        player = GameObject.FindWithTag("Player");
        if (player == null)
            Debug.LogError("Player object not found! Did you tagged it correctly?");
    }

    // Ne pas oublier de configurer les layers pour ne pas trigger avec n’importe quoi
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(TeleportCoroutine());
        player.transform.position = endPoint.transform.position;
    }

    private IEnumerator TeleportCoroutine()
    {
        // On récupère la camera virtuelle qui est actuellement activée
        var activeCamera = cameraBrain.ActiveVirtualCamera;
        // On copie le prefab dans la scène, il va automatiquement lancer l’animation d’apparition
        var loadingAnimator = Instantiate(loadingScreen);
        // On récupère le temps en seconde que dure l’animation d’apparition
        var animTime = loadingAnimator.GetCurrentAnimatorStateInfo(0).length;
        // On récupère le moment où on lance la coroutine (temps depuis le lancement du jeu en secondes)
        var startTime = Time.time;
        // On attend que le temps de jeu ait dépassé le startTime + animTime
        // Si on lance cette coroutine à 15 secondes de jeu et que l’animation dure 1 seconde,
        // Alors on attend que le temps de jeu(Time.time) soit à 16 secondes (15+1) soit le temps du début + la durée
        yield return new WaitUntil(() => Time.time > startTime + animTime);
        // À ce moment là, l’animation d’apparition est terminée, l’écran de chargement est opaque
        // On peut donc téléporter l’avatar et la caméra de façon masquée
        // On calcule ainsi le vecteur qui représente le déplacement de l’avatar 
        // Vecteur A moins Vecteur B = Vecteur qui va de B vers A 
        var teleportDelta = endPoint.position - player.transform.position;
        // On téléporte la caméra virtuelle active en lui spécifiant que c’est l’avatar qui se téléporte 
        // Ainsi que le vecteur représentant le trajet parcouru
        activeCamera.OnTargetObjectWarped(player.transform, teleportDelta);

        // On téléporte notre avatar
        loadingAnimator.SetTrigger("Disappear");
        // On lance l’animation de disparition de l’écran de chargement
        loadingAnimator.SetTrigger("Disappear");
    }
}