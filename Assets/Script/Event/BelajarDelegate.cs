using UnityEngine;
using System;

public class BelajarDelegate : MonoBehaviour
{
    delegate void ContohDelegate();

    void Start()
    {
        UjiDelegate1();
        UjiDelegate2();
        UjiDelegate3();
    }

    void Update()
    {
    }

    void UjiDelegate1()
    {
        ContohDelegate halo = PanggilNama;
        halo();
    }

    void UjiDelegate2()
    {
        ContohDelegate halo = PanggilHello;
        halo += PanggilHello;
        halo();
    }

    void UjiDelegate3()
    {
        Action halo = PanggilHello;
        halo += PanggilHello;
        halo();
    }

    void PanggilNama()
    {
        Debug.Log("Pak Surya");
    }

    void PanggilHello()
    {
        Debug.Log("Hello");
    }
}