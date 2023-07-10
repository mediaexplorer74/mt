using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using ReLogic.Graphics;
using GameManager.GameContent;
using GameManager.Utilities;

namespace GameManager.Localization
{
	public class LanguageManager
	{
		public static LanguageManager Instance = new LanguageManager();

		private readonly Dictionary<string, LocalizedText> _localizedTexts = new Dictionary<string, LocalizedText>();

		private readonly Dictionary<string, List<string>> _categoryGroupedKeys = new Dictionary<string, List<string>>();

		private GameCulture _fallbackCulture = GameCulture.DefaultCulture;

		public GameCulture ActiveCulture
		{
			get;
			private set;
		}

		public event LanguageChangeCallback OnLanguageChanging;

		public event LanguageChangeCallback OnLanguageChanged;

		private LanguageManager()
		{
			_localizedTexts[""] = LocalizedText.Empty;
		}

		public int GetCategorySize(string name)
		{
			if (_categoryGroupedKeys.ContainsKey(name))
			{
				return _categoryGroupedKeys[name].Count;
			}
			return 0;
		}

		public void SetLanguage(int legacyId)
		{
			GameCulture language = GameCulture.FromLegacyId(legacyId);
			SetLanguage(language);
		}

		public void SetLanguage(string cultureName)
		{
			GameCulture language = GameCulture.FromName(cultureName);
			SetLanguage(language);
		}

		private void SetAllTextValuesToKeys()
		{
			foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
			{
				localizedText.Value.SetValue(localizedText.Key);
			}
		}

		private string[] GetLanguageFilesForCulture(GameCulture culture)
		{
			Assembly.GetExecutingAssembly();
			return Array.FindAll(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.StartsWith("Terraria.Localization.Content." + culture.CultureInfo.Name) && element.EndsWith(".json"));
		}

		public void SetLanguage(GameCulture culture)
		{
			if (ActiveCulture != culture)
			{
				if (culture != _fallbackCulture && ActiveCulture != _fallbackCulture)
				{
					SetAllTextValuesToKeys();
					LoadLanguage(_fallbackCulture);
				}
				LoadLanguage(culture);
				ActiveCulture = culture;
				Thread.CurrentThread.CurrentCulture = culture.CultureInfo;
				Thread.CurrentThread.CurrentUICulture = culture.CultureInfo;
				if (this.OnLanguageChanged != null)
				{
					this.OnLanguageChanged(this);
				}
				_ = FontAssets.DeathText;
			}
		}

		private void LoadLanguage(GameCulture culture)
		{
			LoadFilesForCulture(culture);
			if (this.OnLanguageChanging != null)
			{
				this.OnLanguageChanging(this);
			}
			ProcessCopyCommandsInTexts();
		}

		private void LoadFilesForCulture(GameCulture culture)
		{
			string[] languageFilesForCulture = GetLanguageFilesForCulture(culture);
			foreach (string text in languageFilesForCulture)
			{
				try
				{
					string text2 = Utils.ReadEmbeddedResource(text);
					if (text2 == null || text2.Length < 2)
					{
						//throw new FileFormatException();
					}
					LoadLanguageFromFileText(text2);
				}
				catch (Exception)
				{
					if (Debugger.IsAttached)
					{
						Debugger.Break();
					}
					Console.WriteLine("Failed to load language file: " + text);
					return;
				}
			}
		}

		private void ProcessCopyCommandsInTexts()
		{
			Regex regex = new Regex("{\\$(\\w+\\.\\w+)}", RegexOptions.Compiled);
			foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
			{
				LocalizedText value = localizedText.Value;
				for (int i = 0; i < 100; i++)
				{
					string text = regex.Replace(value.Value, (Match match) => GetTextValue(match.Groups[1].ToString()));
					if (text == value.Value)
					{
						break;
					}
					value.SetValue(text);
				}
			}
		}

