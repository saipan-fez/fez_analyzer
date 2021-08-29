using FEZAnalyzer.Entity;

namespace FEZAnalyzer.SkillCount
{
    public interface ISkillUseRecognizer
    {
        Skill RecognizeUsedSkill(long timeStamp, int pow, Skill[] skills, PowDebuff[] powDebuff);
    }
}
