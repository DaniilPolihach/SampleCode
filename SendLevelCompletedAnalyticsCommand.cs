using System.Collections.Generic;
using Common;

public sealed class SendLevelCompletedAnalyticsCommand : CommandBase<CrossContext>
{
    public int LevelId { get; set; }

    public SendLevelCompletedAnalyticsCommand(CrossContext ctx) : base(ctx)
    {
    }

    public override void Execute()
    {
        Context.AnalyticsManager.LogEvent(AnalyticsEvents.LevelCompleted, new Dictionary<string, object>
        {
            { AnalyticsParameters.LevelId, LevelId }
        });   

        CleanUp();
    }

    public override void CleanUp()
    {
        LevelId = -1;
    }
}