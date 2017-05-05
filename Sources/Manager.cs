using System;
using OpenUIX.Data.Collections;

namespace OpenUIX
{
	/// <summary> Manages the OpenUIX framework. </summary>
	public sealed class Manager
	{
		#region Private Fields

		/// <summary> The list of Manager instances. </summary>
		private static List<Manager> _Instances = new List<Manager>();

		#endregion

		#region Public Properties

		/// <summary> Gets or sets the root document. </summary>
		public Document Root { get; private set; }

		#endregion

		#region Public Constructor

		/// <summary> Initializes a new instance of the Manager class.</summary>
		/// <param name="root"> The root OpenUIX Document. </param>
		public Manager(Document root = null)
		{
			// Save the given value
			Root = root;

			// Store the new instance in the list
			if (_Instances == null) _Instances = new List<Manager>();
			_Instances.Add(this);
		}

		#endregion

		#region Public Methods

		/// <summary> Verifies that a string is a valid name (That only contains
		/// ASCII letters, numbers and slashes, and that it doesn't start with a
		/// number). </summary>
		/// <param name="name"> The name to validate. </param>
		/// <returns> true if the name is valid, false otherwise. </returns>
		public static bool IsValidName(string name)
		{
			// Try to validate the name
			try { ValidateName(name); }

			// If it fails, create a debug message and quit
			catch (Exception exception)
			{
				//if (Debug.Verbose == Debug.VerbosityLevel.Complete)
				//	Debug.LogWarning(exception.Message);
				return false;
			}

			// Otherwise, return true to indicate the name is valid
			return true;
		}


		/// <summary> Verifies that a string is a valid name (That only contains
		/// ASCII letters, numbers and slashes, and that it doesn't start with a
		/// number). </summary>
		/// <param name="name"> The name to validate. </param>
		/// <remarks> In case the validation process fails, it will throw an 
		/// exception indicating the first invalid character. </remarks>
		public static void ValidateName(string name)
		{
			// Check if the string is null or empty
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException();

			// Travel through the character
			for (int charIndex = 0; charIndex < name.Length; charIndex++)
			{
				// Get the current character
				char currentChar = name[charIndex];

				// Check if it is a valid character
				if (!((currentChar == '-') || (currentChar == '_') ||
					((currentChar >= '0') && (currentChar <= '9')) ||
					((currentChar >= 'A') && (currentChar <= 'z'))))
					throw new Exception("Invalid character: '" + currentChar +
						"' in char " + charIndex + "of name \"" + name + "\"");

				// Make sure the name does not start with a number
				if (charIndex == 0 && char.IsDigit(currentChar))
					throw new Exception("Invalid name \"" + name + "\"" +
						" because it starts with a number");
			}
		}

		#endregion
	}
}