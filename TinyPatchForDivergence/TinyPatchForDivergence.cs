﻿using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;

namespace TinyPatchForDivergence {
    public class TinyPatchForDivergence : ModBehaviour {
        public static TinyPatchForDivergence Instance;

        private void Awake() {
            Instance = this;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        private void Start() {
            ModHelper.Console.WriteLine($"{nameof(TinyPatchForDivergence)} is loaded!", MessageType.Success);
            Patch.Initialize();
        }
    }
}