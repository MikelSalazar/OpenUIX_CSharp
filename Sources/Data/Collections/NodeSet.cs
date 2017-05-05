using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenUIX.Data.Collections
{
	/// <summary> Defines an ordered collection of Nodes. </summary>
	/// <remarks> It has to maintain the order of the Nodes. </remarks>
	public class NodeSet : IEnumerable, IEnumerable<Node>
	{
		#region Protected Fields

		/// <summary> The type of the nodes in the NodeSet. </summary>
		protected NodeType _Type = null;

		/// <summary> The list of node names. </summary>
		protected List<string> _Keys;

		/// <summary> The ordered collection of nodes. </summary>
		protected List<Node> _Values;

		/// <summary> The parent NodeSet. </summary>
		protected NodeSet _Parent;

		/// <summary> The collection of subsets. </summary>
		protected Dictionary<NodeType, NodeSet> _Children;

		/// <summary> The associated Node reference. </summary>
		protected Node _Node;

		#endregion

		#region Public Properties

		/// <summary> Returns a particular Node in the NodeSet. </summary>
		/// <param name="index"> The index of the Node to retrieve. </param>
		/// <returns> The requested Node. </returns>
		public Node this[int index] { get { return Get(index); } }

		/// <summary> Returns a particular Node in the NodeSet. </summary>
		/// <param name="name"> The name of the Node to retrieve. </param>
		/// <returns> The requested Node. </returns>
		public Node this[string name] { get { return Get(name); } }

		/// <summary> Gets the type of the nodes in the Nodeset. </summary>
		public NodeType Type { get { return _Type; } }

		/// <summary> Gets the number of Nodes contained in the NodeSet.
		/// </summary>
		public int Count { get { return _Values.Count; } }

		/// <summary> Gets the parent NodeSet. </summary>
		public NodeSet Parent { get { return _Parent; } }

		/// <summary> Gets the collection of subsets. </summary>
		public Dictionary<NodeType, NodeSet> Children
		{ get { return _Children; } }

		/// <summary> Gets the associated Node reference. </summary>
		public Node Node { get { return _Node; } }

		#endregion

		#region Public Constructors

		/// <summary> Initializes a new NodeSet class instance. </summary>
		/// <param name="type"> The type of the nodes in the NodeSet. </param>
		/// <param name="parent"> The parent NodeSet. </param>
		public NodeSet(NodeType type = null, NodeSet parent = null)
		{
			// Check the given values
			_Type = type; _Parent = parent;

			// Add the given values
			if (_Parent != null)
			{
				// A subset must have a proper type
				if (_Type == null) throw new ArgumentNullException("type");
				if (_Type.Name == null || _Type.InnerType == null)
					throw new Exception("Invalid Type");

				// Add the this instance to the children of the parent
				_Parent._Children.Add(_Type, this);
			}

			// Initialize the encapsulated collections
			_Keys = new List<string>();
			_Values = new List<Node>();
			_Children = new Dictionary<NodeType, NodeSet>();
		}


		/// <summary> Initializes a new NodeSet class instance. </summary>
		/// <param name="type"> The type of the nodes in the NodeSet. </param>
		/// <param name="node"> The Node of the parent NodeSet. </param>
		public NodeSet(NodeType type, Node node) : this(type, node.Children) { }

		#endregion

		#region Public Methods

		/// <summary> Indicates whether the NodeSet contains a Node or not. 
		/// </summary>
		/// <param name="name"> The name of the node to look for. </param>
		/// <returns> True if the Nodeset contains the node, false otherwise. 
		/// </returns>
		public bool Contains(string name) { return _Keys.Contains(name); }


		/// <summary> Indicates whether the NodeSet contains a Node or not. 
		/// </summary>
		/// <param name="node"> The node to look for. </param>
		/// <returns> True if the Nodeset contains the node, false otherwise. 
		/// </returns>
		public bool Contains(Node node) { return _Values.Contains(node); }


		/// <summary> Returns the index of a given Node. </summary>
		/// <param name="name"> The name of the Node to look for. </param>
		/// <returns> The index of the Node within the collection. </returns>
		public int IndexOf(string name) { return _Keys.IndexOf(name); }


		/// <summary> Returns the index of a given Node. </summary>
		/// <param name="node"> The Node to look for. </param>
		/// <returns> The index of the Node within the collection. </returns>
		public int IndexOf(Node node) { return _Values.IndexOf(node); }


		/// <summary> Returns a particular Node in the NodeSet. </summary>
		/// <param name="index"> The index of the Node to retrieve.
		/// (negative values can be used for reverse access). </param>
		/// <returns> The requested Node. </returns>
		public Node Get(int index)
		{
			// Allow inverse access
			if (index < 0) index += _Values.Count;

			// Make sure  the index is in range
			if (index < 0 || index >= _Values.Count)
				throw new ArgumentOutOfRangeException("index");

			// Return the requested node
			return _Values[index];
		}


		/// <summary> Returns a particular Node in the NodeSet. </summary>
		/// <param name="name"> The name of the Node to retrieve. </param>
		/// <returns> The requested Node. </returns>
		public Node Get(string name)
		{
			// Check the given values
			if (name == null) throw new ArgumentNullException("name");

			// Look for the node in the list of names
			int index = _Keys.IndexOf(name);
			if (index < 0)
				throw new KeyNotFoundException("Not found: " + name);

			// Return the requested node
			return _Values[index];
		}


		/// <summary> Adds a Node to the Nodeset at a given index. </summary>
		/// <param name="node"> The node to add to the NodeSet. </param>
		/// <param name="index"> The position where to add the node 
		/// (negative values can be used for reverse access). </param>
		public void Add(Node node, int index = -1)
		{
			// Check the given parameter
			if (node == null) throw new ArgumentNullException("node");

			// Allow reverse access
			if (index < 0) index += _Values.Count + 1;

			// Get the name of the node
			string name = (_Parent == null) ? node.Path.LastPart : node.Name;

			// TODO allow overriding 
			if (_Keys.Contains(name)) throw new Exception(
				"Duplicated name \"" + name + "\" in " + _Node.Path);

			// Add the node to the collections
			_Keys.Insert(index, name); _Values.Insert(index, node);

			// Add the node to the parent NodeSet
			if (_Parent != null)
			{
				// TODO Make sure the order is maintained
				if (!_Parent.Contains(node.Path.LastPart)) _Parent.Add(node);
			}
			else
			{
				//if (node.Type.Name != null && !Contains(node.Type, name))
				//	_Children[node.Type].Add(node);
			}
			//Tools.Debug.LogInfo("ADDED " + name);
		}


		/// <summary> Changes the position of a Node in the NodeSet. </summary>
		/// <param name="oldIndex"> The index of the Node to move 
		/// (negative values can be used for reverse access). </param>
		/// <param name="newIndex"> The new index of the Node
		/// (negative values can be used for reverse access). </param>
		public void Move(int oldIndex, int newIndex = -1)
		{ }


		/// <summary> Changes the position of a Node in the NodeSet. </summary>
		/// <param name="name"> The name of the Node to move. </param>
		/// <param name="index"> The new index of the Node
		/// (negative values can be used for reverse access). </param>
		public void Move(string name, int index = -1)
		{ }


		/// <summary> Changes the position of a Node in the NodeSet. </summary>
		/// <param name="node"> The Node to move. </param>
		/// <param name="index"> The new index of the Node
		/// (negative values can be used for reverse access). </param>
		public void Move(Node node, int index = -1)
		{ }


		/// <summary> Removes a Node from the NodeSet. </summary>
		/// <param name="index"> The index of the Node to remove
		/// (negative values can be used for reverse access). </param>
		public void Remove(int index = -1)
		{
			// Allow reverse access
			if (index < 0) index += _Values.Count + 1;

			// Make sure the index is in range
			if (index < 0 || index >= _Values.Count)
				throw new ArgumentOutOfRangeException("index");

			// Remove the node from the parent NodeSet
			if (_Parent != null) { _Parent.Remove(_Values[index]); }

			// Remove the node from the collections
			_Values.RemoveAt(index); _Values.RemoveAt(index);
		}


		/// <summary> Removes a Node from the NodeSet. </summary>
		/// <param name="name"> The name of the Node to remove. </param>
		public void Remove(string name)
		{
			// Check the given values
			if (name == null) throw new ArgumentNullException("name");
			if (!_Keys.Contains(name)) throw new KeyNotFoundException();

			// Find the index of the node
			int index = _Keys.IndexOf(name);
			if (index < 0) throw new KeyNotFoundException();

			// Remove the node by its index
			Remove(index);
		}


		/// <summary> Removes a Node from the NodeSet. </summary>
		/// <param name="node"> The Node to remove. </param>
		public void Remove(Node node)
		{
			// Check the given values
			if (node == null) throw new ArgumentNullException("node");
			if (!_Values.Contains(node)) throw new KeyNotFoundException();

			// Find the index of the node
			int index = _Values.IndexOf(node);
			if (index < 0) throw new KeyNotFoundException();

			// Remove the node by its index
			Remove(index);
		}


		/// <summary> Removes all Nodes from the NodeSet. </summary>
		public void Clear()
		{
			// Remove the nodes in the parent NodeSet
			if (_Parent != null)
				foreach (Node node in _Values)
					_Parent.Remove(node);

			// Clean the encapsulated collections
			_Keys.Clear(); _Values.Clear();
		}

		#endregion

		#region IEnumeration Implementation

		/// <summary> Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns> An IEnumerator object that can be used to iterate through 
		/// the collection. </returns>
		public IEnumerator<Node> GetEnumerator()
		{ return _Values.GetEnumerator(); }


		/// <summary> Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns> An IEnumerator object that can be used to iterate through 
		/// the collection. </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{ return _Values.GetEnumerator(); }

		#endregion
	}
}