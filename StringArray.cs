// Copyright Eric Chauvin 2018.
// My blog is at:
// ericsourcecode.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
// using System.Threading.Tasks;


namespace CodeAnalysis
{
  class StringArray
  {
  private MainForm MForm;
  private string[] MainSArray;
  private int MainSArrayLast = 0;



  private StringArray()
    {
    }



  internal StringArray( MainForm UseForm )
    {
    MForm = UseForm;
    MainSArray = new string[3];
    }



  internal int GetLast()
    {
    return MainSArrayLast;
    }



  internal bool AppendStringToArray( string InString )
    {
    try
    {
    if( MainSArrayLast >= MainSArray.Length )
      {
      Array.Resize( ref MainSArray, MainSArray.Length + 1024 );
      }

    MainSArray[MainSArrayLast] = InString;
    MainSArrayLast++;
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in StringArray.AppendStringToArray(). " + Except.Message );
      return false;
      }
    }



  internal bool SetStringAt( string InString, int Where )
    {
    try
    {
    if( Where >= MainSArray.Length )
      {
      MainSArrayLast = Where + 1;
      Array.Resize( ref MainSArray, Where + 1024 );
      }

    MainSArray[Where] = InString;
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in StringArray.SetStringAt(). " + Except.Message );
      return false;
      }
    }



  internal bool AppendStringAt( string InString, int Where )
    {
    try
    {
    if( Where >= MainSArray.Length )
      {
      MainSArrayLast = Where + 1;
      Array.Resize( ref MainSArray, Where + 1024 );
      }

    if( MainSArray[Where] == null )
      MainSArray[Where] = "";

    MainSArray[Where] = MainSArray[Where] + InString;
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in StringArray.SetStringAt(). " + Except.Message );
      return false;
      }
    }



  internal string GetStringAt( int Where )
    {
    try
    {
    if( Where >= MainSArrayLast )
      return "";

    string Line = MainSArray[Where];
    if( Line == null )
      Line = "";

    return Line;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in StringArray.GetStringAt(). " + Except.Message );
      return "";
      }
    }



  }
}
