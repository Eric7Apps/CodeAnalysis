// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  class CSharpRemoveComments
  {
  private MainForm MForm;



  private CSharpRemoveComments()
    {
    }



  internal CSharpRemoveComments( MainForm UseForm )
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
    Result = RemoveStarComments( Result );
    // Result = FixLineSplices();
    Result = RemoveAllDoubleSlashComments( Result );
    return Result;
    }



  private string RemoveStarComments( string InString )
    {
    // According to the Gnu Gcc documentation, it
    // says that "Comments are not recognized within
    // string literals."
    // Notice how I have quoted text in the two
    // lines above, where I quoted the Gnu Gcc
    // documentation.  They are comments.  They are
    // not string literals.
    // In C# code I have to define the string
    // with separated characters like this:
    // string StarSlash = "*" + "/";
    // Otherwise it will interpret it as the end
    // of a comment.  I am removing all commented
    // out text before I look for string literals.

    StringBuilder SBuilder = new StringBuilder();

    string SlashStar = "/" + "*";
    string StarSlash = "*" + "/";

    InString = InString.Replace( SlashStar, Char.ToString( Markers.SlashStar ));
    InString = InString.Replace( StarSlash, Char.ToString( Markers.StarSlash ));

    bool IsInsideComment = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char OneChar = InString[Count];
      if( OneChar == Markers.SlashStar )
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

      if( OneChar == Markers.StarSlash )
        {
        IsInsideComment = false;
        continue;
        }

      if( !IsInsideComment )
        SBuilder.Append( Char.ToString( OneChar ));

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



  private string RemoveAllDoubleSlashComments( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();
    string[] SplitS = InString.Split( new Char[] { '\n' } );
    int Last = SplitS.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      string Line = SplitS[Count];
      Line = RemoveDoubleSlashComments( Line );

      string StartString =
              Char.ToString( Markers.Begin ) +
              Char.ToString( Markers.TypeLineNumber );

      string TLine = Line.Trim();
      if( !TLine.StartsWith( StartString ))
        SBuilder.Append( Line + "\n" );

      }

    return SBuilder.ToString();
    }



  private string RemoveDoubleSlashComments( string Line )
    {
    StringBuilder SBuilder = new StringBuilder();

    string DoubleSlash = "/" + "/";
    Line = Line.Replace( DoubleSlash, Char.ToString( Markers.DoubleSlash ));
    int LineLength = Line.Length;
    bool IsInside = true;
    for( int Count = 0; Count < LineLength; Count++ )
      {
      char OneChar = Line[Count];
      if( OneChar == Markers.DoubleSlash )
        IsInside = false;

      // This is for the line number markers.
      if( OneChar == Markers.Begin )
        IsInside = true;

      if( IsInside )
        SBuilder.Append( Char.ToString( OneChar ));

      }

    return SBuilder.ToString();
    }



  private string MarkLineNumbers( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    string[] SplitS = InString.Split( new Char[] { '\n' } );
    int Last = SplitS.Length;
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




 }
}
