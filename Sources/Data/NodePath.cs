using System;
using OpenUIX.Data.Collections;

namespace OpenUIX.Data
{
	/// <summary> Simplifies the navigation through the node hierarchy. 
	/// </summary>
	/// <remarks> Similar to XPath and URI mechanisms. </remarks>
	public sealed class NodePath
	{
		#region Public Constants

		/// <summary> The separator for NodePath items. </summary>
		public const char ITEM_SEPARATOR = '/';

		/// <summary> The separator for node types. </summary>
		public const char TYPE_SEPARATOR = '.';

		/// <summary> The strings associated to the relations. </summary>
		public static readonly string[] RELATION_STRINGS = { "", "/",
			"..", "{child}", "{ancestor}", "{descendant}" };

		#endregion

		#region Public Enumeration

		/// <summary> The different types of relation. </summary>
		public enum Relation
		{
			/// <summary> No relation. </summary>
			none,
			
			/// <summary> Root Node. </summary>
			root,
			
			/// <summary> Parent Node. </summary>
			parent,
			
			/// <summary> Child Node. </summary>
			child,

			/// <summary> Ancestor Node. </summary>
			ancestor,

			/// <summary> Descendant Node. </summary>
			descendant
		};

		#endregion

		#region Public Subclasses

		/// <summary> Defines an Item of a NodePath. </summary>
		public struct Item
		{
			/// <summary> The Relation of the NodePath Item. </summary>
			public Relation Relation;

			/// <summary> The type of the node. </summary>
			public NodeType Type;

			/// <summary> The name of the node. </summary>
			public string Name;

			/// <summary> The predicate of the NodePath Item. </summary>
			public string Predicate;

			/// <summary> Initializes an instance of the NodePath.Item 
			/// structure. </summary>
			public Item(Relation relation = Relation.none)
			{ Type = null; Name = null; Relation = relation; Predicate = null; }

			/// <summary> Initializes an instance of the NodePath.Item 
			/// structure. </summary>
			public Item(Relation relation = Relation.none, NodeType type = null,
				string name = null, string predicate = null)
			{
				Type = type; Name = name;
				Relation = relation; Predicate = predicate;
			}

			/// <summary> Returns a string that represents the current object.
			/// </summary>
			/// <returns> A string that represents the current object. </returns>
			public override string ToString()
			{
				if (Relation != Relation.none)
					return RELATION_STRINGS[(int)Relation];
				else return Type.Name + TYPE_SEPARATOR + Name;
			}
		}

		#endregion

		#region Private Fields

		/// <summary> The list of items that compose the path. </summary>
		private List<Item> _Items = new List<Item>();

		#endregion

		#region Public Properties

		/// <summary> Gets the list of items that compose the path. </summary>
		public Item[] Items { get { return _Items.ToArray(); } }

		/// <summary> Gets the last item of the path. </summary>
		public string LastPart { get { return _Items[_Items.Count - 1].ToString(); } }

		#endregion

		#region Public Constructor

		/// <summary> Initializes a new instance of the NodePath class.
		/// </summary>
		/// <param name="path"> The string with the NodePath data. </param>
		public NodePath(string path) { FromString(path); }


		/// <summary> Initializes a new instance of the NodePath class.
		/// </summary>
		/// <param name="node"> The node . </param>
		public NodePath(Node node)
		{
			// Check the given parameter
			if (node == null) throw new ArgumentNullException("node");

			_Items.Add(new Item(Relation.root));

			// Travel upwards through the hierarchy creating the path
			Node searchNode = node;
			while (searchNode.Parent != null)
			{
				_Items.Insert(1, new Item(Relation.none,
					searchNode.Type, searchNode.Name));
				searchNode = searchNode.Parent;
			}
		}

		/// <summary> Initializes a new instance of the NodePath class.
		/// </summary>
		/// <param name="relation"></param>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <param name="predicate"></param>
		public NodePath(Relation relation = Relation.none, NodeType type = null,
			string name = null, string predicate = null)
		{ _Items.Add(new Item(relation, type, name, predicate)); }

		#endregion

