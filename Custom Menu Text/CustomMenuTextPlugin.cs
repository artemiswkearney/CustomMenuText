using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;
using TMPro;
using System.Text;

namespace TestBSPlugin
{
    public class CustomMenuTextPlugin : IPlugin
    {
        // path to the file to load text from
        private const string FILE_PATH = "/UserData/CustomMenuText.txt";
        public static TMP_FontAsset beon;
        // used if we can't load any custom entries
        public static readonly string[] DEFAULT_TEXT = { "BEAT", "SABER" };
        public static readonly Color defaultMainColor = new Color(0, 0.5019608f, 1);
        public static readonly Color defaultBottomColor = Color.red;

        public const string DEFAULT_CONFIG =
@"# Custom Menu Text v2.2.0
# by Arti
# Special Thanks: Kyle1413
#
# Use # for comments!
# Separate entries with empty lines; a random one will be picked each time the menu loads.
# Appears just like in the vanilla game (except not quite because the vanilla logo is an image now):
Beat
Saber

# Entries with a number of lines other than 2 won't be colored by default.
# Color them yourself with formatting!
<#0080FF>B<#FF0000>S

# Finally allowed again!
MEAT
SABER

# You can override the colors even when the text is 2 lines, plus do a lot of other stuff!
# (contributed by @Rolo)
<size=+5><#ffffff>SBU<#ffff00>BBY
        <size=-15><#1E5142>eef freef.

# Some more random messages:
BEAT
SAMER

1337 
SABER

YEET
SABER

BEET
SABER

BAT
SAVER

SATE
BIEBER

BEAR
BEATS

<#0080FF>BEAR <#FF0000>BEATS
<#DDDDDD>BATTLESTAR GALACTICA

BEE
MOVIE

MEME

BEAM
TASER

ENVOY OF
NEZPHERE

BEER
TASTER

ABBA
TREES

EAT
ASS

BERATE
ABS

FLYING
CARS

BEATMANIA
IIDX

# requested by Reaxt
<#8A0707>HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK
HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK HECK

<size=+125><#FF0000>HECK

HECK
OFF

Having problems?
Ask in <#7289DA>#support

READ
BOOKS

# wrong colors
<#FF0000>BEAT
<#0080FF>SABER

<#0080FF>HARDER
<#FF0000>BETTER
<#0080FF>FASTER
<#FF0000>SABER

DON'T
PANIC";

        // caches entries loaded from the file so we don't need to do IO every time the menu loads
        public static List<string[]> allEntries = null;

        public string Name => "Custom Menu Text";
        public string Version => "2.2.0";

        // Store the text objects so when we leave the menu and come back, we aren't creating a bunch of them
        public static TextMeshPro mainText;
        public static TextMeshPro bottomText; // BOTTOM TEXT

        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Menu") // Only run in menu scene
            {
                if (allEntries == null)
                {
                    reloadFile();
                }
                if (allEntries.Count == 0)
                {
                    Console.WriteLine("[CustomMenuText] File found, but it contained no entries! Leaving original logo intact.");
                }
                else
                {
                    // Choose an entry randomly

                    // Unity's random seems to give biased results
                    // int entryPicked = UnityEngine.Random.Range(0, entriesInFile.Count);
                    // using System.Random instead
                    System.Random r = new System.Random();
                    int entryPicked = r.Next(allEntries.Count);

                    // Set the text
                    setText(allEntries[entryPicked]);
                }
            }
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
           
        }

