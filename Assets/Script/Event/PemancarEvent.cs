using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PemancarEvent : MonoBehaviour
{
    public static event Action saatTombolDiTekan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Tombol Space ditekan");
            saatTombolDiTekan?.Invoke();
        }
    }
}
