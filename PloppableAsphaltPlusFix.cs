using ColossalFramework.IO;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;

namespace PloppableAsphaltFix
{
    public class PloppableAsphaltFixMod : IUserMod
    {
        public string Name => "Ploppable Asphalt Plus [Loading Time Fix]";
        public string Description => "Allows using road shaders on props for ploppable asphalt, pavement, cliff, grass, gravel surfaces.";
        private static Configuration settings;
        public static Configuration Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = Configuration.LoadConfiguration();
                    if (settings == null)
                    {
                        settings = new Configuration();
                        Configuration.SaveConfiguration();
                    }

                }
                return settings;
            }
            set
            {
                settings = value;
            }
        }

        #region UserInterface
        private float sliderWidth = 700f;
        private float sliderHeight = 10f;
        private float labelSize = 1.2f;
        private string toolTipText = "Hold SHIFT to drag all sliders";
        private UISlider redSlider;
        private UISlider greenSlider;
        private UISlider blueSlider;
        private UITextField redLabel;
        private UITextField greenLabel;
        private UITextField blueLabel;

        private static void UpdateSlider(UISlider slider, UITextField textField, float value)
        {
            slider.value = value;
            textField.text = value.ToString();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase sliderGroup = helper.AddGroup("\t\t\t\t\t\t     RGB Values");
            sliderGroup.AddSpace(40);
            redLabel = (UITextField)sliderGroup.AddTextfield(" ", Settings.AsphaltColor.r.ToString(), (t) => { }, (t) => { });
            redLabel.disabledTextColor = Color.red;
            redLabel.textScale = labelSize;
            redLabel.Disable();

            redSlider = (UISlider)sliderGroup.AddSlider(" ", 0f, 255f, 1f, Settings.AsphaltColor.r, (f) =>
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    var difference = f - Settings.AsphaltColor.r;
                    var green = Settings.AsphaltColor.g;
                    var blue = Settings.AsphaltColor.b;
                    if (blue + difference >= 0 && blue + difference <= 255)
                    {
                        UpdateSlider(blueSlider, blueLabel, blue + difference);
                        Settings.AsphaltColor.b = blue + difference;
                    }
                    if (green + difference >= 0 && green + difference <= 255)
                    {
                        UpdateSlider(greenSlider, greenLabel, green + difference);
                        Settings.AsphaltColor.g = green + difference;
                    }
                    redSlider.tooltipBox.isVisible = false;
                }
                else redSlider.tooltipBox.isVisible = true;
                Settings.AsphaltColor.r = f;
                UpdateSlider(redSlider, redLabel, f);
                PloppableAsphalt.ApplyColors();
            });

            redSlider.color = Color.red;
            redSlider.tooltip = toolTipText;
            redSlider.scrollWheelAmount = 1f;
            redSlider.width = sliderWidth;
            redSlider.height = sliderHeight;
            UpdateSlider(redSlider, redLabel, Settings.AsphaltColor.r);
            sliderGroup.AddSpace(65);

            greenLabel = (UITextField)sliderGroup.AddTextfield(" ", Settings.AsphaltColor.g.ToString(), (t) => { }, (t) => { });
            greenLabel.disabledTextColor = Color.green;
            greenLabel.textScale = labelSize;
            greenLabel.Disable();

            greenSlider = (UISlider)sliderGroup.AddSlider(" ", 0f, 255f, 1f, Settings.AsphaltColor.g, (f) =>
            {

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {

                    var difference = f - Settings.AsphaltColor.g;
                    var red = Settings.AsphaltColor.r;
                    var blue = Settings.AsphaltColor.b;
                    if (red + difference >= 0 && red + difference <= 255)
                    {
                        UpdateSlider(redSlider, redLabel, red + difference);
                        Settings.AsphaltColor.r = red + difference;
                    }
                    if (blue + difference >= 0 && blue + difference <= 255)
                    {
                        UpdateSlider(blueSlider, blueLabel, blue + difference);
                        Settings.AsphaltColor.b = blue + difference;
                    }
                    greenSlider.tooltipBox.isVisible = false;
                }
                else greenSlider.tooltipBox.isVisible = true;
                greenSlider.RefreshTooltip();
                Settings.AsphaltColor.g = f;
                UpdateSlider(greenSlider, greenLabel, f);
                PloppableAsphalt.ApplyColors();
            });

            greenSlider.color = Color.green;
            greenSlider.tooltip = toolTipText;
            greenSlider.scrollWheelAmount = 1f;
            greenSlider.width = sliderWidth;
            greenSlider.height = sliderHeight;
            UpdateSlider(greenSlider, greenLabel, Settings.AsphaltColor.g);
            sliderGroup.AddSpace(65);

            blueLabel = (UITextField)sliderGroup.AddTextfield(" ", Settings.AsphaltColor.b.ToString(), (t) => { }, (t) => { });
            blueLabel.disabledTextColor = Color.blue;
            blueLabel.textScale = labelSize;
            blueLabel.Disable();

            blueSlider = (UISlider)sliderGroup.AddSlider(" ", 0f, 255f, 1f, Settings.AsphaltColor.b, (f) =>
            {

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {

                    var difference = f - Settings.AsphaltColor.b;
                    var red = Settings.AsphaltColor.r;
                    var green = Settings.AsphaltColor.g;
                    if (red + difference >= 0 && red + difference <= 255)
                    {
                        UpdateSlider(redSlider, redLabel, red + difference);
                        Settings.AsphaltColor.r = red + difference;
                    }
                    if (green + difference >= 0 && green + difference <= 255)
                    {
                        UpdateSlider(greenSlider, greenLabel, green + difference);
                        Settings.AsphaltColor.g = green + difference;
                    }
                    blueSlider.tooltipBox.isVisible = false;
                }
                else blueSlider.tooltipBox.isVisible = true;
                blueSlider.RefreshTooltip();
                Settings.AsphaltColor.b = f;
                UpdateSlider(blueSlider, blueLabel, f);
                PloppableAsphalt.ApplyColors();
            });

            blueSlider.color = Color.blue;
            blueSlider.tooltip = toolTipText;
            blueSlider.scrollWheelAmount = 1f;
            blueSlider.width = sliderWidth;
            blueSlider.height = sliderHeight;
            UpdateSlider(blueSlider, blueLabel, Settings.AsphaltColor.b);

            sliderGroup.AddSpace(143);
        }

        #endregion
    }

    public class Configuration
    {
        [XmlIgnore]
        private static readonly string configurationPath = Path.Combine(DataLocation.localApplicationData, "PloppableAsphaltFix.xml");

        public Color AsphaltColor = new Color(128f, 128f, 128f, 1f);
        public bool AutoDeployed = false;

        public Configuration() { }
        public void OnPreSerialize() { }
        public void OnPostDeserialize() { }

        public static void SaveConfiguration()
        {
            var fileName = configurationPath;
            var config = PloppableAsphaltFixMod.Settings;
            var serializer = new XmlSerializer(typeof(Configuration));
            using (var writer = new StreamWriter(fileName))
            {
                config.OnPreSerialize();
                serializer.Serialize(writer, config);
            }
        }


        public static Configuration LoadConfiguration()
        {
            var fileName = configurationPath;
            var serializer = new XmlSerializer(typeof(Configuration));
            try
            {
                using (var reader = new StreamReader(fileName))
                {
                    var config = serializer.Deserialize(reader) as Configuration;
                    return config;
                }
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("[Ploppable Asphalt Fix]: Error Parsing {0}: {1}", fileName, ex.Message.ToString()));
                return null;
            }
        }
    }

    public class PloppableAsphaltLoading : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            //register our event delegates
            UIView.library.Get<UIPanel>("OptionsPanel").eventVisibilityChanged += (c, i) => PloppableAsphalt.SetRenderPropertiesAll();
            UIView.GetAView().FindUIComponent<UIDropDown>("LevelOfDetail").eventSelectedIndexChanged += (c, i) => PloppableAsphalt.SetRenderPropertiesAll();

        }
        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            PloppableAsphalt.Loaded = false;
            //unregister event delegates
            UIView.library.Get<UIPanel>("OptionsPanel").eventVisibilityChanged -= (c, i) => PloppableAsphalt.SetRenderPropertiesAll();
            UIView.GetAView().FindUIComponent<UIDropDown>("LevelOfDetail").eventSelectedIndexChanged -= (c, i) => PloppableAsphalt.SetRenderPropertiesAll();
        }
    }

    public class PloppableAsphalt
    {
        public static bool Loaded;

        private static Shader shader; //will be used to contain the magic shader, but also will be used as a marker once the colors get replaced
        private static Shader shader2; //will be used to contain the magic shader, but also will be used as a marker once the colors get replaced
        private static Shader shader3; //will be used to contain the magic shader, but also will be used as a marker once the colors get replaced


        internal static void SetRenderProperties(PropInfo prefab)
        {
            prefab.m_lodRenderDistance = prefab.m_mesh.name == "ploppableasphalt-prop" ? 200 : 18000;
            prefab.m_maxRenderDistance = prefab.m_mesh.name == "ploppableasphalt-decal" ? prefab.m_maxRenderDistance : 18000;
            prefab.m_lodRenderDistance = prefab.m_mesh.name == "ploppablecliffgrass" ? 200 : 18000;
            prefab.m_lodRenderDistance = prefab.m_mesh.name == "ploppablegravel" ? 200 : 18000;
        }

        internal static void SetRenderProperties(BuildingInfo prefab)
        {
            prefab.m_maxPropDistance = 18000;
        }

        internal static void SetRenderPropertiesAll()
        {
            for (uint i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                var prefab = PrefabCollection<PropInfo>.GetLoaded(i);

                if (prefab == null) continue;

                if (prefab.m_mesh.name == "ploppableasphalt-prop" ||
                    prefab.m_mesh.name == "ploppableasphalt-decal" ||
                    prefab.m_mesh.name == "ploppablecliffgrass" ||
                    prefab.m_mesh.name == "ploppablegravel") SetRenderProperties(prefab);
            }

            for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++)
            {
                var prefab = PrefabCollection<BuildingInfo>.GetLoaded(i);
                if (prefab == null || prefab.m_props == null || prefab.m_props.Length == 0) continue;
                for (uint j = 0; j < prefab.m_props.Length; j++)
                {
                    if (prefab.m_props[j].m_finalProp == null) continue;
                    if (prefab.m_props[j].m_finalProp.m_mesh == null) continue;
                    if (prefab.m_props[j].m_finalProp.m_mesh.name == "ploppableasphalt-prop" ||
                        prefab.m_props[j].m_finalProp.m_mesh.name == "ploppableasphalt-decal" ||
                        prefab.m_props[j].m_finalProp.m_mesh.name == "ploppablecliffgrass" ||
                        prefab.m_props[j].m_finalProp.m_mesh.name == "ploppablegravel") SetRenderProperties(prefab);
                }
            }
        }

        internal static void ApplyProperties()
        {
            shader = Shader.Find("Custom/Net/RoadBridge");
            shader2 = Shader.Find("Custom/Net/Road");
            shader3 = Shader.Find("Custom/Net/TrainBridge");

            var rgb = PloppableAsphaltFixMod.Settings.AsphaltColor;
            var color = new Color(rgb.r / 255, rgb.g / 255, rgb.b / 255);

            for (uint i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                var prefab = PrefabCollection<PropInfo>.GetLoaded(i);

                if (prefab == null) continue;

                if (prefab.m_mesh.name == "ploppableasphalt-prop" ||
                    prefab.m_mesh.name == "ploppablecliffgrass" ||
                    prefab.m_mesh.name == "ploppablegravel")
                {
                    //get ACI textures, to be used as APR textures
                    Texture texture2D = new Texture2D(1, 1);
                    Texture texture2Dlod = new Texture2D(1, 1);
                    texture2D = (prefab.m_material.GetTexture("_ACIMap") as Texture2D);
                    texture2Dlod = (prefab.m_lodMaterial.GetTexture("_ACIMap") as Texture2D);

                    //change the shader
                    if (prefab.m_mesh.name == "ploppableasphalt-prop")
                    {
                        if (prefab.m_material != null) prefab.m_material.shader = shader;
                        if (prefab.m_lodMaterial != null) prefab.m_lodMaterial.shader = shader;
                    }
                    else if (prefab.m_mesh.name == "ploppablecliffgrass")
                    {
                        if (prefab.m_material != null) prefab.m_material.shader = shader2;
                        if (prefab.m_lodMaterial != null) prefab.m_lodMaterial.shader = shader2;
                    }
                    else if (prefab.m_mesh.name == "ploppablegravel")
                    {
                        if (prefab.m_material != null) prefab.m_material.shader = shader3;
                        if (prefab.m_lodMaterial != null) prefab.m_lodMaterial.shader = shader3;
                    }


                    //add the ACI textures as APR textures
                    prefab.m_material.SetTexture("_APRMap", texture2D);
                    prefab.m_lodMaterial.SetTexture("_APRMap", texture2Dlod);

                    //set high render distance
                    //this sets the render distance properties on this item
                    SetRenderProperties(prefab);

                    //change size
                    prefab.m_lodMaterialCombined = null;
                    prefab.m_generatedInfo.m_size.z = prefab.m_generatedInfo.m_size.z * 2.174f;
                    if (prefab.m_generatedInfo.m_size.y < 16) prefab.m_generatedInfo.m_size.y = 16f;
                    prefab.m_generatedInfo.m_size.x = prefab.m_generatedInfo.m_size.x * 0.4f;
                    prefab.m_generatedInfo.m_size.z = prefab.m_generatedInfo.m_size.z * 0.4f;

                    //change the color variation for the props
                    prefab.m_color0 = color; prefab.m_color1 = color;
                    prefab.m_color2 = color; prefab.m_color3 = color;
                }

                else if (prefab.m_mesh.name == "ploppableasphalt-decal")
                {
                    prefab.m_material.SetTexture("_MainTex", UnityEngine.Object.FindObjectOfType<NetProperties>().m_upwardDiffuse);
                    prefab.m_lodMaterial.SetTexture("_MainTex", UnityEngine.Object.FindObjectOfType<NetProperties>().m_upwardDiffuse);
                    //this sets the render distance properties on this item
                    SetRenderProperties(prefab);

                    //change the color variation for the props
                    prefab.m_color0 = color; prefab.m_color1 = color;
                    prefab.m_color2 = color; prefab.m_color3 = color;
                }
            }

            for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++)
            {
                var prefab = PrefabCollection<BuildingInfo>.GetLoaded(i);
                if (prefab == null || prefab.m_props == null || prefab.m_props.Length == 0) continue;
                for (uint j = 0; j < prefab.m_props.Length; j++)
                {
                    if (prefab.m_props[j].m_finalProp == null) continue;
                    if (prefab.m_props[j].m_finalProp.m_mesh == null) continue;
                    if (prefab.m_props[j].m_finalProp.m_mesh.name == "ploppableasphalt-prop" ||
                        prefab.m_props[j].m_finalProp.m_mesh.name == "ploppableasphalt-decal" ||
                        prefab.m_props[j].m_finalProp.m_mesh.name == "ploppablecliffgrass" ||
                        prefab.m_props[j].m_finalProp.m_mesh.name == "ploppablegravel") SetRenderProperties(prefab);
                }
            }
        }

        internal static void ApplyColors() //change the color variation for the props, slider adjustment should only call this
        {

            for (uint i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                var rgb = PloppableAsphaltFixMod.Settings.AsphaltColor;
                var prefab = PrefabCollection<PropInfo>.GetLoaded(i);
                var color = new Color(rgb.r / 255, rgb.g / 255, rgb.b / 255);

                if (prefab == null) continue;

                if (prefab.m_mesh.name == "ploppableasphalt-prop" ||
                    prefab.m_mesh.name == "ploppableasphalt-decal" ||
                    prefab.m_mesh.name == "ploppablegravel" ||
                    prefab.m_mesh.name == "ploppablecliffgrass")
                {
                    prefab.m_color0 = color; prefab.m_color1 = color;
                    prefab.m_color2 = color; prefab.m_color3 = color;
                }
            }
            Configuration.SaveConfiguration();
            //this sets the render distance properties on all ploppable asphalt items
            //we no longer need to use ApplyColorsAgain I dont think.
            SetRenderPropertiesAll();
        }
    }

    public class MainThread : ThreadingExtensionBase
    {
        private UIComponent component;
        private TimeSpan ts;

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            //moved this here because we had duplicate ThreadingExtension class.
            if (!PloppableAsphalt.Loaded && LoadingManager.instance.m_loadingComplete)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                PloppableAsphalt.ApplyProperties();
                sw.Stop();
                ts = sw.Elapsed;
                PloppableAsphalt.Loaded = true;
            }

            //UnityEngine.Debug.Log($"Ploppable AsphaltPlus ApplyProperties() : {ts.TotalSeconds} sec");


            if (component == null) component = UIView.library.Get("OptionsPanel");

            if (component != null && component.isVisible)
            {
                UITextureSprite uITextureSprite = Util.ReadPrivate<DesaturationControl, UITextureSprite>(UnityEngine.Object.FindObjectOfType<DesaturationControl>(), "m_Target");
                if (uITextureSprite.opacity != 0f)
                {
                    uITextureSprite.opacity = 0f;
                    Util.WritePrivate<DesaturationControl, UITextureSprite>(UnityEngine.Object.FindObjectOfType<DesaturationControl>(), "m_Target", uITextureSprite);
                }
            }
        }
    }

    public static class Util
    {
        public static Q ReadPrivate<T, Q>(T o, string fieldName)
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo fieldInfo = null;
            FieldInfo[] array = fields;
            for (int i = 0; i < array.Length; i++)
            {
                FieldInfo fieldInfo2 = array[i];
                if (fieldInfo2.Name == fieldName)
                {
                    fieldInfo = fieldInfo2;
                    break;
                }
            }
            return (Q)((object)fieldInfo.GetValue(o));
        }

        public static void WritePrivate<T, Q>(T o, string fieldName, object value)
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo fieldInfo = null;
            FieldInfo[] array = fields;
            for (int i = 0; i < array.Length; i++)
            {
                FieldInfo fieldInfo2 = array[i];
                if (fieldInfo2.Name == fieldName)
                {
                    fieldInfo = fieldInfo2;
                    break;
                }
            }
            fieldInfo.SetValue(o, value);
        }
    }
}