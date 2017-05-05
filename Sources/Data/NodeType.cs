using System;

namespace OpenUIX.Data
{
	/// <summary> Describes the type of the node. </summary>
	public sealed class NodeType
	{
		#region Public Enumerations

		/// <summary> Gets or sets the name of the NodeType </summary>
		public enum Modifiers 
		{
			/// <summary> Abstract type. </summary>
			Abstract
		}

		#endregion

		#region Public Properties

		/// <summary> Gets or sets the name of the NodeType </summary>
		public string Name { get; private set; }

		/// <summary> Gets or sets the inner type of the NodeType </summary>
		public Type InnerType { get; private set; }

		/// <summary> Gets or sets the ids of the nodes </summary>
		public string[] Extends { get; private set; }

		/// <summary> Gets or sets the ids of the nodes </summary>
		public string Modifier { get; private set; }

		#endregion

		#region Public Constructor

		/// <summary> Initializes an instance of the NodeType class. </summary>
		/// <param name="name"> The name of the NodeType. </param>
		/// <param name="innerType"> The inner Type of the NodeType. </param>
		public NodeType(string name, System.Type innerType = null)
		{
			// Check the given values
			if (name == null) throw new ArgumentNullException("name");
			if (innerType == null) throw new ArgumentNullException("innerType");

			// Store the given values
			Name = name; InnerType = innerType;
		}

		#endregion

		#region Public Methods

		/// <summary> Determines whether the specified object is equal to the 
		/// current object. </summary>
		/// <param name="obj"> The object to compare with the current object. 
		/// </param>
		/// <returns> true if the specified object is equal to the current 
		/// object; otherwise, false. </returns>
		public override bool Equals(object obj)
		{
			// Check the object type
			if (obj.GetType() != GetType()) return false;

			// Type names must be unique, so that we can compare them
			return ((Type)obj).Name == Name;
		}

		/// <summary> Serves as a hash function for a particular type.</summary>
		/// <returns> A hash code for the current Object. </returns>
		public override int GetHashCode() { return base.GetHashCode(); }

		#endregion
	}
}
