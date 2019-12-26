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



        [Display(Name = "เกณฑ์การให้คะแนน 1 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate1 { get; set; } = 1;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 1 คะแนน รอบที่ 1")]
        public string DetailRate1 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 1 คะแนน รอบที่ 1")]
        public DateTime? Rate1MonthStart { get; set; }

        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStart {
            get { return this.Rate1MonthStart?.ToString("yyyy-MM-dd",enCulture); }
            set { if(value != null) this.Rate1MonthStart = DateTime.Parse(value,enCulture); } }

        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 1 คะแนน รอบที่ 1")]
        public DateTime? Rate1MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStop
        {
            get { return this.Rate1MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate1MonthStop = DateTime.Parse(value,enCulture); }
        }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 1 คะแนน รอบที่ 2")]
        public string Detail2Rate1 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 1 คะแนน รอบที่ 2")]
        public DateTime? Rate1MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStart2
        {
            get { return this.Rate1MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate1MonthStart2 = DateTime.Parse(value, enCulture); }
        }

        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 1 คะแนน รอบที่ 2")]
        public DateTime? Rate1MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStop2
        {
            get { return this.Rate1MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate1MonthStop2 = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "เกณฑ์การให้คะแนน 2 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate2 { get; set; } = 2;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 2 คะแนน รอบที่ 1")]
        public string DetailRate2 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 2 คะแนน รอบที่ 1")]
        public DateTime? Rate2MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStart
        {
            get { return this.Rate2MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStart = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 2 คะแนน รอบที่ 1")]
        public DateTime? Rate2MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStop
        {
            get { return this.Rate2MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStop = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 2 คะแนน รอบที่ 2")]
        public string Detail2Rate2 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 2 คะแนน รอบที่ 2")]
        public DateTime? Rate2MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStart2
        {
            get { return this.Rate2MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStart2 = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 2 คะแนน รอบที่ 2")]
        public DateTime? Rate2MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStop2
        {
            get { return this.Rate2MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStop2 = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "เกณฑ์การให้คะแนน 3 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate3 { get; set; } = 3;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 3 คะแนน รอบที่ 1")]
        public string DetailRate3 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 3 คะแนน รอบที่ 1")]
        public DateTime? Rate3MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStart
        {
            get { return this.Rate3MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStart = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 3 คะแนน รอบที่ 1")]
        public DateTime? Rate3MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStop
        {
            get { return this.Rate3MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStop = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 3 คะแนน รอบที่ 2")]
        public string Detail2Rate3 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 3 คะแนน รอบที่ 2")]
        public DateTime? Rate3MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStart2
        {
            get { return this.Rate3MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStart2 = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 3 คะแนน รอบที่ 2")]
        public DateTime? Rate3MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStop2
        {
            get { return this.Rate3MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStop2 = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "เกณฑ์การให้คะแนน 4 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate4 { get; set; } = 4;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 4 คะแนน รอบที่ 1")]
        public string DetailRate4 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 4 คะแนน รอบที่ 1")]
        public DateTime? Rate4MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStart
        {
            get { return this.Rate4MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStart = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 4 คะแนน รอบที่ 1")]
        public DateTime? Rate4MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStop
        {
            get { return this.Rate4MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStop = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 4 คะแนน รอบที่ 2")]
        public string Detail2Rate4 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 4 คะแนน รอบที่ 2")]
        public DateTime? Rate4MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStart2
        {
            get { return this.Rate4MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStart2 = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 4 คะแนน รอบที่ 2")]
        public DateTime? Rate4MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStop2
        {
            get { return this.Rate4MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStop2 = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "เกณฑ์การให้คะแนน 5 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate5 { get; set; } = 5;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 5 คะแนน รอบที่ 1")]
        public string DetailRate5 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 5 คะแนน รอบที่ 1")]
        public DateTime? Rate5MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStart
        {
            get { return this.Rate5MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStart = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 5 คะแนน รอบที่ 1")]
        public DateTime? Rate5MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStop
        {
            get { return this.Rate5MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStop = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 5 คะแนน รอบที่ 2")]
        public string Detail2Rate5 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 5 คะแนน รอบที่ 2")]
        public DateTime? Rate5MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStart2
        {
            get { return this.Rate5MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStart2 = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 5 คะแนน รอบที่ 2")]
        public DateTime? Rate5MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStop2
        {
            get { return this.Rate5MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStop2 = DateTime.Parse(value, enCulture); }
        }
    }
}
