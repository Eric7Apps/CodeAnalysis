// Copyright Eric Chauvin 2018.
// My blog is at:
// ericsourcecode.blogspot.com



// Ignore these debug lines:
// #if defined(DBX_DEBUGGING_INFO) || defined(XCOFF_DEBUGGING_INFO)
// #include "dbxout.h"
// #endif




// Is something #Defined in more than one place?
// In different files?


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace CodeAnalysis
{
  class DefinesDictionary
  {
  private MainForm MForm;
  private SortedDictionary<string, string> DefStringDictionary;



  private DefinesDictionary()
    {
    }



  internal DefinesDictionary( MainForm UseForm )
    {
    MForm = UseForm;

    DefStringDictionary = new SortedDictionary<string, string>();
    }


/*
  internal void AddDefString( string Line )
    {
    // #define RADTODEG(x) ((x) * 57.29578)
    // #define GCC_TOPLEV_H
    // #define __STDC_FORMAT_MACROS
    // #   define va_copy(d,s)  __va_copy (d, s)
    // #  define putc(C, Stream) putc_unlocked (C, Stream)


    DefStringDictionary[Line] = ""; // Value;
    // try
    // CDictionary.Add( KeyWord, Value );
    }
*/


  }
}
