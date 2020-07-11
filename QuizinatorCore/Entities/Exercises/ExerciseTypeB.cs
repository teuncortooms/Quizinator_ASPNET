using QuizinatorCore.Entities.Idioms;
using QuizinatorCore.Entities.Questions;
using QuizinatorCore.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizinatorCore.Entities.Exercises
{
    class ExerciseTypeB : Exercise
    {
        private readonly Randomizer randomizer;

        public new List<QuestionTypeB> Questions { get; private set; }

        public string TranslationsBox { get; set; }

        public ExerciseTypeB(
            (ExerciseType type, string description, List<IdiomInCollection> idioms, QuestionFactory factory) args,
            Randomizer randomizer
            )
            : base(args)
        {
            this.randomizer = randomizer;
            this.FillTranslationsBox(args.idioms);
        }

        private string FillTranslationsBox(List<IdiomInCollection> idioms)
        {
            List<string> translations = new List<string>();
            for (int i = 0; i < idioms.Count; i++)
            {
                translations.Add(idioms[i].Translation);
            }
            return ShuffleAndJoin(translations);
        }

        private string ShuffleAndJoin(List<string> translations)
        {
            List<string> shuffledTranslations = this.randomizer.Shuffle<string>(translations);
            return this.TranslationsBox = string.Join("  *  ", shuffledTranslations);
        }

        public override void ReplaceQuestion(int questionIndex, IdiomInCollection newIdiom)
        {
            base.ReplaceQuestion(questionIndex, newIdiom);
            ReplaceTranslationInBox(questionIndex, newIdiom);
        }

        private void ReplaceTranslationInBox(int questionIndex, IdiomInCollection newIdiom)
        {
            string oldTranslation = this.Questions[questionIndex].Translation;
            this.TranslationsBox.Replace(oldTranslation, newIdiom.Translation);
        }
    }
}
