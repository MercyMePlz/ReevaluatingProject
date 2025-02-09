﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ADOPSEV1._1.Models
{
    public class Anwser
    {
        [Key]
        [DisplayName("Id")]
        public int id { get; set; }
        [Required]
        [DisplayName("Anwser's text")]
        public string text { get; set; }
        [Required]
        [DisplayName("Question")]
        public int questionId { get; set; }
        [Required]
        [DisplayName("Correct anwser")]
        public bool isCorrect { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public Anwser(int id, string text, int questionId, bool isCorrect)
        {
            this.id = id;
            this.text = text;
            this.questionId = questionId;
            this.isCorrect = isCorrect;
            this.CreatedDateTime = DateTime.Now;
        }

        public Anwser()
        {

        }
    }
}
