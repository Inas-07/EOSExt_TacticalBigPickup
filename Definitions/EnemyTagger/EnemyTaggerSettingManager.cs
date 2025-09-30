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

        protected override string DEFINITION_NAME => "EnemyTagger_EOS";

        private EnemyTaggerSettingManager()
        {

        }

        static EnemyTaggerSettingManager()
        {
            Current = new();
        }

    }
}
