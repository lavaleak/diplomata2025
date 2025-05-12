namespace Diplomata.Editor.Models.Contents {
    public class Talk : IDataModel {
        public struct TalkInput {
            public string Name;
            public string Description;
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public Talk(TalkInput input) {
            Name = input.Name;
            Description = input.Description;
        }
    }
}