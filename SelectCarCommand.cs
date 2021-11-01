using Common;

namespace Menu.Commands
{
    public sealed class SelectCarCommand : CommandBase<MenuContext>
    {
        private SendCarSelectedAnalyticsCommand SendAnalyticsCommand => Context.SharedContext.SendCarSelectedAnalyticsCommand;

        public SelectCarCommand(MenuContext ctx) : base(ctx)
        {
        }

        public override void Execute()
        {
            Context.ActiveGameModel.ActiveCarId.Value = Context.CarsModel.SelectedCarId.Value;

            SendAnalyticsCommand.CarId = Context.ActiveGameModel.ActiveCarId.Value;
            SendAnalyticsCommand.Execute();
        }
    }
}