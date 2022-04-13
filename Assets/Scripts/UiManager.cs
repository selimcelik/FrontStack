using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public Text scoreText;
    public Text touchToStartText;
    public GameObject restartBtn;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(touchToStartTextAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyStacker.Instance.canLevelBegin)
        {
            scoreText.text = KeyStacker.Instance.score.ToString();
            touchToStartText.gameObject.SetActive(false);
        }
    }

    IEnumerator touchToStartTextAnim()
    {
        if (!KeyStacker.Instance.canLevelBegin)
        {

            touchToStartText.gameObject.transform.DOScale(new Vector3(.5f, .5f, .5f), .25f).OnComplete(() => touchToStartText.gameObject.transform.DOScale(new Vector3(1, 1f, 1f), .25f));
            yield return new WaitForSeconds(.25f);
            StartCoroutine(touchToStartTextAnim());
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }


}
