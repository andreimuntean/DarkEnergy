using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Characters.Hero
{
    public enum Gender { Male, Female }

    public class Features : GameSystem, ILoadable, ISaveable
    {
        public Gender Gender { get; set; }
        public byte Skin { get; set; }
        public byte Face { get; set; }
        public byte Hair { get; set; }

        public TexturedElement FaceTexture { get; protected set; }
        public TexturedElement HairTexture { get; protected set; }
        public TexturedElement HandOverWeaponTexture { get; protected set; }

        public Features()
        {
            FaceTexture = new TexturedElement(32, 32) { Parent = this };
            HairTexture = new TexturedElement(128, 128) { Parent = this };
            HandOverWeaponTexture = new TexturedElement(80, 80) { Parent = this };

            Skin = Face = Hair = 1;
        }

        public void SetState(Hero hero)
        {
            var origin = hero.PositionRectangle.TopLeft;
            HairTexture.Position = origin + new Vector2(59.8f, -40.9f) * hero.Scale;
            FaceTexture.Position = origin + new Vector2(114.8f, 14.1f) * hero.Scale;
            HandOverWeaponTexture.Position = origin + new Vector2(-11.2f, 131.7f) * hero.Scale;
        }
        
        public void Refresh()
        {
            FaceTexture.Path = @"Characters\Hero\" + Gender.ToString() + @"\Face" + Face.ToString() + ".dds";
            HairTexture.Path = @"Characters\Hero\" + Gender.ToString() + @"\Hair" + Hair.ToString() + ".dds";
            HandOverWeaponTexture.Path = @"Characters\Hero\Skin" + Skin.ToString() + "_HandOverWeapon.dds";
        }

        public override void Initialize()
        {
            base.Initialize();
            FaceTexture.Initialize();
            HairTexture.Initialize();
            HandOverWeaponTexture.Initialize();
            Refresh();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            FaceTexture.LoadContent(contentManager);
            HairTexture.LoadContent(contentManager);
            HandOverWeaponTexture.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            FaceTexture.UnloadContent(contentManager);
            HairTexture.UnloadContent(contentManager);
            HandOverWeaponTexture.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        public void LoadData()
        {
            Gender = DataStorageManager.Load<Gender>("HeroGender");
            Skin = DataStorageManager.Load<byte>("HeroSkin");
            Face = DataStorageManager.Load<byte>("HeroFace");
            Hair = DataStorageManager.Load<byte>("HeroHair");
        }

        public void SaveData()
        {
            DataStorageManager.Save(Gender, "HeroGender");
            DataStorageManager.Save(Skin, "HeroSkin");
            DataStorageManager.Save(Face, "HeroFace");
            DataStorageManager.Save(Hair, "HeroHair");
        }
    }
}
