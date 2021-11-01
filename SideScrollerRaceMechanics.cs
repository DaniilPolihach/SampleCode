using Game.Commands;
using Game.Conditions;

namespace Game.SideScrollerRace
{
    public sealed class SideScrollerRaceMechanics : GameMechanicsBase
    {
        private GameCommandBase[] _tickCommands;
        
        public SideScrollerRaceMechanics(GameContext ctx) : base(ctx)
        {
        }

        public override void SetUp()
        {
            CreateActions();
            RegisterGameConditions();

            Context.SpawnCarCommand.Execute();
            Context.StartLevelCommand.Execute();
            
            RegisterRouters();
        }

        public override void Tick(float tick)
        {
            for (int i = 0; i < _tickCommands.Length; i++)
            {
                _tickCommands[i].Execute();
            }
        }

        public override void TearDown()
        {
            Context.ConditionsManager.Clear();

            // Context.GameResultRouter.Close();
            // Context.GameHUDRouter.Close();
            
            _tickCommands = null;
            
            Context.SetState(GameState.Disposed);
        }

        private void RegisterRouters()
        {
            Context.GameHUDRouter.Navigate();
        }

        private void CreateActions()
        {
            _tickCommands = new GameCommandBase[]
            {
                new FollowCameraByCarCommand(Context),
                new CalculateCarPassedDistanceCommand(Context),
                new BurnCarFuelCommand(Context),
                
                new ProcessCarOverturnedCommand(Context),
                new ProcessContactLavaVolumesCommand(Context),
                new ProcessCarDestroyedCommand(Context),
                new ProcessCarLandedCommand(Context),

                new CollectCoinsCommand(Context),
                new CollectChestsCommand(Context),
                new CollectFuelTanksCommand(Context),
                new CollectRocketsCommand(Context),
                new CheckGameConditionsCommand(Context),
                new ProcessSpeedRampsCommand(Context),
                new ProcessActivatedRocketsCommand(Context),

                new ProcessPlayerInputCommand(Context),
                new CarTickCommand(Context)
            };
        }

        private void RegisterGameConditions()
        {
            // lose
            Context.ConditionsManager.RegisterLoseCondition(new FuelEmptyLoseCondition(Context));
            Context.ConditionsManager.RegisterLoseCondition(new DestroyCarLoseCondition(Context));

            // win
            Context.ConditionsManager.RegisterWinCondition(new CrossFinishLineWinCondition(Context));
        }
    }
}