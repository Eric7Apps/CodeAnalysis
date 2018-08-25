// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



// When you use star-slash comments you usually use
// it to block out a larger section of code.  Maybe
// even hundreds of lines.  So you can't worry about
// if there is a string literal somewhere in there
// that would cause a problem.

// This removes anything inside the star-slash
// comments no matter what it is.  Whether it's in
// a string literal or not.

// In C# code I have to define the string
// with separated characters like this:
// string StarSlash = "*" + "/";
// Otherwise it will interpret it as the end
// of a comment, not as part of a string literal.



using System;
using System.Text;



namespace CodeAnalysis
{
  class RemoveStarComments
  {
  private MainForm MForm;



  private RemoveStarComments()
    {
    }



  internal RemoveStarComments( MainForm UseForm )
    {
    MForm = UseForm;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }



  internal string RemoveAllComments( string InString )
    {
    string Result = InString;

    Result = MarkLineNumbers( Result );
    Result = RemoveComments( Result );
    // Result = FixLineSplices();

    return Result;
    }



  private string MarkLineNumbers( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    string[] SplitS = InString.Split( new Char[] { '\n' } );
    int Last = SplitS.Length;
    if( Last == 0 )
      return "";

    for( int Count = 0; Count < Last; Count++ )
      {
      string Line = SplitS[Count];
      string TLine = Line.Trim();
      if( TLine.Length == 0 )
        continue;

      int LineNumber = Count + 1;
      Line = Line +
         Char.ToString( Markers.Begin ) +
         Char.ToString( Markers.TypeLineNumber ) +
         LineNumber.ToString() +
         Char.ToString( Markers.End ) +
         "\n";

      SBuilder.Append( Line );
      }

    return SBuilder.ToString();
    }



  private string RemoveComments( string InString )
    {
    // This ignores Markers.Begin, Markers.End
    // and any other markers.

    if( InString.Trim().Length == 0 )
      return "";

    StringBuilder SBuilder = new StringBuilder();

    string SlashStar = "/" + "*";
    string StarSlash = "*" + "/";

    // This replaces the comment marker strings
    // anywhere and everywhere in the file.  Whether
    // they are inside quotes or not.
    InString = InString.Replace( SlashStar, Char.ToString( Markers.SlashStar ));
    InString = InString.Replace( StarSlash, Char.ToString( Markers.StarSlash ));

    bool IsInsideComment = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];

      if( TestChar == Markers.SlashStar )
        {
        if( IsInsideComment )
          {
          // It shouldn't find this start marker
          // if it's already inside a comment.
          SBuilder.Append( Char.ToString( Markers.ErrorPoint ));

          ShowStatus( " " );
          ShowStatus( "Error with nested comment at: " + Count.ToString());
          return SBuilder.ToString();
          }

        IsInsideComment = true;
        // Replace a comment with white space so
        // that identifier strings don't come
        // together.
        SBuilder.Append( " " );
        continue;
        }

      if( TestChar == Markers.StarSlash )
        {
        IsInsideComment = false;
        continue;
        }

      if( !IsInsideComment )
        {
        if( TestChar == Markers.StarSlash )
          {
          // It shouldn't find this end marker
          // if it's not already inside a comment.
          SBuilder.Append( Char.ToString( Markers.ErrorPoint ));

          ShowStatus( " " );
          ShowStatus( "Error with start-slash outside of a comment at: " + Count.ToString());
          return SBuilder.ToString();
          }

        SBuilder.Append( Char.ToString( TestChar ));
        }
      }

    return SBuilder.ToString();
    }



/*
Do something like this?

  private void FixLineSplices()
    {
    int Last = MainSArray.GetLast();
    StringBuilder SBuilder = new StringBuilder();
    for( int Count = 0; Count < Last; Count++ )
      {
      string Line = MainSArray.GetStringAt( Count );
      // Something could be in a string literal.
      // So this would have to be tested after
      // strings are in tokens.
      if( ContainsTriGraph( Line ))
        throw( new Exception( "Is this an actual Trigraph in here?\r\n" + Line ));

      string DoubleSlash = "/" + "/";
      if( Line.Contains( DoubleSlash ))
        Line = RemoveDoubleSlashComments( Line );

      // Using \n to be compatible with Linux.
      SBuilder.Append( Line.TrimEnd() + "\n" );
      }

    string FileS = SBuilder.ToString();

    // This would be a bad idea, but somebody
    // could split a word right at the end
    // of a line and it should join the word
    // together with no space.
    // FileS = FileS.Replace( "\\\n", " " );
    FileS = FileS.Replace( "\\\n", "" );

    // Remove the beginning and ending white space.
    FileS = FileS.Trim();

    MainSArray.Clear();
    string[] FileLines = FileS.Split( new Char[] { '\n' } );

    Last = FileLines.Length;
    for( int Count = 0; Count < Last; Count++ )
      MainSArray.AppendStringToArray( FileLines[Count] );

    }



  private bool ContainsTriGraph( string Line )
    {
    // There are also DiGraphs.
    // Digraph:        <%  %>   <:  :>  %:  %:%:
    // Punctuator:      {   }   [   ]   #    ##

    // Test:
    // if( Line.Contains( "??" ))
      // return true;

    if( Line.Contains( "??=" ))
      return true;

    if( Line.Contains( "??/" ))
      return true;

    if( Line.Contains( "??'" ))
      return true;

    if( Line.Contains( "??(" ))
      return true;

    if( Line.Contains( "??)" ))
      return true;

    if( Line.Contains( "??!" ))
      return true;

    if( Line.Contains( "??<" ))
      return true;

    if( Line.Contains( "??>" ))
      return true;

    if( Line.Contains( "??-" ))
      return true;

    return false;
    }
*/



 }
}
