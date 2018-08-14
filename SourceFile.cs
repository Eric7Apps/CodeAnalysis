// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;
using System.IO;



namespace CodeAnalysis
{
  class SourceFile
  {
  private MainForm MForm;
  private StringArray MainSArray;
  private string FileName = "";

  private const char SlashStarMarker = (char)0x2700;
  private const char StarSlashMarker = (char)0x2701;
  private const char DoubleSlashMarker = (char)0x2702;


  private SourceFile()
    {
    }



  internal SourceFile( MainForm UseForm,
                       string UseFileName )
    {
    MForm = UseForm;
    FileName = UseFileName;

    // File.GetAttributes()
    // File.ReadAllBytes()
    // File.WriteAllBytes()
    // Path.ChangeExtension()
    // Path.GetExtension()
    // Path.GetFileName()
    // Path.GetFullPath()

    MainSArray = new StringArray( MForm );
    }




  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }




  internal bool ReadFromTextFile()
    {
    try
    {
    MainSArray.Clear();
    if( !File.Exists( FileName ))
      {
      ShowStatus( "The file doesn't exist." );
      ShowStatus( FileName );
      return false;
      }

    using( StreamReader SReader = new StreamReader( FileName, Encoding.UTF8 ))
      {
      while( SReader.Peek() >= 0 )
        {
        string Line = SReader.ReadLine();
        if( Line == null )
          continue;

        // This is the first level of lexical analysis
        // and processing.
        Line = GetCleanUnicodeString( Line );
        Line = Line.TrimEnd();

        string SlashStar = "/" + "*";
        string StarSlash = "*" + "/";

        Line = Line.Replace( SlashStar, Char.ToString( SlashStarMarker ));
        Line = Line.Replace( StarSlash, Char.ToString( StarSlashMarker ));

        // ShowStatus( Line );

        if( !MainSArray.AppendStringToArray( Line ))
          return false;

        }
      }

    return true;
    }
    catch( Exception Except )
      {
      ShowStatus( "Could not read the file: \r\n" + FileName );
      ShowStatus( Except.Message );
      return false;
      }
    }


/*
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



  // This is the first level of lexical analysis
  // and processing.
  private string GetCleanUnicodeString( string InString )
    {
    if( InString == null )
      return "";

    StringBuilder SBuilder = new StringBuilder();
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      char ToCheck = InString[Count];

      // Get rid of control characters.
      // This also replaces a tab with a space.
      if( ToCheck < ' ' )
        ToCheck = ' ';

      //  Don't go higher than D800 (Surrogates).
      if( ToCheck >= 0xD800 )
        ToCheck = ' ';

      // This has already been converted from UTF8
      // to 16 bit characters.
      // if( (ToCheck >= 127) && (ToCheck <= 160))
      if( (ToCheck >= 127) && (ToCheck <= 255))
        ToCheck = ' ';

      // Don't exclude any characters in the Basic
      // Multilingual Plane except what are called
      // the "Dingbat" characters which are used as
      // markers or delimiters so they shouldn't
      // be in this.  See the Markers.cs file.
      if( Markers.IsMarker( ToCheck ))
        ToCheck = ' ';

      // Basic Multilingual Plane
      // C0 Controls and Basic Latin (Basic Latin) (0000–007F)
      // C1 Controls and Latin-1 Supplement (0080–00FF)
      // Latin Extended-A (0100–017F)
      // Latin Extended-B (0180–024F)
      // IPA Extensions (0250–02AF)
      // Spacing Modifier Letters (02B0–02FF)
      // Combining Diacritical Marks (0300–036F)
      // General Punctuation (2000–206F)
      // Superscripts and Subscripts (2070–209F)
      // Currency Symbols (20A0–20CF)
      // Combining Diacritical Marks for Symbols (20D0–20FF)
      // Letterlike Symbols (2100–214F)
      // Number Forms (2150–218F)
      // Arrows (2190–21FF)
      // Mathematical Operators (2200–22FF)
      // Box Drawing (2500–257F)
      // Geometric Shapes (25A0–25FF)
      // Miscellaneous Symbols (2600–26FF)
      // Dingbats (2700–27BF)
      // Miscellaneous Symbols and Arrows (2B00–2BFF)

      SBuilder.Append( Char.ToString( ToCheck ));
      }

    return SBuilder.ToString();
    }



  internal void RemoveStarComments()
    {
    // According to the Gnu Gcc documentation, it
    // says that "Comments are not recognized within
    // string literals."
    // Notice how I have quoted text in the two
    // lines above, where I quoted the Gnu Gcc
    // documentation.  They are comments.  They are
    // not string literals.
    // In this C# code I have to define the string
    // with separated characters like this:
    // string StarSlash = "*" + "/";
    // Otherwise it will interpret it as the end
    // of a comment.  I am removing all commented
    // out text before I look for string literals.

    StringBuilder SBuilder = new StringBuilder();
    int Last = MainSArray.GetLast();
    bool IsInsideComment = false;
    for( int Count = 0; Count < Last; Count++ )
      {
      string Line = MainSArray.GetStringAt( Count );
      SBuilder.Clear();
      int LineLength = Line.Length;
      for( int CharCount = 0; CharCount < LineLength; CharCount++ )
        {
        char OneChar = Line[CharCount];
        if( OneChar == SlashStarMarker )
          {
          if( IsInsideComment )
            {
            // It shouldn't find this start marker
            // if it's already inside a comment.
            SBuilder.Append( Char.ToString( Markers.ErrorPoint ));
            MainSArray.SetStringAt(
                       SBuilder.ToString(), Count );

            ShowStatus( " " );
            ShowStatus( "Error with nested comment at: " + Count.ToString());
            return;
            }

          IsInsideComment = true;
          // Replace a comment with white space so
          // that identifier strings don't come
          // together.
          SBuilder.Append( " " );
          continue;
          }

        if( OneChar == StarSlashMarker )
          {
          IsInsideComment = false;
          continue;
          }

        if( !IsInsideComment )
          SBuilder.Append( Char.ToString( OneChar ));

        }

      MainSArray.SetStringAt(
                       SBuilder.ToString(), Count );

      }
    }




  internal void ShowLines()
    {
    int Last = MainSArray.GetLast();
    for( int Count = 0; Count < Last; Count++ )
      {
      string Line = MainSArray.GetStringAt( Count );
      ShowStatus( Line );
      }
    }



/*
Use this?

  internal void FixLineSplices()
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
*/



  internal void RemoveAllDoubleSlashComments()
    {
    int Last = MainSArray.GetLast();
    for( int Count = 0; Count < Last; Count++ )
      {
      string Line = MainSArray.GetStringAt( Count );
      Line = RemoveDoubleSlashComments( Line );
      MainSArray.SetStringAt( Line, Count );
      }
    }



  private string RemoveDoubleSlashComments( string Line )
    {
    string DoubleSlash = "/" + "/";
    Line = Line.Replace( DoubleSlash, Char.ToString( DoubleSlashMarker ));
    StringBuilder SBuilder = new StringBuilder();
    int LineLength = Line.Length;
    for( int Count = 0; Count < LineLength; Count++ )
      {
      char OneChar = Line[Count];
      if( OneChar == DoubleSlashMarker )
        break;

      SBuilder.Append( Char.ToString( OneChar ));
      }

    return SBuilder.ToString();
    }



  internal string GetFileAsString()
    {
    StringBuilder SBuilder = new StringBuilder();

    int Last = MainSArray.GetLast();
    for( int Count = 0; Count < Last; Count++ )
      {
      string Line = MainSArray.GetStringAt( Count );
      if( Line.Length == 0 )
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
