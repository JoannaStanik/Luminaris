using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceSpellRecognizer : MonoBehaviour
{
    [Header("Recognition")]
    public ConfidenceLevel minConfidence = ConfidenceLevel.Medium;
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

        // stworzenie listy zaklêæ/komend, które bêd¹ rozpoznawane przez system
        actions = new Dictionary<string, Action>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "ignis",   () => spellManager.CastIgnis()   },
            { "kuratio",   () => spellManager.CastKuratio()   },
            { "lux",     () => spellManager.CastLux()     },
            { "abrario", () => spellManager.CastAbrario() }
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
