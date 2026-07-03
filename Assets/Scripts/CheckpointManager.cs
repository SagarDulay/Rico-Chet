using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [SerializeField] private Vector3 startPosition;
    private Vector3 _currentCheckpoint;

    private void Awake()
    {
        Instance = this;
        _currentCheckpoint = startPosition;
    }

    public void SetCheckpoint(Vector3 position)
    {
        _currentCheckpoint = position;
        Debug.Log("Checkpoint set: " + position);
    }

    public void RespawnPlayer(Transform player)
    {
        
        DeathEffect.Instance.PlayDeathFlash();

        
        AudioManager.Instance.PlayDeath();

        
        ResetAllPuzzles();

       
        StartCoroutine(DelayedRespawn(player));
    }

    private IEnumerator DelayedRespawn(Transform player)
    {
        yield return new WaitForSeconds(0.3f);

        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.position = _currentCheckpoint;
        cc.enabled = true;
    }

    private void ResetAllPuzzles()
    {
        
        IResettable[] resettables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            as IResettable[];

        
        foreach (MonoBehaviour mb in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (mb is IResettable resettable)
            {
                resettable.ResetState();
            }
        }
    }
}