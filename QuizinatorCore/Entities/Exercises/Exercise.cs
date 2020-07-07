using QuizinatorCore.Services;
using System;
using System.Collections.Generic;
using QuizinatorCore.Entities.Questions;
using QuizinatorCore.Entities.Idioms;

namespace QuizinatorCore.Entities.Exercises
{
    public class Exercise
    {
        public Guid ExerciseId { get; private set; }
        public ExerciseType Type { get; private set; }
        public string Description { get; private set; }
        public List<Question> Questions { get; private set; }

        public Exercise((ExerciseType type, string description, List<IdiomInCollection> idioms) args)
        {
            this.Type = args.type;
            this.Description = args.description;
            this.Questions = new List<Question>();
            this.AddQuestionsFromIdioms(args.idioms);
        }

        private void AddQuestionsFromIdioms(List<IdiomInCollection> idioms)
        {
            for (int i = 0; i < idioms.Count; i++)
            {
                this.AddQuestion(idioms[i]);
            }
        }

        public void AddQuestion(IdiomInCollection idiom)
        {
            Question question = QuestionFactory.Create(this.Type, idiom); // TODO: NEXT ON THE LIST
            this.Questions.Add(question);
        }

        public void ReplaceQuestion(int questionIndex, IdiomInCollection newIdiom)
        {
            Question question = QuestionFactory.Create(this.Type, newIdiom);
            this.Questions[questionIndex] = question;
        }

        public void DeleteQuestion(int questionIndex)
        {
            throw new NotImplementedException();
        }
    }
}