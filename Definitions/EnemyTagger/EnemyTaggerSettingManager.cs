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
using EOSExt.TacticalBigPickup.Impl.EnemyTagger;

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
            base.FileChanged(e);
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

        private EnemyTaggerSettingManager()
        {

        }

        static EnemyTaggerSettingManager()
        {
            Current = new();
        }

    }
}
