using UnityEngine;

public class TestButton : MonoBehaviour, IActivatable
{
    public void Activate()
    {
        
        GetComponent<Renderer>().material.color = Color.green;
    }
}