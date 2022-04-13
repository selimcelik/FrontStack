using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KeyStacker : MonoBehaviour
{
    public static KeyStacker Instance;

    public List<GameObject> keys = new List<GameObject>();
    public List<GameObject> deleteKeys = new List<GameObject>();
    public List<GameObject> levelEndKeyboardKeys = new List<GameObject>();

    public List<GameObject> confetties = new List<GameObject>();

    public GameObject hand;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject keyboard;

    public int doJumpIndex = 0;
    public int printerIndex = 0;

    public int score = 0;

    Coroutine nullCheck;

    public bool canLevelBegin = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EventManager.Instance.onkeyStacked += KeyStack;
        EventManager.Instance.onKeyMoved += KeyMove;
        EventManager.Instance.onObstacleTriggered += ObstacleTrigger;
        EventManager.Instance.onPrinterTriggered += PrinterTrigger;
        EventManager.Instance.onGateTriggered += GateTrigger;
        EventManager.Instance.onTouchToStart += TouchStart;
    }

    private void Update()
    {
        if (canLevelBegin)
        {
            KeyMove();
            if (score < 0)
            {
                score = 0;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                TouchStart();
            }
        }

    }

    private void TouchStart()
    {
        canLevelBegin = true;
        PlayerHolder.Instance.canMove = true;
    }

    private void KeyStack()
    {
        if (PlayerHolder.Instance.canMove)
        {

            nullCheck = StartCoroutine(Rescale());
        }



    }

    private void KeyMove()
    {

        if (keys.Count > 0)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i] == null)
                {
                    keys.Remove(keys[i]);
                }
            }
            keys[0].transform.DOMoveX(hand.transform.position.x, .2f);
            keys[0].transform.DOMoveZ(hand.transform.position.z + 14.5f, .2f);
            if (keys.Count > 1)
            {
                for (int i = 0; i < keys.Count - 1; i++)
                {
                    keys[i + 1].transform.DOMoveX(keys[i].transform.position.x, .2f);
                    keys[i + 1].transform.DOMoveZ(keys[i].transform.position.z + 10.5f, .2f);

                }
            }

        }
        
    }

    private void PrinterTrigger()
    {
        if (keys.Contains(keys[printerIndex]))
        {
            GameObject objectToDestroy = keys[printerIndex];
            switch (objectToDestroy.GetComponent<Key>().keyType)
            {
                case Key.KeyType.normal:
                    GameObject particleGO = ObjectPooler.Instance.SpawnForGameObject("CloudExplosionBlue", new Vector3(objectToDestroy.transform.position.x, objectToDestroy.transform.position.y + 3, objectToDestroy.transform.position.z), objectToDestroy.transform.rotation, null);
                    Destroy(particleGO, 2);
                    objectToDestroy.SetActive(false);
                    GameObject burgerKeyGO = ObjectPooler.Instance.SpawnForGameObject("Burger_Key", objectToDestroy.transform.position, objectToDestroy.transform.rotation, hand.transform);
                    keys[keys.IndexOf(objectToDestroy)] = burgerKeyGO;
                    Destroy(objectToDestroy);
                    burgerKeyGO.tag = "KeyCollector";
                    score += 2;
                    break;

                case Key.KeyType.burger:
                    GameObject particleGO1 = ObjectPooler.Instance.SpawnForGameObject("CloudExplosionBread", new Vector3(objectToDestroy.transform.position.x, objectToDestroy.transform.position.y + 3, objectToDestroy.transform.position.z), objectToDestroy.transform.rotation, null);
                    Destroy(particleGO1, 2);
                    objectToDestroy.SetActive(false);
                    GameObject crownKeyGO = ObjectPooler.Instance.SpawnForGameObject("Crown_Key", objectToDestroy.transform.position, objectToDestroy.transform.rotation, hand.transform);
                    keys[keys.IndexOf(objectToDestroy)] = crownKeyGO;
                    Destroy(objectToDestroy);
                    crownKeyGO.tag = "KeyCollector";
                    score += 3;
                    break;
            }
        }
        
        
        
    }

    private void ObstacleTrigger()
    {
        if (nullCheck != null)
        {
            StopCoroutine(nullCheck);
            nullCheck = null;
        }
        PlayerHolder.Instance.canMove = false;
        for (int i = doJumpIndex; i <= keys.Count-1; i++)
        {
            keys[i].transform.parent = null;
            keys[i].transform.DOKill();
            keys[i].transform.DOJump(new Vector3(Random.Range(-20, 20), keys[i].transform.position.y, keys[i].transform.position.z + (Random.Range(-10, 11))), 3, 1, .25f).OnComplete(() => keys[i].transform.DOScale(new Vector3(3, 3, 3), .1f));
            keys[i].tag = "Key";
            deleteKeys.Add(keys[i]);

        }
        for (int i = 0; i < deleteKeys.Count; i++)
        {
            deleteKeys[i].transform.DOScale(new Vector3(4, 4, 4), .1f);
            switch (deleteKeys[i].gameObject.GetComponent<Key>().keyType)
            {
                case Key.KeyType.normal:
                    score--;
                    break;
                case Key.KeyType.crown:
                    score -= 2;
                    break;
                case Key.KeyType.burger:
                    score -= 3;
                    break;
            }
            
            keys.Remove(deleteKeys[i]);
        }

        deleteKeys.Clear();
        hand.transform.parent.transform.DOKill();
        hand.transform.parent.transform.DOMoveZ(hand.transform.parent.transform.position.z - 20, .1f).OnComplete(() =>
        hand.transform.parent.transform.DOShakePosition(1, 1).OnStepComplete(() => PlayerHolder.Instance.canMove = true));
        doJumpIndex = 0;
    }

    private void GateTrigger()
    {
        StartCoroutine(levelEndCor());
    }

    IEnumerator Rescale()
    {
        score++;

        if (keys.Count > 0)
        {
            for (int i = keys.Count - 1; i >= 0; i--)
            {
                keys[i].tag = "KeyCollector";
                keys[i].transform.DOScale(new Vector3(5, 5, 5), .01f).OnComplete(() => keys[i].transform.DOScale(new Vector3(3, 3, 3), .01f));
                yield return new WaitForSeconds(0);
            }
        }
        

    }

    IEnumerator levelEndCor()
    {
        PlayerHolder.Instance.canMove = false;

        yield return new WaitForSeconds(.1f);

        cam1.SetActive(false);
        cam2.SetActive(true);

        yield return new WaitForSeconds(.5f);
        if(score >= levelEndKeyboardKeys.Count)
        {
            for (int i = 0; i < levelEndKeyboardKeys.Count; i++)
            {
                levelEndKeyboardKeys[i].SetActive(true);
                yield return new WaitForSeconds(.1f);
                levelEndKeyboardKeys[i].transform.DOMoveY(-41f, .3f);
            }
        }
        else
        {
            for (int i = 0; i < score; i++)
            {
                levelEndKeyboardKeys[i].SetActive(true);
                yield return new WaitForSeconds(.1f);
                levelEndKeyboardKeys[i].transform.DOMoveY(-41f, .3f);
            }
        }
        yield return new WaitForSeconds(1f);

        keyboard.transform.DOMove(new Vector3(-0.35f, -31.31f, 1110.35f), 2f);

        yield return new WaitForSeconds(.5f);

        UiManager.Instance.restartBtn.SetActive(true);

        for (int i = 0; i < confetties.Count; i++)
        {
            confetties[i].SetActive(true);
            yield return new WaitForSeconds(.1f);
        }
    }

}
