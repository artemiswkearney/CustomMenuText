using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;
using TMPro;

namespace TestBSPlugin
{
    public class CustomMenuTextPlugin : IPlugin
    {
        // path to the file to load text from
        private const string FILE_PATH = "/UserData/CustomMenuText.txt";

        // used if we can't load any custom entries
        public static readonly string[] DEFAULT_TEXT = { "BEAT", "SABER" };

        // caches entries loaded from the file so we don't need to do IO every time the menu loads
        public static List<string[]> allEntries = null;

        public string Name => "Custom Menu Text";
        public string Version => "2.1.0";
        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "Menu") // Only run in menu scene
            {
                if (allEntries == null)
                {
                    reloadFile();
                }
                if (allEntries.Count == 0)
                {
                    setText(DEFAULT_TEXT);
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

        public static List<string[]> readFromFile(string relPath)
        {
            List<string[]> entriesInFile = new List<string[]>();

            // Look for the custom text file
            string gameDirectory = Environment.CurrentDirectory;
            gameDirectory = gameDirectory.Replace('\\', '/');
            if (File.Exists(gameDirectory + FILE_PATH))
            {
                var linesInFile = File.ReadLines(gameDirectory + FILE_PATH);

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
                if (entriesInFile.Count == 0)
                {
                    // No entries; warn and continue
                    Console.WriteLine("[CustomMenuText] File found, but it contained no entries!");
                }
            }
            else
            {
                // No custom text file found!
                // Print an error in the console
                Console.WriteLine("[CustomMenuText] No custom text file found!");
                Console.WriteLine("Make sure the file is in the UserData folder and named CustomMenuText.txt!");
            }

            return entriesInFile;
        }

        /// <summary>
        /// Sets the text in the main menu (which normally reads BEAT SABER) to
        /// the text of your choice. TextMeshPro formatting can be used here.
        /// Additionally:
        /// - If the text is exactly 2 lines long, the first line will be
        ///   displayed in blue, and the second will be displayed in red.
        ///   - If the first line contains exactly 4 characters, the second will
        ///     flicker (like the E in BEAT SABER).
        /// Warning: Only call this function from the main menu scene!
        /// </summary>
        /// <param name="lines">
        /// The text to display, separated by lines (from top to bottom).
        /// </param>
        public static void setText(string[] lines)
        {
            TextMeshPro wasB = GameObject.Find("B").GetComponent<TextMeshPro>();
            TextMeshPro wasE = GameObject.Find("E").GetComponent<TextMeshPro>();
            TextMeshPro wasAT = GameObject.Find("AT").GetComponent<TextMeshPro>();
            TextMeshPro wasSABER = GameObject.Find("SABER").GetComponent<TextMeshPro>();

            if (lines.Length == 2)
            {
                string newFirstLine = lines[0];
                string newSecondLine = lines[1];

                // TODO: put more thought/work into keeping the flicker
                // currently this relies on the font being monospace, which it's not even
                if (newFirstLine.Length == 4)
                {
                    // we can fit it onto the existing text meshes perfectly
                    // thereby keeping the flicker effect on the second character
                    wasB.text = newFirstLine[0].ToString();
                    wasE.text = newFirstLine[1].ToString();
                    wasAT.text = newFirstLine.Substring(2);
                }
                else
                {
                    // hide the original B and E; we're just going to use AT
                    wasB.text = "";
                    wasE.text = "";

                    // to make sure the text is centered, line up the AT with SABER's position
                    // but keep its y value
                    Vector3 newPos = wasSABER.transform.position;
                    newPos.y = wasAT.transform.position.y;
                    wasAT.transform.position = newPos;

                    wasAT.text = newFirstLine;
                }

                wasSABER.text = newSecondLine;

                // make sure text of any length won't wrap onto multiple lines
                wasAT.overflowMode = TextOverflowModes.Overflow;
                wasSABER.overflowMode = TextOverflowModes.Overflow;
                wasAT.enableWordWrapping = false;
                wasSABER.enableWordWrapping = false;
            }
            else
            {
                // Hide "BEAT" entirely; we're just going to use SABER
                wasB.text = "";
                wasE.text = "";
                wasAT.text = "";

                // Center "SABER" vertically (halfway between the original positions)
                Vector3 newPos = wasSABER.transform.position;
                newPos.y = (newPos.y + wasB.transform.position.y) / 2;
                wasSABER.transform.position = newPos;

                // Set text color to white by default (users can change it with formatting anyway)
                wasSABER.color = Color.white;

                // Prevent undesired word wrap
                wasSABER.overflowMode = TextOverflowModes.Overflow;
                wasSABER.enableWordWrapping = false;

                // Set the text
                wasSABER.text = String.Join("\n", lines);
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