
using System.Collections.Generic;
using System;
using System.Text;

namespace Sid
{
	public static partial class Str
	{
		// Given a string, returns an object of the converted type
		public static object ConvertStringToTypedObject(string valueAsString)
		{
			object value;
			// Bool? (False/True)
			bool valueAsBool;
			if (bool.TryParse(valueAsString, out valueAsBool))
			{
				value = valueAsBool;
			}
			else
			{
				// If it parses as a double, it could also be an int
				// An int can be converted upwards to a double, but you can't cast a double down to an
				//  int because you would lose the fraction
				// Double?
				double valueAsDouble;
				if (double.TryParse(valueAsString, out valueAsDouble))
				{
					// Int?
					int valueAsInt;
					if (int.TryParse(valueAsString, out valueAsInt))
					{
						// Int.
						value = valueAsInt;
					}
					else
					{
						// Double.
						value = valueAsDouble;
					}
				}
				else
				{
					// String or something that isn't double, int or bool
					value = valueAsString;
				}
			}
			return value;
		}

		// Attempts to parse the object as a bool and if it fails returns the default value
		public static bool TryParseDefault(
		 object obj,
		 bool defaultValue)
		{
			bool rv = defaultValue;
			if (obj != null)
			{
				if (obj is bool)
				{
					rv = (bool)obj;
				}
				else
				{
					if (!bool.TryParse(obj.ToString(), out rv))
					{
						rv = defaultValue;
					}
				}
			}
			return rv;
		}

		public static string EscapeTextForJavascript(string text)
		{
			const string CrLf = "\r\n";
			string rv = text.Replace("\\", "\\\\");
			rv = rv.Replace(CrLf, "\\r\\n");
			rv = rv.Replace("\n", "\\n");
			rv = rv.Replace("'", "\\'");
			rv = rv.Replace("\"", "\\\"");
			return rv;
		}

		// Moved from globVars.cs to here
		public static string CleanString(string theString)
		{
			const string strAlphaNumeric = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
			string strChar, CleanedString = "";

			for (int i = 0; i < theString.Length; i++)
			{
				strChar = theString.Substring(i, 1);

				if (strAlphaNumeric.IndexOf(strChar) > -1)
				{
					CleanedString += strChar;
				}
			}

			return CleanedString;
		}

		// Find an array within an array
		// Unfortunately I can't find/don't know of inbuilt functionality that would do this better.
		// A .NET equivalent of the C++ 'memcmp' function would be much faster
		public static int PosOfArrayWithinArray(
		 byte[] haystack,
		 byte[] needle,
		 int startIndex,
		 int count)
		{
			for (int i = startIndex; (i - startIndex) < count; ++i)
			{
				int numElementsLeft = count - i;
				if (numElementsLeft < needle.Length)
				{
					break;
				}

				int j;
				int ii;
				for (ii = i, j = 0; j < needle.Length; ++ii, ++j)
				{
					char haystackChar = char.ToUpper((char)haystack[ii]);
					if (haystackChar != char.ToUpper((char)needle[j]))
					{
						break;
					}
				}
				if (j == needle.Length)
				{
					// Found it at position 'i'
					return i;
				}
			}
			// Not found
			return -1;
		}

