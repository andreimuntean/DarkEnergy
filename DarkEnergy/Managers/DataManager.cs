using System;
using System.Collections.Generic;
using System.Xml;
using DarkEnergy.Abilities;
using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Inventory;
using DarkEnergy.Inventory.Slots;

namespace DarkEnergy
{
    internal struct Data
    {
        public object Value { get; set; }
        public string Name { get; set; }

        public Data(object value, string name) : this()
        {
            Value = value;
            Name = name;
        }
    }

    static class DataManager
    {
        public static string AbilityDirectory { get { return RootDirectory + @"Abilities\"; } }
        public static string CharacterDirectory { get { return RootDirectory + @"Characters\"; } }
        public static string ItemDirectory { get { return RootDirectory + @"Items\"; } }
        public static string RootDirectory { get { return @"Resources\Data\"; } }

        #region Read Item
        private static string getItemType(string id)
        {
            switch (id.Substring(0, 2))
            {
                case "10": return typeof(Weapon).Name;
                case "11": return typeof(Relic).Name;
                case "12": return typeof(Head).Name;
                case "13": return typeof(Neck).Name;
                case "14": return typeof(Chest).Name;
                case "15": return typeof(Back).Name;
                case "16": return typeof(Hands).Name;
                case "17": return typeof(Finger).Name;
                case "18": return typeof(Legs).Name;
                case "19": return typeof(Feet).Name;
                case "20": return typeof(GenericItem).Name;
                default: return "";
            }
        }

        private static List<Data> readItem(XmlReader reader, string id)
        {
            List<Data> data = new List<Data>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "item")
                    {
                        break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    var element = reader.Name;
                    switch (element)
                    {
                        case "name":
                            var language = reader["language"];
                            if (language == Resources.Strings.ResourceLanguage)
                            {
                                data.Add(new Data(reader.ReadElementContentAsString(), element));
                            }
                            break;

                        case "value":
                            data.Add(new Data(reader.ReadElementContentAsInt(), element));
                            break;

                        case "element":
                            data.Add(new Data(reader.ReadElementContentAsInt(), element));
                            break;

                        case "attributes":
                            var damage = (reader["damage"] != null) ? int.Parse(reader["damage"]) : 0;
                            var armor = (reader["armor"] != null) ? int.Parse(reader["armor"]) : 0;
                            var strength = (reader["strength"] != null) ? int.Parse(reader["strength"]) : 0;
                            var intuition = (reader["intuition"] != null) ? int.Parse(reader["intuition"]) : 0;
                            var reflexes = (reader["reflexes"] != null) ? int.Parse(reader["reflexes"]) : 0;
                            var vitality = (reader["vitality"] != null) ? int.Parse(reader["vitality"]) : 0;
                            var vigor = (reader["vigor"] != null) ? int.Parse(reader["vigor"]) : 0;
                            data.Add(new Data(new Attributes(damage, armor, strength, intuition, reflexes, vitality, vigor), element));
                            break;
                    }
                }
            }

            return data;
        }
        #endregion

        #region Read Ability
        private static List<Data> readAbility(XmlReader reader, string id)
        {
            var effects = new List<Effect>();
            List<Data> data = new List<Data>() { new Data(int.Parse(id), "id") };

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "ability")
                    {
                        data.Add(new Data(effects, "effects"));
                        break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    var element = reader.Name;
                    switch (element)
                    {
                        case "name":
                            var language = reader["language"];
                            if (language == Resources.Strings.ResourceLanguage)
                            {
                                data.Add(new Data(reader.ReadElementContentAsString(), element));
                                break;
                            }
                            break;

                        case "rank":
                            var current = int.Parse(reader["current"]);
                            var highest = int.Parse(reader["highest"]);
                            var previousId = (reader["previousId"] != null) ? int.Parse(reader["previousId"]) : -1;
                            var nextId = (reader["nextId"] != null) ? int.Parse(reader["nextId"]) : -1;
                            data.Add(new Data(new int[] { current, highest, previousId, nextId }, element));
                            break;

                        case "icon":
                            var iconId = int.Parse(reader["id"]);
                            data.Add(new Data(iconId, element));
                            break;

                        case "requiredLevel":
                            data.Add(new Data(reader.ReadElementContentAsInt(), element));
                            break;

                        case "darkEnergy":
                            data.Add(new Data(reader.ReadElementContentAsInt(), element));
                            break;

                        case "effect":
                            var name = reader["name"].ToString();
                            var minValue = int.Parse(reader["minValue"]);
                            var maxValue = int.Parse(reader["maxValue"]);
                            var roundsActive = (reader["roundsActive"] != null) ? int.Parse(reader["roundsActive"]) : 0;
                            var areaEffect = (reader["areaEffect"] != null) ? bool.Parse(reader["areaEffect"]) : false;                            
                            effects.Add(new Effect((EffectType)Enum.Parse(typeof(EffectType), name), minValue, maxValue, roundsActive, areaEffect));
                            break;

                        case "animation":
                            var index = int.Parse(reader["id"]);
                            var visual = (reader["visual"] != null) ? int.Parse(reader["visual"]) : 10000;
                            data.Add(new Data(new object[] { index, visual }, element));
                            break;

                        case "targets":
                            var self = (reader["self"] != null) ? bool.Parse(reader["self"]) : false;
                            var friendly = (reader["friendly"] != null) ? bool.Parse(reader["friendly"]) : false;
                            var hostile = (reader["hostile"] != null ) ? bool.Parse(reader["hostile"]) : false;
                            var alive = (reader["alive"] != null ) ? bool.Parse(reader["alive"]) : true;
                            var dead = (reader["dead"] != null ) ? bool.Parse(reader["dead"]) : false;
                            data.Add(new Data(new TargetRestrictions(self, friendly, hostile, alive, dead), element));
                            break;
                    }
                }
            }

            return data;
        }
        #endregion

        #region Read Enemy
        private static List<Data> readEnemy(XmlReader reader, string id)
        {
            var abilitySets = new List<AbilitySet>();
            List<Data> data = new List<Data>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "enemy")
                    {
                        data.Add(new Data(abilitySets, "abilitySets"));
                        break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    var element = reader.Name;
                    switch (element)
                    {
                        case "name":
                            var language = reader["language"];
                            if (language == Resources.Strings.ResourceLanguage)
                            {
                                data.Add(new Data(reader.ReadElementContentAsString(), element));
                                break;
                            }
                            break;

                        case "element":
                            var defensive = (Element)int.Parse(reader["defensive"]);
                            var offensive = (Element)int.Parse(reader["offensive"]);
                            data.Add(new Data(new Element[] { defensive, offensive }, element));
                            break;

                        case "attributes":
                            var damage = (reader["damage"] != null) ? int.Parse(reader["damage"]) : 0;
                            var armor = (reader["armor"] != null) ? int.Parse(reader["armor"]) : 0;
                            var strength = (reader["strength"] != null) ? int.Parse(reader["strength"]) : 0;
                            var intuition = (reader["intuition"] != null) ? int.Parse(reader["intuition"]) : 0;
                            var reflexes = (reader["reflexes"] != null) ? int.Parse(reader["reflexes"]) : 0;
                            var vitality = (reader["vitality"] != null) ? int.Parse(reader["vitality"]) : 0;
                            var vigor = (reader["vigor"] != null) ? int.Parse(reader["vigor"]) : 0;
                            data.Add(new Data(new Attributes(damage, armor, strength, intuition, reflexes, vitality, vigor), element));
                            break;

                        case "abilitySet":
                            var setName = reader["name"].ToString();
                            var setTemplate = int.Parse(reader["template"]);
                            var idList = new List<int>();

                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.EndElement)
                                {
                                    if (reader.Name == "abilitySet")
                                    {
                                        abilitySets.Add(new AbilitySet(setName, setTemplate, idList));
                                        break;
                                    }
                                }
                                else if (reader.NodeType == XmlNodeType.Element)
                                {
                                    var abilityId = reader.ReadElementContentAsInt();
                                    idList.Add(abilityId);
                                }
                            }

                            break;

                        case "currency":
                            data.Add(new Data(reader.ReadElementContentAsInt(), element));
                            break;

                        case "experience":
                            data.Add(new Data(reader.ReadElementContentAsInt(), element));
                            break;

                        case "texture":
                            var textureId = int.Parse(reader["id"]);
                            var width = int.Parse(reader["width"]);
                            var height = int.Parse(reader["height"]);
                            data.Add(new Data(new int[] { textureId, width, height }, element));
                            break;
                    }
                }
            }

            return data;
        }
        #endregion

