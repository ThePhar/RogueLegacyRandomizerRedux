using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.GameStructs;
using RogueCastle.Resources;

namespace RogueCastle;

internal static class LocaleBuilder
{
    private static readonly Dictionary<string, string> StringDict = new();
    private static readonly List<TextObj> TextObjRefreshList = [];
    private static readonly CultureInfo DefaultCultureInfo = new("en-US", false);
    private static LanguageType _languageType;
    private static string _spaceSeparator = " ";

    public static LanguageType LanguageType
    {
        get => _languageType;
        set
        {
            if (_languageType != value)
            {
                _languageType = value;
                var newCI = value switch
                {
                    LanguageType.English           => new CultureInfo("en-US", false),
                    LanguageType.French            => new CultureInfo("fr", false),
                    LanguageType.German            => new CultureInfo("de", false),
                    LanguageType.PortugueseBrazil => new CultureInfo("pt-BR", false),
                    LanguageType.SpanishSpain     => new CultureInfo("es-ES", false),
                    LanguageType.Russian           => new CultureInfo("ru-RU", false),
                    LanguageType.Polish            => new CultureInfo("pl", false),
                    LanguageType.ChineseSimple      => new CultureInfo("zh-CHS", false),
                    _                              => new CultureInfo("en-US", false),
                };

                newCI.NumberFormat.CurrencyDecimalSeparator = ".";
                LocStrings.Culture = newCI;
                _spaceSeparator = _languageType == LanguageType.ChineseSimple ? "" : " ";
            }
            else
            {
                _languageType = value;
            }
        }
    }

    public static string FormatResourceString(this string stringID, params object[] args)
    {
        return string.Format(stringID, args);
    }

    public static string GetResourceString(this string stringID, bool forceMale = false)
    {
        return GetResourceStringCustomFemale(stringID, Game.PlayerStats.IsFemale, forceMale);
    }

    public static string GetResourceStringCustomFemale(this string stringID, bool isFemale, bool forceMale = false)
    {
        if (forceMale || LanguageType == LanguageType.English || LanguageType == LanguageType.ChineseSimple)
        {
            isFemale = false;
        }

        if (stringID.Length <= 0)
        {
            return "";
        }

        var resourceString = isFemale == false
            ? LocStrings.ResourceManager.GetString(stringID, LocStrings.Culture)
            : LocStrings.ResourceManager.GetString(stringID + "_F", LocStrings.Culture);

        if (resourceString == null)
        {
            // There is no female version, try again with the male version.
            if (isFemale)
            {
                resourceString = LocStrings.ResourceManager.GetString(stringID, LocStrings.Culture);
            }

            // If it's still null, then the entire string is missing both a male and female version.
            // (i.e., missing completely)
            if (isFemale == false || resourceString == null)
            {
                resourceString = $@"{{NULLSTRING: {stringID}}}";
            }
        }

        resourceString = resourceString.Replace("\\n", "\n");
        return resourceString;
    }

    public static int GetResourceInt(this string stringID)
    {
        // First try getting from language
        var resourceString = LocStrings.ResourceManager.GetString(stringID, LocStrings.Culture);

        // If empty string or not found, use default english values.
        if (string.IsNullOrEmpty(resourceString))
        {
            resourceString = LocStrings.ResourceManager.GetString(stringID, DefaultCultureInfo);
        }

        var resourceInt = 0;
        if (resourceString is { Length: > 0 })
        {
            resourceInt = Convert.ToInt32(resourceString);
        }

        return resourceInt;
    }

    public static string GetString(this string stringID, TextObj textObj, bool forceMale = false)
    {
        if (textObj != null)
        {
            textObj.locStringID = stringID;
            AddToTextRefreshList(textObj);

            if (LanguageType != LanguageType.English)
            {
                textObj.Text = "";
                textObj.isLogographic = LanguageType == LanguageType.ChineseSimple;
                textObj.ChangeFontNoDefault(GetLanguageFont(textObj));
            }
        }

        var textString = GetResourceString(stringID, forceMale);
        return textString;
    }

    public static SpriteFont GetLanguageFont(this TextObj textObj)
    {
        var ignoreLanguage = false;
        var font = textObj.defaultFont;

        if (font == Game.BitFont ||
            font == Game.EnemyLevelFont ||
            font == Game.PlayerLevelFont ||
            font == Game.NotoSansSCFont ||
            font == Game.GoldFont ||
            font == Game.PixelArtFont ||
            font == Game.PixelArtFontBold)
        {
            ignoreLanguage = true;
        }

        if (ignoreLanguage)
        {
            return font;
        }

        return LanguageType switch
        {
            LanguageType.ChineseSimple => Game.NotoSansSCFont,
            LanguageType.Russian      => Game.RobotoSlabFont,
            _                         => font,
        };
    }

    public static void LoadLanguageFile(ContentManager content, string filePath)
    {
        var languageText = new StringBuilder();
        StringDict.Clear(); // Clear the dictionary first.

        var path = $"{AppDomain.CurrentDomain.BaseDirectory}\\{content.RootDirectory}\\Languages\\{filePath}";
        using (var reader = new StreamReader(path))
        {
            try
            {
                while (reader.ReadLine() is { } line)
                {
                    languageText.Append($"{line};");
                    languageText = languageText.Replace("\\n", "\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Could not load language file - Error: {e.Message}");
            }
            finally
            {
                reader.Close();
            }
        }

        // Removes all tab instances.
        languageText = languageText.Replace("\t", "");
        // Putting all lines into a list, delineated by the semicolon.
        List<string> languageList = languageText.ToString().Split(';').ToList();
        // Remove the last entry. ToList() adds a blank entry at the end because the code ends with a semicolon.
        languageList.RemoveAt(languageList.Count - 1);

        foreach (var value in languageList)
        {
            // Ignore blank strings.
            if (value.Length <= 0)
            {
                continue;
            }

            var indexOfFirstComma = value.IndexOf(",", StringComparison.Ordinal);
            var stringID = value.Substring(0, indexOfFirstComma);
            var text = value.Substring(indexOfFirstComma + 1);
            text = text.TrimStart(' '); // Trims any leading whitespaces.

            if (StringDict.ContainsKey(stringID) == false)
            {
                StringDict.Add(stringID, text);
            }
        }
    }

    public static void RefreshAllText()
    {
        foreach (var textObj in TextObjRefreshList.Where(textObj => textObj != null))
        {
            textObj.ChangeFontNoDefault(GetLanguageFont(textObj));
            textObj.Text = GetResourceString(textObj.locStringID);
            textObj.isLogographic = LanguageType == LanguageType.ChineseSimple;
        }

        Screen[] screenList = Game.ScreenManager.GetScreens();
        foreach (var screen in screenList)
        {
            screen.RefreshTextObjs();
        }
    }

    public static void AddToTextRefreshList(TextObj textObj)
    {
        if (TextObjRefreshList.Contains(textObj) == false)
        {
            TextObjRefreshList.Add(textObj);
        }
    }

    public static void RemoveFromTextRefreshList(TextObj textObj)
    {
        if (TextObjRefreshList.Contains(textObj))
        {
            TextObjRefreshList.Remove(textObj);
        }
    }

    public static void ClearTextRefreshList()
    {
        TextObjRefreshList.Clear();
    }

    public static bool TextRefreshListContains(TextObj textObj)
    {
        return TextObjRefreshList.Contains(textObj);
    }
}
