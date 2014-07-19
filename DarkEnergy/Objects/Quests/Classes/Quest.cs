namespace DarkEnergy.Quests
{
    public class Quest
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Quest(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }
    }
}
