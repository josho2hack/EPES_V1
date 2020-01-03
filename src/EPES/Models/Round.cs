using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace EPES.Models
{
    public class Round
    {
        private static IFormatProvider enCulture = CultureInfo.CreateSpecificCulture("en-US");

        [Key]
        public int Id { get; set; }

        [Display(Name ="รอบที่")]
        public int RoundNumber { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 1 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate1 { get; set; } = 1;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 1 คะแนน")]
        public string DetailRate1 { get; set; }

        [Display(Name = "เดือนเริ่ม 1 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate1MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStart {
            get { return this.Rate1MonthStart?.ToString("yyyy-MM-dd",enCulture); }
            set { if(value != null) this.Rate1MonthStart = DateTime.Parse(value,enCulture); } }

        [Display(Name = "เดือนสิ้นสุด 1 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate1MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStop
        {
            get { return this.Rate1MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate1MonthStop = DateTime.Parse(value,enCulture); }
        }

        [Display(Name = "เกณฑ์การให้คะแนน 2 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate2 { get; set; } = 2;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 2 คะแนน")]
        public string DetailRate2 { get; set; }

        [Display(Name = "เดือนเริ่ม 2 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate2MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStart
        {
            get { return this.Rate2MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStart = DateTime.Parse(value, enCulture); }
        }

        [Display(Name = "เดือนสิ้นสุด 2 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate2MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStop
        {
            get { return this.Rate2MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStop = DateTime.Parse(value, enCulture); }
        }

        [Display(Name = "เกณฑ์การให้คะแนน 3 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate3 { get; set; } = 3;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 3 คะแนน")]
        public string DetailRate3 { get; set; }

        [Display(Name = "เดือนเริ่ม 3 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate3MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStart
        {
            get { return this.Rate3MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStart = DateTime.Parse(value, enCulture); }
        }

        [Display(Name = "เดือนสิ้นสุด 3 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate3MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStop
        {
            get { return this.Rate3MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStop = DateTime.Parse(value, enCulture); }
        }

        [Display(Name = "เกณฑ์การให้คะแนน 4 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate4 { get; set; } = 4;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 4 คะแนน")]
        public string DetailRate4 { get; set; }

        [Display(Name = "เดือนเริ่ม 4 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate4MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStart
        {
            get { return this.Rate4MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStart = DateTime.Parse(value, enCulture); }
        }

        [Display(Name = "เดือนสิ้นสุด 4 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate4MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStop
        {
            get { return this.Rate4MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStop = DateTime.Parse(value, enCulture); }
        }

        [Display(Name = "เกณฑ์การให้คะแนน 5 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate5 { get; set; } = 5;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 5 คะแนน")]
        public string DetailRate5 { get; set; }

        [Display(Name = "เดือนเริ่ม 5 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate5MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStart
        {
            get { return this.Rate5MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStart = DateTime.Parse(value, enCulture); }
        }
        
        [Display(Name = "เดือนสิ้นสุด 5 คะแนน")]
        [DataType(DataType.Date)]
        public DateTime? Rate5MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStop
        {
            get { return this.Rate5MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStop = DateTime.Parse(value, enCulture); }
        }

        public int PointOfEvaluationId { get; set; }
        [Display(Name = "ตัวชี้วัด")]
        public PointOfEvaluation PointOfEvaluation { get; set; }

        [Display(Name = "ข้อมูลในการประเมิน")]
        public IList<DataForEvaluation> DataForEvaluations { get; set; }
    }
}
