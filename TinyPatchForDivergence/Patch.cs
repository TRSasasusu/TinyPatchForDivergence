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

        public static void Initialize() {
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) => {
                if (loadScene == OWScene.SolarSystem) {
                    if(_removeArtificialSun != null) {
                        TinyPatchForDivergence.Instance.StopCoroutine(_removeArtificialSun);
                        _removeArtificialSun = null;
                    }
                    _removeArtificialSun = TinyPatchForDivergence.Instance.StartCoroutine(RemoveArtificialSun());
                }
            };
        }

        static IEnumerator RemoveArtificialSun() {
            TinyPatchForDivergence.Instance.ModHelper.Console.WriteLine("disalbe artificial sun");
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
    }
}
