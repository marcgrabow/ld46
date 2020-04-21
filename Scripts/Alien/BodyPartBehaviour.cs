using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _42.Dialogs;
using _42.Effects;
using Assets;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

public class BodyPartBehaviour : MonoBehaviour
{
    [Inject] private GameEvents _gameEvents;
    [Inject] private Prefabs _prefabs;
    [Inject] private DialogFactory _dialogs;
    
    public SpriteRenderer BodyPartRenderer;

    public List<InjuryBehaviour> Injuries = new List<InjuryBehaviour>();

    private Sprite _healthySprite;
    private ParticleSystem _particles;
    private ParticleSystem.EmissionModule _particlesEmission;
    private ParticleSystem.MainModule _particlesMain;

    public float BloodLoss
    {
        get { return Injuries.Sum(o => o.Config.BloodLossPerSecond); }
    }

    private void Awake()
    {
        BodyPartRenderer = GetComponentInChildren<SpriteRenderer>();
        _particles = GetComponentInChildren<ParticleSystem>();
        _particlesEmission = _particles.emission;
        _particlesMain = _particles.main;
        // find all configured injuries
        Injuries = GetComponentsInChildren<InjuryBehaviour>().ToList();
  //      _healthySprite = BodyPartRenderer.sprite;
//        SetCurrentInjurySprite();
        InvokeRepeating("AdjustParticles", 1, 1);
    }

    private void AdjustParticles()
    {
        var bloodLoss = BloodLoss; // 0.1
        if (bloodLoss == 0)
        {
            _particlesEmission.enabled = false;
        }
        else
        {
            _particlesEmission.enabled = true;
            var maxParticles = Mathf.CeilToInt(Mathf.Lerp(2, 1000, bloodLoss));
            _particlesMain.maxParticles = maxParticles;
        }
    }

    //
    // private void SetCurrentInjurySprite()
    // {
    //     if (Injuries.Count>0)
    //     {
    //         BodyPartRenderer.sprite = Injuries[0].Config.Sprite;
    //         // todo set particles etc
    //     }
    //     else
    //     {
    //         BodyPartRenderer.sprite = _healthySprite;
    //     }
    //
    // }

    public Treatment FindTreatment(ToolConfig tool)
    {
        if (tool == null) return null;

        if (tool.ToolType == ToolTypeEnum.Scanner)
        {
            // generic tool, can be applied to everything
            return _prefabs.ScannerTreatment;
        }
        
        
        if (Injuries.Count == 0) return null;
        var injury = Injuries[0];
        var treatment = injury.Config.Treatments.FirstOrDefault(t => t.ToolType == tool.ToolType);
        return treatment;
    }

    public void ApplyTreatment(Treatment treat)
    {
        if (treat.ToolType == ToolTypeEnum.Scanner)
        {
            var txt = new StringBuilder();
            txt.AppendFormat("Body part: <b>{0}</b>", name);
            txt.AppendLine();
            if (Injuries.Count == 0)
            {
                txt.AppendLine("No treatment required");    
            }
            else
            {
                foreach (var injury in Injuries)
                {
                    txt.AppendLine();
                    txt.AppendLine(injury.Config.ToString());
                }
            }
            
            _dialogs.Create("Scanner results", txt+"", new string[] {"ok"}, true);
            _gameEvents.OnScannerComplete.Invoke();
            // todo maybe pause game during dialog
            return;
        }
        
        Injuries[0].SetAsTreated();
        Injuries.RemoveAt(0);
        //SetCurrentInjurySprite();
    }

    public void Deactivate()
    {

        _particlesEmission.enabled = false;
        foreach (var injury in Injuries)
        {
            injury.GetComponent<FlashColorEffect>().CancelInvoke();
            Destroy(injury);
        }
        Destroy(this);
    }
}