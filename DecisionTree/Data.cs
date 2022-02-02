using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    internal static class Data
    {
        /// <summary>
        /// These is the training data as enum jagged-Array
        /// </summary>
        public static Enum[][] data = new Enum[][] {
            new Enum[] { Outlook.Sunny, Temp.Hot, Humidity.High, Wind.Weak, Play.No },
            new Enum[] { Outlook.Sunny, Temp.Hot, Humidity.High, Wind.Strong, Play.No },
            new Enum[] { Outlook.Overcast, Temp.Hot, Humidity.High, Wind.Weak, Play.Yes },
            new Enum[] { Outlook.Rain, Temp.Mild, Humidity.High, Wind.Weak, Play.Yes },
            new Enum[] { Outlook.Rain, Temp.Cold, Humidity.Normal, Wind.Weak, Play.Yes },
            new Enum[] { Outlook.Rain, Temp.Cold, Humidity.Normal, Wind.Strong, Play.No },
            new Enum[] { Outlook.Overcast, Temp.Cold, Humidity.Normal, Wind.Weak, Play.Yes },
            new Enum[] { Outlook.Sunny, Temp.Mild, Humidity.High, Wind.Weak, Play.No },
            new Enum[] { Outlook.Sunny, Temp.Cold, Humidity.Normal, Wind.Weak, Play.Yes },
            new Enum[] { Outlook.Rain, Temp.Mild, Humidity.Normal, Wind.Strong, Play.Yes },
            new Enum[] { Outlook.Sunny, Temp.Mild, Humidity.Normal, Wind.Strong, Play.Yes },
            new Enum[] { Outlook.Overcast, Temp.Mild, Humidity.High, Wind.Strong, Play.Yes },
            new Enum[] { Outlook.Overcast, Temp.Hot, Humidity.Normal, Wind.Weak, Play.Yes },
            new Enum[] { Outlook.Rain, Temp.Mild, Humidity.High, Wind.Strong, Play.No }
        };
        public static int labelPosition = 4;
        public static int distinctLabel;

        private static void funcs()
        {
            // How to read enum values dynamically in Runtime
            // Enum values as String
            string[] enumVals = data[0][0].GetType().GetEnumNames();
            foreach (var enumVal in enumVals)
            {
                Console.WriteLine(enumVal);
            }

            // Enum values as "Enum"
            Array enumValues = Enum.GetValues(data[0][0].GetType());
            foreach (var enumItem in enumValues)
                Console.WriteLine(enumItem);

        }

        // The enum datatypes to use for this sample
        public enum Outlook
        {
            Sunny,
            Overcast,
            Rain
        }
        public enum Humidity
        {
            Low,
            Normal,
            High
        }
        public enum Wind
        {
            Weak,
            Strong
        }
        public enum Temp
        {
            Cold,
            Mild,
            Hot
        }
        public enum Play
        {
            Yes,
            No
        }
    }
}
