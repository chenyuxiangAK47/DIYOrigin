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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
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
    /// 瓦兰迪亚非预设出身模板
    /// 这是一个基础模板，后续可以添加更多瓦兰迪亚特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建瓦兰迪亚非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为瓦兰迪亚特定的选项
        /// </summary>
        private static NarrativeMenu CreateVlandiaNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "vlandia_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("瓦兰迪亚非预设出身"),
                new TextObject("自由组装你的瓦兰迪亚出身背景（高自由度）"),
                characters,
                GetVlandiaNonPresetCharacterArgs
            );

            // 这里可以添加瓦兰迪亚特定的选项
            // 例如：瓦兰迪亚特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetVlandiaNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 瓦兰迪亚非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用瓦兰迪亚特定的效果
        /// </summary>
        public static void ApplyVlandiaNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Vlandia non-preset origin template");

                // 设置文化锚点为瓦兰迪亚
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "vlandia";
                }

                // 瓦兰迪亚特定的初始设置
                // 例如：初始位置在瓦兰迪亚附近、初始装备倾向等

                // 这里可以添加更多瓦兰迪亚特定的逻辑
                // 例如：
                // - 根据社会出身设置初始装备
                // - 根据技能背景设置初始技能
                // - 根据当前状态设置初始状态

                OriginLog.Info("Vlandia non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyVlandiaNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}







