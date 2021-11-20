namespace ObjectAbstraction.ModelChanger
{
    public interface IModelChanger
    {
        public bool Shootable { get; }
        public bool IsAbstract { get; }

        void ToggleModels();
    }
}
