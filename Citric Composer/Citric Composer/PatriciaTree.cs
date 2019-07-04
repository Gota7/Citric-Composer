using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CitraFileLoader
{
    public class PatriciaTree
    {
        private PatriciaTree.NodeCollection _nodes = new PatriciaTree.NodeCollection();
        private PatriciaTree.ItemCollection _items = new PatriciaTree.ItemCollection();
        public const int InvalidIndex = -1;
        private PatriciaTree.Node _root;

        public PatriciaTree.INode Root
        {
            get
            {
                return (PatriciaTree.INode)this._root;
            }
        }

        public interface IPatriciaTreeItem
        {
            string Key { get; set; }
            SoundType SoundType { get; set; }
            int ItemIndex { get; set; }
            int StringTableIndex { get; set; }
        }

        public class PatriciaTreeItem : IPatriciaTreeItem
        {
            public string Key { get; set; }
            public SoundType SoundType { get; set; }
            public int ItemIndex { get; set; }
            public int StringTableIndex { get; set; }
        }

        public PatriciaTree.INodeCollection Nodes
        {
            get
            {
                return (PatriciaTree.INodeCollection)this._nodes;
            }
        }

        public PatriciaTree.IItemCollection Items
        {
            get
            {
                return (PatriciaTree.IItemCollection)this._items;
            }
        }

        public void Add(IPatriciaTreeItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            this._items.Add(item);
            this.AddNode(this._root, item);
        }

        public bool Remove(IPatriciaTreeItem item)
        {
            return this.Remove(item.Key);
        }

        public bool Remove(string key)
        {
            PatriciaTree.Node node = this.FindNode(key);
            if (node == null)
                return false;
            this.CutLink(node);
            this._nodes.Remove((PatriciaTree.INode)node);
            this._items.Remove(key);
            return true;
        }

        public void Clear()
        {
            this._nodes.Clear();
            this._items.Clear();
        }

        private void AddNode(PatriciaTree.Node node, IPatriciaTreeItem item)
        {
            if (node == null)
            {
                this._root = (PatriciaTree.Node)new PatriciaTree.Leaf(this._nodes.Count, item);
                this._nodes.Add((PatriciaTree.INode)this._root);
            }
            else
            {
                int start = node.Parent == null ? 0 : node.Parent.Bit;
                if (node is PatriciaTree.BranchNode)
                {
                    int bit = PatriciaTree.BitHelper.CompareBit(node.Key, item.Key, start, node.Bit);
                    if (0 <= bit)
                    {
                        PatriciaTree.Leaf leaf = new PatriciaTree.Leaf(this._nodes.Count, item);
                        this._nodes.Add((PatriciaTree.INode)leaf);
                        this.Branch(node, (PatriciaTree.Node)leaf, bit);
                    }
                    else
                        this.AddNode(PatriciaTree.BitHelper.GetBit(item.Key, node.Bit) ? node.Right : node.Left, item);
                }
                else
                {
                    if (!(node is PatriciaTree.Leaf))
                        throw new Exception("invalid node type.");
                    int bit = PatriciaTree.BitHelper.CompareBit(node.Key, item.Key, start);
                    if (0 >= bit)
                        throw new ArgumentException(nameof(item));
                    PatriciaTree.Leaf leaf = new PatriciaTree.Leaf(this._nodes.Count, item);
                    this._nodes.Add((PatriciaTree.INode)leaf);
                    this.Branch(node, (PatriciaTree.Node)leaf, bit);
                }
            }
        }

        private void Branch(PatriciaTree.Node node, PatriciaTree.Node newNode, int bit)
        {
            if (PatriciaTree.BitHelper.GetBit(node.Key, bit) == PatriciaTree.BitHelper.GetBit(newNode.Key, bit))
                throw new ArgumentException("node or newNode");
            PatriciaTree.Node node1 = (PatriciaTree.Node)new PatriciaTree.BranchNode(this._nodes.Count, this.GetBranchKey(node, newNode, bit), bit);
            this._nodes.Add((PatriciaTree.INode)node1);
            if (node.Parent != null)
            {
                if (node.Parent.Left == node)
                    node.Parent.Left = node1;
                else
                    node.Parent.Right = node1;
            }
            else
                this._root = node1;
            if (node.Bit > bit && PatriciaTree.BitHelper.GetBit(node.Key, bit))
            {
                node1.Left = newNode;
                node1.Right = node;
            }
            else
            {
                node1.Left = node;
                node1.Right = newNode;
            }
        }

        private string GetBranchKey(PatriciaTree.Node node1, PatriciaTree.Node node2, int bit)
        {
            return PatriciaTree.BitHelper.SubString(PatriciaTree.BitHelper.GetBitCount(node1.Key) <= bit ? node2.Key : node1.Key, bit);
        }

        private void CutLink(PatriciaTree.Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (!(node is PatriciaTree.Leaf))
                throw new Exception();
            if (node == this._root)
            {
                this._root = (PatriciaTree.Node)null;
            }
            else
            {
                PatriciaTree.Node node1 = node == node.Parent.Left ? node.Parent.Right : node.Parent.Left;
                if (node.Parent == this._root)
                {
                    node1.Parent = (PatriciaTree.Node)null;
                    this._root = node1;
                }
                else if (node.Parent == node.Parent.Parent.Left)
                    node.Parent.Parent.Left = node1;
                else
                    node.Parent.Parent.Right = node1;
            }
        }

        private PatriciaTree.Node FindNode(string key)
        {
            return this.FindNode(this._root, key);
        }

        private PatriciaTree.Node FindNode(PatriciaTree.Node node, string key)
        {
            if (node == null)
                return (PatriciaTree.Node)null;
            if (!(node is PatriciaTree.Leaf))
                return this.FindNode(PatriciaTree.BitHelper.GetBit(key, node.Bit) ? node.Right : node.Left, key);
            if (!(node.Key == key))
                return (PatriciaTree.Node)null;
            return node;
        }

        public interface INode
        {
            int Index { get; }

            PatriciaTree.INode Left { get; }

            PatriciaTree.INode Right { get; }

            string Key { get; }

            int Bit { get; }

            SoundType SoundType { get; }

            int ItemIndex { get; }

            int StringTableIndex { get; }

            bool ContainsData { get; }

        }

        public interface ILeaf : PatriciaTree.INode
        {
            IPatriciaTreeItem Item { get; }
        }

        public interface IBranchNode : PatriciaTree.INode
        {
        }

        protected class Node : PatriciaTree.INode
        {
            private string _key = string.Empty;
            private int _index;
            private PatriciaTree.Node _parent;
            private PatriciaTree.Node _left;
            private PatriciaTree.Node _right;
            private int _bit;
            private IPatriciaTreeItem _item;
            private SoundType _soundType;
            private int _itemIndex;
            private int _stringTableIndex;

            protected Node(int index, string key, int bit)
              : this(index, key, bit, (IPatriciaTreeItem)null)
            {
            }

            protected Node(int index, string key, int bit, IPatriciaTreeItem data)
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (0 > bit)
                    throw new ArgumentOutOfRangeException(nameof(bit));
                this._index = index;
                this._key = key;
                this._bit = bit;
                this._item = data;
            }

            public int Index
            {
                get
                {
                    return this._index;
                }
            }

            public PatriciaTree.Node Parent
            {
                get
                {
                    return this._parent;
                }
                set
                {
                    if (value == this._parent)
                        return;
                    if (value != null)
                    {
                        if (string.Compare(this.Key, 0, value.Key, 0, value.Key.Length) != 0)
                            throw new Exception();
                        if (PatriciaTree.BitHelper.GetBitCount(this.Key) <= value.Bit)
                            throw new Exception();
                        if (PatriciaTree.BitHelper.GetBit(this.Key, value.Bit))
                            value.Right = this;
                        else
                            value.Left = this;
                    }
                    this._parent = value;
                }
            }

            public PatriciaTree.Node Left
            {
                get
                {
                    return this._left;
                }
                set
                {
                    if (value == this._left)
                        return;
                    if (this._left != null)
                        this._left._parent = (PatriciaTree.Node)null;
                    if (value != null)
                    {
                        if (value._parent != null)
                            throw new Exception();
                        value._parent = this;
                    }
                    this._left = value;
                }
            }

            public PatriciaTree.Node Right
            {
                get
                {
                    return this._right;
                }
                set
                {
                    if (value == this._right)
                        return;
                    if (this._right != null)
                        this._right._parent = (PatriciaTree.Node)null;
                    if (value != null)
                    {
                        if (value._parent != null)
                            throw new Exception();
                        value._parent = this;
                    }
                    this._right = value;
                }
            }

            public string Key
            {
                get
                {
                    return this._key;
                }
            }

            public int Bit
            {
                get
                {
                    return this._bit;
                }
            }

            public IPatriciaTreeItem Item
            {
                get
                {
                    return this._item;
                }
                set
                {
                    this._item = value;
                }
            }

            public PatriciaTree.Node GetNode(bool bitValue)
            {
                if (!bitValue)
                    return this.Left;
                return this.Right;
            }

            PatriciaTree.INode PatriciaTree.INode.Left
            {
                get
                {
                    return (PatriciaTree.INode)this.Left;
                }
            }

            PatriciaTree.INode PatriciaTree.INode.Right
            {
                get
                {
                    return (PatriciaTree.INode)this.Right;
                }
            }

            public SoundType SoundType {

                get {
                    return _item.SoundType;
                }

            }

            public int ItemIndex {

                get {
                    return _item.ItemIndex;
                }

            }

            public int StringTableIndex {

                get {
                    return _item.StringTableIndex;
                }

            }

            public bool ContainsData => _item != null;

        }

        protected class Leaf : PatriciaTree.Node, PatriciaTree.ILeaf, PatriciaTree.INode
        {
            public Leaf(int index, IPatriciaTreeItem item)
              : base(index, item.Key, PatriciaTree.BitHelper.GetBitCount(item.Key), item)
            {
            }
        }

        protected class BranchNode : PatriciaTree.Node, PatriciaTree.IBranchNode, PatriciaTree.INode
        {
            public BranchNode(int index, string key, int bit)
              : base(index, key, bit)
            {
            }
        }

        public interface INodeCollection : IEnumerable<PatriciaTree.INode>, IEnumerable
        {
            int Count { get; }

            bool Contains(PatriciaTree.INode item);

            int IndexOf(PatriciaTree.INode item);

            PatriciaTree.INode this[int index] { get; }
        }

        public interface IItemCollection : IEnumerable<IPatriciaTreeItem>, IEnumerable
        {
            int Count { get; }

            bool Contains(IPatriciaTreeItem item);

            bool Contains(string key);

            int IndexOf(IPatriciaTreeItem item);

            int IndexOf(string key);

            IPatriciaTreeItem this[int index] { get; }

            IPatriciaTreeItem this[string key] { get; }
        }

        private class NodeCollection : Collection<PatriciaTree.INode>, PatriciaTree.INodeCollection, IEnumerable<PatriciaTree.INode>, IEnumerable
        {
        }

        private class ItemCollection : KeyedCollection<string, IPatriciaTreeItem>, PatriciaTree.IItemCollection, IEnumerable<IPatriciaTreeItem>, IEnumerable
        {
            public int IndexOf(string key)
            {
                return this.IndexOf(this[key]);
            }

            protected override string GetKeyForItem(IPatriciaTreeItem item)
            {
                return item.Key;
            }

            bool PatriciaTree.IItemCollection.Contains([In] string obj0)
            {
                return this.Contains(obj0);
            }

            /*
            [SpecialName]
            IPatriciaTreeItem PatriciaTree.IItemCollection.get_Item([In] string obj0)
            {
                return this[obj0];
            }*/

        }

        private class BitHelper
        {
            public static bool GetBit(char ch, int bit)
            {
                if (bit < 0 || 7 < bit)
                    throw new ArgumentOutOfRangeException(nameof(bit));
                return 0 != ((int)ch >> 7 - bit & 1);
            }

            public static bool GetBit(string str, int bit)
            {
                if (str == null)
                    throw new ArgumentNullException(nameof(str));
                if (0 > bit)
                    throw new ArgumentOutOfRangeException(nameof(bit));
                int index = bit / 8;
                return PatriciaTree.BitHelper.GetBit(index < str.Length ? str[index] : char.MinValue, bit - index * 8);
            }

            public static int GetBitCount(string str)
            {
                if (str == null)
                    throw new ArgumentNullException(nameof(str));
                return str.Length * 8;
            }

            public static int CompareBit(string str1, string str2, int start)
            {
                return PatriciaTree.BitHelper.CompareBit(str1, str2, start, -1);
            }

            public static int CompareBit(string str1, string str2, int start, int end)
            {
                if (str1 == null)
                    throw new ArgumentNullException("str");
                if (str2 == null)
                    throw new ArgumentNullException("str");
                if (0 > start)
                    throw new ArgumentOutOfRangeException("bit");
                int num = end;
                if (0 > end)
                    num = PatriciaTree.BitHelper.GetBitCount(str1.Length < str2.Length ? str2 : str1);
                for (int bit = start; bit < num; ++bit)
                {
                    if (PatriciaTree.BitHelper.GetBit(str1, bit) ^ PatriciaTree.BitHelper.GetBit(str2, bit))
                        return bit;
                }
                return -1;
            }

            public static string SubString(string str, int bit)
            {
                if (str == null)
                    throw new ArgumentNullException(nameof(str));
                if (0 > bit)
                    throw new ArgumentOutOfRangeException(nameof(bit));
                int length = bit / 8;
                if (0 < bit - length * 8)
                    ++length;
                return str.Substring(0, length);
            }



        }
    }
}