		public void LoadLanguageFromFileText(string fileText)
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> item in JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(fileText))
			{
				_ = item.Key;
				foreach (KeyValuePair<string, string> item2 in item.Value)
				{
					string key = item.Key + "." + item2.Key;
					if (_localizedTexts.ContainsKey(key))
					{
						_localizedTexts[key].SetValue(item2.Value);
						continue;
					}
					_localizedTexts.Add(key, new LocalizedText(key, item2.Value));
					if (!_categoryGroupedKeys.ContainsKey(item.Key))
					{
						_categoryGroupedKeys.Add(item.Key, new List<string>());
					}
					_categoryGroupedKeys[item.Key].Add(item2.Key);
				}
			}
		}

		[Conditional("DEBUG")]
		private void ValidateAllCharactersContainedInFont(DynamicSpriteFont font)
		{
			if (font == null)
			{
				return;
			}
			foreach (LocalizedText value2 in _localizedTexts.Values)
			{
				string value = value2.Value;
				for (int i = 0; i < value.Length; i++)
				{
					_ = value[i];
				}
			}
		}

		public LocalizedText[] FindAll(Regex regex)
		{
			int num = 0;
			foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
			{
				if (regex.IsMatch(localizedText.Key))
				{
					num++;
				}
			}
			LocalizedText[] array = new LocalizedText[num];
			int num2 = 0;
			foreach (KeyValuePair<string, LocalizedText> localizedText2 in _localizedTexts)
			{
				if (regex.IsMatch(localizedText2.Key))
				{
					array[num2] = localizedText2.Value;
					num2++;
				}
			}
			return array;
		}

		public LocalizedText[] FindAll(LanguageSearchFilter filter)
		{
			LinkedList<LocalizedText> linkedList = new LinkedList<LocalizedText>();
			foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
			{
				if (filter(localizedText.Key, localizedText.Value))
				{
					linkedList.AddLast(localizedText.Value);
				}
			}
			return linkedList.ToArray();
		}

		public LocalizedText SelectRandom(LanguageSearchFilter filter, UnifiedRandom random = null)
		{
			int num = 0;
			foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
			{
				if (filter(localizedText.Key, localizedText.Value))
				{
					num++;
				}
			}
			int num2 = (random ?? Main.rand).Next(num);
			foreach (KeyValuePair<string, LocalizedText> localizedText2 in _localizedTexts)
			{
				if (filter(localizedText2.Key, localizedText2.Value) && --num == num2)
				{
					return localizedText2.Value;
				}
			}
			return LocalizedText.Empty;
		}

		public LocalizedText RandomFromCategory(string categoryName, UnifiedRandom random = null)
		{
			if (!_categoryGroupedKeys.ContainsKey(categoryName))
			{
				return new LocalizedText(categoryName + ".RANDOM", categoryName + ".RANDOM");
			}
			List<string> list = _categoryGroupedKeys[categoryName];
			return GetText(categoryName + "." + list[(random ?? Main.rand).Next(list.Count)]);
		}

		public bool Exists(string key)
		{
			return _localizedTexts.ContainsKey(key);
		}

		public LocalizedText GetText(string key)
		{
			if (!_localizedTexts.ContainsKey(key))
			{
				return new LocalizedText(key, key);
			}
			return _localizedTexts[key];
		}

		public string GetTextValue(string key)
		{
			if (_localizedTexts.ContainsKey(key))
			{
				return _localizedTexts[key].Value;
			}
			return key;
		}

		public string GetTextValue(string key, object arg0)
		{
			if (_localizedTexts.ContainsKey(key))
			{
				return _localizedTexts[key].Format(arg0);
			}
			return key;
		}

		public string GetTextValue(string key, object arg0, object arg1)
		{
			if (_localizedTexts.ContainsKey(key))
			{
				return _localizedTexts[key].Format(arg0, arg1);
			}
			return key;
		}

		public string GetTextValue(string key, object arg0, object arg1, object arg2)
		{
			if (_localizedTexts.ContainsKey(key))
			{
				return _localizedTexts[key].Format(arg0, arg1, arg2);
			}
			return key;
		}

		public string GetTextValue(string key, params object[] args)
		{
			if (_localizedTexts.ContainsKey(key))
			{
				return _localizedTexts[key].Format(args);
			}
			return key;
		}

		public void SetFallbackCulture(GameCulture culture)
		{
			_fallbackCulture = culture;
		}
	}
}
