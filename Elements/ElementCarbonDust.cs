using STRINGS;
using System;
using UnityEngine;

namespace KeroseneIndustry
{

    public static class CarbonDust
    {
        public static readonly Color32 CarbonDust_COLOR = new Color32((byte)201, (byte)201, (byte)195, byte.MaxValue);
        public const string SOLID_ID = "CarbonDust";
        public static readonly SimHashes SolidSimHash = (SimHashes)Hash.SDBMLower("CarbonDust");

        private static Texture2D TintTextureCarbonDustColor(Texture sourceTexture, string name)
        {
            Texture2D texture2D = Util.DuplicateTexture(sourceTexture as Texture2D);
            Color32[] pixels32 = texture2D.GetPixels32();
            for (int index = 0; index < pixels32.Length; ++index)
            {
                float num = ((Color)pixels32[index]).grayscale * 1.5f;
                pixels32[index] = (Color32)((Color)CarbonDust_COLOR * num);
            }
            texture2D.SetPixels32(pixels32);
            texture2D.Apply();
            texture2D.name = name;
            return texture2D;
        }

        private static Material CreateCarbonDustMaterial(Material source)
        {
            Material CarbonDustMaterial = new Material(source);
            CarbonDustMaterial.mainTexture = (Texture)TintTextureCarbonDustColor(CarbonDustMaterial.mainTexture, "CarbonDust");
            CarbonDustMaterial.name = "CarbonDust";
            return CarbonDustMaterial;
        }

        public static void RegisterCarbonDustSubstance()
        {
            Substance substance = Assets.instance.substanceTable.GetSubstance(SimHashes.Diamond);
            ElementUtil.CreateRegisteredSubstance("CarbonDust", Element.State.Solid, ElementUtil.FindAnim("Carbon_kanim"), CreateCarbonDustMaterial(substance.material), CarbonDust_COLOR);
        }
        public static class ElementUtil {
        public static void RegisterElementStrings(string elementId, string name, string description) {
            string upper = elementId.ToUpper();
            Strings.Add("STRINGS.ELEMENTS." + upper + ".NAME", UI.FormatAsLink(name, upper));
            Strings.Add("STRINGS.ELEMENTS." + upper + ".DESC", description);
        }

        public static KAnimFile FindAnim(string name) {
            KAnimFile anim1 = Assets.Anims.Find((Predicate<KAnimFile>)(anim => anim.name == name));
            if ((UnityEngine.Object)anim1 == (UnityEngine.Object)null)
                Debug.LogError((object)("Failed to find KAnim: " + name));
            return anim1;
        }

        public static void AddSubstance(Substance substance) => Assets.instance.substanceTable.GetList().Add(substance);

        public static Substance CreateSubstance(
          string name,
          Element.State state,
          KAnimFile kanim,
          Material material,
          Color32 colour) {
            return ModUtil.CreateSubstance(name, state, kanim, material, colour, colour, colour);
        }

        public static Substance CreateRegisteredSubstance(
          string name,
          Element.State state,
          KAnimFile kanim,
          Material material,
          Color32 colour) {
            Substance substance = ElementUtil.CreateSubstance(name, state, kanim, material, colour);
            SimHashUtil.RegisterSimHash(name);
            ElementUtil.AddSubstance(substance);
            return substance;
        }
    }
    }
}