		public static int FindSpace(
		 string text,
		 int startIndex)
		{
			for (int i = startIndex; i < text.Length; ++i)
			{
				if (char.IsWhiteSpace(text[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Used in the splitting functions
		// Return value of true means split
		public delegate bool SplitDelegate(char chr);

		public static bool IsComma(
		 char chr)
		{
			bool rv = (chr == ',');
			return rv;
		}

		static TokenAndPosition CreateTokenAndPosition(
		 string input,
		 int beginPos,
		 int endPos)
		{
			TokenAndPosition tok = new TokenAndPosition(
			 input.Substring(beginPos, endPos - beginPos), beginPos);
			return tok;
		}

		// To embed quotes use:
		// "test\"test" (\")
		// 'includeQuotes': if true, "one two three" will be returned as "one two three" i.e. with the quotes embedded
		// if false it will be returned as: one two three
		public static TokenAndPosition[] QuoteAwareSplit(
		 string input,
		 SplitDelegate splitDelegate,
		 bool includeQuotes)
		{
			// The output list of tokens
			List<TokenAndPosition> list = new List<TokenAndPosition>();

			// Are we inside quotes?
			bool insideQuotes = false;

			// The beginning position of the current token
			int beginPos = 0;

			// The position of the last non-whitespace character that doesn't match the delegate
			int lastNonWhitespacePos = 0;

			// Iterate the input string character by character
			for (int i = 0; i < input.Length; ++i)
			{
				char thisChar = input[i];

				// Update the last non-whitespace position if this isn't whitespace
				bool isWhitespace = char.IsWhiteSpace(input[i]);
				if (!isWhitespace)
				{
					lastNonWhitespacePos = i;
				}

				if (insideQuotes)
				{
					// Look for the ending quote
					if (thisChar == '\"' && input[i - 1] != '\\')
					{
						// Add this segment
						if (includeQuotes)
						{
							list.Add(CreateTokenAndPosition(input, beginPos - 1, i + 1));
						}
						else
						{
							list.Add(CreateTokenAndPosition(input, beginPos, i));
						}
						beginPos = i + 1;
						insideQuotes = false;
					}
				}
				else
				{
					if (thisChar == '\"')
					{
						insideQuotes = true;
						beginPos = i + 1;
					}
					else
					{
						if (splitDelegate(thisChar))
						{
							// Split here
							// This won't include any whitespace at the end of the token due to 'lastNonWhitespacePos'
							list.Add(CreateTokenAndPosition(input, beginPos, lastNonWhitespacePos + 1));

							beginPos = i + 1;
						}
					}
				}
			}

			if (insideQuotes)
			{
				// No ending quote
				string text = string.Format("Missing closing quote in input string: {0}", input);
				throw new System.Exception(text);
			}

			// Add remaining token/segment
			int endCurrentToken = lastNonWhitespacePos + 1;
			if (endCurrentToken > beginPos)
			{
				list.Add(CreateTokenAndPosition(input, beginPos, endCurrentToken));
			}
			return list.ToArray();
		}

		public static bool TokenListEquals(
		 TokenAndPosition[] tokenAndPositions,
		 string[] compare,
		 int startIndex,
		 StringComparison comparisonType)
		{
			if (compare.Length == 0)
			{
				throw new System.Exception("Software fault. compare.Length==0");
			}

			// Check the length first
			if (startIndex + compare.Length > tokenAndPositions.Length)
			{
				return false;
			}

			// Compare compare[i] against tokenAndPositions[i+startIndex]
			bool rv = true;
			for (int i = 0; i < compare.Length; ++i)
			{
				rv = string.Equals(tokenAndPositions[i + startIndex].Token, compare[i], comparisonType);
				if (!rv)
				{
					break;
				}
			}

			return rv;
		}

		public static bool SkipWhitespace(
		 string str,
		 ref int pos)
		{
			for (; pos < str.Length; ++pos)
			{
				if (!char.IsWhiteSpace(str[pos]))
				{
					return true;
				}
			}
			return false;
		}

		public static int FindClosingQuote(
		 int pos,
		 string str,
		 bool containsEmbeddedQuotes)
		{
			int rv = -1;
			if (containsEmbeddedQuotes)
			{
				for (int i = pos; i < str.Length;)
				{
					i = str.IndexOf('\"', i);
					bool finished;
					switch (i)
					{
						case -1:
							// No closing quote
							finished = true;
							break;

						case 0:
							// The first character is "
							finished = true;
							break;
						default:
							// Skip embedded quotes (\")
							finished = (str[i - 1] != '\\');
							break;
					}

					if (finished)
					{
						rv = i;
						break;
					}
					++i;
				}
			}
			else
			{
				// Untested
				rv = str.IndexOf('\"', pos);
			}

			return rv;
		}

		public static string Unescape(string str)
		{
			// When we add more escape sequences (\n) etc, then it would be quicker to search for '\\' within a loop
			string replaced;
			// Replace quotes
			replaced = str.Replace("\\\"", "\"");
			return replaced;
		}

		public delegate bool CharComparisonDel(
		 char lhs,
		 char rhs);

		public static bool CaseInsensitiveComp(
		 char lhs,
		 char rhs)
		{
			return (Char.ToUpper(lhs) == Char.ToUpper(rhs));
		}

		public static bool CaseSensitiveComp(
		 char lhs,
		 char rhs)
		{
			return (lhs == rhs);
		}

		// Parse e.g.:
		// test[1]
		// 'str' will be returned as 'test' and 1 will be returned
		public static bool ParseArrayNotation(
		 ref string str,
		 out string arrayIndex,
		 out string errorMsg)
		{
			// Is it an array?
			int arrayOpenIndex = str.IndexOf('[');
			bool rv;
			if (arrayOpenIndex == -1)
			{
				arrayIndex = null;
				errorMsg = "";
				rv = true;
			}
			else
			{
				// Find the ]
				int arrayCloseIndex = str.IndexOf(']', arrayOpenIndex + 1);
				if (arrayCloseIndex == -1)
				{
					errorMsg = "Invalid array. Opening bracket ([) without closing bracket (]) found.";
					rv = false;
					arrayIndex = null;
				}
				else
				{
					errorMsg = "";
					arrayIndex = str.Substring(arrayOpenIndex + 1, arrayCloseIndex - arrayOpenIndex - 1);

					// Update the string to not include the array
					str = str.Substring(0, arrayOpenIndex);
					rv = true;
				}
			}

			return rv;
		}

		// Returns the position of the closing character or -1 if not found/bad syntax
		public static int FindNestedOpenClose(
		 string str,
		 int beginPos,
		 char open,
		 char close)
		{
			// This assumes that 'beginPos' is after the first instance of 'open'. Hence why the depth begins at
			// 1 rather than 0.
			int currentDepth = 1;
			// Iterate 'str' beginning at 'beginPos'
			for (int i=beginPos;i<str.Length;++i)
			{
				char chr = str[i];
				if (chr == open)
				{
					++currentDepth;
				}
				else if(chr==close)
				{
					--currentDepth;
					if(currentDepth==0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		public static string ReplaceFormat(
			IDictionary<string, string> valuesMap,
			string format,
			char openChr='{',
			char closeChr='}')
		{
			var replaced = new StringBuilder();
			for (int i = 0; i < format.Length; ++i)
			{
				char chr = format[i];
				if (chr == openChr)
				{
					++i;
					if (format[i] == openChr)
					{
						// Escape sequence
						replaced.Append(chr);
					}
					else
					{
						int endPos = format.IndexOf(closeChr, i);
						if (endPos == -1)
						{
							throw new Exception($"Invalid format - {format}: missing {closeChr}");
						}

						string valueName = format.Substring(i, endPos-i);

						string value;
						if (!valuesMap.TryGetValue(valueName, out value))
						{
							throw new Exception($"Invalid format - {value} is missing from the values map.");
						}

						replaced.Append(value);

						// Continue from here.
						i = endPos;
					}
				}
				else
				{
					replaced.Append(chr);
				}
			}

			return replaced.ToString();
		}

		public static string FixStringForExport(string str)
		{
			str = str.Replace(",", "");
			str = str.Replace("\r", "");
			str = str.Replace("\n", " ");
			str = str.Replace("\t"," ");
			return str;
		}
	}

	public class TokenAndPosition
	{
		public string Token
		{
			get;
			private set;
		}

		public int Position
		{
			get;
			private set;
		}

		public TokenAndPosition(
		 string token,
		 int position)
		{
			Token = token;
			Position = position;
		}
	}
}