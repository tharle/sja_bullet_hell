using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameParameters
{

    public class AnimationEnemy
    {
        public const string TRIGGER_DIE = "die";
        public const string TRIGGER_ATTACK = "attack";
        public const string FLOAT_VELOCITY = "velocity";
    }

    public class AnimationMainMenu
    {
        public const string TRIGGER_FADE_OUT = "menu_fade_out";
        public const string TRIGGER_LOAD_ENTER = "menu_load_enter";
        public const string TRIGGER_LOAD_EXIT = "menu_load_exit";
    }


    public class AnimationPlayer
    {
        public const string FLOAT_VELOCITY = "velocity";
        public const string TRIGGER_INTERACT = "interact";
        public const string TRIGGER_IDLE = "idle";
    }

    public class BundleNames
    {
        public const string PREFAB_ACHIVEMENTS = "prefab_achivement";
        public const string PREFAB_ENEMY = "prefab_enemy";
        public const string PREFAB_EFFECT = "prefab_effect";
        public const string PREFAB_ITEM_COLLETABLE = "prefab_item_colletable";
        public const string BULLET = "bullet";
        public const string ITEM = "item";
        public const string SCRIT_OBJETS = "data";
        public const string SFX = "sfx";
        public const string SPRITE_ITEM = "sprite_item";
    }

    public class BundlePath
    {
        public const string RESOURCES_RANK = "Sprites/stamp_mark/";
        public const string BUNDLE_ASSETS = "Assets/BundleAssets";
        public const string STREAMING_ASSETS = "Assets/StreamingAssets";

        public const string PREFAB_ACHIVEMENTS = "/Prefabs/Achivements";
        public const string PREFAB_ENEMY = "/Prefabs/Enemies";
        public const string PREFAB_EFFECT = "/Prefabs/Effects";
        public const string PREFAB_ITEM_COLLETABLE = "/Prefabs/Itens";
        public const string BULLETS = "/Data/Bullets";
        public const string ITEM = "/Data/Itens";
        public const string SCRIT_OBJETS = "/Data";
        public const string SFX = "/Sounds/SFX";
        public const string SPRITES_STAMPS = "/Sprites/Stamps";
    }

    public class InputName
    {
        public const string AXIS_HORIZONTAL = "Horizontal";
        public const string AXIS_VERTICAL = "Vertical";
        public const string AXIS_MOUSE_HORIZONTAL = "Mouse X";
        public const KeyCode GAME_MENU = KeyCode.Escape;
    }

    public class Prefs
    {
        public const float GRID_SIZE_IN_PIXEL = 0.16f;
        public const float ENEMY_TICK_CHECK_IN_SECONDS = 0.1f;
        public const float ENEMY_DIE_DURATION = 1f;
        public const float UI_LIFE_BAR_DISPLAY_TIME = 1f;
        public const float WAVE_COOLDOWN = 4f;
        public const float ENEMY_SPAWN_COOLDOWN = 1f;
        public const float ITEM_DESPAWN_TIME = 4f;
        public const int ITEM_DROP_CHANCE = 99; // in %
    }

    public class SceneName
    {
        public const string MAIN_MENU = "MainMenu";
        public const string GAME = "Game";
    }

    public class ScreenConfig
    {
        public const float DENSITY_PIXELS = 0.16f;
    }

    public class TagName {
        public const string PLAYER = "Player";
    }
}
