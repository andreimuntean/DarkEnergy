using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Inventory.Slots
{
    public class Hands : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Hands; } }
        public TexturedElement HandHoldingWeapon { get; protected set; }

        public Hands(string name, int value, Attributes attributes) : base(name, value, attributes, 250, 120)
        {
            HandHoldingWeapon = new TexturedElement(80, 80) { Parent = this };
        }

        public void SetState(Hero hero)
        {
            var origin = hero.PositionRectangle.TopLeft;
            Position = origin + new Vector2(-11.2f, 91.7f) * hero.Scale;
            HandHoldingWeapon.Position = origin + new Vector2(-11.2f, 131.7f) * hero.Scale;
        }

        public override void Initialize()
        {
            base.Initialize();
            HandHoldingWeapon.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            HandHoldingWeapon.Path = Path.Substring(0, Path.Length - 4) + "_H.dds";
            HandHoldingWeapon.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HandHoldingWeapon.Update(gameTime);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
            HandHoldingWeapon.UnloadContent(contentManager);
        }
    }
}
