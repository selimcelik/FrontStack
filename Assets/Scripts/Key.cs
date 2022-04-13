using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public enum KeyType
    {
        normal,
        crown,
        burger
    }
    public KeyType keyType;

    // Start is called before the first frame update
    void Start()
    {
        if(keyType == KeyType.crown || keyType == KeyType.burger)
        {
            StartCoroutine(colliderTriggerAfterLetterPrinter());
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, -40.17f, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Key":
                if(gameObject.tag == "KeyCollector")
                {
                    KeyStacker.Instance.keys.Add(other.gameObject);
                    other.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    other.gameObject.transform.parent = gameObject.transform.parent.transform;
                    GameObject particleGO = ObjectPooler.Instance.SpawnForGameObject("StarExplosion", new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+5,gameObject.transform.position.z), gameObject.transform.rotation, null);
                    Destroy(particleGO, 2);
                    EventManager.Instance.KeyStacked();

                }
                break;
            case "Obstacle":
                if (gameObject.tag == "KeyCollector" && KeyStacker.Instance.keys.Contains(gameObject))
                {
                    GameObject particleGO = ObjectPooler.Instance.SpawnForGameObject("LightningExplosionBlue", new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3, gameObject.transform.position.z), gameObject.transform.rotation, null);
                    Destroy(particleGO, 2);
                    KeyStacker.Instance.doJumpIndex = KeyStacker.Instance.keys.IndexOf(gameObject);
                    EventManager.Instance.ObstacleTriggered();
                }
                break;
            case "LetterPrinter":
                if(gameObject.transform.parent.gameObject.tag == "Player")
                {
                    KeyStacker.Instance.printerIndex = KeyStacker.Instance.keys.IndexOf(gameObject);
                    EventManager.Instance.PrinterTriggered();
                }
                break;
            case "Gate":
                gameObject.SetActive(false);
                break;

        }
    }

    IEnumerator colliderTriggerAfterLetterPrinter()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
}
