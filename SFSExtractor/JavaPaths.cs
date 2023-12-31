namespace Editor.JavaClasses
{
    using System;
    using System.IO;

    public class JavaPaths
    {
        public const string _3DO_Primitive_Marker = "3do/primitive/Marker/mono.sim";
        public const string AI_AGGRO = "data/Ai/aggro.ini";
        public const string AI_AmmoRating = "Data/Ai/AmmoRating.ini";
        public const string AI_AmmoTypeProp = "Data/Ai/AmmoTypeProp.ini";
        public const string AI_DANGER = "data/Ai/DangerLevel.ini";
        public const string AI_DETOUR = "data/Ai/Detour.ini";
        public const string AI_HUMANBATTLEMODEL = "data/Ai/BattleModel.ini";
        public const string AI_Morale = "data/Ai/MoraleSystem.ini";
        public const string AI_Rating = "data/Ai/UnitRating.ini";
        public const string AI_RatingTypes = "data/Ai/RatingTypes.ini";
        public const string AI_RETREAT = "data/Ai/RetreatSystem.ini";
        public const string AI_RootScripts = "data/Ai/scripts/";
        public const string AI_Types = "data/Ai/AITypes.ini";
        public const string AI_Visibility = "data/Ai/visibility.ini";
        public const string AI_Weather = "data/Ai/weather.ini";
        public const string ANIM_ANIMATOR_RC = "Ai/Animator/Animator.rc";
        public const string ANIM_ARTILLERY_RC = "Ai/Animator/artillery.rc";
        public const string ANIM_HMESH_ANIMATIONS = "Ai/Animator/HMesh/Animations.ini";
        public const string ANIM_HMESH_ANIMPATHS = "3do/Animations/";
        public const string ANIM_HMESH_SWITCHTABLES = "Ai/Animator/HMesh/SwitchTables.ini";
        public const string ANIM_HUMAN_RC = "Ai/Animator/human.rc";
        public const string ANIM_MODEL_GREENDUDE = "models/Human/sk_0/GreenDude/";
        public const string ANIM_MODEL_SKEL0 = "models/Human/sk_0/";
        public const string ANIM_MODEL_SKEL1 = "models/Human/sk_1/";
        public const string ANIM_MODEL_WOMAN = "models/Human/sk_1/woman1/";
        public const string ANIM_SMESH_ANIMATIONS = "Ai/Animator/SMesh/Animations.ini";
        public const string ANIM_SMESH_MARKERS = "Ai/Animator/SMesh/Markers.ini";
        public const string ANIM_SMESH_SWITCHTABLES = "Ai/Animator/SMesh/SwitchTables.ini";
        public const string CAMERA_PROPERTIES_FILE_NAME = "Data/Settings/camera.properties";
        public const string CAMERA_SHOOTCAMERA = @"data\Settings\debug\ShootCamera.ini";
        public const string CAMERA_USER_PROPERTIES_FILE_NAME = "camera.properties";
        public const string CAMPAIGN_FILE = "campaign.xml";
        public const string CAMPAIGN_PATH = "missions/";
        public const string CAMPAIGN_PATH_WO_DELIMITER = "missions";
        public const string CAMPAIGNS_XML = "campaigns.xml";
        public const string CREDITS_AUDIO = "avi/title.wav";
        public const string CREDITS_VIDEO = "title.avi";
        public const string CURSOR_Attack = "DATA/GUI/Cursors/Attack.mat";
        public const string CURSOR_AttackAir = "DATA/GUI/Cursors/attack_air.mat";
        public const string CURSOR_AttackArt = "DATA/GUI/Cursors/attack_artillery.mat";
        public const string CURSOR_AttackGround = "DATA/GUI/Cursors/AttackGround.mat";
        public const string CURSOR_AttackStorm = "DATA/GUI/Cursors/AttackStorm.mat";
        public const string CURSOR_Defend = "DATA/GUI/Cursors/Guard.mat";
        public const string CURSOR_DownCamera = "DATA/GUI/Cursors/CameraDown.mat";
        public const string CURSOR_DownLeft = "DATA/GUI/Cursors/CameraLeftDown.mat";
        public const string CURSOR_DownRight = "DATA/GUI/Cursors/CameraRightDown.mat";
        public const string CURSOR_Hook = "DATA/GUI/Cursors/attach_vehicle.mat";
        public const string CURSOR_Impossible = "DATA/GUI/Cursors/Impossible.mat";
        public const string CURSOR_LeftCamera = "DATA/GUI/Cursors/CameraLeft.mat";
        public const string CURSOR_MoveToTrench = "DATA/GUI/Cursors/MoveToTrench.mat";
        public const string CURSOR_Normal = "DATA/GUI/Cursors/Normal.mat";
        public const string CURSOR_PlaceCrew = "DATA/GUI/Cursors/InUnit.mat";
        public const string CURSOR_ReconAir = "DATA/GUI/Cursors/reconair.mat";
        public const string CURSOR_Retreat = "DATA/GUI/Cursors/Retreat.mat";
        public const string CURSOR_RightCamera = "DATA/GUI/Cursors/CameraRight.mat";
        public const string CURSOR_Rotate = "DATA/GUI/Cursors/Rotate.mat";
        public const string CURSOR_RotateCamera = "DATA/GUI/Cursors/RotateCamera.mat";
        public const string CURSOR_Select = "DATA/GUI/Cursors/Normal.mat";
        public const string CURSOR_UpCamera = "DATA/GUI/Cursors/CameraUp.mat";
        public const string CURSOR_UpLeftCamera = "DATA/GUI/Cursors/CameraLeftUp.mat";
        public const string CURSOR_UpRightCamera = "DATA/GUI/Cursors/CameraRightUp.mat";
        public const string DAMAGE_INI = "data/Settings/damage.ini";
        public const string DECAL_CAMERA = "data/Settings/Debug/decal_camera.ini";
        public const string DECAL_DIR = "3dobj/decals/";
        public const string DECAL_PRESET_INI = "data/Settings/DecalPreset.ini";
        public const string DYNAMICS_Transmission = "data/settings/transmission.ini";
        public const string EFFECTS_DAMAGE = "data/Settings/DamageEffects.ini";
        public const string EFFECTS_DAMAGESOUNDS = "data/Settings/DamageSounds.ini";
        public const string EFFECTS_DiePresetsFileName = "data/Settings/DieEffects.ini";
        public const string EFFECTS_DieselPresetsFileName = "data/Settings/DieselEffects.ini";
        public const string EFFECTS_DustPresetsFileName = "data/Settings/DustEffects.ini";
        public const string EFFECTS_GLASS = "PierceEffects_011-019mm";
        public const string EFFECTS_GLASS_SOUND = "PierceEffects_011-019mm";
        public const string EFFECTS_GROUNDHOLESINI = "data/Settings/GroundHoles.ini";
        public const string EFFECTS_PlaneDestroySound = "objects.detonate";
        public const string ENCYCLOPAEDIA_CONTENT = "Data/Settings/EncyclopaediaCont.xml";
        public const string ENTITY_DATA = "data/";
        public const string ENTITY_DATAITEMS = "data/items/";
        public const string ENTITY_DATANAME = "data";
        public const string ENTITY_DATAUNITS = "data/units/";
        public const string ENTITY_EMPTY = "data/items/_empty_";
        public const string ENTITY_ITEMS = "items/";
        public const string ENTITY_MAGAZINES = "data/items/magazines";
        public const string ENTITY_StandardItemContainer = "data/Items/Misc/Container/unit.ini";
        public const string ENTITY_Static = "data/Settings/static.ini";
        public const string ENTITY_StaticPreset = "data/Settings/StaticTypePresets.ini";
        public const string ENTITY_UNIT = "/unit.ini";
        public static readonly string ENTITY_UNIT_WO_DELIMITER = Path.GetFileName("/unit.ini");
        public const string FILES_ID_FILENAME = "data/settings/filesid.ini";
        public const string GAME_HotKeys = "keys.ini";
        public const string GAME_HotKeysDefault = @"Data\Settings\key_defaults.ini";
        public const string GAME_LOADING_MAT = "Data/Gui/FrontEnd/gameloading.mat";
        public const string GAME_SoundDefault = "Data/Settings/sound_defaults.ini";
        public const string GROUND_DEFORM_NAME_DESCRIPTION = "deform1";
        public const string GUI_Animator_Elements = "data/GUI/animator/elements.mat";
        public const string GUI_Animator_ElementsS = "data/GUI/animator/elementss.mat";
        public const string GUI_avi = "intro_FMV_gen_v02.avi";
        public const string GUI_ClearSetZ = "data/gui/clear_set_z.mat";
        public const string GUI_DetachmentIcon = "Data/Gui/Ingame/Icons/Detachment/default.mat";
        public const string GUI_DispositionGroundMat = "Data/GUI/Ingame/dispositionground.mat";
        public const string GUI_Faces = "data/gui/Ingame/Faces/";
        public const string GUI_FacesDefault = "data/gui/Ingame/Faces/default.mat";
        public const string GUI_FacesDefaultSmall = "data/gui/Ingame/Faces/default_sm.mat";
        public const string GUI_FrontEnd_avi01_mat = "Data/GUI/FrontEnd/avi01.mat";
        public const string GUI_FrontEnd_Back = "Data/GUI/FrontEnd/back01.mat";
        public const string GUI_FrontEnd_Controls_mat = "Data/GUI/FrontEnd/controls.mat";
        public const string GUI_FrontEnd_Controls_xml = "Data/GUI/FrontEnd/controls.xml";
        public const string GUI_FrontEnd_CountryDependentSkinsDir = "Data/GUI/FrontEnd/CountryDependent/";
        public const string GUI_FrontEnd_LoadingBck_mat = "loading.mat";
        public const string GUI_FrontEnd_MainMenu_back01_mat = "Data/GUI/FrontEnd/MainMenu/back01.mat";
        public const string GUI_FrontEnd_MenuSkinsMat = "Data/GUI/FrontEnd/ngmenuskins.mat";
        public const string GUI_FrontEnd_MenuSkinsXml = "Data/GUI/FrontEnd/ngmenuskins.xml";
        public const string GUI_FrontEnd_MissionFail_mat = "missionfail.mat";
        public const string GUI_FrontEnd_MissionWin_mat = "missionwin.mat";
        public const string GUI_IconsUnits = "Data/GUI/Ingame/Icons/Units/";
        public const string GUI_IngameDefault = "Data/GUI/Ingame/default.mat";
        public const string GUI_IngameIcons = "Data/GUI/Ingame/Icons/";
        public const string GUI_IngameIcons_Barrel = "WeaponStatic/barrel_default.mat";
        public const string GUI_IngameIcons_MagazinesGren = "Data/GUI/Ingame/Icons/Magazines/grenade_default.mat";
        public const string GUI_IngameIcons_MagazinesShell = "Data/GUI/Ingame/Icons/Magazines/shell_default.mat";
        public const string GUI_IngameIcons_Throw = "WeaponItem/throw.mat";
        public const string GUI_IngameIndicators = "Data/GUI/Ingame/Indicators";
        public const string GUI_IngameMain = "Data/GUI/Ingame/main.mat";
        public const string GUI_IngameMainParams = "Data/GUI/ingame/main.xml";
        public const string GUI_IngameMessagesXml = "Data/AI/messages.xml";
        public const string GUI_MenuSystem_Credits = "data/GUI/MenuSystem/Credits.mat";
        public const string GUI_MenuSystem_Ingame_MessagePanel_panel_mat = "data/GUI/MenuSystem/Ingame/MessagePanel/panel.mat";
        public const string GUI_Minimap = "minimap.mat";
        public const string GUI_MinimapHole = "Data/GUI/Ingame/Hole.mat";
        public const string GUI_Skin = "Data/Settings/gui.ini";
        public const string GUI_SoundPreset_Click = "ui_click";
        public const string GUI_Win95_Cursors = "data/GUI/win95/cursors.mat";
        public const string GUI_Win95_CursorsS = "data/GUI/win95/cursorss.mat";
        public const string GUI_Win95_Elements = "data/GUI/win95/elements.mat";
        public const string GUI_Win95_ElementsS = "data/GUI/win95/elementss.mat";
        public const string GUI_XMLSkins = "Data/gui/skins/ordinary/";
        public const string MAPS_PATH = "maps/";
        public const string MAPS_PATH_WO_PREFIX = "maps";
        public static readonly string MAPS_ROADS_PATH_WO_DELIMITER = @"Maps\_Roads";
        public const string MISSION_BACKPACKS_SETS = "Data/Units/Units_Backpacks.ini";
        public const string MISSION_CLASSES_SETS = "Data/Units/Units_Classes.ini";
        public const string MISSION_CREW_SETS = "Data/Units/Units_CrewSets.ini";
        public const string MISSION_Faces = "Data/Units/Units_Faces.ini";
        public const string MISSION_FILENAMEAMBIENT = "ambient.xml";
        public const string MISSION_FILENAMEBRIEFING = "Briefing.xml";
        public const string MISSION_FILENAMELOADINI = "/load.ini";
        public static readonly string MISSION_FILENAMELOADINI_WO_DELIMITER = Path.GetFileName("/load.ini");
        public const string MISSION_FILENAMEMISSIONXML = "Mission.xml";
        public const string MISSION_PATHS = "/paths/";
        public const string MISSION_SCRIPTS = "/scripts/";
        public const string MOVIE_BONUS_AUDIO = null;
        public const string MOVIE_BONUS_VIDEO = null;
        public const string MP_SETTINGS_FILE = "users/mp.ini";
        public const string MP_SETTINGS_FILE_DEFAULT = "data/Settings/mp_default.ini";
        public const string NAV_SPEEDPRESETS = "Data/Settings/SpeedPresets.ini";
        public const string NAVIGATION_INI_FILE_NAME = "Data/Settings/navigation.ini";
        public const string PERSON_LASTNAMES = "data/Units/Units_PersonLastNames.ini";
        public const string PERSON_NAMES = "data/Units/Units_PersonNames.ini";
        public const string PFSMAP_FILE = "pfsmap.bin";
        public const string PLANE_EXPLOSION_DEFAULT = "data/ammo/rus/fab_100";
        public const string PLAYER_INDICATORS_INI = "data/settings/playerindicators.ini";
        public const string SKILLS_ACCURACY_INI = "data/ai/accuracy.ini";
        public const string SKILLS_AWARDS_INI = "data/ai/awards.ini";
        public const string SKILLS_Dispersion = "data/Ai/SkillsImpact/Dispersion.ini";
        public const string SKILLS_EXPERIENCE_INI = "data/ai/experience.ini";
        public const string SKILLS_INI = "data/ai/skills.ini";
        public const string SKILLS_LEVELS_INI = "data/ai/levels.ini";
        public const string SKILLS_NoiseRange = "data/Ai/SkillsImpact/NoiseRange.ini";
        public const string SKILLS_ObservableRange = "data/Ai/SkillsImpact/ObservableRange.ini";
        public const string SKILLS_RANKS_INI = "data/ai/ranks.ini";
        public const string SKILLS_ReloadSpeed = "data/Ai/SkillsImpact/ReloadSpeed.ini";
        public const string SKILLS_SHOULDERS_INI = "data/settings/shoulders.ini";
        public const string SKILLS_TransferSpeed = "data/Ai/SkillsImpact/TransferSpeed.ini";
        public const string SOUND_FALLING_TREE = "crash.tree";
        public const string SOUND_HUMAN_STEPS = "steps";
        public const string SOUND_INI = "users/sound.ini";
        public const string SOUND_PRESETS = "presets/sounds/";
        public const string SOUND_PRESETS_AMBIENCE = "presets/sounds/ambience/";
        public const string SOUND_STATIC_DIE_DEFAULT = "_error_";
        public const string SOUND_VariationsINI = "Variations.ini";
        public const string SUNFLARE_mat0 = "effects/SunFlare/Flare0.mat";
        public const string SUNFLARE_mat1 = "effects/SunFlare/Flare1.mat";
        public const string SUNFLARE_mat2 = "effects/SunFlare/Flare2.mat";
        public const string SUNFLARE_mat3 = "effects/SunFlare/Flare3.mat";
        public const string SUPPORT_INI = "data/Settings/support.ini";
        public const string TRIGGER_INI = "trigger.ini";
        public const string UNIT_Hier = "/hier.ini";
        public static readonly string UNIT_Hier_WO_DELIMITER = Path.GetFileName("/hier.ini");
        public const string UNIT_Parts = "/parts.ini";
        public static readonly string UNIT_Parts_WO_DELIMITER = Path.GetFileName("/parts.ini");
        public const string UNIT_Skins = "skins.ini";
        public const string VIDEO_PRESET_INI = "Data/Settings/video_presets.ini";
        public const string VOICE_MainDir = "samples/";
        public const string VOICE_VoiceDir = "voice/";
        public const string WATERMAP_FILE = "pfswatermap.bin";
        public const string WEAPON_Ammo = "data/Ammo/";
        public const string WEAPON_AmmoSack = "data/Units/Container/ANY/AmmoSack";
        public const string WEAPON_DATA_GUNS = "data/guns/";
        public const string WEAPON_GunEffects = "data/settings/GunEffects.ini";
        public const string WEAPON_Magazines = "data/items/magazines/";
        public const string WEAPON_MagazinesIni = "/unit.ini";
        public const string WEAPON_Parse_Data = "data";
        public const string WEAPON_Parse_Items = "items";
        public const string WEAPON_Parse_Magazines = "magazines";
        public const string WEAPON_Parse_UnitIni = "unit.ini";
        public const string WEAPON_ThrowIni = "data/guns/Throw.ini";
    }
}