		#region Public Methods

		/// <summary> Retrieves the object data from a string representation.
		/// </summary>
		/// <param name="path"> The string with the object data. </param>
		public void FromString(string path)
		{

		}

		/// <summary> Returns a string that represents the current object.
		/// </summary>
		/// <returns> A string that represents the current object. </returns>
		public override string ToString()
		{
			// The path string starts
			string path = "";

			// Convert each item to its string representation
			for (int itemIndex = 0; itemIndex < _Items.Count; itemIndex++)
			{
			}

			// Return the resulting path string
			return path;
		}


		/// <summary> Looks for a node with a given NodePath. </summary>
		/// <typeparam name="NodeType"> The type of the node. </typeparam>
		/// <param name="origin"> The origin for the search. </param>
		/// <param name="path"> The NodePath to use in the search. </param>
		/// <param name="recursion"> The maximum recursion level (negative
		/// values can be used to indicate unlimited recursion). </param>
		/// <returns> The node with the given NodePath (or null, it no one
		/// was found). </returns>
		public static NodeType FindNode<NodeType>(Node origin,
			NodePath path, int recursion = -1) where NodeType : Node
		{
			NodeType[] nodes = FindNodes<NodeType>(origin, path, recursion, 1);
			return nodes.Length != 0 ? nodes[0] : null;
		}


		/// <summary> Looks for a node with a given NodePath. </summary>
		/// <typeparam name="NodeType"> The type of the node. </typeparam>
		/// <param name="origin"> The origin for the search. </param>
		/// <param name="path"> The NodePath to use in the search. </param>
		/// <param name="recursion"> The maximum recursion level (negative
		/// values can be used to indicate unlimited recursion). </param>
		/// <param name="maxResults"> The maximum number of resulting nodes
		/// (negative values indicate that there is no limit). </param>
		/// <returns> The node with the given NodePath (or null, it no one
		/// was found). </returns>
		public static NodeType[] FindNodes<NodeType>(Node origin,
			NodePath path, int recursion = -1, int maxResults = -1)
			where NodeType : Node
		{
			// Check the given parameters
			if (origin == null) throw new ArgumentNullException("origin"); ;
			if (path == null) throw new ArgumentNullException("path"); ;

			// Create a list of nodes to store the result
			List<NodeType> nodes = new List<NodeType>();

			//	// Look for the next node ID separator in the path
			//	string id, type, name, nextPath;
			//	int nextId = path.IndexOf(ID_SEPARATOR);
			//	if (nextId < 0) { id = path; path = null; }
			//	else {
			//		id = path.Substring(0, nextId);
			//		path = path.Substring(nextId+1);
			//	}

			//	if (path != null)
			//	{

			//	}

			//	UnityEngine.Debug.Log("Test: '" + id + "' -> '" + path + "'");

			//	//// Extract the path axis from the NodePath
			//	//int pathAxis = 0;
			//	//if (path.StartsWith(ANCESTOR_AXIS))
			//	//{ pathAxis = -1; path = path.Substring(ANCESTOR_AXIS.Length + 1); }
			//	//if (path.StartsWith(DESCENDANT_AXIS))
			//	//{ pathAxis = 1; path = path.Substring(DESCENDANT_AXIS.Length + 1); }

			//	//string[] nodeIds = path.Split(ID_SEPARATOR);

			//	//if (nodes.Count == 0) throw new ArgumentNullException("path");

			// Return the resulting list of nodes as an array
			return nodes.ToArray();
		}

		#endregion

		#region Public Operators

		/// <summary> Converts a string into a NodePath. </summary>
		/// <param name="path"> The string with the NodePath data. </param>
		/// <returns> A NodePath instance with the given path data. </returns>
		public static implicit operator NodePath(string path)
		{ return new NodePath(path); }


		/// <summary> Converts a NodePath into an string. </summary>
		/// <param name="path"> The NodePath instance to convert. </param>
		/// <returns> A string with the NodePath data. </returns>
		public static implicit operator string(NodePath path)
		{ return path.ToString(); }

		#endregion
	}
}
