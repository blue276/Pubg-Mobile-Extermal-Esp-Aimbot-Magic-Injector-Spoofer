using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using ShpVector3 = SharpDX.Vector3;
using ShpVector2 = SharpDX.Vector2;
namespace PUBGMESP
{
    public class DisplayData
    {
        public long ViewMatrixBase;
        public long myObjectAddress;
        public PlayerData[] Players;
        public ItemData[] Items;
        public VehicleData[] Vehicles;
        public BoxData[] Boxes;
        public GrenadeData[] Grenades;

        public DisplayData(long viewMatrixBase,long myObjectAddress)
        {
            this.ViewMatrixBase = viewMatrixBase;
            this.myObjectAddress = myObjectAddress;
        }
    }

    public class PlayerData
    {
        public string Type;
        public long Address;s
        public int Status;
        public ShpVector3 Position;
        public int Pose;
        public float Health;
        public string Name;
        public bool IsRobot;
        public int TeamID;
        public int IsTeam;
    }

    public class ItemData
    {
        public Item Type;
        public ShpVector3 Position;
        public string Name;
    }

    public class VehicleData
    {
        public Vehicle Type;
        public ShpVector3 Position;
        public string Name;
    }

    public class BoxData
    {
        public string[] Items;
        public ShpVector3 Position;
    }

    public class GrenadeData
    {
        public Grenade Type;
        public ShpVector3 Position;
    }

    /// <summary>
    /// Item Type
    /// </summary>
    public enum Item
    {
        [Description("Useless")]
        Useless,
        [Description("[Med] Enegy Drink")]
        EnegyDrink,
        [Description("[Med] Epinephrine")]
        Epinephrine,
        [Description("[Med] Pain Killer")]
        PainKiller,
        [Description("[Med] First Aid Kit")]
        AidKit,
        [Description("[Armor] Lv.3 Bag")]
        BagLv3,
        [Description("[Armor] Lv.2 Bag")]
        BagLv2,
        [Description("[Armor] Lv.2 Armor")]
        ArmorLv2,
        [Description("[Armor] Lv.3 Armor")]
        ArmorLv3,
        [Description("[Armor] Lv.3 Helmet")]
        HelmetLv3,
        [Description("[Armor] Lv.2 Helmet")]
        HelmetLv2,
        [Description("[Sniper] AWM")]
        AWM,
        [Description("[Rifle] SCAR-L")]
        SCARL,
        [Description("[Sniper] Kar-98")]
        Kar98,
        [Description("[Rifle] M762")]
        M762,
        [Description("[MachineGun] DP-28")]
        DP28,
        [Description("[Rifle] Groza")]
        Groza,
        [Description("[Rifle] AKM")]
        AKM,
        [Description("[Rifle] AUG")]
        AUG,
        [Description("[Rifle] QBZ")]
        QBZ,
        [Description("[MachineGun] M249")]
        M249,
        [Description("[Rifle] M4A1")]
        M4A1,
        [Description("[Ammo] 300 Magnum Ammo")]
        AmmoMagnum,
        [Description("[Ammo] 7.62 Ammo")]
        Ammo762,
        [Description("[Ammo] 5.56 Ammo")]
        Ammo556,
        [Description("[Scope] 4x Scope")]
        Scope4x,
        [Description("[Scope] 6x Scope")]
        Scope6x,
        [Description("[Scope] 8x Scope")]
        Scope8x,
        [Description("[Apendix] Rifle Silenter")]
        RifleSilenter,
        [Description("[Apendix] Rifle Rapid Expansion Magazine")]
        RifleMagazine,
        [Description("[Armor] Ghillie Suit")]
        GhillieSuit,
        [Description("[Pistol] Flare Gun")]
        FlareGun,
        [Description("[Sniper] M24")]
        M24,
        [Description("[Apendix] Sniper Silenter")]
        SniperSilenter,
        [Description("[Sniper] MK14")]
        MK14,
        [Description("[Sniper] SKS")]
        SKS,
        [Description("[Ammo] Grenade")]
        Grenade
    }

    /// <summary>
    /// Grenade Type
    /// </summary>
    public enum Grenade
    {
        [Description("Unknown")]
        Unknown,
        [Description("Smoke Grenade")]
        Smoke,
        [Description("Cocktail Grenade")]
        Burn,
        [Description("Flash Grenade")]
        Flash,
        [Description("Fragment Grenade")]
        Explode
    }

    /// <summary>
    /// Vehicle Type
    /// </summary>
    public enum Vehicle
    {
        [Description("Unknown")]
        Unknown,
        [Description("BRDM")]
        BRDM,
        [Description("Scooter")]
        Scooter,
        [Description("Motorcycle")]
        Motorcycle,
        [Description("MotorcycleCart")]
        MotorcycleCart,
        [Description("Snowmobile")]
        Snowmobile,
        [Description("Tuk")]
        Tuk,
        [Description("Buggy")]
        Buggy,
        [Description("Sports")]
        Sports,
        [Description("Dacia")]
        Dacia,
        [Description("Rony")]
        Rony,
        [Description("PickUp")]
        PickUp,
        [Description("UAZ")]
        UAZ,
        [Description("MiniBus")]
        MiniBus,
        [Description("PG117")]
        PG117,
        [Description("AquaRail")]
        AquaRail,
        [Description("AirPlane")]
        BP_AirDropPlane_C

    }

    /// <summary>
    /// Aimbot Position
    /// </summary>
    public enum AimPosition
    {
        [Description("Head")]
        Head,
        [Description("Chest")]
        Chest,
        [Description("Waist")]
        Waist
    }
}
