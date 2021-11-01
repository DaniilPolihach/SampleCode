using Menu.Models;

namespace Game.Commands
{
    public class CompleteLevelCommand : GameCommandBase
    {
        private SendLevelCompletedAnalyticsCommand SendLevelCompletedAnalyticsCommand => Context.SharedContext.SendLevelCompletedAnalyticsCommand;
        private SendDeathAnalyticsCommand SendDeathAnalyticsCommand => Context.SharedContext.SendDeathAnalyticsCommand;

        public LevelCompleteReason Reason { get; set; }   

        public CompleteLevelCommand(GameContext ctx) : base(ctx)
        {
        }

        public override void Execute()
        {
            Context.SetState(GameState.Finished);

            Context.ActiveGameModel.Reason = Reason;

            switch (Reason)
            {
                case LevelCompleteReason.OutOfFuel:
                    Context.ActiveGameModel.LevelPasssed.Value = false;
                    break;

                case LevelCompleteReason.DestroyCar:
                    Context.ActiveGameModel.LevelPasssed.Value = false;
                    break;

                case LevelCompleteReason.CrossFinishLine:
                    Context.ActiveGameModel.LevelPasssed.Value = true;
                    break;
            }

            if (Reason == LevelCompleteReason.CrossFinishLine)
            {
                SendLevelCompletedAnalyticsCommand.LevelId = Context.ActiveGameModel.ActiveLevelId.Value;
                SendLevelCompletedAnalyticsCommand.Execute();
            }
            else
            {
                SendDeathAnalyticsCommand.Reason = Reason.ToString();
                SendDeathAnalyticsCommand.Distance = (int) Context.ActiveGameModel.Distance.Value;
                SendDeathAnalyticsCommand.Execute();
            }

            Context.GameResultRouter.Navigate();
        }

        public override void CleanUp()
        {
            Reason = LevelCompleteReason.None;
        }
    }
}