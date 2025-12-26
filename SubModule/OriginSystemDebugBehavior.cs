using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace OriginSystemMod
{
    /// <summary>
    /// 调试监视器：用于对照实验
    /// 记录玩家手动加入库塞特成为封臣的过程，包括所有状态变化和调用链
    /// </summary>
    public class OriginSystemDebugBehavior : CampaignBehaviorBase
    {
        /// <summary>
        /// 是否启用监视器（可从配置/常量开关）
        /// </summary>
        public static bool DebugWatchVassalJoin { get; set; } = true;

        private float _lastSnapshotTime = 0f;
        private const float SNAPSHOT_INTERVAL = 0.5f; // 每 0.5 秒打印一次快照（只在状态变化时打印）

        // 上一次的状态快照（用于检测变化）
        private bool _lastIsLord = false;
        private Occupation? _lastOccupation = null;
        private string _lastKingdomId = null;
        private bool _lastIsUnderMercenaryService = false;
        private bool _lastIsClanTypeMercenary = false;
        private bool _lastIsMinorFaction = false;
        private bool _lastIsNoble = false;
        private bool _lastIsVassalLike = false; // 新增：可验证的封臣判据
        private int _lastTier = -1;
        private float _lastRenown = -1f;
        private float _lastInfluence = -1f;

        public override void RegisterEvents()
        {
            CampaignEvents.TickEvent.AddNonSerializedListener(this, OnTick);
        }

        public override void SyncData(IDataStore dataStore)
        {
            // 不需要序列化数据
        }

        private void OnTick(float dt)
        {
            if (!DebugWatchVassalJoin)
                return;

            if (Hero.MainHero == null || Clan.PlayerClan == null)
                return;

            _lastSnapshotTime += dt;

            // 每 0.5 秒检查一次状态变化
            if (_lastSnapshotTime >= SNAPSHOT_INTERVAL)
            {
                _lastSnapshotTime = 0f;

                // 获取当前状态
                bool currentIsLord = Hero.MainHero.IsLord;
                Occupation? currentOccupation = Hero.MainHero.Occupation;
                string currentKingdomId = Clan.PlayerClan.Kingdom?.StringId;
                bool currentIsUnderMercenaryService = Clan.PlayerClan.IsUnderMercenaryService;
                bool currentIsClanTypeMercenary = Clan.PlayerClan.IsClanTypeMercenary;
                bool currentIsMinorFaction = Clan.PlayerClan.IsMinorFaction;
                bool currentIsNoble = Clan.PlayerClan.IsNoble;
                int currentTier = Clan.PlayerClan.Tier;
                float currentRenown = Clan.PlayerClan.Renown;
                float currentInfluence = Clan.PlayerClan.Influence;

                // 计算 IsVassalLike（可验证的封臣判据）
                bool currentIsVassalLike = Clan.PlayerClan?.Kingdom != null
                    && !Clan.PlayerClan.IsUnderMercenaryService
                    && (Hero.MainHero?.Occupation == Occupation.Lord || Hero.MainHero?.IsLord == true);

                // 检测是否有任何状态变化
                bool hasChanged = 
                    currentIsLord != _lastIsLord ||
                    currentOccupation != _lastOccupation ||
                    currentKingdomId != _lastKingdomId ||
                    currentIsUnderMercenaryService != _lastIsUnderMercenaryService ||
                    currentIsClanTypeMercenary != _lastIsClanTypeMercenary ||
                    currentIsMinorFaction != _lastIsMinorFaction ||
                    currentIsNoble != _lastIsNoble ||
                    currentIsVassalLike != _lastIsVassalLike ||
                    currentTier != _lastTier ||
                    Math.Abs(currentRenown - _lastRenown) > 0.01f ||
                    Math.Abs(currentInfluence - _lastInfluence) > 0.01f;

                // 只在状态变化时打印快照（避免刷屏）
                if (hasChanged)
                {
                    OriginLog.Info("========== [WATCH] State snapshot (changed) ==========");
                    OriginLog.Info($"[WATCH] Hero.MainHero.IsLord = {currentIsLord}");
                    OriginLog.Info($"[WATCH] Hero.MainHero.Occupation = {currentOccupation?.ToString() ?? "null"}");
                    OriginLog.Info($"[WATCH] Clan.PlayerClan.Kingdom?.StringId = {currentKingdomId ?? "null"}");
                    OriginLog.Info($"[WATCH] Clan.PlayerClan.IsUnderMercenaryService = {currentIsUnderMercenaryService}");
                    OriginLog.Info($"[WATCH] Clan.PlayerClan.IsClanTypeMercenary = {currentIsClanTypeMercenary}");
                    OriginLog.Info($"[WATCH] Clan.PlayerClan.IsMinorFaction = {currentIsMinorFaction}");
                    OriginLog.Info($"[WATCH] Clan.PlayerClan.IsNoble = {currentIsNoble}");
                    OriginLog.Info($"[WATCH] IsVassalLike = {currentIsVassalLike} (Kingdom!=null && !Mercenary && Occupation==Lord)");
                    OriginLog.Info($"[WATCH] Clan.PlayerClan.Tier = {currentTier}");
                    OriginLog.Info($"[WATCH] Clan.PlayerClan.Renown = {currentRenown:F2}");
                    OriginLog.Info($"[WATCH] Clan.PlayerClan.Influence = {currentInfluence:F2}");
                    OriginLog.Info("========================================================");

                    // 更新上一次的状态
                    _lastIsLord = currentIsLord;
                    _lastOccupation = currentOccupation;
                    _lastKingdomId = currentKingdomId;
                    _lastIsUnderMercenaryService = currentIsUnderMercenaryService;
                    _lastIsClanTypeMercenary = currentIsClanTypeMercenary;
                    _lastIsMinorFaction = currentIsMinorFaction;
                    _lastIsNoble = currentIsNoble;
                    _lastIsVassalLike = currentIsVassalLike;
                    _lastTier = currentTier;
                    _lastRenown = currentRenown;
                    _lastInfluence = currentInfluence;
                }
            }
        }
    }
}
