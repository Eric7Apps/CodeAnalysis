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


  private SourceFile()
    {
    }



  internal SourceFile( MainForm UseForm )
    {
    MForm = UseForm;
    }




  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }




  internal string ReadFromTextFile( string FileName )
    {
    try
    {
    // File.GetAttributes()
    // File.ReadAllBytes()
    // File.WriteAllBytes()
    // Path.ChangeExtension()
    // Path.GetExtension()
    // Path.GetFileName()
    // Path.GetFullPath()

    if( !File.Exists( FileName ))
      {
      ShowStatus( "The file doesn't exist." );
      ShowStatus( FileName );
      return "";
      }

    StringBuilder SBuilder = new StringBuilder();

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

        // ShowStatus( Line );

        SBuilder.Append( Line + "\n" );
        }
      }

    return SBuilder.ToString();
    }
    catch( Exception Except )
      {
      ShowStatus( "Could not read the file: \r\n" + FileName );
      ShowStatus( Except.Message );
      return "";
      }
    }



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
      // Multilingual Plane except markers that
      // shouldn't be in this.  See the Markers.cs
      // file.
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



 }
}
