using System;
using OpenUIX.Data.Collections;

namespace OpenUIX
{
	/// <summary> Manages the OpenUIX Framework. </summary>
	public sealed class Document
	{
		#region Private Fields

		// IDENTIFICATION AND ACCESS MECHANISMS

		/// <summary> The name of the Document. </summary>
		private string _Name;

		/// <summary> The URI (Universal Resource Identifier) of the Document.
		/// </summary>
		private Uri _Uri;

		// HIERARCHICAL STRUCTURE

		/// <summary> The parent Document instance. </summary>
		private Document _Parent;

		/// <summary> The child Document instances. </summary>
		private Dictionary<string, Document> _Children;


		// INTERNAL REFERENCES

		/// <summary> The associated Manager reference. </summary>
		private Manager _Manager;

		/// <summary> The associated Node references. </summary>
		private NodeSet _Nodes;

		#endregion

		#region Public Properties

		// IDENTIFICATION AND ACCESS MECHANISMS

		/// <summary> Gets or sets the name of the Document. </summary>
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

				// Check the parent document first
				if (_Parent != null && _Parent._Children.ContainsKey(value))
					throw new Exception("Duplicated OpenUIX Document name" +
						"'" + value + "'");

				// Save the value as the new name of the document
				_Name = value;
			}
		}


		/// <summary> Gets or sets the URI of the Document. </summary>
		public Uri Uri
		{
			get { return _Uri; }
			set
			{
				// Save the value as the new URI of the document
				_Uri = value;
			}
		}


		// HIERARCHICAL STRUCTURE

		/// <summary> Gets or sets the parent Document instance. </summary>
		public Document Parent
		{
			get { return _Parent; }
			set
			{
				// Check the given value
				if (value == null) throw new NullReferenceException();

				// Check the old parent document
				if (_Parent != null) _Parent._Children.Remove(_Name);

				// Check the new parent document
				if (value != null && value._Children.ContainsKey(_Name))
					throw new Exception("Duplicated OpenUIX Document name" +
						"'" + value + "'");

				// Save the value as the new parent of the document
				_Parent = value;
			}
		}


		// INTERNAL REFERENCES

		/// <summary> Gets or sets the associated Manager reference. </summary>
		public Manager Manager
		{
			get { return _Manager; }
		}

		/// <summary> Gets the associated Node references. </summary>
		public NodeSet Nodes { get { return _Nodes; } }


		#endregion

		#region Public Constructor

		/// <summary> Initializes a new instance of the Document class.
		/// </summary>
		/// <param name="name"> The name of the Document. </param>
		/// <param name="uri"> The URI of the Document. </param>
		/// <param name="parent"> The parent Document instance. </param>
		public Document(string name, string uri, Document parent = null)
		{
			Name = name; Parent = parent;
			_Children = new Dictionary<string, Document>();

			_Nodes = new NodeSet();
		}

		#endregion
	}
}