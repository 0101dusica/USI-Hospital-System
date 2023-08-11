using System;

namespace ZdravoCorp.CommunicatonManagement.Polls.Model
{
    public class Question
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }

        public Question() { }

        public Question(string id, string text, int rating)
        {
            Id = id;
            Text = text;
            Rating = rating;
        }
    }
}
