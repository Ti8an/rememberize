using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action OnWinning;
    public static event Action OnCount;
    public static event Action OnTimer;

    [SerializeField] private GameObject Prefab;
    [SerializeField] private Sprite[] SpritesCard;

    [SerializeField] private MapComplexityScript[] Maps;
    [SerializeField] private float StartShowCardsSeconds = 2f;

    private List<CardScript> _cardsList;
    [HideInInspector] public CardScript selectCard_1, selectCard_2;


    # region Audio 
    
    [SerializeField] private AudioSource audio_Destroy;
    [SerializeField] private AudioSource audio_Win;
    public AudioSource audio_Click;

    #endregion

    private bool timerEnable = false;
    
    private float timerVal = 0;
    public float TimerVal
    {
        get => timerVal;
        private set
        {
            timerVal = value;
            OnTimer?.Invoke();
        }
    }

    private int motionCount = 0;
    
    [HideInInspector]
    public int MotionCount
    {
        get => motionCount; 
        set
        {
            motionCount = value;
            OnCount?.Invoke();
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    [ContextMenu("Start")]
    public void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        SpawnCardsToGrig();
        StartCoroutine(OpenStartCards());
        StartCoroutine(Timer());

        MotionCount = 0;
        StartTimer();
    }

    private void SpawnCardsToGrig()
    {
        if (_cardsList != null)
            foreach (CardScript card in _cardsList) Destroy(card.gameObject);

        _cardsList = new List<CardScript>();

        MapComplexityScript map = Maps[GameData.complexity];
        List<Transform> _points = map.points.ToList();

        while (_points.Count > 0)
        {
            int _spriteIndex = Random.Range(0, SpritesCard.Length);

            while (CheckContains(_spriteIndex)) _spriteIndex = Random.Range(0, SpritesCard.Length);

            for (int i = 0; i < 2; i++){

                int _pointRandomIndex = Random.Range(0, _points.Count);
                GameObject _go = Instantiate(Prefab, _points[_pointRandomIndex].position, Quaternion.identity);
                _points.RemoveAt(_pointRandomIndex);

                _go.transform.localScale = new Vector3(map.scale, map.scale, map.scale);
                CardScript _cs = _go.GetComponent<CardScript>();
                _cs.Init(SpritesCard[_spriteIndex], _spriteIndex);
                _cardsList.Add(_cs);
            }
        }

        bool CheckContains(int i)
        {
            if (_cardsList == null || _cardsList.Count == 0) return false;
            foreach (CardScript cs in _cardsList)
            {
                if (cs.id == i) return true;
            }
            return false;
        }
    }

    private void SelectAllCards(bool v)
    {
        foreach (CardScript cs in _cardsList) if (v) cs.Anim_open(); else cs.Anim_close();
    }

    private IEnumerator OpenStartCards()
    {
        SelectAllCards(true);
        yield return new WaitForSeconds(StartShowCardsSeconds);
        SelectAllCards(false);
    }

    public void Logic()
    {
        if (selectCard_1.id == selectCard_2.id)
        {
            StartCoroutine(DestroyCard(selectCard_1));
            StartCoroutine(DestroyCard(selectCard_2));

            selectCard_1 = null;
            selectCard_2 = null;
        }
        else
        {
            StartCoroutine(selectCard_1.Anim_delayAndClose(1));
            StartCoroutine(selectCard_2.Anim_delayAndClose(1));
        }

        if (_cardsList.Count <= 0)
        {
            //win
            StopTimer();
            StartCoroutine(WinInvoke());
        }
    }

    private IEnumerator WinInvoke()
    {
        yield return new WaitForSeconds(2f);
        audio_Win.Play();
        OnWinning?.Invoke();       
    }

    private IEnumerator DestroyCard(CardScript card)
    {
        _cardsList.Remove(card);
        audio_Destroy.Play();
        yield return new WaitForSeconds(1f);
        card.Anim_destroy();
        card.gameObject.AddComponent<Rigidbody2D>();
        Destroy(card.gameObject, 8f);
    }

    #region Timer
   
    private IEnumerator Timer()
    {
        const float timerPow = 1f;

        yield return new WaitForSeconds(timerPow);
        if (timerEnable) TimerVal += timerPow;
        StartCoroutine(Timer());
    }
    private void StartTimer()
    {
        timerEnable = true;
        TimerVal = 0f;
    }
    private void StopTimer()
    {
        timerEnable = false;
    }
   
    #endregion
}
