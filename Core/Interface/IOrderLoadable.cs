namespace MysteriousAlchemy.Core.Interface
{
    public interface IOrderLoadable
    {
        public void Load();

        public void Unload();

        public int LoaderIndex { get; }
    }
}
