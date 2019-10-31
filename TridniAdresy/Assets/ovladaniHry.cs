using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ovladaniHry : MonoBehaviour {
    //Random generátor
    Random rnd = new Random(Guid.NewGuid().GetHashCode());

    //Vlastnosti obálky
    private Collider2D colliderObalky;
    private Rigidbody2D rigidbodyObalky;

    //Definice košů a obálky samotné
    public GameObject kos1; //Box pro třídu A
    public GameObject kos2; //Box pro třídu B
    public GameObject kos3; //Box pro třídu C
    public GameObject kos4; //Box pro třídu D
    public GameObject kos5; //Box pro třídu E
    public GameObject obalka;

    //Texty pro zobrazení hráči
    public Text IPAdresa;
    public Text skoreText;
    public Text pocetText;

    //Zdroje k textům
    private IPAddress zobrazenaAdresa;
    private int skore = 0;
    private int pocet = 0;

    //Inicializace proměnných
	void Start () {
        colliderObalky = GetComponent<Collider2D>();
        rigidbodyObalky = GetComponent<Rigidbody2D>();

        zobrazenaAdresa = vygenerujAdresu(rnd.Next(1, 6));
        IPAdresa.text = zobrazenaAdresa.ToString();
    }

    //Ovládání obálky
    void Update()
    {
        //Pohyb obálky doleva/doprava
        float pohyb = Input.GetAxis("Horizontal");
        rigidbodyObalky.velocity = new Vector2(pohyb * 6, rigidbodyObalky.velocity.y);

        //Pohyb obálky dolu
        float pohybDolu = Input.GetAxis("Vertical");
        if (pohybDolu <= -0.2)
            rigidbodyObalky.velocity = new Vector2(rigidbodyObalky.velocity.x, pohybDolu * 10);
        //else
        //rigidbodyObalky.velocity = new Vector2(rigidbodyObalky.velocity.x, (float)-0.5);

        obalka.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    //Ověření, zda obálka padla do správného koše
    private void OnCollisionEnter2D(Collision2D protikus)
    {
        byte prvniOktet = zobrazenaAdresa.GetAddressBytes()[0];
        switch (protikus.collider.name)
        {
            case "kos_A": //Třída A
                if (prvniOktet < 128) skore = skore + 1;
                else skore = skore - 1;
                break;
            case "kos_B": //Třída B
                if (prvniOktet >= 128 && prvniOktet < 192) skore = skore + 1;
                else skore = skore - 1;
                break;
            case "kos_C": //Třída C
                if (prvniOktet >= 192 && prvniOktet < 224) skore = skore + 1;
                else skore = skore - 1;
                break;
            case "kos_D": //Třída D
                if (prvniOktet >= 224 && prvniOktet < 240) skore = skore + 1;
                else skore = skore - 1;
                break;
            case "kos_E": //Třída E
                if (prvniOktet >= 240 && prvniOktet <= 255) skore = skore + 1;
                else skore = skore - 1;
                break;
            default: //Náraz do kraje hrací plochy
                skore = skore - 1;
                break;
        }
        pocet = pocet + 1;

        //Zapsání skóre a počtu do GUI prvků
        skoreText.text = skore.ToString();
        pocetText.text = pocet.ToString();

        //Nastavení nové výchozí pozice obálky
        obalka.transform.rotation = new Quaternion(0,0,0,0);
        
        rigidbodyObalky.velocity = new Vector3(0,0,0);
        obalka.transform.position = new Vector2((float)0.07, (float)3.16);
        zobrazenaAdresa = vygenerujAdresu(rnd.Next(1, 6));
        IPAdresa.text = zobrazenaAdresa.ToString();

        Thread.Sleep(500);
    }


    //Metoda pro vygenerování adresy
    private IPAddress vygenerujAdresu(int trida)
    {
        byte[] oktety = new byte[4];
        oktety[1] = (byte)rnd.Next(0, 256);
        oktety[2] = (byte)rnd.Next(0, 256);
        oktety[3] = (byte)rnd.Next(1, 255);

        switch (trida)
        {
            case 1:
                oktety[0] = (byte)rnd.Next(1, 128);
                break;
            case 2:
                oktety[0] = (byte)rnd.Next(128, 192);
                break;
            case 3:
                oktety[0] = (byte)rnd.Next(192, 224);
                break;
            case 4:
                oktety[0] = (byte)rnd.Next(224, 240);
                break;
            case 5:
                oktety[0] = (byte)rnd.Next(240, 256);
                break;
            default:
                oktety[0] = (byte)0;
                break;
        }
        return new IPAddress(oktety);
    }
}