        public static List<string[]> readFromFile(string relPath)
        {
            List<string[]> entriesInFile = new List<string[]>();

            // Look for the custom text file
            string gameDirectory = Environment.CurrentDirectory;
            gameDirectory = gameDirectory.Replace('\\', '/');
            if (File.Exists(gameDirectory + relPath))
            {
                var linesInFile = File.ReadLines(gameDirectory + relPath);

                // Strip comments (all lines beginning with #)
                linesInFile = linesInFile.Where(s => s == "" || s[0] != '#');

                // Collect entries, splitting on empty lines
                List<string> currentEntry = new List<string>();
                foreach (string line in linesInFile)
                {
                    if (line == "")
                    {
                        entriesInFile.Add(currentEntry.ToArray());
                        currentEntry.Clear();
                    }
                    else
                    {
                        currentEntry.Add(line);
                    }
                }
                if (currentEntry.Count != 0)
                {
                    // in case the last entry doesn't end in a newline
                    entriesInFile.Add(currentEntry.ToArray());
                }
            }
            else
            {
                // No custom text file found!
                // Create the file and populate it with the default config
                try
                {
                    using (FileStream fs = File.Create(gameDirectory + relPath))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(DEFAULT_CONFIG
                            // normalize newlines to CRLF
                            .Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n"));
                        fs.Write(info, 0, info.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[CustomMenuText] No custom text file found, and an error was encountered trying to generate a default one!");
                    Console.WriteLine("[CustomMenuText] Error:");
                    Console.WriteLine(ex);
                    Console.WriteLine("[CustomMenuText] To use this plugin, manually create the file " + relPath + " in your Beat Saber install directory.");
                    return entriesInFile;
                }
                // File was successfully created; load from it with a recursive call.
                return readFromFile(relPath);
            }

            return entriesInFile;
        }

        /// <summary>
        /// Replaces the logo in the main menu (which is an image and not text
        /// as of game version 0.12.0) with an editable TextMeshPro-based
        /// version. Performs only the necessary steps (if the logo has already
        /// been replaced, restores the text's position and color to default
        /// instead).
        /// Warning: Only call this function from the main menu scene!
        /// 
        /// Code generously donated by Kyle1413; edited some by Arti
        /// </summary>
        public static void replaceLogo()
        {
            if (beon == null)
            {
                var fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
                foreach (TMP_FontAsset font in fonts)
                {
                    if (font.name == "Beon SDF")
                        beon = font;
                }
            }

            // Logo Top Pos : 0.63, 21.61, 24.82
            // Logo Bottom Pos : 0, 17.38, 24.82

            if (mainText == null) mainText = GameObject.Find("CustomMenuText")?.GetComponent<TextMeshPro>();
            if (mainText == null)
            {
                GameObject textObj = new GameObject("CustomMenuText");
                mainText = textObj.AddComponent<TextMeshPro>();
                mainText.alignment = TextAlignmentOptions.Center;
                mainText.fontSize = 12;
                mainText.font = beon;
                mainText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2f);
                mainText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2f);
                mainText.richText = true;
                textObj.transform.localScale *= 3.7f;
                mainText.overflowMode = TextOverflowModes.Overflow;
                mainText.enableWordWrapping = false;
            }
            mainText.rectTransform.position = new Vector3(0f, 21.61f, 24.82f);
            mainText.color = defaultMainColor;
            mainText.text = "BEAT";

            if (bottomText == null) bottomText = GameObject.Find("CustomMenuText-Bot")?.GetComponent<TextMeshPro>();
            if (bottomText == null)
            {
                GameObject textObj2 = new GameObject("CustomMenuText-Bot");
                bottomText = textObj2.AddComponent<TextMeshPro>();
                bottomText.alignment = TextAlignmentOptions.Center;
                bottomText.fontSize = 12;
                bottomText.font = beon;
                bottomText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2f);
                bottomText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2f);
                bottomText.richText = true;
                textObj2.transform.localScale *= 3.7f;
                bottomText.overflowMode = TextOverflowModes.Overflow;
                bottomText.enableWordWrapping = false;
            }
            bottomText.rectTransform.position = new Vector3(0f, 17f, 24.82f);
            bottomText.color = defaultBottomColor;
            mainText.text = "SABER";

            // Destroy Default Logo
            GameObject defaultLogo = GameObject.Find("Logo");
            if (defaultLogo != null) GameObject.Destroy(defaultLogo);
        }

        /// <summary>
        /// Sets the text in the main menu (which normally reads BEAT SABER) to
        /// the text of your choice. TextMeshPro formatting can be used here.
        /// Additionally:
        /// - If the text is exactly 2 lines long, the first line will be
        ///   displayed in blue, and the second will be displayed in red.
        /// Warning: Only call this function from the main menu scene!
        /// </summary>
        /// <param name="lines">
        /// The text to display, separated by lines (from top to bottom).
        /// </param>
        public static void setText(string[] lines)
        {
            // Set up the replacement logo
            replaceLogo();

            if (lines.Length == 2)
            {
                mainText.text = lines[0];
                bottomText.text = lines[1];
            }
            else
            {
                // Hide the bottom line entirely; we're just going to use the main one
                bottomText.text = "";

                // Center the text vertically (halfway between the original positions)
                Vector3 newPos = mainText.transform.position;
                newPos.y = (newPos.y + bottomText.transform.position.y) / 2;
                mainText.transform.position = newPos;

                // Set text color to white by default (users can change it with formatting anyway)
                mainText.color = Color.white;

                // Set the text
                mainText.text = String.Join("\n", lines);
            }
        }

        public void reloadFile()
        {
            allEntries = readFromFile(FILE_PATH);
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {

        }

        public void OnFixedUpdate()
        {
        }
    }
}