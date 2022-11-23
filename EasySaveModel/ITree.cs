namespace EasySave {
    public interface ITree<T>  {
        ITree<T> this[int k] { get; set; }

        int Count { get; }
        T Data { get; set; }
        ITree<T> Last { get; set; }
        ITree<T> Parent { get; set; }

        void Add(ITree<T> branch);
    }
}