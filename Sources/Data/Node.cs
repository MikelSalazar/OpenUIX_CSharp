using System;
using OpenUIX.Data.Collections;

namespace OpenUIX.Data
{
	/// <summary> Defines the basic node of the OpenUIX Data Model. </summary>
	public abstract class Node
	{
		#region Public events

		/// <summary> An event handler to manage the status changes of the
		/// nodes.</summary>
		public delegate void EventHandler(Node node);

		/// <summary> Raised when a node is being created. </summary>
		public static event EventHandler Creating;

		/// <summary> Raised when a update operation is requested. </summary>
		public static event EventHandler UpdateRequested;

		/// <summary> Raised when a update operation is performed. </summary>
		public static event EventHandler UpdatePerformed;

		/// <summary> Raised when a node is about to be destroyed. </summary>
		public static event EventHandler Destroying;

		#endregion

		#region Private Fields

		// IDENTIFICATION AND ACCESS MECHANISMS

		/// <summary> The local name of the Node. </summary>
		private string _Name;

		/// <summary> The identifier (global name) of the Node. </summary>
		private string _Id;


		// POO-STYLE MECHANISMS

		/// <summary> The type data of the node. </summary>
		private NodeType _Type;


		// HIERARCHICAL STRUCTURE

		/// <summary> The parent instance of the Node. </summary>
		private Node _Parent = null;

		/// <summary> The child instances the Node. </summary>
		private NodeSet _Children = new NodeSet();


		// INTERNAL REFERENCES

		/// <summary> The Document instance associated with the Node. </summary>
		private Document _Document;


		// CONTEXT

		/// <summary> The a. </summary>
		private NodePath _Access;

		/// <summary> . </summary>
		private NodePath _Shared;



		/// <summary> Indicates whether the Node is updated or not. </summary>
		private bool _Updated = false;

		/// <summary> The number of times the Node has been updated. </summary>
		private long _UpdateCount = 0;

		/// <summary> The last time the node was updated. </summary>
		private DateTime _UpdateTime = DateTime.MinValue;

		#endregion

		#region Public Properties

		// IDENTIFICATION AND ACCESS MECHANISMS

		/// <summary> Gets or sets the name of the Node. </summary>
		public string Name
		{
			get { return _Name; }
			set
			{
				// Check the given value
				if (value == null) throw new NullReferenceException();
				try { Manager.ValidateName(value); }
				catch (Exception e)
				{ throw new Exception("Invalid OpenUIX Document name", e); }

				// Check the parent node first
				if (_Parent != null && _Parent._Children.Contains(value))
					throw new Exception("Duplicated OpenUIX Node name" +
						"'" + value + "'");

				// Save the value as the new name of the node
				_Name = value;
			}
		}


		/// <summary> Gets or sets the identifier (global name) of the Node.
		/// </summary>
		public string Id
		{
			get { return _Id; }
			set
			{
				// TODO Check the given value
				//if (value == null) ;
				//else

				// Save the value as the new name of the node
				_Id = value;
			}
		}


		/// <summary> Gets or sets the Path of the Node. </summary>
		public NodePath Path { get { return new NodePath(this); } }


		// POO-STYLE MECHANISMS

		/// <summary> Gets the type data of the node. </summary>
		public NodeType Type { get { return _Type;  } }


		// HIERARCHICAL STRUCTURE

		/// <summary> The parent instance of the Node. </summary>
		public Node Parent
		{
			get { return _Parent; }
			set
			{
				// Check if the node has a parent
				if (_Parent != null) _Parent._Children.Remove(this);

				// Set the new value
				_Parent = value;

				// Add the element to the node to the parent child list
				if (_Parent != null) _Parent._Children.Add(this);
			}
		}


		/// <summary> The child instances the Node. </summary>
		public NodeSet Children { get { return _Children; } }


		// INTERNAL REFERENCES

		/// <summary> Gets or sets the Document instance associated with the 
		/// Node. </summary>
		public Document Document
		{
			get { return _Document; }
			set
			{
				// Clean the previous reference
				if (_Document != null) _Document.Nodes.Remove(this);

				// Set the new value
				_Document = value;

				// Establish the new reference
				if (_Document != null) _Document.Nodes.Add(this);
			}
		}


		// CONTEXT

		/// <summary> Indicates whether the Node is updated or not. </summary>
		public bool Updated { get { return Updated; }
}

		/// <summary> Gets the number of times the Node has been updated.
		/// </summary>
		public long UpdateCount { get { return _UpdateCount; } }


		/// <summary> Gets the last time the node was updated. </summary>
		private DateTime UpdateTime { get { return _UpdateTime; } }


		#endregion

		#region Public Constructor

		/// <summary> Initializes a new instance of the Node. </summary>
		/// <param name="name"> The Name of the Node. </param>
		/// <param name="parent"> The parent of the Node. </param>
		/// <param name="type"> The type of the Node. </param>
		public Node(string name, Node parent = null, NodeType type = null)
		{
			// Check the given values and store them
			if ((name == null) || !Manager.IsValidName(name))
				throw new System.Exception("Invalid node name: '" + name + "'");
			if (type == null) type = new NodeType(null, GetType());
			Name = name; _Parent = parent; _Type = type;

			// Initialize the nodeset with the children
			_Children = new NodeSet();

			// If there is a parent, add this node as a child
			if (Parent != null) Parent.Children.Add(this);

			// Mark the node as "not updated"
			_Updated = false;

			// Raise the Creating event
			if (Creating != null) Creating(this);
		}

		#endregion

		#region Public Methods

		/// <summary> Updates the node. </summary>
		/// <param name="forced"> Indicates whether to force the update
		/// operation or not. </param>
		public void Update(bool forced = false)
		{
			// Check if it is already updated
			if (_Updated && !forced) return;

			// Propagate the node Downwards in the node hierarchy
			foreach (Node child in _Children) child.Update(forced);

			// Mark the node as updated
			_Updated = true;

			// Send the update event
			if (UpdatePerformed != null) UpdatePerformed(this);
		}

		#endregion
	}
}
