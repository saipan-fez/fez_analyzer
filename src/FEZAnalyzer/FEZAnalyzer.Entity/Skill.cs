using System;

namespace FEZAnalyzer.Entity
{
    public enum SkillState
    {
        Invalid,
        Active,
        NonActive,
        Disable
    }

    public class Skill
    {
        private const string UnknownSkillName = "Unknown";
        private const string UnknownWorkName = "Unknown";

        public static Skill UnknownSkill => new Skill();

        public string Name { get; }
        public string ShortName { get; }
        public string WorkName { get; }
        public int[] Pow { get; }
        public SkillState State { get; }

        private Skill()
            : this(UnknownSkillName, UnknownSkillName, UnknownWorkName, new int[] { }, SkillState.Invalid)
        { }

        public Skill(string name, string shortName, string workName, int[] pow, SkillState state)
        {
            Name      = name;
            ShortName = shortName;
            WorkName  = workName;
            Pow       = pow;
            State     = state;
        }

        public bool IsUnknownSkill()
        {
            return Name == UnknownSkillName && WorkName == UnknownWorkName;
        }
    }
}
