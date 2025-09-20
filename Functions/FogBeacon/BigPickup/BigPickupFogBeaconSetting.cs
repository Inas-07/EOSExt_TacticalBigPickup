namespace EOSExt.TacticalBigPickup.Functions.FogBeacon.BigPickup
{
    public class RepellerSphereSetting
    {
        public bool InfiniteDuration { get; set; } = false;
        public float GrowDuration { get; set; } = 10f;
        public float ShrinkDuration { get; set; } = 10f;
        public float Range { get; set; } = 11f;
    }

    public class BigPickupFogBeaconSetting
    {
        public float TimeToPickup { set; get; } = 1f;
        public float TimeToPlace { set; get; } = 1f;
        public RepellerSphereSetting RSHold { get; set; } = new();
        public RepellerSphereSetting RSPlaced { get; set; } = new();
    }
}
