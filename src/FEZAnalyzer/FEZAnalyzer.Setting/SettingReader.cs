using FEZAnalyzer.Entity;
using IniParser;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FEZAnalyzer.Setting
{
    public static class SettingReader
    {
        public static async Task<FezGlobalSetting> ReadGlobalSettingAsync()
        {
            return await Task.Run(() =>
            {
                var globalIniFile = GetGlobalIniFile();
                var globalSetting = ReadGlobalSetting(globalIniFile.FullName);

                return globalSetting;
            });
        }

        private static FezGlobalSetting ReadGlobalSetting(string path)
        {
            try
            {
                var parser = new FileIniDataParser();
                var data   = parser.ReadFile(path);

                int? readInt(string key)
                {
                    string str = null;
                    try
                    {
                        str = data["GLOBAL"][key];
                    }
                    catch
                    {
                        return null;
                    }

                    if (int.TryParse(str, out int val))
                    {
                        return val;
                    }
                    else
                    {
                        return null;
                    }
                }
                float? readFloat(string key)
                {
                    string str = null;
                    try
                    {
                        str = data["GLOBAL"][key];
                    }
                    catch
                    {
                        return null;
                    }

                    if (float.TryParse(str, out float val))
                    {
                        return val;
                    }
                    else
                    {
                        return null;
                    }
                }

                return new FezGlobalSetting()
                {
                    ScreenWidth            = readInt("SCREEN_WIDTH"),
                    ScreenHeight           = readInt("SCREEN_HEIGHT"),
                    Fullscreen             = readInt("FULLSCREEN"),
                    SoundEnable            = readInt("SOUND_ENABLE"),
                    ReflectEnable          = readInt("REFLECT_ENABLE"),
                    ReflectLevel           = readInt("REFLECT_LEVEL"),
                    ShadowLevel            = readInt("SHADOW_LEVEL"),
                    BgmVolume              = readInt("BGM_VOLUME"),
                    BgmAlbum               = readInt("BGM_ALBUM"),
                    BgmVersion             = readInt("BGM_VERSION"),
                    SeVolume               = readInt("SE_VOLUME"),
                    Gamma                  = readInt("GAMMA"),
                    FarClip                = readInt("FAR_CLIP"),
                    InputMode              = readInt("INPUT_MODE"),
                    MouseSensitivity       = readInt("MOUSE_SENSITIVITY"),
                    CameraReverseVertical  = readInt("CAMERA_REVERSE_VERTICAL"),
                    CameraReverseHorizon   = readInt("CAMERA_REVERSE_HORIZON"),
                    OpeningMovie           = readInt("OPENING_MOVIE"),
                    TextureResolution      = readInt("TEXTURE_RESOLUTION"),
                    CharaInfo              = readInt("CHARA_INFO"),
                    HpBar                  = readInt("HP_BAR"),
                    CharaName              = readInt("CHARA_NAME"),
                    ForceName              = readInt("FORCE_NAME"),
                    Icons                  = readInt("ICONS"),
                    SkillInfo              = readInt("SKILL_INFO"),
                    DoubleKeyStep          = readInt("DOUBLE_KEY_STEP"),
                    DispCharacterNum       = readInt("DISP_CHARACTER_NUM"),
                    DispStageEffect        = readInt("DISP_STAGE_EFFECT"),
                    DispItemEffect         = readInt("DISP_ITEM_EFFECT"),
                    ActiveLog              = readInt("ACTIVE_LOG"),
                    DispChatSay            = readInt("DISP_CHAT_SAY"),
                    DispChatAll            = readInt("DISP_CHAT_ALL"),
                    DispChatArmy           = readInt("DISP_CHAT_ARMY"),
                    DispChatArmySay        = readInt("DISP_CHAT_ARMY_SAY"),
                    DispChatWhisper        = readInt("DISP_CHAT_WHISPER"),
                    DispChatParty          = readInt("DISP_CHAT_PARTY"),
                    DispChatLeader         = readInt("DISP_CHAT_LEADER"),
                    DispChatEmotion        = readInt("DISP_CHAT_EMOTION"),
                    DispChatBalloon        = readInt("DISP_CHAT_BALLOON"),
                    DispChatSystem         = readInt("DISP_CHAT_SYSTEM"),
                    DispChatBattleFriend   = readInt("DISP_CHAT_BATTLE_FRIEND"),
                    DispChatBattleEnemy    = readInt("DISP_CHAT_BATTLE_ENEMY"),
                    DispChatBattleEtc      = readInt("DISP_CHAT_BATTLE_ETC"),
                    DispWarSituationInfo   = readInt("DISP_WAR_SITUATION_INFO"),
                    PartyInvitePermit      = readInt("PARTY_INVITE_PERMIT"),
                    GuildInvitePermit      = readInt("GUILD_INVITE_PERMIT"),
                    TradeInvitePermit      = readInt("TRADE_INVITE_PERMIT"),
                    DispLogLines           = readInt("DISP_LOG_LINES"),
                    WorldDispLogLines      = readInt("WORLD_DISP_LOG_LINES"),
                    WindowColor            = readInt("WINDOW_COLOR"),
                    CameraRadiusRate       = readFloat("CAMERA_RADIUS_RATE"),
                    DispChatLog            = readInt("DISP_CHAT_LOG"),
                    WorldDispChatLog       = readInt("WORLD_DISP_CHAT_LOG"),
                    DispUpBattleLog        = readInt("DISP_UP_BATTLE_LOG"),
                    AutoLogEnable          = readInt("AUTO_LOG_ENABLE"),
                    ColorChatSay           = readInt("COLOR_CHAT_SAY"),
                    ColorChatAll           = readInt("COLOR_CHAT_ALL"),
                    ColorChatArmy          = readInt("COLOR_CHAT_ARMY"),
                    ColorChatArmySay       = readInt("COLOR_CHAT_ARMY_SAY"),
                    ColorChatWhisper       = readInt("COLOR_CHAT_WHISPER"),
                    ColorChatParty         = readInt("COLOR_CHAT_PARTY"),
                    ColorChatLeader        = readInt("COLOR_CHAT_LEADER"),
                    ColorChatEmotion       = readInt("COLOR_CHAT_EMOTION"),
                    RadarMapEdgeEnable     = readInt("RADAR_MAP_EDGE_ENABLE"),
                    BuildingAlertDisp      = readInt("BUILDING_ALERT_DISP"),
                    BuildingAlertColorDisp = readInt("BUILDING_ALERT_COLOR_DISP"),
                    BallistaEdgeEnable     = readInt("BALLISTA_EDGE_ENABLE"),
                    PlayVoiceType          = readInt("PLAY_VOICE_TYPE"),
                    PlayKingVoiceEnable    = readInt("PLAY_KING_VOICE_ENABLE"),
                    VoiceVolume            = readInt("VOICE_VOLUME"),
                    CameraInterfaceMode    = readInt("CAMERA_INTERFACE_MODE"),
                    FpsLimit               = readInt("FPS_LIMIT"),
                    NpcName                = readInt("NPC_NAME"),
                    PartyDirection         = readInt("PARTY_DIRECTION"),
                    AutochangeChattype     = readInt("AUTOCHANGE_CHATTYPE"),
                    MapRotateMode          = readInt("MAP_ROTATE_MODE"),
                    MapSmoothMode          = readInt("MAP_SMOOTH_MODE"),
                    ReplayAutoSave         = readInt("REPLAY_AUTO_SAVE"),
                    BestRankCategory       = readInt("BEST_RANK_CATEGORY"),
                    WindowEditMode         = readInt("WINDOW_EDIT_MODE"),
                    SkillTreeRow           = readInt("SKILL_TREE_ROW"),
                    ItemSortOrder          = readInt("ITEM_SORT_ORDER"),
                    InventorySortOrder     = readInt("INVENTORY_SORT_ORDER"),
                    FootprintType          = readInt("FOOTPRINT_TYPE"),
                    MiniMapMode            = readInt("MINI_MAP_MODE"),
                    MiniMapPosX            = readInt("MINI_MAP_POS_X"),
                    MiniMapPosY            = readInt("MINI_MAP_POS_Y"),
                    MiniMapZoom            = readInt("MINI_MAP_ZOOM"),
                    MiniMapSize            = readInt("MINI_MAP_SIZE"),
                    MiniMapAlpha           = readInt("MINI_MAP_ALPHA"),
                    SummonCountOpen        = readInt("SUMMON_COUNT_OPEN"),
                    SummonCountPosX        = readInt("SUMMON_COUNT_POS_X"),
                    SummonCountPosY        = readInt("SUMMON_COUNT_POS_Y"),
                    SummonCountDispType    = readInt("SUMMON_COUNT_DISP_TYPE"),
                    SummonCountGripType    = readInt("SUMMON_COUNT_GRIP_TYPE"),
                    SummonCountGripWindow  = readInt("SUMMON_COUNT_GRIP_WINDOW"),
                    SummonCountGripOffsetX = readInt("SUMMON_COUNT_GRIP_OFFSET_X"),
                    SummonCountGripOffsetY = readInt("SUMMON_COUNT_GRIP_OFFSET_Y"),
                    DispDraweffectitem     = readInt("DISP_DRAWEFFECTITEM"),
                    DispScoreDetailInfo    = readInt("DISP_SCORE_DETAIL_INFO"),
                    DispHighModel          = readInt("DISP_HIGH_MODEL"),
                    DispPingWarningAlert   = readInt("DISP_PING_WARNING_ALERT"),
                };
            }
            catch
            {
                return null;
            }
        }

        private static FileInfo GetGlobalIniFile()
        {
            // インストール先フォルダ取得
            var rootDir = GetInstallRootDirectory();
            if (rootDir == null)
            {
                throw new DirectoryNotFoundException();
            }

            // Settingsフォルダ取得
            var settingsDir = rootDir.GetDirectories("Settings").FirstOrDefault();
            if (settingsDir == null || !settingsDir.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            // GLOBAL.INI ファイル取得
            var globalIniFile = settingsDir.GetFiles("GLOBAL.INI").FirstOrDefault();
            if (globalIniFile == null || !globalIniFile.Exists)
            {
                throw new FileNotFoundException();
            }

            return globalIniFile;
        }

        private static DirectoryInfo GetInstallRootDirectory()
        {
            var subkey = Environment.Is64BitOperatingSystem ?
                @"SOFTWARE\WOW6432Node\SquareEnix\FantasyEarthZero" :
                @"SOFTWARE\SquareEnix\FantasyEarthZero";

            using (var key = Registry.LocalMachine.OpenSubKey(subkey))
            {
                var value = key.GetValue("Install_Dir");
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }

                //  [レジストリに入っている値]
                //   "C:\\Program Files (x86)\\SquareEnix\\FantasyEarth\\"
                //
                //   実際には "FantasyEarthZero" というフォルダが作成されるが、
                //   なぜか上記のようにキーとして出来るのは "FantasyEarth" として保存される。
                //   そのため、値を置き換えて DirectoryInfo を作成する。
                //
                var replecedValue = Regex.Replace(value.ToString(), "FantasyEarth\\\\$", "FantasyEarthZero\\");
                var directoryInfo = new DirectoryInfo(replecedValue);
                if (directoryInfo.Exists)
                {
                    return directoryInfo;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
