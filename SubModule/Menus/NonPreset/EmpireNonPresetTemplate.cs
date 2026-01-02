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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
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
    /// 帝国非预设出身模板
    /// 这是一个基础模板，后续可以添加更多帝国特定的选项和效果
    /// </summary>
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 创建帝国非预设出身菜单（模板）
        /// 注意：这个菜单目前使用通用的nonpreset流程，但可以扩展为帝国特定的选项
        /// </summary>
        private static NarrativeMenu CreateEmpireNonPresetTemplateMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "empire_non_preset_template",
                "origin_type_selection",
                "non_preset_culture_anchor",
                new TextObject("帝国非预设出身"),
                new TextObject("自由组装你的帝国出身背景（高自由度）"),
                characters,
                GetEmpireNonPresetCharacterArgs
            );

            // 这里可以添加帝国特定的选项
            // 例如：帝国特有的社会阶层、技能背景等
            // 目前作为模板，使用通用的nonpreset流程

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetEmpireNonPresetCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        /// <summary>
        /// 帝国非预设出身应用逻辑（模板）
        /// 在PresetOriginSystem.cs中调用，用于应用帝国特定的效果
        /// </summary>
        public static void ApplyEmpireNonPresetOrigin(Hero hero, NonPresetOriginData data)
        {
            try
            {
                OriginLog.Info("Applying Empire non-preset origin template");

                // 设置文化锚点为帝国
                if (string.IsNullOrEmpty(data.CultureAnchor))
                {
                    data.CultureAnchor = "empire";
                }

                // 帝国特定的初始设置
                // 例如：初始位置在帝国附近、初始装备倾向等

                OriginLog.Info("Empire non-preset origin template applied");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ApplyEmpireNonPresetOrigin failed: {ex.Message}");
            }
        }
    }
}







