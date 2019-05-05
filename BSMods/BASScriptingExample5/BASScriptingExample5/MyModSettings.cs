namespace BASScriptingExample5
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ModLoader;
    using ModLoader.Attributes;

    namespace Settings
    {
        /**
         * The Mod Loader config tool will look for a class in the mod inheriting from ModLoader.ModSettings,
         * to generate the settings GUI from.
         * You should not have multiple classes inheriting from ModSettings. The Mod Loader will use the first one it encounters.
         *
         * ModSettings.GetInstance<T>() will get a singleton instance of the class with the values in the settings.json.
         * The example below stores the instance in a static field, so it can be more
         * conveniently retrieved using MyModSettings.Instance.
         *
         * Each field in the class (unless marked with [NotSerialized]) will be serialized to and from the settings.json file.
         * The fields can be of any type, but only these types are supported by the GUI:
         * int
         * long
         * float
         * double
         * bool
         * string
         * System.DateTime (The GUI does not support time, only date)
         * And any class or struct defined in the mod assembly (Granted it also contains only fields of these types).
         * The type cannot be a type defined in another assembly, including types defined in the .NET Framework.
         * Arrays and List<T> of the types above are supported as well.
         *
         * The ModLoader.ModSettingAttribute attribute is not required,
         * but can be used to specify a description and/or name for the setting.
         * The description will be displayed in a tooltip when hovering the mouse over the setting in the GUI.
         * If no name is provided, the name will be generated from the field name.
         *
         * The Newtonsoft.Json.JsonPropertyAttribute attribute can be used to specify the name to use when serializing to settings.json.
         * If the attribute is not specified, the field name will be used.
         *
         * Default values cannot currently be specified.
         * The GUI will default integral types to zero, strings to empty strings, booleans to true and DateTimes to the current time.
         * You should include a settings.json with your mod, containing the desired default values.
         **/
        public sealed class MyModSettings : ModSettings
        {
            private static MyModSettings instance;

            // Will appear in the settings GUI as "Setting Number One" with no tooltip.
            public bool SettingNumberOne;

            // Will appear in the settings GUI as "Setting Number Two" with a tooltip.
            [ModSetting("This is the second setting")]
            public int SettingNumberTwo;

            // Will appear in the settings GUI as "Setting #3" with a tooltip.
            [ModSetting("This is the third setting", "Setting #3")]
            public float SettingNumberThree;

            [ModSetting("This is the fourth setting", "Setting #4")]
            public double Setting4;

            [ModSetting("This is the fifth setting", "Setting #5")]
            public string Setting5;

            [ModSetting("This is the sixth setting", "Setting #6")]
            public SomeObject[] Setting6;

            [ModSetting("This is the seventh setting", "Setting #7")]
            public List<DateTime> Setting7;

            [ModSetting("This is the eighth setting", "Setting #8")]
            public bool[] Setting8;

            [ModSetting("This is the ninth setting", "Setting #9")]
            public SomeEnum Setting9;

            [ModSetting("This is the tenth setting", "Setting #10")]
            //[JsonConverter(typeof(StringEnumConverter))] // If you reference Newtonsoft.Json.dll you can use this property to serialize enums as strings instead of numeric values.
            public SomeEnum Setting10;

            static MyModSettings()
            {
                MyModSettings.instance = ModSettings.GetInstance<MyModSettings>();
            }

            public static MyModSettings Instance
            {
                get { return MyModSettings.instance; }
            }
        }

        public struct SomeObject
        {
            public string A;
            public int B;
            public float C;
            public Vector3 D;
        }

        public struct Vector3
        {
            public float X;
            public float Y;
            public float Z;

            public UnityEngine.Vector3 ToEngineVector3()
            {
                return new UnityEngine.Vector3(this.X, this.Y, this.Z);
            }
        }

        // For enums, int and long are supported.
        public enum SomeEnum : long
        {
            Alpha = -6,
            Bravo = 42,
            Charlie = 1337
        }
    }
}