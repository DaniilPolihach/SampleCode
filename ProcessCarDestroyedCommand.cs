using Common;
using Game.UI;

namespace Game.Commands
{
    public sealed class ProcessCarDestroyedCommand : GameCommandBase
    {
        public ProcessCarDestroyedCommand(GameContext ctx) : base(ctx)
        {
        }

        public override void Execute()
        {
            if (Context.ActiveCarModel.Destroyed.Value)
            {
                return;
            }

            if (Context.ActiveCarModel.Health.Value > 0)
            {
                return;
            }

            Context.ActiveCarModel.Destroyed.Value = true;
            Context.SetState(GameState.Finished);

            Context.AudioManager.Play(AudioClips.CarDestroyed, Context.CarController.Position, volume: 1.5f);
            Context.CarController.Destroy();

            Context.UIManager.ShowPanel<CarDestroyedPanel>(new CarDestroyedPanelArgs
            {
                ContinueClickedCallback = () =>
                {
                    Context.UIManager.ClosePanel<CarDestroyedPanel>();
                    Context.StartLevelCommand.Execute();
                }
            });
        }
    }
}