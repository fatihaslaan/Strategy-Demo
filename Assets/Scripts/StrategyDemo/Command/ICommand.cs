namespace StrategyDemo.Command_NS
{
    public interface ICommand
    {
        public void Execute();

        public void Undo();

        public void Terminate();
    }
}