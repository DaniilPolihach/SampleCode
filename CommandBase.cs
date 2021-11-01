namespace Common
{
    public abstract class CommandBase<T> where T : ContextBase
    {
        protected T Context { get; private set; }

        protected CommandBase(T ctx)
        {
            Context = ctx;
        }

        public abstract void Execute();
        
        public virtual void CleanUp() { }
    }
}