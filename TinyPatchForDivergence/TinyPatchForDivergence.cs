using OWML.Common;
using OWML.ModHelper;

namespace TinyPatchForDivergence {
    public class TinyPatchForDivergence : ModBehaviour {
        public static TinyPatchForDivergence Instance;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            ModHelper.Console.WriteLine($"{nameof(TinyPatchForDivergence)} is loaded!", MessageType.Success);
            Patch.Initialize();
        }
    }
}