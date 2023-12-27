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
        static Coroutine _updateFog = null;
        static Coroutine _tuneRiverColor = null;
        static Coroutine _changeLantern = null;
        static Coroutine _changeTextOfMainframe = null;

        static Material _riverMat = null;

        public static void Initialize() {
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) => {
                if (loadScene == OWScene.SolarSystem) {
                    if(_removeArtificialSun != null) {
                        TinyPatchForDivergence.Instance.StopCoroutine(_removeArtificialSun);
                        _removeArtificialSun = null;
                    }
                    _removeArtificialSun = TinyPatchForDivergence.Instance.StartCoroutine(RemoveArtificialSun());

                    if(_updateFog != null) {
                        TinyPatchForDivergence.Instance.StopCoroutine(_updateFog);
                        _updateFog = null;
                    }
                    _updateFog = TinyPatchForDivergence.Instance.StartCoroutine(UpdateFog());

                    if(_tuneRiverColor != null) {
                        TinyPatchForDivergence.Instance.StopCoroutine(_tuneRiverColor);
                        _tuneRiverColor = null;
                    }
                    if(_riverMat != null) {
                        GameObject.Destroy(_riverMat);
                        _riverMat = null;
                    }
                    _tuneRiverColor = TinyPatchForDivergence.Instance.StartCoroutine(TuneRiverColor());

                    if(_changeLantern != null) {
                        TinyPatchForDivergence.Instance.StopCoroutine(_changeLantern);
                        _changeLantern = null;
                    }
                    _changeLantern = TinyPatchForDivergence.Instance.StartCoroutine(ChangeLantern());

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
                var ambientLightIPSurface = GameObject.Find("RingWorld_Body/Sector_RingInterior/Lights_RingInterior/AmbientLight_IP_Surface");
                if (ambientLightIPSurface) {
                    yield return null; // See https://github.com/Outer-Wilds-New-Horizons/new-horizons/blob/b6702b46a7943a50877fd8f8db4cf0a0621331ee/NewHorizons/Handlers/PlanetCreationHandler.cs#L951-L952C29
                    yield return null; // but it does not have waiting idk https://github.com/Outer-Wilds-New-Horizons/new-horizons/blob/b6702b46a7943a50877fd8f8db4cf0a0621331ee/NewHorizons/Builder/Props/DetailBuilder.cs#L194
                    ambientLightIPSurface.SetActive(false);
                    //GameObject.Destroy(ambientLightIPSurface); // simply destroying causes broken river shader idk
                    break;
                }
            }
            while(true) {
                yield return null;
                var ipSunLight = GameObject.Find("RingWorld_Body/Sector_RingInterior/Lights_RingInterior/IP_SunLight");
                if (ipSunLight) {
                    ipSunLight.SetActive(false);
                    break;
                }
            }

            while(true) {
                yield return null;
                var artificialSunBulb = GameObject.Find("Sector_RingInterior/Geometry_RingInterior/Structure_IP_ArtificialSun/ArtificialSun_Bulb");
                if (artificialSunBulb) {
                    artificialSunBulb.GetComponent<OWEmissiveRenderer>().SetEmissionColor(Color.black);
                    break;
                }
            }

            while(true) {
                yield return null;
                var hazardVolumeArtificialSun = GameObject.Find("RingWorld_Body/Sector_RingInterior/Interactibles_RingInterior/HazardVolume_ArtificialSun");
                if(hazardVolumeArtificialSun) {
                    hazardVolumeArtificialSun.GetComponent<OWTriggerVolume>().SetTriggerActivation(false);
                    break;
                }
            }
        }

        static IEnumerator TuneRiverColor() {
            TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("tune river color");
            //TessellatedRingRenderer tessellatedRingRenderer;
            while(true) {
                yield return null;
                var river = GameObject.Find("RingWorld_Body/Sector_RingInterior/Volumes_RingInterior/RingRiverFluidVolume/RingworldRiver");
                if(river) {
                    var tessellatedRingRenderer = river.GetComponent<TessellatedRingRenderer>();
                    _riverMat = new Material(tessellatedRingRenderer.sharedMaterial);
                    _riverMat.SetColor("_Color", Color.black); // index: 1, Water Surface Color
                    _riverMat.SetColor("_FogColor", Color.black); // index: 18, Particulate Color
                    _riverMat.SetFloat("_Glossiness", 0.02f); // index: 2, Water Smoothness
                    tessellatedRingRenderer.sharedMaterial = _riverMat;
                    break;
                }
            }
        }

        static IEnumerator UpdateFog() {
            TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("updating fog");
            PlanetaryFogController fog;
            while(true) {
                yield return null;
                var fogSphere = GameObject.Find("RingWorld_Body/Atmosphere_IP/FogSphere");
                if(fogSphere) {
                    yield return null;
                    yield return null;
                    fog = fogSphere.GetComponent<PlanetaryFogController>();
                    break;
                }
            }

            while(true) {
                yield return null;
                if(!fog) {
                    yield break;
                }
                fog.fogDensity = 0.05f;
            }
        }

        static IEnumerator ChangeLantern() {
            TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("changing lantern");
            while(true) {
                yield return null;
                var brokenLantern = GameObject.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone1/Structures_Zone1/BrokenLanternHouse_Zone1/Props_BrokenLanternHouse_Zone1/Prefab_IP_BROKENLanternItem (1)");
                if(brokenLantern) {
                    yield return null;
                    yield return null;
                    brokenLantern.SetActive(false);
                    break;
                }
            }

            while(true) {
                yield return null;
                //var originalLantern = GameObject.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone3/Sector_HiddenGorge/Sector_DreamFireHouse_Zone3/Interactables_DreamFireHouse_Zone3/Lanterns_DFH_Zone3/Prefab_IP_SimpleLanternItem_Zone3DFH_5");
                var originalLantern = GameObject.Find("RingWorld_Body/Sector_RingInterior/Sector_Zone1/Sector_DreamFireHouse_Zone1/Interactables_DreamFireHouse_Zone1/Lanterns/Prefab_IP_SimpleLanternItem_Zone1DFH_4");
                if(originalLantern) {
                    var lantern = GameObject.Instantiate(originalLantern);
                    lantern.transform.parent = GameObject.Find("RingWorld_Body").transform;
                    lantern.transform.localPosition = new Vector3(-127.7076f, 39.9843f, -254.1584f);
                    lantern.transform.localEulerAngles = new Vector3(19.7637f, 118.1841f, 90.0002f);
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
