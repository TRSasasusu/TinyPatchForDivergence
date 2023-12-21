using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace TinyPatchForDivergence {
    public static class Patch {
        static Coroutine _removeArtificialSun = null;
        static Coroutine _changeTextOfMainframe = null;

        public static void Initialize() {
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) => {
                if (loadScene == OWScene.SolarSystem) {
                    if(_removeArtificialSun != null) {
                        TinyPatchForDivergence.Instance.StopCoroutine(_removeArtificialSun);
                        _removeArtificialSun = null;
                    }
                    _removeArtificialSun = TinyPatchForDivergence.Instance.StartCoroutine(RemoveArtificialSun());

                    if(_changeTextOfMainframe != null) {
                        TinyPatchForDivergence.Instance.StopCoroutine(_changeTextOfMainframe);
                        _changeTextOfMainframe = null;
                    }
                    _changeTextOfMainframe = TinyPatchForDivergence.Instance.StartCoroutine(ChangeTextOfMainframe());
                }
            };
        }

        static IEnumerator RemoveArtificialSun() {
            TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("disable artificial sun");
            while(true) {
                yield return null;
                var lightsRingInterior = GameObject.Find("Sector_RingInterior/Lights_RingInterior");
                if (lightsRingInterior) {
                    yield return null; // See https://github.com/Outer-Wilds-New-Horizons/new-horizons/blob/b6702b46a7943a50877fd8f8db4cf0a0621331ee/NewHorizons/Handlers/PlanetCreationHandler.cs#L951-L952C29
                    yield return null; // but it does not have waiting idk https://github.com/Outer-Wilds-New-Horizons/new-horizons/blob/b6702b46a7943a50877fd8f8db4cf0a0621331ee/NewHorizons/Builder/Props/DetailBuilder.cs#L194
                    lightsRingInterior.SetActive(false);
                    //GameObject.Destroy(lightsRingInterior);
                    break;
                }
            }

            while(true) {
                yield return null;
                var artificialSunBulb = GameObject.Find("Sector_RingInterior/Geometry_RingInterior/Structure_IP_ArtificialSun/ArtificialSun_Bulb");
                if (artificialSunBulb) {
                    yield return null;
                    yield return null;
                    artificialSunBulb.SetActive(false);
                    //GameObject.Destroy(artificialSunBulb);
                    break;
                }
            }
        }

        static IEnumerator ChangeTextOfMainframe() {
            TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("change text of mainframe");
            while(true) {
                yield return null;
                var dreamWorldBody = GameObject.Find("DreamWorld_Body/Sector_DreamWorld");
                if(!dreamWorldBody) {
                    continue;
                }
                //TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("dream world body found");
                var mainframeCDT = dreamWorldBody.GetComponentsInChildren<CharacterDialogueTree>().FirstOrDefault(x => x._characterName == "Mainframe");
                if(!mainframeCDT) {
                    continue;
                }
                //TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("mainframeCDT found");
                var text = mainframeCDT._xmlCharacterDialogueAsset.text.Replace("DAY", "NIGHT");
                var textAsset = new TextAsset(text);
                mainframeCDT.SetTextXml(textAsset);
                break;
            }
            TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("correctly change text of mainframe");

            foreach (var key in new string[] {
                "VerifyRingworldARTIFICIAL LIGHTING: OK (STAGE: NIGHT)",
                "VerifyRingworld_SignalARTIFICIAL LIGHTING: OK (STAGE: NIGHT)",
            }) {
                if (TextTranslation.s_theTable.m_language == TextTranslation.Language.JAPANESE) {
                    TextTranslation.s_theTable.m_table.theTable[key] = "人工太陽:OK(ステージ:夜)";
                }
                else {
                    TextTranslation.s_theTable.m_table.theTable[key] = "ARTIFICIAL LIGHTING: OK (STAGE: NIGHT)";
                }
            }
            TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("correctly update translation of mainframe");
            //TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("look up keys of translation table");
            //foreach(var key in TextTranslation.s_theTable.m_table.theTable.Keys) {
            //    if(key.Contains("VerifyRingworld")) {
            //        TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine($"{key}: {TextTranslation.s_theTable.m_table.theTable[key]}");
            //    }
            //}
        }
    }
}
