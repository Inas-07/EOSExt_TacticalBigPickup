using GTFO.API.Utilities;
using GTFO.API;
using LevelGeneration;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ExtraObjectiveSetup.BaseClasses;
using ExtraObjectiveSetup.Utils;
using EOSExt.TacticalBigPickup.Definitions.EnemyTagger;

namespace EOSExt.TacticalBigPickup.Functions.EnemyTagger
{
    public class EnemyTaggerSettingManager: GenericDefinitionManager<EnemyTaggerSetting>
    {
        public static EnemyTaggerSettingManager Current { get; } = new();

        protected override string DEFINITION_NAME => "EnemyTagger";

        private static readonly EnemyTaggerSetting DEFAULT = new();

        public EnemyTaggerSetting SettingForCurrentLevel { private set; get; } = DEFAULT;

        protected override void FileChanged(LiveEditEventArgs e)
        {
            if (GameStateManager.IsInExpedition)
            {
                UpdateSetting();
            }
        }

        private void UpdateSetting()
        {
            uint mainLevelLayout = RundownManager.ActiveExpedition.LevelLayoutData;
            SettingForCurrentLevel = definitions.ContainsKey(mainLevelLayout) ? definitions[mainLevelLayout].Definition : DEFAULT;
            EOSLogger.Debug($"EnemyTaggerSettingManager: updated setting for level with main level layout id {mainLevelLayout}");
        }

        private List<GameObject> obsVisuals = new();

        public IEnumerable<GameObject> OBSVisuals => obsVisuals;

        public void SetupAsObserver(LG_PickupItem __instance)
        {
            var setting = SettingForCurrentLevel;

            CarryItemPickup_Core core = __instance.m_root.GetComponentInChildren<CarryItemPickup_Core>();
            Interact_Pickup_PickupItem interact = core.m_interact.Cast<Interact_Pickup_PickupItem>();
            LG_PickupItem_Sync sync = core.m_sync.Cast<LG_PickupItem_Sync>();

            EnemyTaggerComponent tagger = core.gameObject.AddComponent<EnemyTaggerComponent>();

            tagger.Parent = core;
            tagger.gameObject.SetActive(true);

            interact.InteractDuration = setting.TimeToPickup;
            tagger.MaxTagPerScan = setting.MaxTagPerScan;
            tagger.TagInterval = setting.TagInterval;
            tagger.TagRadius = setting.TagRadius;
            tagger.WarmupTime = setting.WarmupTime;

            GameObject obsVisual = null;
            if (setting.UseVisual)
            {
                obsVisual = GameObject.Instantiate(Assets.OBSVisual, __instance.transform);
                obsVisual.transform.localScale = new Vector3(setting.TagRadius, setting.TagRadius, setting.TagRadius);
                obsVisual.SetActive(false);

                obsVisuals.Add(obsVisual);
            }

            sync.OnSyncStateChange += new Action<ePickupItemStatus, pPickupPlacement, PlayerAgent, bool>((status, placement, playerAgent, isRecall) =>
            {
                switch (status)
                {
                    case ePickupItemStatus.PlacedInLevel:
                        tagger.PickedByPlayer = null;
                        tagger.ChangeState(setting.TagWhenPlaced ? eEnemyTaggerState.Active_Warmup : eEnemyTaggerState.Inactive);
                        interact.InteractDuration = setting.TimeToPickup;
                        if (obsVisual != null)
                        {
                            obsVisual.gameObject.transform.SetPositionAndRotation(placement.position, placement.rotation);
                            if (isRecall)
                            {
                                if (core.CanWarp)
                                {
                                    CoroutineManager.BlinkIn(obsVisual, tagger.WarmupTime);
                                }
                            }
                            else
                            {
                                if (!obsVisual.active && setting.TagWhenPlaced)
                                {
                                    CoroutineManager.BlinkIn(obsVisual, tagger.WarmupTime);
                                }
                            }
                        }
                        break;

                    case ePickupItemStatus.PickedUp:
                        tagger.gameObject.SetActive(true);
                        tagger.PickedByPlayer = playerAgent;
                        tagger.ChangeState(setting.TagWhenHold ? eEnemyTaggerState.Active_Warmup : eEnemyTaggerState.Inactive);
                        interact.InteractDuration = setting.TimeToPlace;
                        if (obsVisual != null && obsVisual.active)
                        {
                            CoroutineManager.BlinkOut(obsVisual);
                        }
                        break;
                }
            });
        }

        private void AddOBSVisualRenderers()
        {
            //foreach (var go in OBSVisuals)
            //{
            //    var renderer = go.GetComponentInChildren<Renderer>();
            //    float intensity = renderer.material.GetFloat("_Intensity");
            //    float behindWallIntensity = -1.0f;
            //    TSAManager.Current.RegisterPuzzleVisual(new TSAManager.PuzzleVisualWrapper()
            //    {
            //        GO = go,
            //        Renderer = renderer,
            //        Intensity = intensity,
            //        BehindWallIntensity = behindWallIntensity
            //    });
            //}
        }

        private void Clear()
        {
            obsVisuals.Clear();
        }

        private EnemyTaggerSettingManager()
        {
            LevelAPI.OnBuildStart += Clear;
            LevelAPI.OnLevelCleanup += Clear;
            LevelAPI.OnEnterLevel += AddOBSVisualRenderers;
        }

        static EnemyTaggerSettingManager()
        {
            Current = new();
        }

    }
}
