using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}


using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}


using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}



using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}


using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}


using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}



using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}


using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}


using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}

using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// 巴丹尼亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多巴丹尼亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建巴丹尼亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为巴丹尼亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateBattaniaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "battania_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("巴丹尼亚非预设出身"),
                new TextObject("自由组装你的巴丹尼亚出身背景（高自由度）"),
                characters,
                GetBattaniaNonPresetCharacterArgs
            );

            // 这里可以添加巴丹尼亚特定的选项
            // 例如：巴丹尼亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBattaniaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 巴丹尼亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用巴丹尼亚特定的效果
        /// </summary>
        public static void ApplyBattaniaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Battania non-preset origin template");

                // 设置文化锚点为巴丹尼亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "battania";
                }

                // 巴丹尼亚特定的初始设置
                // 例如：初始位置在巴丹尼亚附近、初始装备倾向等

                OriginLog.Info("Battania non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyBattaniaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}







