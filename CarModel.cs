namespace Game
{
    public sealed class ActiveCarModel
    {
        public bool Accelerate { get; set; }
        public bool Brake { get; set; }

        public float RocketBoostTime { get; set; }
        public float RocketBoostForce { get; set; }

        public readonly ReactiveProperty<int> Coins = new ReactiveProperty<int>();
        public readonly ReactiveProperty<int> Chests = new ReactiveProperty<int>();
        public readonly ReactiveProperty<int> Rockets = new ReactiveProperty<int>();
        
        public readonly ReactiveProperty<float> Speed = new ReactiveProperty<float>();
        
        public readonly ReactiveProperty<int> Fuel = new ReactiveProperty<int>();
        public readonly ReactiveProperty<int> MaxFuel = new ReactiveProperty<int>();

        public readonly ReactiveProperty<int> BasicFuelConsumption = new ReactiveProperty<int>();
        public readonly ReactiveProperty<int> AccelerateFuelConsumption = new ReactiveProperty<int>();

        public readonly ReactiveProperty<int> Health = new ReactiveProperty<int>();
        public readonly ReactiveProperty<int> MaxHealth = new ReactiveProperty<int>();

        public readonly ReactiveProperty<float> Distance = new ReactiveProperty<float>();
        public readonly ReactiveProperty<float> MaxDistance = new ReactiveProperty<float>();

        public readonly ReactiveProperty<bool> Destroyed = new ReactiveProperty<bool>();
    }
}