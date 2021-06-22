using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class RecordClass
    {
    }

    [DelimitedRecord(";")]
    [IgnoreEmptyLines()]
    public class Canvass56
    {
        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public string officeID;

        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public decimal expect;

        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public decimal result;

        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public int month;

        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public int year;
    }

    [DelimitedRecord(",")]
    [IgnoreEmptyLines()]
    public class P11
    {
        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public string officeID;

        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public decimal expect;

        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public decimal result;

        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public int month;

        //[FieldDelimiter("\"")]
        [FieldQuoted]
        public int year;
    }
}
