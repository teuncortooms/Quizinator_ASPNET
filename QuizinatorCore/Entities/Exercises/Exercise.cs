using QuizinatorCore.Services;
using System;
using System.Collections.Generic;
using QuizinatorCore.Entities.Questions;
using QuizinatorCore.Entities.Idioms;
using AutoMapper.Mappers;
using System.Text;

namespace QuizinatorCore.Entities.Exercises
{
    public class Exercise
    {
        public Guid ExerciseId { get; set; }
        public ExerciseType Type { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }

        private readonly QuestionFactory questionFactory;

        public Exercise()
        {

        }

        public Exercise((ExerciseType type, string description, List<IdiomInCollection> idioms, QuestionFactory factory) args)
        {
            this.ExerciseId = Guid.NewGuid();
            this.Type = args.type;
            this.Description = args.description;
            this.questionFactory = args.factory;
            this.Questions = new List<Question>();
            this.AddQuestions(args.idioms);
        }

        private void AddQuestions(List<IdiomInCollection> idioms)
        {
            for (int i = 0; i < idioms.Count; i++)
            {
                this.AddQuestion(idioms[i]);
            }
        }

        public void AddQuestion(IdiomInCollection idiom)
        {
            Question question = questionFactory.Create(this.Type, idiom);
            this.Questions.Add(question);
        }

        public virtual void ReplaceQuestion(int questionIndex, IdiomInCollection newIdiom)
        {
            Question question = questionFactory.Create(this.Type, newIdiom);
            this.Questions[questionIndex] = question;
        }

        public void DeleteQuestion(int questionIndex)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Id: {ExerciseId}")
            .AppendLine($"Type: {Type}. {Description}")
            .AppendLine()
            .AppendLine($"Questions:")
            .AppendLine();
            Questions.ForEach(x => builder.AppendLine(x.ToString()));
            builder.AppendLine();
            return builder.ToString();
        }
    }
}