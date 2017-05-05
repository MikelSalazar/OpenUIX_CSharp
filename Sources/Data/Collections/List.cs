namespace OpenUIX.Data.Collections
{
	/// <summary> Defines a typed collection of elements. </summary>
	public class List<ValueType> : System.Collections.Generic.List<ValueType>
	{
		#region Public Constructors

		/// <summary> Initializes a new instance of the List class. </summary>
		public List() : base()
		{ }

		/// <summary> Initializes a new instance of the List class that 
		/// contains elements copied from the specified collection. </summary>
		/// <param name="values"> The collection whose elements are copied
		/// to the new list. </param>
		public List(System.Collections.Generic.IEnumerable<ValueType> values) 
			: base(values)
		{ }

		#endregion
	}
}