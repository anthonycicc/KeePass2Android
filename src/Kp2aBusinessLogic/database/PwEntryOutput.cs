using System;
using KeePass.Util.Spr;
using KeePassLib;
using KeePassLib.Collections;
using KeePassLib.Security;

namespace keepass2android
{
	/// <summary>
	/// Represents the strings which are output from a PwEntry.
	/// </summary>
	/// In contrast to the original PwEntry, this means that placeholders are replaced. Also, plugins may modify
	/// or add fields.
	public class PwEntryOutput
	{
		private readonly PwEntry _entry;
		private readonly PwDatabase _db;
		private readonly ProtectedStringDictionary _outputStrings = new ProtectedStringDictionary();

		/// <summary>
		/// Constructs the PwEntryOutput by replacing the placeholders
		/// </summary>
		public PwEntryOutput(PwEntry entry, PwDatabase db)
		{
			_entry = entry;
			_db = db;

			foreach (var pair in entry.Strings)
			{
				_outputStrings.Set(pair.Key, new ProtectedString(entry.Strings.Get(pair.Key).IsProtected, GetStringAndReplacePlaceholders(pair.Key)));
			}
		}

		string GetStringAndReplacePlaceholders(string key)
		{
			String value = Entry.Strings.ReadSafe(key);
			value = SprEngine.Compile(value, new SprContext(Entry, _db, SprCompileFlags.All));
			return value;
		}


		/// <summary>
		/// Returns the ID of the entry
		/// </summary>
		public PwUuid Uuid 
		{
			get { return Entry.Uuid; }
		}

		/// <summary>
		/// The output strings for the represented entry
		/// </summary>
		public ProtectedStringDictionary OutputStrings { get { return _outputStrings; } }

		public PwEntry Entry
		{
			get { return _entry; }
		}
	}
}