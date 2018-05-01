// Copyright Eric Chauvin 2018.
// My blog is at:
// ericsourcecode.blogspot.com


// https://docs.microsoft.com/en-us/cpp/preprocessor/preprocessor-directives

// Preprocessor operators:
// charizing (#@), or token-pasting (##) operator,
// https://docs.microsoft.com/en-us/cpp/preprocessor/preprocessor-operators


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



/*
#define
#error
#import
#undef
#elif
#if
#include
#using
#else
#ifdef
#line
#endif
#ifndef
#pragma
*/



namespace CodeAnalysis
{
  class PreProcessor
  {
  private MainForm MForm;



  private PreProcessor()
    {
    }



  internal PreProcessor( MainForm UseForm )
    {
    MForm = UseForm;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm == null )
      {
      // Writing from another thread?
      return;
      }

    MForm.ShowStatus( ToShow );
    }


    // #define GCC_TOPLEV_H
    // #define __STDC_FORMAT_MACROS
    // #   define va_copy(d,s)  __va_copy (d, s)
    // #  define putc(C, Stream) putc_unlocked (C, Stream)


  private string RemoveFirstPoundSymbol( string Line )
    {
    Line = Line.Trim();
    int Last = Line.Length;
    if( Last == 0 )
      return "";

    if( Line[0] != '#' )
      throw( new Exception( "This line should start with the pound symbol.\r\n" + Line ));

    Line = Line.Remove( 0, 1 );
    return Line;
    }



  internal void ParseLine( string Line )
    {
    Line = RemoveFirstPoundSymbol( Line );
    ShowStatus( "Preprocessor: " + Line );

    // When a macro is replaced, mark it in
    // the Comment field.
    TheToken.Comment = "This macro was replaced with...";

    }




  }
}