        private static List<Data> read(string path, string name, string id)
        {
            XmlReaderSettings xmlSettings = new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document };

            using (var reader = XmlReader.Create(path, xmlSettings))
            {
                // Reads the xml.
                while (reader.Read())
                {
                    // Verifies whether the current line matches the sought element.
                    if ((reader.IsStartElement()) && (id == reader["id"]))
                    {
                        switch (name)
                        {
                            case "Ability": return readAbility(reader, id);
                            case "Enemy": return readEnemy(reader, id);
                            case "Item": return readItem(reader, id);
                        }
                    }
                }
            }

            return null;
        }

        #region Create Object
        private static object create(string tag, List<Data> attributes)
        {
            switch (tag)
            {
                case "Weapon":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Element element = Element.None;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "element": element = (Element)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Weapon(name, value, itemAttributes) { Element = element };
                    }

                case "Relic":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Element element = Element.None;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "element": element = (Element)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Relic(name, value, itemAttributes) { Element = element };
                    }

                case "Head":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Head(name, value, itemAttributes);
                    }

                case "Neck":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Neck(name, value, itemAttributes);
                    }

                case "Chest":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Chest(name, value, itemAttributes);
                    }

                case "Back":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Back(name, value, itemAttributes);
                    }

                case "Hands":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Hands(name, value, itemAttributes);
                    }

                case "Finger":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Finger(name, value, itemAttributes);
                    }

                case "Legs":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Legs(name, value, itemAttributes);
                    }

                case "Feet":
                    {
                        string name = "Unknown";
                        int value = 0;
                        Attributes itemAttributes = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "value": value = (int)attribute.Value; break;
                                case "attributes": itemAttributes = (Attributes)attribute.Value; break;
                            }
                        }

                        return new Feet(name, value, itemAttributes);
                    }

                case "Ability":
                    {
                        int id = 0;
                        int icon = 0;
                        string name = "Unknown";
                        TargetRestrictions targets = new TargetRestrictions();
                        int rank = 0;
                        int highestRank = 0;
                        int previousId = 0;
                        int nextId = 0;
                        int requiredLevel = 0;
                        int darkEnergy = 0;
                        int visual = 0;
                        CharacterState animation = CharacterState.Stand;
                        List<Effect> effects = null;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "id": id = (int)attribute.Value; break;
                                case "icon": icon = (int)attribute.Value; break;
                                case "name": name = (string)attribute.Value; break;
                                case "rank": rank = (int)((int[])attribute.Value)[0];
                                    highestRank = (int)((int[])attribute.Value)[1];
                                    previousId = (int)((int[])attribute.Value)[2];
                                    nextId = (int)((int[])attribute.Value)[3]; break;
                                case "requiredLevel": requiredLevel = (int)attribute.Value; break;
                                case "darkEnergy": darkEnergy = (int)attribute.Value; break;
                                case "effects": effects = (List<Effect>)attribute.Value; break;
                                case "animation": animation = (CharacterState)((object[])attribute.Value)[0];
                                    visual = (int)((object[])attribute.Value)[1]; break;
                                case "targets": targets = (TargetRestrictions)attribute.Value; break;
                            }
                        }

                        return new Ability(id, icon, name, rank, highestRank, previousId, nextId, requiredLevel, darkEnergy, effects, animation, visual, targets);
                    }

                case "Enemy":
                    {
                        string name = "unknown";
                        Element[] elements = null;
                        Attributes enemyAttributes = null;
                        List<AbilitySet> abilitySets = null;
                        int currency = 0;
                        int experience = 0;
                        int textureId = 0;
                        int width = 0;
                        int height = 0;

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "name": name = (string)attribute.Value; break;
                                case "element": elements = (Element[])attribute.Value; break;
                                case "attributes": enemyAttributes = (Attributes)attribute.Value; break;
                                case "abilitySets": abilitySets = (List<AbilitySet>)attribute.Value; break;
                                case "currency": currency = (int)attribute.Value; break;
                                case "experience": experience = (int)attribute.Value; break;
                                case "texture": textureId = ((int[])attribute.Value)[0];
                                    width = ((int[])attribute.Value)[1];
                                    height = ((int[])attribute.Value)[2]; break;
                            }
                        }

                        return new Enemy(name, elements[0], elements[1], enemyAttributes, abilitySets, currency, experience, textureId, width, height);
                    }
            }
            return null;
        }
        #endregion

        public static T Load<T>(int id)
        {
            return (T)Load(id, typeof(T));
        }

        private static object Load(int id, Type type)
        {
            if (id > 0)
            {
                var index = id.ToString();
                var name = type.Name;

                try
                {
                    List<Data> attributes = null;

                    if (type == typeof(Ability))
                    {
                        attributes = read(AbilityDirectory + name + ".xml", name, index);
                    }
                    else if (type == typeof(Enemy))
                    {
                        attributes = read(CharacterDirectory + name + ".xml", name, index);
                    }
                    else if (typeof(IItem).IsAssignableFrom(type))
                    {
                        name = getItemType(index);
                        attributes = read(ItemDirectory + name + ".xml", "Item", index);
                    }
                    else
                    {
                        ExceptionManager.Log("Cannot load an object of type " + name + ".");
                        return null;
                    }

                    if (attributes == null)
                    {
                        return null;
                    }
                    else
                    {
                        var result = create(name, attributes);

                        if (result is EquippableItem)
                        {
                            (result as TexturedElement).Path = @"Items\" + name + @"\" + index + ".dds";
                        }

                        return result;
                    }
                }
                catch
                {
                    ExceptionManager.Log("Could not load an object of type " + name + " with id " + index + '.');
                }
            }
            return null;
        }
    }
}
