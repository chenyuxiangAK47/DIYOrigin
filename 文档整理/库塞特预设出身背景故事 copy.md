```txt
============================================================
瓦兰迪亚（Vlandia）预设出身背景故事与开局效果 v1.0
用于：OriginSystemMod / CharacterCreation NarrativeMenus
作者：你（设计）+ ChatGPT（整理与补全）
============================================================

【总览】
瓦兰迪亚预设出身总数：10
每个出身结构：出身概述 + 4个节点（Node1~Node4）+ 每节点2~4个选项
每个选项包含：技能/关系/资源/部队/标记Tag/（可选）开局任务Quest

【命名规范建议】
- origin_id: vla_origin_xxx
- menu_id: vla_xxx_nodeN
- option_id: vla_xxx_nodeN_optY
- tag: vla_xxx_yyy
- quest_id: quest_vla_xxx_yyy

【资源/部队说明】
- “基础部队”是开局固定盘子；节点会在此基础上追加
- 食物/马匹/金币可按你实际平衡微调
- “关系”默认是对阵营关键人物/氏族的泛关系（不硬绑具体人名，兼容版本）

============================================================
1) 远征的骑士（没落家名的誓言远征）
============================================================
origin_id = vla_origin_oath_expedition_knight

【出身概述】
你出身瓦兰迪亚没落骑士家族。家名仍被承认，但封地与家兵已散。
家族只剩你与哥哥两人。你立誓远征：不完成誓言，不以家名回归。

【开局固定设定】
- Clan Tier: 3
- Renown: 30
- Gold: 2500
- Spawn: vlandia_border_march（瓦兰迪亚边境行省/海岸附近）
- Companion: 哥哥（唯一NPC，不写死）
  npc_id = vla_npc_brother_older
  relation_to_player = +30
  建议技能：Leadership+20, Tactics+15, Riding+20, Polearm+25
- 基础部队：
  Squire ×6
  Vlandian Levy Crossbowman ×4
  Food 15, Horses 2
- 玩家基础装备（底盘）：旧骑士轻中甲、盾、单手剑、骑乘马1

Node1（没落原因） menu_id=vla_oath_knight_node1_fall
- vla_oath_node1_erased_war（被抹掉的败仗）
  Tactics+30, Leadership+15
  关系：国王-10；受益氏族-20
  Gold+500
  Tag: oath_wound_erased
- vla_oath_node1_usurped_estate（封地被“合法”吞并）
  Charm+20, Steward+20
  关系：吞并氏族-30；城镇行会+5
  驮马+1
  Tag: oath_wound_usurped
- vla_oath_node1_debt（债务压垮纹章）
  Trade+15, Steward+30
  关系：商人/行会+10；贵族圈-5
  Gold+6000, Food+5
  Tag: oath_wound_debt
- vla_oath_node1_purge（政治清算）
  Roguery+15, Scouting+20
  关系：国王-20；反对派氏族+10
  Scout×2
  Tag: oath_wound_purge

Node2（立誓远征=开局任务） menu_id=vla_oath_knight_node2_oath
通用Tag：oathbound（誓约中）；完成后置：oathfulfilled；放弃：oathbreaker
- vla_oath_node2_searaiders_1000（杀1000海寇）
  quest_id=quest_vla_oath_searaiders_1000
  Crossbow+10, Leadership+20, Renown+1
  Crossbowman×4, Food+10
  Tag: oath_target_sea
- vla_oath_node2_take_quyaz（远征古亚兹，夺城立旗）
  quest_id=quest_vla_oath_take_quyaz
  Engineering+20, Tactics+20, Renown+2
  Gold+3000, Pack+1, Sergeant×2, Food+10
  Tag: oath_target_quyaz
- vla_oath_node2_battania_head（斩巴丹尼亚家族一人）
  quest_id=quest_vla_oath_battania_target
  OneHanded+15, Scouting+15
  关系：巴丹尼亚整体-20
  Squire×4, Horses+1
  Tag: oath_target_battania
- vla_oath_node2_reclaim_relic（寻回失旗与印玺）
  quest_id=quest_vla_oath_reclaim_relic
  Charm+20, Roguery+10, Renown+1
  Gold+1500, Scout×4, Food+5
  Tag: oath_target_relic

Node3（骑士道） menu_id=vla_oath_knight_node3_chivalry
- vla_oath_node3_mercy（仁慈）
  Charm+15；村民/市民关系+10（泛）
  Food+5
  Tag: chivalry_mercy
- vla_oath_node3_valor（勇武）
  Polearm+20, Riding+10
  给玩家骑枪/长枪
  Tag: chivalry_valor
- vla_oath_node3_prudence（谨慎）
  Tactics+15, Scouting+15
  Gold+1000, Food+3
  Tag: chivalry_prudence

Node4（兄弟规矩） menu_id=vla_oath_knight_node4_rules
- vla_oath_node4_brother_leads（哥哥掌军，你主外交）
  兄弟关系+10, Leadership+10
  Sergeant×2
  Tag: brother_role_commander
- vla_oath_node4_player_leads（你掌军，哥哥做见证人）
  兄弟关系+5, Renown+1
  Squire×2
  Tag: brother_role_witness
- vla_oath_node4_co_rule（共同决策）
  兄弟关系+15, Steward+10
  Food+8
  Tag: brother_role_co_rule


============================================================
2) 破产的旗主继承人（债券与纹章）
============================================================
origin_id = vla_origin_bankrupt_banneret

【出身概述】
你家曾是旗主（banneret），拥有附庸与税权，但一轮饥荒+战争赔款让家族破产。
你学会：骑士的荣耀要靠账本维持。

【开局固定设定】
- Clan Tier: 2
- Renown: 60
- Gold: 9000
- Spawn: vlandia_central_towns（瓦兰迪亚腹地城镇附近）
- Companion: 无（干净开局）
- 基础部队：Levy Crossbowman×6, Vlandian Recruit×8
- Food: 20, Horses: 1
- 玩家装备：中等护甲、弩/或无（可由Node决定）

Node1（破产原因） menu_id=vla_bankrupt_node1_cause
- vla_bankrupt_node1_ransom（为赎回亲族倾家荡产）
  Charm+20, Leadership+10
  Gold+2000（借来的），但DebtTag
  Tag: vla_debt_ransom
- vla_bankrupt_node1_bad_invest（押错商路/工坊亏空）
  Trade+30, Steward+15
  关系：商人+10；贵族-5
  Tag: vla_debt_invest
- vla_bankrupt_node1_war_fine（战败赔款/罚金）
  Tactics+15, OneHanded+10
  关系：国王-10
  Tag: vla_debt_war_fine

Node2（你怎么“守住体面”） menu_id=vla_bankrupt_node2_style
- vla_bankrupt_node2_sell_heirloom（卖祖传甲换现金）
  Gold+8000；给玩家更差护甲
  Tag: vla_style_cashfirst
- vla_bankrupt_node2_keep_armor（死保盔甲，钱紧）
  Gold+0；玩家装备更好；士气+2
  Tag: vla_style_honorfirst
- vla_bankrupt_node2_mortgage（抵押纹章与税权）
  Gold+5000；后续触发“赎回债券”小任务
  quest_id=quest_vla_redeem_bond
  Tag: vla_style_mortgage

Node3（你的家兵剩下什么） menu_id=vla_bankrupt_node3_retinue
- vla_bankrupt_node3_crossbow_core（弩手为主）
  Crossbow+20
  Crossbowman×6, Food+5
- vla_bankrupt_node3_foot_sergeants（步兵硬骨）
  Polearm+10, Athletics+10
  Voulgier×4, Sergeant×2
- vla_bankrupt_node3_mixed（混编保命）
  Tactics+10
  Recruit×6, Crossbowman×3, Footman×3

Node4（你要向谁借势） menu_id=vla_bankrupt_node4_patron
- vla_bankrupt_node4_king（向国王宣誓）
  关系：国王+10；精英氏族+5；但债主氏族-10
  Tag: vla_patron_king
- vla_bankrupt_node4_guild（投靠行会）
  关系：商人+20
  Trade+10
  Tag: vla_patron_guild
- vla_bankrupt_node4_neutral（不站队，先赚钱）
  Steward+10, Scouting+10
  Tag: vla_patron_neutral


============================================================
3) 城镇弩匠行会的执旗人（弩与契约）
============================================================
origin_id = vla_origin_crossbow_guild

【出身概述】
你不是大贵族，但你握着“弩与供给”的命脉。行会给你资金与人手，
条件是：你要为瓦兰迪亚的战争机器找利润。

【开局固定设定】
- Clan Tier: 1
- Renown: 35
- Gold: 12000
- Spawn: vlandia_city（瓦兰迪亚主城附近）
- 基础部队：Crossbowman×8, Militia/Recruit×6
- Food 15
- 玩家装备：弩、轻中甲、匕首/短剑（偏商战）

Node1（行会给你的身份） menu_id=vla_guild_node1_role
- vla_guild_node1_contractor（军需承包）
  Steward+20, Trade+20
  Tag: vla_guild_contractor
- vla_guild_node1_enforcer（行会执法）
  Roguery+20, OneHanded+10
  Tag: vla_guild_enforcer
- vla_guild_node1_inventor（弩匠改良）
  Engineering+20, Crossbow+15
  Tag: vla_guild_inventor

Node2（你更重视什么） menu_id=vla_guild_node2_value
- vla_guild_node2_profit（利润第一）
  Trade+20, Gold+3000
  Tag: vla_value_profit
- vla_guild_node2_reputation（名声第一）
  Charm+20, Renown+1
  Tag: vla_value_reputation
- vla_guild_node2_loyalty（忠诚供军）
  Leadership+15
  关系：国王+5
  Tag: vla_value_loyalty

Node3（开局资源包） menu_id=vla_guild_node3_package
- vla_guild_node3_ammo（弹药充足）
  Food+5；补给物资（箭矢/弩矢）+大量
  Crossbowman×4
- vla_guild_node3_cash（现金流）
  Gold+6000
- vla_guild_node3_contacts（人脉）
  关系：商人+20；触发“介绍信”小任务
  quest_id=quest_vla_guild_letter

Node4（你对骑士道的态度） menu_id=vla_guild_node4_chivalry
- vla_guild_node4_respect（尊重骑士道：不坑前线）
  Charm+10, Leadership+10
  Tag: vla_chivalry_respect
- vla_guild_node4_mock（嘲笑骑士道：合同就是法律）
  Trade+10, Roguery+10
  Tag: vla_chivalry_mock
- vla_guild_node4_balance（两头吃：能赚也能打）
  Tactics+10, Crossbow+10
  Tag: vla_chivalry_balance


============================================================
4) 边境行省的守境骑长（边境法与血仇）
============================================================
origin_id = vla_origin_march_warden

【出身概述】
你长在瓦兰迪亚-巴丹尼亚边境。你见过劫掠与复仇循环。
你学会：骑士的职责不是漂亮，而是“守住线”。

【开局固定设定】
- Clan Tier: 2
- Renown: 70
- Gold: 6000
- Spawn: vlandia_battania_border
- 基础部队：Vlandian Footman×8, Vlandian Crossbowman×6, Scout×2
- Food 20
- 玩家装备：盾+单手剑+中甲（偏守备）

Node1（你守的是什么线） menu_id=vla_march_node1_line
- vla_march_node1_forest_edge（林线：防巴丹尼亚伏击）
  Scouting+25, Tactics+10
  Tag: vla_line_forest
- vla_march_node1_river_ford（河口：守渡口税）
  Steward+20, Trade+10
  Gold+1500
  Tag: vla_line_river
- vla_march_node1_castle_road（城堡大道：护补给）
  Leadership+20, Athletics+10
  Food+10
  Tag: vla_line_road

Node2（你如何执法） menu_id=vla_march_node2_law
- vla_march_node2_strict（铁血军法）
  Leadership+15；士气+3；村民关系-5
  Tag: vla_law_strict
- vla_march_node2_fair（公平裁断）
  Charm+15；村民关系+10
  Tag: vla_law_fair
- vla_march_node2_pragmatic（实用主义：能用钱解决就别流血）
  Trade+10；Roguery+10；Gold+1000
  Tag: vla_law_pragmatic

Node3（你的边境部队） menu_id=vla_march_node3_force
- vla_march_node3_pikes（长枪/反骑）
  Polearm+20
  Voulgier×6
- vla_march_node3_shields（盾墙步兵）
  OneHanded+10, Athletics+10
  Footman×6, Sergeant×2
- vla_march_node3_scouts（侦骑）
  Riding+10, Scouting+15
  Scout×6, Horses+2

Node4（你背负的仇/债） menu_id=vla_march_node4_burden
- vla_march_node4_bloodfeud（血仇氏族）
  关系：巴丹尼亚某氏族-40
  Renown+1
  Tag: vla_burden_bloodfeud
- vla_march_node4_royal_order（王命清单）
  quest_id=quest_vla_march_order（清剿边境劫掠者）
  关系：国王+10
  Tag: vla_burden_order
- vla_march_node4_refugees（护送难民）
  Charm+10, Steward+10
  Food+10
  Tag: vla_burden_refugees


============================================================
5) 比武场的落魄冠军（荣耀与赌债）
============================================================
origin_id = vla_origin_tourney_champion

【出身概述】
你曾是比武场明星，靠枪术与观众欢呼活着。后来一次“被安排的败局”
让你背上赌债、被贵族圈踢出局。你决定：用真正的战场赢回名声。

【开局固定设定】
- Clan Tier: 1
- Renown: 80
- Gold: 1500（很穷）
- Spawn: vlandia_tournament_cities
- 基础部队：Squire×4, Recruit×10
- Food 12
- 玩家装备：长枪/骑枪、旧披甲（好武器但破甲）

Node1（你怎么跌下神坛） menu_id=vla_tourney_node1_fall
- vla_tourney_node1_fixed_match（假赛背锅）
  Roguery+15, Charm+10
  关系：某贵族-20
  Tag: vla_fall_fixed
- vla_tourney_node1_injury（重伤复出失败）
  Medicine+10, Athletics+10
  Tag: vla_fall_injury
- vla_tourney_node1_insult（当众羞辱权贵）
  Charm+15, OneHanded+10
  关系：国王-10；精英氏族-15
  Tag: vla_fall_insult

Node2（你靠什么翻身） menu_id=vla_tourney_node2_comeback
- vla_tourney_node2_pure_chivalry（纯骑士道：拒绝黑幕）
  Charm+20, Renown+1
  Tag: vla_comeback_chivalry
- vla_tourney_node2_dirty_win（不择手段赢回来）
  Roguery+20
  Gold+2000
  Tag: vla_comeback_dirty
- vla_tourney_node2_train_army（把冠军经验变成军队纪律）
  Leadership+20, Tactics+10
  Tag: vla_comeback_discipline

Node3（开局随队风格） menu_id=vla_tourney_node3_team
- vla_tourney_node3_lancers（枪骑小队）
  Squire×6, Horses+2
- vla_tourney_node3_showmen（表演型随从）
  Charm+10
  Recruit×8, Food+5
- vla_tourney_node3_hardmen（硬汉步兵）
  Footman×6

Node4（你欠谁的债） menu_id=vla_tourney_node4_debt
- vla_tourney_node4_gambler（赌徒债主）
  quest_id=quest_vla_pay_debt（分期还债）
  Tag: vla_debt_gambler
- vla_tourney_node4_noble（贵族债主）
  关系：某氏族-20；完成任务可修复
  quest_id=quest_vla_noble_favor
  Tag: vla_debt_noble
- vla_tourney_node4_none（你不欠债，你欠的是“名声”）
  Renown+1
  Tag: vla_debt_pride


============================================================
6) 海岸私掠者（王旗之下的黑船）
============================================================
origin_id = vla_origin_coastal_privateer

【出身概述】
瓦兰迪亚的海岸线不只属于渔民，也属于私掠者。
你拿到一纸“模糊的王权授权”，可以抢敌国，也可以顺便发家。

【开局固定设定】
- Clan Tier: 1
- Renown: 30
- Gold: 7000
- Spawn: vlandia_west_coast
- 基础部队：Sea-raider-hunters（本地水手/轻步）×10, Crossbowman×6
- Food 15
- 玩家装备：弩+短剑+轻甲（便于跑图）

Node1（你是合法还是灰色） menu_id=vla_privateer_node1_status
- vla_privateer_node1_letter（有授权信）
  关系：国王+5
  Charm+10
  Tag: vla_privateer_legal
- vla_privateer_node1_forged（伪造授权）
  Roguery+20
  Tag: vla_privateer_forged
- vla_privateer_node1_none（纯海盗出身）
  Roguery+15, Scouting+10
  关系：商人-10
  Tag: vla_privateer_pirate

Node2（你抢什么） menu_id=vla_privateer_node2_target
- vla_privateer_node2_supply（抢补给船）
  Food+15, Trade+10
- vla_privateer_node2_ransom（抓人勒索）
  Gold+4000, Roguery+10
- vla_privateer_node2_map（抢海图与航线）
  Scouting+20
  quest_id=quest_vla_sea_route_map

Node3（你的船员风格） menu_id=vla_privateer_node3_crew
- vla_privateer_node3_crossbow_deck（甲板弩手）
  Crossbow+15
  Crossbowman×6
- vla_privateer_node3_boarders（跳帮手）
  OneHanded+15
  Footman×6
- vla_privateer_node3_fast_sailors（快船水手）
  Scouting+10, Riding+10
  Horses+1

Node4（你与骑士道的关系） menu_id=vla_privateer_node4_chivalry
- vla_privateer_node4_under_flag（王旗下：不杀俘虏）
  Charm+10；士气+2
  Tag: vla_privateer_honor
- vla_privateer_node4_profit_only（钱比旗重要）
  Trade+10；Roguery+10
  Tag: vla_privateer_profit
- vla_privateer_node4_redemption（想洗白回贵族圈）
  quest_id=quest_vla_privateer_pardon
  Tag: vla_privateer_redemption


============================================================
7) 侍奉骑士团的扈从（誓言与戒律）
============================================================
origin_id = vla_origin_order_squire

【出身概述】
你在骑士团长大，学过戒律、礼仪与枪术。你可以成为真正的骑士，
也可能被教条压得喘不过气。你出走，是为了证明“誓言不是枷锁”。

【开局固定设定】
- Clan Tier: 1
- Renown: 25
- Gold: 3500
- Spawn: vlandia_castle_region
- 基础部队：Squire×8, Footman×6
- Food 15
- 玩家装备：骑枪+盾+中甲（非常瓦兰迪亚）

Node1（你为何离开骑士团） menu_id=vla_order_node1_leave
- vla_order_node1_test（接受试炼：外出立功）
  quest_id=quest_vla_order_trial
  关系：骑士团+20（泛）
  Tag: vla_order_trial
- vla_order_node1_conflict（与团内权贵冲突）
  Charm+10, Roguery+10
  Tag: vla_order_conflict
- vla_order_node1_doubt（信念动摇，想看真实世界）
  Scouting+10, Medicine+10
  Tag: vla_order_doubt

Node2（你的戒律是哪条） menu_id=vla_order_node2_rule
- vla_order_node2_mercy（仁慈）
  Charm+15；村民关系+10
- vla_order_node2_honor（荣誉：不抢平民）
  Leadership+15；士气+2
- vla_order_node2_purity（清修：拒绝走灰色）
  Trade-（不减技能，改为：Roguery-倾向）
  Tag: vla_order_purity

Node3（骑士团给你的资源） menu_id=vla_order_node3_supply
- vla_order_node3_armor（给你一套更好的甲）
  玩家装备升级；Gold+0
- vla_order_node3_troops（给你更多扈从）
  Squire×6, Food+5
- vla_order_node3_money（给你旅费）
  Gold+5000

Node4（你要证明什么） menu_id=vla_order_node4_prove
- vla_order_node4_defend_peasants（守护村庄免遭劫掠）
  quest_id=quest_vla_order_defend
- vla_order_node4_win_battle（赢下一场正规战）
  quest_id=quest_vla_order_victory
- vla_order_node4_duel（击败一名成名骑士）
  quest_id=quest_vla_order_duel


============================================================
8) 雇佣军连队副官（合同写在血上）
============================================================
origin_id = vla_origin_sellsword_lieutenant

【出身概述】
你不是贵族，但你懂战争怎么赚钱。你当过副官，知道怎么管人、怎么活命。
你可以成为“可雇之刃”，也可以攒成自己的旗。

【开局固定设定】
- Clan Tier: 1
- Renown: 50
- Gold: 8000
- Spawn: near_war_front（靠近交战边境）
- 基础部队：Footman×8, Crossbowman×6, Scout×2
- Food 18
- 玩家装备：步战更强（斧/剑+盾+中甲）

Node1（你在哪场战争学会这些） menu_id=vla_sellsword_node1_war
- vla_sellsword_node1_battania（对巴丹尼亚）
  Scouting+15, Tactics+10
- vla_sellsword_node1_empire（对帝国）
  Engineering+10, Leadership+10
- vla_sellsword_node1_sturgia（对斯特吉亚）
  Athletics+10, OneHanded+10

Node2（你的副官风格） menu_id=vla_sellsword_node2_style
- vla_sellsword_node2_discipline（纪律）
  Leadership+20；士气+3
- vla_sellsword_node2_cunning（狡猾）
  Roguery+20；Gold+2000
- vla_sellsword_node2_care（照顾士兵）
  Medicine+15；Food+10

Node3（开局合同） menu_id=vla_sellsword_node3_contract
- vla_sellsword_node3_short（短约：先拿预支）
  Gold+4000
  Tag: vla_contract_short
- vla_sellsword_node3_long（长约：拿装备）
  玩家装备提升；Sergeant×2
  Tag: vla_contract_long
- vla_sellsword_node3_broken（合同被坑：你出来单干）
  关系：某雇主-20；Renown+1（你活下来了）
  Tag: vla_contract_broken

Node4（你最终想成为什么） menu_id=vla_sellsword_node4_goal
- vla_sellsword_node4_own_banner（自立旗帜）
  Renown+1；Leadership+10
- vla_sellsword_node4_noble_service（投贵族）
  关系：精英氏族+10
- vla_sellsword_node4_wealth（赚到退役）
  Trade+10；Gold+3000


============================================================
9) 王室税吏的护卫（法律的刀）
============================================================
origin_id = vla_origin_tax_bailiff_enforcer

【出身概述】
你从小跟着税吏走村镇。你知道谁富、谁穷、谁藏粮。
当“法律”需要刀时，你就是那把刀。问题是：你究竟为谁的法律效忠？

【开局固定设定】
- Clan Tier: 2
- Renown: 20
- Gold: 6500
- Spawn: vlandia_market_towns
- 基础部队：Footman×6, Recruit×8, Crossbowman×4
- Food 16
- 玩家装备：短剑/钉头锤+盾+轻中甲

Node1（你如何收税） menu_id=vla_tax_node1_method
- vla_tax_node1_strict（严苛：一分不少）
  Steward+15；村民关系-10；Gold+2000
  Tag: vla_tax_strict
- vla_tax_node1_negotiation（协商：用话术拿到更多）
  Charm+20；Gold+1000；村民关系+5
  Tag: vla_tax_negotiate
- vla_tax_node1_corrupt（腐败：你截留了一部分）
  Roguery+20；Gold+4000；被追查风险Tag
  Tag: vla_tax_corrupt

Node2（你见过最黑的一次） menu_id=vla_tax_node2_dark
- vla_tax_node2_starvation（饥荒）
  Medicine+10；Food+15
  Tag: vla_dark_famine
- vla_tax_node2_revolt（暴动）
  Leadership+10；Tactics+10
  Tag: vla_dark_revolt
- vla_tax_node2_noble_cover（权贵掩盖罪行）
  Charm+10；Roguery+10；某氏族-15
  Tag: vla_dark_coverup

Node3（你的护卫队） menu_id=vla_tax_node3_guard
- vla_tax_node3_shields（盾卫）
  Footman×6；士气+2
- vla_tax_node3_crossbows（弩卫）
  Crossbowman×6
- vla_tax_node3_mounted（骑巡）
  Scout×4；Horses+2

Node4（你要不要洗白） menu_id=vla_tax_node4_redemption
- vla_tax_node4_serve_king（向国王递交账册）
  quest_id=quest_vla_tax_report
  关系：国王+10
- vla_tax_node4_hide（把账册埋掉）
  Roguery+10；Gold+2000
- vla_tax_node4_help_people（暗中救济）
  Charm+10；村民关系+10；Gold-1000


============================================================
10) 自由民团的推旗人（乡土与长矛）
============================================================
origin_id = vla_origin_free_militia_leader

【出身概述】
你不是骑士，你是被战争逼出来的“推旗人”。
当领主不在、城防空虚时，是你把村镇男人组织起来。
你相信：骑士道不该只属于贵族。

【开局固定设定】
- Clan Tier: 1
- Renown: 10
- Gold: 2000
- Spawn: vlandia_villages
- 基础部队：Militia/Recruit×16, Levy Crossbowman×4
- Food 20
- 玩家装备：长矛/叉枪+小盾+轻甲（朴素）

Node1（你为何举旗） menu_id=vla_militia_node1_reason
- vla_militia_node1_raiders（海寇/强盗逼村）
  Scouting+15；村民关系+10
  Tag: vla_reason_raiders
- vla_militia_node1_noble_absent（领主失职）
  Charm+10；与某领主-15
  Tag: vla_reason_absent
- vla_militia_node1_hunger（饥荒）
  Steward+15；Food+10
  Tag: vla_reason_hunger

Node2（你的“乡土骑士道”） menu_id=vla_militia_node2_code
- vla_militia_node2_protect（先护弱）
  Charm+10；村民关系+10
- vla_militia_node2_punish（先惩恶）
  OneHanded+10；士气+2
- vla_militia_node2_survive（先活命）
  Scouting+10；Roguery+5；Gold+1000

Node3（民团武装） menu_id=vla_militia_node3_arms
- vla_militia_node3_pikes（长矛阵）
  Polearm+15；Recruit×6
- vla_militia_node3_crossbows（家用弩）
  Crossbow+10；Levy Crossbowman×6
- vla_militia_node3_axes（斧与木盾）
  Athletics+10；Footman×4

Node4（你要不要成为真正的骑士） menu_id=vla_militia_node4_future
- vla_militia_node4_seek_knighthood（求封骑）
  quest_id=quest_vla_militia_knighthood（立功换封号）
  Tag: vla_future_knight
- vla_militia_node4_stay_free（保持自由民）
  Trade+10；村民关系+10
  Tag: vla_future_free
- vla_militia_node4_join_faction（投奔某大氏族）
  关系：精英氏族+10
  Tag: vla_future_patron


============================================================
【总结】
============================================================
瓦兰迪亚预设出身 10 个：
1 远征骑士（带哥哥NPC，Tier3/Renown30）
2 破产旗主继承人（债券与纹章）
3 城镇弩匠行会执旗人
4 边境守境骑长
5 落魄比武冠军
6 海岸私掠者
7 骑士团扈从
8 雇佣军副官
9 税吏护卫（法律之刀）
10 自由民团推旗人

你可以直接把本文件拆成各 origin 的 NarrativeMenu 链：
- 每个Node对应一个menu_id
- 每个选项对应一个option_id + effect bundle + tag/quest
============================================================
```

如果你下一步要“直接落地到代码”，我也可以给你一个**统一的数据结构模板**（JSON/Scriptable/或C#类），把每个 `origin_id -> nodes -> options -> effects` 都变成可读取的配置，这样你改数值不再改代码。
