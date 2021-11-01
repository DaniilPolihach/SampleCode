using Common;
using Game.Commands;
using Game.Conditions;
using Game.SideScrollerRace;
using Game.UI;
using Menu.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public sealed class GameContext : SubContextBase<CrossContext>
    {
        public override CrossContext SharedContext => CrossContext.SharedInstance;

        public GameFactories Factories { get; private set; }
        public GameConditionsManager ConditionsManager { get; private set; }

        public GameMechanicsBase ActiveMechanics { get; private set; }
        public GameState ActiveState { get; private set; } = GameState.Init;

        public ActiveGameModel ActiveGameModel { get; private set; }
        public ActiveCarModel ActiveCarModel { get; private set; }

        public LeaveGameCommand LeaveGameCommand { get; private set; }
        public StartLevelCommand StartLevelCommand { get; private set; }
        public CompleteLevelCommand CompleteLevelCommand { get; private set; }
        public SpawnCarCommand SpawnCarCommand { get; private set; }
        public ResetCarCommand ResetCarCommand { get; private set; }

        public GameHUDRouter GameHUDRouter { get; private set; }
        public GameResultRouter GameResultRouter { get; private set; }

        public CarController CarController { get; set; }

        public TrackRoot TrackRoot { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            ActiveGameModel = new ActiveGameModel();
            ActiveGameModel.Load();
            
            ActiveCarModel = new ActiveCarModel();

            LeaveGameCommand = new LeaveGameCommand(this);
            StartLevelCommand = new StartLevelCommand(this);
            CompleteLevelCommand = new CompleteLevelCommand(this);
            SpawnCarCommand = new SpawnCarCommand(this);
            ResetCarCommand = new ResetCarCommand(this);

            GameHUDRouter = new GameHUDRouter(this);
            GameResultRouter = new GameResultRouter(this);
            ConditionsManager = new GameConditionsManager();

            ActiveMechanics = new SideScrollerRaceMechanics(this);
            ActiveMechanics.SetUp();
        }

        protected override void Update()
        {
            base.Update();

            if (ActiveState == GameState.InProgress)
            {
                ActiveMechanics.Tick(Time.time);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ActiveGameModel.PendingGameResult.Value = true;
            ActiveGameModel.Save();
            ActiveGameModel = null;

            ActiveMechanics.TearDown();
            ActiveMechanics = null;

            LeaveGameCommand = null;
            StartLevelCommand = null;
            CompleteLevelCommand = null;
            SpawnCarCommand = null;
            ResetCarCommand = null;

            GameHUDRouter = null;
            GameResultRouter = null;
            
            CarController = null;

            TrackRoot = null;
        }

        public void SetState(GameState state)
        {
            ActiveState = state;
            Debug.Log($"[GameState] Changed: {state}");
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            Factories = GameObject
                .FindGameObjectWithTag(GameTags.Factories)
                ?.GetComponent<GameFactories>();

            Assert.IsNotNull(Factories);

            TrackRoot = GameObject
                .FindGameObjectWithTag(GameTags.TrackRoot)
                ?.GetComponent<TrackRoot>();

            Assert.IsNotNull(TrackRoot);
        }
    }
}