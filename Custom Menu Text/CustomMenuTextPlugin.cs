using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private const string FILE_PATH = "/CustomMenuText.txt";

        public string Name => "Custom Menu Text";
        public string Version => "1.0.0";
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
            if (arg0.buildIndex == 1) // Menu scene
            {
                // TODO: factor code out into sensible functions rather than one big block of code

                // keep the base game's text in case we don't find the file
                string newFirstLine = "BEAT";
                string newSecondLine = "SABER";

                string gameDirectory = Environment.CurrentDirectory;
                gameDirectory = gameDirectory.Replace('\\', '/');
                if (File.Exists(gameDirectory + FILE_PATH))
                {
                    string dataInFile = File.ReadAllText(gameDirectory + FILE_PATH);
                    string[] entriesInFile = dataInFile.Split(new string[]{ "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    System.Random r = new System.Random();
                    string[] linesInFile = entriesInFile[r.Next(entriesInFile.Length)].Split(new string[]{ "\n","\r\n"},StringSplitOptions.RemoveEmptyEntries);

                    // if there's no text in the file, we leave the default values
                    if (linesInFile.Length > 0)
                    {
                        // the first line exists, so use it
                        newFirstLine = linesInFile[0];
                        if (linesInFile.Length > 1)
                        {
                            newSecondLine = linesInFile[1];
                        }
                        else
                        {
                            // if the file has only one line, don't display a second line
                            newSecondLine = "";
                        }
                    }
                }

                // make sure text is in all caps
                newFirstLine = newFirstLine.ToUpperInvariant();
                newSecondLine = newSecondLine.ToUpperInvariant();

                TextMeshPro wasB = GameObject.Find("B").GetComponent<TextMeshPro>();
                TextMeshPro wasE = GameObject.Find("E").GetComponent<TextMeshPro>();
                TextMeshPro wasAT = GameObject.Find("AT").GetComponent<TextMeshPro>();
                TextMeshPro line2 = GameObject.Find("SABER").GetComponent<TextMeshPro>();

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
                    Vector3 newPos = line2.transform.position;
                    newPos.y = wasAT.transform.position.y;
                    wasAT.transform.position = newPos;

                    wasAT.text = newFirstLine;
                }

                line2.text = newSecondLine;

                // make sure text of any length won't wrap onto multiple lines
                wasAT.overflowMode = TextOverflowModes.Overflow;
                line2.overflowMode = TextOverflowModes.Overflow;
                wasAT.enableWordWrapping = false;
                line2.enableWordWrapping = false;
            }
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