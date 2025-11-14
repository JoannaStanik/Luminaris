using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceSpellRecognizer : MonoBehaviour
{
    [Header("Recognition")]
    public ConfidenceLevel minConfidence = ConfidenceLevel.Medium;
    [Tooltip("Anty-dubler: minimalny odstêp miêdzy kolejnymi rozpoznaniami (s).")]
    public float minInterval = 0.8f;

    [Header("Opcjonalnie: push-to-talk (V)")]
    public bool pushToTalk = false;
    public KeyCode pushToTalkKey = KeyCode.V;

    [Header("References")]
    public SpellManager spellManager;

    private KeywordRecognizer recognizer;
    private Dictionary<string, Action> actions;
    private float lastTime;

    void Start()
    {
        if (spellManager == null)
        {
            spellManager = FindObjectOfType<SpellManager>();
        }

        actions = new Dictionary<string, Action>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "ignis",   () => spellManager.CastIgnis()   },
            { "fulmen",  () => spellManager.CastFulmen()  },
            { "terra",   () => spellManager.CastTerra()   },
            { "aeris",   () => spellManager.CastAeris()   },
            { "lux",     () => spellManager.CastLux()     },
            { "curatio", () => spellManager.CastCuratio() },
            { "tempus",  () => spellManager.CastTempus()  },
            { "clarus",  () => spellManager.CastClarus()  }
        };

        recognizer = new KeywordRecognizer(actions.Keys.ToArray(), minConfidence);
        recognizer.OnPhraseRecognized += OnPhraseRecognized;

        if (!pushToTalk) recognizer.Start();
        Debug.Log("[Voice] KeywordRecognizer ready. PushToTalk: " + pushToTalk);
    }

    void Update()
    {
        if (!pushToTalk || recognizer == null) return;

        if (Input.GetKeyDown(pushToTalkKey) && !recognizer.IsRunning) recognizer.Start();
        if (Input.GetKeyUp(pushToTalkKey) && recognizer.IsRunning) recognizer.Stop();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (Time.time - lastTime < minInterval) return;

        if (actions.TryGetValue(args.text, out var act))
        {
            lastTime = Time.time;
            Debug.Log($"[Voice] {args.text} ({args.confidence})");
            act?.Invoke();
        }
    }

    void OnDestroy()
    {
        if (recognizer != null)
        {
            if (recognizer.IsRunning) recognizer.Stop();
            recognizer.OnPhraseRecognized -= OnPhraseRecognized;
            recognizer.Dispose();
        }
    }
}
