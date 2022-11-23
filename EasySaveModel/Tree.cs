using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave {
#if !TREE_GENERIC
    public class Tree<T> : Tree<T, List<ITree<T>>> { 
        public Tree(): base() { }
    }
#endif

    //TODO: uniform data initialization !!!
    //TODO: ITree as NOT tag remover
    //TODO: Proxy for initialization ?
    public class Tree<T, TList> : ITree<T> where TList : IList<ITree<T>>, new() {
        public T Data { get; set; }
        public ITree<T> Parent { get; set; }
        public TList Branches { get; } = new TList();

        public Tree() {
            Parent = null;
        }

        public ITree<T> this[int k] { get => Branches[k]; set => Branches[k] = value; }
        public ITree<T> Last {
            get => Branches[Branches.Count - 1];
            set => Branches[Branches.Count - 1] = value;
        }
        public int Count { get => Branches.Count; }

        public void Add(ITree<T> branch) {
            Branches.Add(branch);
        }
    }
}
