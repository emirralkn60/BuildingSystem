 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjeBuild : MonoBehaviour
{
    public GameObject[] Objeler;
    public GameObject[] ObjelerOnizleme;

    bool ObjeOlusturulabilirmi=false;
    public bool EtkilesimVarmi = false;

    Transform socket;
    GameObject secilenObje;
    public Image CrossHair;

    void Start()
    {
        ObjelerOnizleme[0].SetActive(true);
        secilenObje = ObjelerOnizleme[0];
    }

    public void ObjeDegisimIslemleri(int fonkDeger,Color crossRenk,string SocketAdi,bool SocketDurumu)
    {
        foreach (var item in ObjelerOnizleme)
        {
            item.SetActive(false);
        }

        secilenObje = ObjelerOnizleme[fonkDeger];
        secilenObje.GetComponent<Renderer>().material.color = Color.green;
        CrossHair.color = crossRenk;

        if (SocketAdi != null) 
        {
            GameObject[] duvarSocketleri = GameObject.FindGameObjectsWithTag(SocketAdi);
            foreach (var item in duvarSocketleri)
            {
                if (item.CompareTag(SocketAdi))
                {
                    item.GetComponent<SphereCollider>().enabled = SocketDurumu;
                }
            }
        }

        
    }

    public void ObjeSocketYonetimi(RaycastHit hit,string[] genelVeriler,GameObject olusacakObje,
        string SocketAdi, bool SocketDurumu,string ObjeTuru)
    {
        if(ObjeTuru =="Single")
        {
            if (hit.transform.CompareTag(genelVeriler[0]) && secilenObje.name == genelVeriler[1])
            {
                secilenObje.transform.SetPositionAndRotation(hit.transform.position, hit.transform.rotation);
                secilenObje.SetActive(true);
                socket = hit.transform;

                if (ObjeOlusturulabilirmi)
                {
                    secilenObje.transform.position = socket.transform.position;
                }
                if (Input.GetMouseButtonDown(0) && ObjeOlusturulabilirmi)
                {
                    GameObject olusanObje = Instantiate(olusacakObje, socket.transform.position, socket.transform.rotation);
                    if (SocketAdi != null)
                    {
                        SphereCollider[] olusanObjeninCocuklari = olusanObje.GetComponentsInChildren<SphereCollider>();
                        foreach (var item in olusanObjeninCocuklari)
                        {
                            if (item.CompareTag(SocketAdi))
                            {
                                item.enabled = SocketDurumu;

                            }
                        }
                    }

                    socket = null;
                }

            }
            else if (secilenObje.name == genelVeriler[1])
            {

                secilenObje.SetActive(true);
                secilenObje.transform.Rotate(10000 * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * Vector3.up);
                secilenObje.transform.position = hit.point;
                if (Input.GetMouseButtonDown(0) && ObjeOlusturulabilirmi)
                {
                    GameObject olusanObje = Instantiate(olusacakObje, secilenObje.transform.position, secilenObje.transform.rotation);
                    if (SocketAdi != null)
                    {
                        SphereCollider[] olusanObjeninCocuklari = olusanObje.GetComponentsInChildren<SphereCollider>();
                        foreach (var item in olusanObjeninCocuklari)
                        {
                            if (item.CompareTag(SocketAdi))
                            {
                                item.enabled = SocketDurumu;
                            }
                        }
                    }

                    socket = null;
                }
            }
        }
        else if (ObjeTuru == "Multi")
        {
            if (hit.transform.CompareTag(genelVeriler[0]) && secilenObje.name == genelVeriler[1])
            {
                secilenObje.SetActive(true);
                ObjeOlusturulabilirmi = true;
                socket = hit.transform;
                secilenObje.transform.SetPositionAndRotation(socket.transform.position, socket.transform.rotation);

                if (Input.GetMouseButtonDown(0) && ObjeOlusturulabilirmi)
                {
                    Instantiate(olusacakObje, secilenObje.transform.position, secilenObje.transform.rotation);
                    socket = null;
                }

            }
            else if (secilenObje.name == genelVeriler[1])
            {

                secilenObje.transform.position = hit.point;
                ObjeOlusturulabilirmi = false;

            }
        }
        else if(ObjeTuru== "BalyozOnizleme")
        {
            if (secilenObje.name == ObjeTuru)
            {

                if (hit.transform.CompareTag("Platform"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Destroy(hit.transform.gameObject);
                    }

                }

            }
        }

    }
    public void SecilenObje(int deger)
    {
        switch(deger)
        {
            case 0:
                ObjeDegisimIslemleri(deger,Color.black, "DuvarSocket",false); 
               
                break;
            case 1:
                ObjeDegisimIslemleri(deger, Color.black, "DuvarSocket", true);
                break;
            case 5:
                ObjeDegisimIslemleri(deger, Color.red, null,false);
                break;
        }
    }
    void Update()
    {

        

        if (ObjeOlusturulabilirmi)
        {
            secilenObje.GetComponent<Renderer>().material.color = Color.green;
        }   
        else
        {
            secilenObje.GetComponent<Renderer>().material.color = Color.red;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray,out RaycastHit hit, 10f))
        {
            secilenObje.SetActive(true);

            if (hit.transform.CompareTag("Platform") || EtkilesimVarmi )
            {
                secilenObje.transform.position = hit.point;
                ObjeOlusturulabilirmi = false;
            }else
            {
                ObjeOlusturulabilirmi = true;
            }

            //Parke
            string[] veriler = { "Socket", "ParkeOnizleme" };
            ObjeSocketYonetimi(hit, veriler, Objeler[0], "DuvarSocket", false,"Single");

            //Duvar
            string[] veriler2 = { "DuvarSocket", "DuvarOnizleme" };
            ObjeSocketYonetimi(hit, veriler2, Objeler[1], null, false, "Multi");

            //Silme
            
            ObjeSocketYonetimi(hit, null, null, null, false, "BalyozOnizleme");

            //--------------------------------



        }
        else
        {
            secilenObje.SetActive(false);

        }

    }
}
/*
           if (hit.transform.CompareTag("Socket") && secilenObje.name== "ParkeOnizleme")
           {
               secilenObje.transform.SetPositionAndRotation(hit.transform.position, hit.transform.rotation);
               secilenObje.SetActive(true);
               socket = hit.transform;

               if(ObjeOlusturulabilirmi)
               {
                   secilenObje.transform.position = socket.transform.position;
               }
               if (Input.GetMouseButtonDown(0) && ObjeOlusturulabilirmi)
               {
                  GameObject olusanObje= Instantiate(Objeler[0], socket.transform.position, socket.transform.rotation);
                   SphereCollider [] olusanObjeninCocuklari = olusanObje.GetComponentsInChildren<SphereCollider>();
                   foreach(var item in olusanObjeninCocuklari)
                   {
                       if(item.CompareTag("DuvarSocket"))
                       {
                           item.enabled = false;
                       }
                   }
                   socket = null;
               }

           }
           else if(secilenObje.name == "ParkeOnizleme"&&!hit.transform.CompareTag("DuvarSocket"))
           {

               secilenObje.SetActive(true);
               secilenObje.transform.Rotate(10000 * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * Vector3.up);
               secilenObje.transform.position = hit.point;
               if (Input.GetMouseButtonDown(0) && ObjeOlusturulabilirmi)
               {
                   GameObject olusanObje = Instantiate(Objeler[0], secilenObje.transform.position, secilenObje.transform.rotation);
                   SphereCollider[] olusanObjeninCocuklari = olusanObje.GetComponentsInChildren<SphereCollider>();
                   foreach (var item in olusanObjeninCocuklari)
                   {
                       if (item.CompareTag("DuvarSocket"))
                       {
                           item.enabled = false;
                       }
                   }
                   socket = null;
               }
           }
if (hit.transform.CompareTag("DuvarSocket") && secilenObje.name == "DuvarOnizleme")
            {
                secilenObje.SetActive(true);
                ObjeOlusturulabilirmi = true;
                socket = hit.transform;
                secilenObje.transform.SetPositionAndRotation(socket.transform.position, socket.transform.rotation);
               
                if (Input.GetMouseButtonDown(0) && ObjeOlusturulabilirmi)
                {
                    Instantiate(Objeler[1], secilenObje.transform.position, secilenObje.transform.rotation);
                    socket = null;
                }

            }
            else if (secilenObje.name == "DuvarOnizleme")
            {
               
                secilenObje.transform.position = hit.point;
                ObjeOlusturulabilirmi = false;
                
            }
// silme iþlemi
if (secilenObje.name == "BalyozOnizleme")
            {

                if(hit.transform.CompareTag("Platform"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Destroy(hit.transform.gameObject);
                    }
                    
                }

            }
           */