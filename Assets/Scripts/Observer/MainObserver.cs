using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainObserver : BaseObserver
{
    // Start is called before the first frame update
    [SerializeField]
    private AudioSource loginSE = null;
    [SerializeField]
    private AudioSource firstVoice = null;
    [SerializeField]
    private GameObject[] players = null;
    [SerializeField]
    private GameObject cave = null;
    //
    [SerializeField]
    private AudioSource caveSE = null;
    [SerializeField]
    private float caveSETime = 30;
    private int caveSEPlayCount = 1;
    [SerializeField]
    private Search[] searches = null;
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private float distanceChange = 100f;
    [SerializeField]
    private AudioSource voice1 = null;
    private bool voice1Flag = false;
    [SerializeField]
    private AudioSource voice2 = null;
    private bool voice2Flag = false;
    //
    [SerializeField]
    private AudioSource longNoiseSE = null;
    private float timeCount = 0f;
    //
    [System.Serializable]
    public class Search
    {
        [SerializeField]
        private AudioSource se = null;
        public AudioSource SE { get { return se; } }
        [SerializeField]
        private GameObject target = null;
        public GameObject Target { get { return target; } }
    }
    private void Awake()
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if (!players[i].activeSelf) continue;
            players[i].SetActive(false);
        }
        cave.SetActive(false);
    }
    public override void Action()
    {
        base.Action();
        Searching();
    }
    public override void CountTimer()
    {
        base.CountTimer();
        if (Parameter.CurrentPlayTime > caveSETime * caveSEPlayCount)
        {
            caveSE.Play();
            ++caveSEPlayCount;
        }
        if (!voice1Flag && Parameter.MaxPlayTime - Parameter.CurrentPlayTime <= 30)
        {
            voice1.Play();
            voice1Flag = true;
        }
        if (!voice2Flag && Parameter.MaxPlayTime - Parameter.CurrentPlayTime <= 5)
        {
            voice2.Play();
            voice2Flag = true;
        }
    }
    public override void GameStart()
    {
        if (!firstVoice.isPlaying && input.LC.IndexTrigger.Axis > 0.5f)
        {
            loginSE.Play();
            for (int i = 0; i < searches.Length; ++i)
            {
                searches[i].SE.Play();
            }
            for (int i = 0; i < players.Length; ++i)
            {
                if (players[i].activeSelf) continue;
                players[i].SetActive(true);
            }
            cave.SetActive(true);
            Parameter.StartGameFlag = true;
        }
    }
    public override void GameOver()
    {
        if (Parameter.Life <= 0)
        {
            Parameter.Life = 0;
            Parameter.EndGameFlag = true;
        }
    }
    public override void GameEnd()
    {
        if (cave.activeSelf)
        {
            cave.SetActive(false);
            longNoiseSE.Play();
            for (int i = 0; i < searches.Length; ++i)
            {
                searches[i].SE.Stop();
            }
        }
        for (int i = 0; i < players.Length; ++i)
        {
            if (!players[i].activeSelf) continue;
            players[i].SetActive(false);
        }
        if (input.LC.IndexTrigger.Axis > 0.5f)
        {
            if (timeCount < 3f)
            {
                timeCount += Time.deltaTime;
                return;
            }
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            timeCount = 0;
        }
    }
    public void Searching()
    {
        for (int i = 0; i < searches.Length; ++i)
        {
            float a = Vector3.Distance(searches[i].Target.transform.position, player.transform.position);
            if (a < distanceChange)
            {
                searches[i].SE.volume = 1 - a / distanceChange;
            }
            else
            {
                searches[i].SE.volume = 0;
            }
        }
    }
}
