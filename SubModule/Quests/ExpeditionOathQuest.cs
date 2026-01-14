using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace OriginSystemMod.Quests
{
    /// <summary>
    /// 远征骑士的誓言任务：杀死1000海寇
    /// </summary>
    public class ExpeditionOathQuest : QuestBase
    {
        private const int TargetKillCount = 1000;
        private const string SeaRaidersFactionId = "sea_raiders";
        
        [SaveableField(1)]
        private int _killedCount = 0;
        
        [SaveableField(2)]
        private JournalLog _killCountLog;

        public ExpeditionOathQuest(string questId, Hero questGiver) 
            : base(questId, questGiver, CampaignTime.Never, 0)
        {
        }

        public override TextObject Title => new TextObject("{=expedition_oath_title}远征誓言：清剿海寇");

        public override bool IsRemainingTimeHidden => true;

        protected override void InitializeQuestOnGameLoad()
        {
            SetDialogs();
            UpdateKillCountLog();
        }

        protected override void OnStartQuest()
        {
            SetDialogs();
            var startLogText = new TextObject("{=expedition_oath_start}你已立下誓言，要清剿海岸线的海寇。杀死 {TARGET_COUNT} 名海寇以恢复你的荣誉。");
            startLogText.SetTextVariable("TARGET_COUNT", TargetKillCount);
            AddLog(startLogText, false);
            UpdateKillCountLog();
        }

        protected override void SetDialogs()
        {
            // 暂时不设置对话，避免API不匹配问题
        }

        protected override void RegisterEvents()
        {
            CampaignEvents.MapEventEnded.AddNonSerializedListener(this, OnMapEventEnded);
        }

        private void OnMapEventEnded(MapEvent mapEvent)
        {
            if (!IsOngoing || mapEvent == null)
                return;

            // 检查玩家是否参与了战斗
            if (!mapEvent.IsPlayerMapEvent)
                return;

            // 简化实现：检查是否有海寇队伍参与战斗
            // 注意：这里使用简化统计，实际应该统计具体杀死的数量
            // 暂时每次战斗+1，后续可以优化为精确统计
            bool hasSeaRaiders = false;
            foreach (var party in mapEvent.InvolvedParties)
            {
                if (party != null && party.MapFaction != null && 
                    party.MapFaction.StringId == SeaRaidersFactionId)
                {
                    hasSeaRaiders = true;
                    break;
                }
            }

            if (hasSeaRaiders)
            {
                // 简化：每次与海寇战斗+10（实际应该统计具体数量）
                // TODO: 优化为精确统计杀死的海寇数量
                _killedCount += 10;
                UpdateKillCountLog();
                OriginLog.Info($"[ExpeditionOathQuest] 已杀死 {_killedCount}/{TargetKillCount} 名海寇（本次战斗：+10）");

                // 检查是否完成任务
                CheckAndCompleteQuest();
            }
        }

        private void UpdateKillCountLog()
        {
            if (_killCountLog != null)
            {
                RemoveLog(_killCountLog);
            }

            var logText = new TextObject("{=expedition_oath_progress}已杀死海寇：{CURRENT}/{TARGET}");
            logText.SetTextVariable("CURRENT", _killedCount);
            logText.SetTextVariable("TARGET", TargetKillCount);
            
            _killCountLog = AddDiscreteLog(
                new TextObject("{=expedition_oath_progress_title}清剿进度"),
                logText,
                _killedCount,
                TargetKillCount
            );
        }

        private void CheckAndCompleteQuest()
        {
            if (_killedCount >= TargetKillCount)
            {
                var successText = new TextObject("{=expedition_oath_success}你已成功完成誓言，清剿了 {TARGET_COUNT} 名海寇！你的荣誉已恢复。");
                successText.SetTextVariable("TARGET_COUNT", TargetKillCount);
                AddLog(successText, false);
                
                base.CompleteQuestWithSuccess();
                OriginLog.Info("[ExpeditionOathQuest] 任务完成！");
            }
        }
    }
}
