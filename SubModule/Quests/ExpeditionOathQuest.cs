using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace OriginSystemMod.Quests
{
    public class ExpeditionOathQuest // : QuestBase
    {
        private string _oathType;

        public ExpeditionOathQuest(string heroId, string oathType, int targetCount) 
            // : base("expedition_oath_quest_" + oathType, null, CampaignTime.Never, 0)
        {
            _oathType = oathType;
        }

        public void StartQuest()
        {
            OriginLog.Info($"[ExpeditionQuest] Fake start for {_oathType}");
        }
/*

        protected override void SetDialogs()
        {
            // Dialogs can cause API mismatch issues. Temporarily disabled for compilation safety.
        }

        public override TextObject Title
        {
            get
            {
                if (_oathType == "kill_sea_raiders") return new TextObject("Oath: Scourge of the Sea Raiders");
                return new TextObject("Oath of the Expedition Knight");
            }
        }

        public override bool IsRemainingTimeHidden => true;

        protected override void InitializeQuestOnGameLoad()
        {
            SetDialogs();
        }

        protected override void OnStartQuest()
        {
            SetDialogs();
            
            if (_oathType == "kill_sea_raiders")
            {
               AddLog(new TextObject("You have sworn an oath to purge the coasts. Kill 1000 Sea Raiders to restore your honor."), false);
            }
            else
            {
               AddLog(new TextObject("You have sworn an oath."), false);
            }
        }
*/
    }
}
