using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Inventory.Slots;
using System;

namespace DarkEnergy.Inventory
{
    public class Equipment : GameSystem, ILoadable, ISaveable
    {
        public static Equipment FromIdList(List<int> idList)
        {
            var equipment = new Equipment();
            idList.ForEach(id => GameManager.Inventory.Equip(equipment, id));
            return equipment;
        }

        public Weapon Weapon { get; set; }
        public Relic Relic { get; set; }
        public Head Head { get; set; }
        public Neck Neck { get; set; }
        public Chest Chest { get; set; }
        public Back Back { get; set; }
        public Hands Hands { get; set; }
        public Finger Finger { get; set; }
        public Legs Legs { get; set; }
        public Feet Feet { get; set; }

        public Attributes Attributes
        {
            get
            {
                Attributes sum = new Attributes();

                if (Weapon != null) sum += Weapon.Attributes;
                if (Relic != null) sum += Relic.Attributes;
                if (Head != null) sum += Head.Attributes;
                if (Neck != null) sum += Neck.Attributes;
                if (Chest != null) sum += Chest.Attributes;
                if (Back != null) sum += Back.Attributes;
                if (Hands != null) sum += Hands.Attributes;
                if (Finger != null) sum += Finger.Attributes;
                if (Legs != null) sum += Legs.Attributes;
                if (Feet != null) sum += Feet.Attributes;

                return sum;
            }
        }

        // This is a workaround that prevents the skin
        // from showing while wearing tight armor.
        public override Vector2 Scale
        {
            get
            {
                return new Vector2((0.016f + Parent.Scale.X) / 1.016f, Parent.Scale.Y);
            }
        }

        public Equipment() { }

        public List<EquippableItem> ToList()
        {
            return new List<EquippableItem>()
            {
                Weapon,
                Relic,
                Head,
                Neck,
                Chest,
                Back,
                Hands,
                Finger,
                Legs,
                Feet
            };
        }

        public List<int> GetIdList()
        {
            return new List<int>()
            {
                GameManager.Inventory.GetId(Weapon),
                GameManager.Inventory.GetId(Relic),
                GameManager.Inventory.GetId(Head),
                GameManager.Inventory.GetId(Neck),
                GameManager.Inventory.GetId(Chest),
                GameManager.Inventory.GetId(Back),
                GameManager.Inventory.GetId(Hands),
                GameManager.Inventory.GetId(Finger),
                GameManager.Inventory.GetId(Legs),
                GameManager.Inventory.GetId(Feet)
            };
        }

        public void Clear()
        {
            Weapon = null;
            Relic = null;
            Head = null;
            Neck = null;
            Chest = null;
            Back = null;
            Hands = null;
            Finger = null;
            Legs = null;
            Feet = null;
        }

        public void Unequip(Type slot)
        {
            if (slot.GetInterface("IItem", false) != null)
            {
                if (slot == typeof(Weapon)) Weapon = null;
                else if (slot == typeof(Relic)) Relic = null;
                else if (slot == typeof(Head)) Head = null;
                else if (slot == typeof(Neck)) Neck = null;
                else if (slot == typeof(Chest)) Chest = null;
                else if (slot == typeof(Back)) Back = null;
                else if (slot == typeof(Hands)) Hands = null;
                else if (slot == typeof(Finger)) Finger = null;
                else if (slot == typeof(Legs)) Legs = null;
                else if (slot == typeof(Feet)) Feet = null;                
            }
        }

        public void SetState(Hero hero)
        {
            if (Weapon != null) Weapon.SetState(hero);
            if (Head != null) Head.SetState(hero);
            if (Chest != null) Chest.SetState(hero);
            if (Back != null) Back.SetState(hero);
            if (Hands != null) Hands.SetState(hero);
            if (Legs != null) Legs.SetState(hero);
            if (Feet != null) Feet.SetState(hero);
        }

        public void StartDrawing(Renderer renderer)
        {
            if (Back != null) Back.Draw(renderer);
        }

        public void FinishDrawing(Renderer renderer)
        {
            if (Hands != null) Hands.HandHoldingWeapon.Draw(renderer);
        }

        public override void Initialize()
        {
            base.Initialize();

            if (Weapon != null)
            {
                Weapon.Initialize();
                Weapon.Parent = this;
            }

            if (Relic != null)
            {
                Relic.Initialize();
                Relic.Parent = this;
            }

            if (Head != null)
            {
                Head.Initialize();
                Head.Parent = this;
            }

            if (Neck != null)
            {
                Neck.Initialize();
                Neck.Parent = this;
            }

            if (Chest != null)
            {
                Chest.Initialize();
                Chest.Parent = this;
            }

            if (Back != null)
            {
                Back.Initialize();
                Back.Parent = this;
            }

            if (Hands != null)
            {
                Hands.Initialize();
                Hands.Parent = this;
            }

            if (Finger != null)
            {
                Finger.Initialize();
                Finger.Parent = this;
            }

            if (Legs != null)
            {
                Legs.Initialize();
                Legs.Parent = this;
            }

            if (Feet != null)
            {
                Feet.Initialize();
                Feet.Parent = this;
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            if (Weapon != null) Weapon.LoadContent(contentManager);
            if (Head != null) Head.LoadContent(contentManager);
            if (Chest != null) Chest.LoadContent(contentManager);
            if (Back != null) Back.LoadContent(contentManager);
            if (Hands != null) Hands.LoadContent(contentManager);
            if (Legs != null) Legs.LoadContent(contentManager);
            if (Feet != null) Feet.LoadContent(contentManager);
        }

        public override void Draw(Renderer renderer)
        {
            if (Legs != null) Legs.Draw(renderer);
            if (Chest != null) Chest.Draw(renderer);
            if (Feet != null) Feet.Draw(renderer);
            if (Head != null) Head.Draw(renderer);
            if (Hands != null) Hands.Draw(renderer);
            if (Weapon != null) Weapon.Draw(renderer);
        }

        public override void Update(GameTime gameTime)
        {
            if (Weapon != null) Weapon.Update(gameTime);
            if (Relic != null) Relic.Update(gameTime);
            if (Head != null) Head.Update(gameTime);
            if (Neck != null) Neck.Update(gameTime);
            if (Chest != null) Chest.Update(gameTime);
            if (Back != null) Back.Update(gameTime);
            if (Hands != null) Hands.Update(gameTime);
            if (Finger != null) Finger.Update(gameTime);
            if (Legs != null) Legs.Update(gameTime);
            if (Feet != null) Feet.Update(gameTime);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            if (Weapon != null) Weapon.UnloadContent(contentManager);
            if (Head != null) Head.UnloadContent(contentManager);
            if (Chest != null) Chest.UnloadContent(contentManager);
            if (Back != null) Back.UnloadContent(contentManager);
            if (Hands != null) Hands.UnloadContent(contentManager);
            if (Legs != null) Legs.UnloadContent(contentManager);
            if (Feet != null) Feet.UnloadContent(contentManager);
        }

        public void LoadData()
        {
            var equipment = DataStorageManager.Load<List<int>>("Equipment");
            GameManager.Inventory.Equip(this, equipment.ToArray());
        }

        public void SaveData()
        {
            var equipment = GetIdList();
            DataStorageManager.Save(equipment, "Equipment");
        }
    }
}
