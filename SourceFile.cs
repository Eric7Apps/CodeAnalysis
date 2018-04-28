// Copyright Eric Chauvin 2018.
// My blog is at:
// ericsourcecode.blogspot.com

// Lexical Conventions:
// https://docs.microsoft.com/en-us/cpp/cpp/lexical-conventions

// https://docs.microsoft.com/en-us/cpp/preprocessor/preprocessor-directives


// https://docs.microsoft.com/en-us/cpp/preprocessor/phases-of-translation



// C++ Language Reference:
// https://docs.microsoft.com/en-us/cpp/cpp/cpp-language-reference

// Identifiers:
// https://docs.microsoft.com/en-us/cpp/cpp/identifiers-cpp


// "A C++ identifier is a name used to identify a
// variable, function, class, module, or any other
// user-defined item. An identifier starts with a
// letter A to Z or a to z or an underscore (_)
// followed by zero or more letters, underscores,
// and digits (0 to 9).

// Like some formal Regular Expression for where
// an identifier ends.

// https://docs.microsoft.com/en-us/cpp/cpp/identifiers-cpp


// https://en.wikipedia.org/wiki/C_preprocessor



using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace CodeAnalysis
{
  class SourceFile
  {
  private MainForm MForm;
  private StringArray MainSArray;
  private string FileName = "";
  private string LocalIncludeDirectory = "";
  private SourceFileToken[] TokenArray;
  private int TokenArrayLast = 0;

  // I am using what are called the "Dingbat"
  // characters as markers or delimiters.
  private const char SlashStarDingbat = (char)0x2700;
  private const char StarSlashDingbat = (char)0x2701;
    // const char DoubleSlashDingbat = (char)0x2702;
  private const char EscapedQuoteDingbat = (char)0x2703;



  private SourceFile()
    {
    }



  internal SourceFile( MainForm UseForm,
                       string UseFileName )
    {
    MForm = UseForm;
    FileName = UseFileName;
    // If the included file has quotes around it.
    LocalIncludeDirectory =
              Path.GetDirectoryName( FileName );

    LocalIncludeDirectory += "\\";

    ShowStatus( "LocalIncludeDirectory: " +
                              LocalIncludeDirectory );


    // "If the filename is enclosed within angle
    // brackets, the file is searched for in the
    // standard compiler include paths. If the
    // filename is enclosed within double quotes,
    // the search path is expanded to include the
    // current source directory."


    // File.GetAttributes()
    // File.ReadAllBytes()
    // File.WriteAllBytes()

    // Path.ChangeExtension()
    // Path.GetExtension()
    // Path.GetFileName()
    // Path.GetFullPath()

    MainSArray = new StringArray( MForm );
    TokenArray = new SourceFileToken[2];

    }



  private void AppendToken( SourceFileToken Token )
    {
    try
    {
    TokenArray[TokenArrayLast] = Token;
    TokenArrayLast++;

    if( TokenArrayLast >= TokenArray.Length )
      {
      Array.Resize( ref TokenArray, TokenArray.Length + (1024 * 16) );
      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Could not resize the array in AppendToken()." );
      ShowStatus( Except.Message );
      }
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



  /*
  internal void ShowDingbatCharacters()
    {
    ShowStatus( "SlashStarDingbat: " +
           Char.ToString( SlashStarDingbat ));

    ShowStatus( "StarSlashDingbat: " +
           Char.ToString( StarSlashDingbat ));

    ShowStatus( "DoubleSlashDingbat: " +
           Char.ToString( DoubleSlashDingbat ));

    for( int Count = 0x2700; Count <= 0x27BF; Count++ )
      {
      char OneChar = (char)Count;
      ShowStatus( Count.ToString( "X4" ) +
             ": " + Char.ToString( OneChar ));

      }
    }
    */



  private bool ReadFromTextFile()
    {
    // try
    // {

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

        Line = Line.Replace( SlashStar, Char.ToString( SlashStarDingbat ));
        Line = Line.Replace( StarSlash, Char.ToString( StarSlashDingbat ));

        // ShowStatus( Line );

        if( !MainSArray.AppendStringToArray( Line ))
          return false;

        }
      }

    return true;
    /*
    }
    catch( Exception Except )
      {
      ShowStatus( "Could not read the file: \r\n" + FileName );
      ShowStatus( Except.Message );
      return false;
      }
      */
    }



  private bool ContainsTriGraph( string Line )
    {
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

      /*
      if( ToCheck == '\r' )
        ToCheck == ' ';

      if( ToCheck == '\n' )
        ToCheck == ' ';

      if( ToCheck == '\t' )
        ToCheck == ' ';
      */

      // Get rid of control characters.
      if( ToCheck < ' ' )
        ToCheck = ' ';

      //  Don't go higher than D800 (Surrogates).
      if( ToCheck >= 0xD800 )
        ToCheck = ' ';

      // if( (ToCheck >= 127) && (ToCheck <= 160))
      if( (ToCheck >= 127) && (ToCheck <= 255))
        ToCheck = ' ';

      // Don't exclude any characters in the Basic
      // Multilingual Plane except what are called
      // the "Dingbat" characters which are used as
      // markers or delimiters so they shouldn't
      // be in this.
      if( (ToCheck >= 0x2700) && (ToCheck <= 0x27BF))
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



  private void RemoveStarComments()
    {
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
        if( OneChar == SlashStarDingbat )
          {
          IsInsideComment = true;
          // Replace a comment with white space so
          // that token strings don't come together.
          SBuilder.Append( " " );
          continue;
          }

        if( OneChar == StarSlashDingbat )
          {
          IsInsideComment = false;
          continue;
          }

        // DoubleSlashDingbat

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
        throw( new Exception( "They used a double slash here:\r\n" + Line ));
        // So remove those comments.
        // Line = Line.Replace( DoubleSlash, Char.ToString( DoubleSlashDingbat ));

      // Or use \n to be compatible with Linux.
      SBuilder.Append( Line.TrimEnd() + "\r" );
      }

    string FileS = SBuilder.ToString();
    FileS = FileS.Replace( "\\\r", " " );

    // Remove the beginning and ending white space.
    FileS = FileS.Trim();

    MainSArray.Clear();
    string[] FileLines = FileS.Split( new Char[] { '\r' } );

    Last = FileLines.Length;
    for( int Count = 0; Count < Last; Count++ )
      MainSArray.AppendStringToArray( FileLines[Count] );

    }



  internal void DoPreprocessing()
    {
    ReadFromTextFile();
    RemoveStarComments();
    FixLineSplices();
    // ShowLines();
    MakeTokens();

    }



  private void MakeTokens()
    {
    TokenArrayLast = 0;
    SourceFileToken Token = new SourceFileToken( MForm );

    int Last = MainSArray.GetLast();
    for( int Count = 0; Count < Last; Count++ )
      {
      if( !MForm.CheckEvents())
        return;

      string Line = MainSArray.GetStringAt( Count );
      Line = Line.Trim();

      if( IsPreprocessorLine( Line ))
        {
        // ShowStatus( "Preprocessor: " + Line );
        Token.Text = Line.Trim();
        Token.TokenType = SourceFileToken.
                     EnumTokenType.PreProcessor;

        AppendToken( Token );
        AppendEndOfLineToken();
        Token = new SourceFileToken( MForm );
        continue;
        }

      MakeTokensFromLine( Line );
      }
    }



  private void MakeTokensFromLine( string Line )
    {
    Line = Line.Replace( "\\\"", Char.ToString( EscapedQuoteDingbat ));

    SourceFileToken Token = new SourceFileToken( MForm );
    StringBuilder SBuilder = new StringBuilder();

    int Last = Line.Length;
    bool IsInsideStringLiteral = false;
    bool IsInsideIdentifier = false;

    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = Line[Count];

      if( IsInsideStringLiteral )
        {
        IsInsideStringLiteral = ProcessLiteralString(
                     TestChar,
                     SBuilder,
                     Line );

        continue;
        }
      else
        {
        if( TestChar == '"' )
          {
          IsInsideStringLiteral = true;
          continue;
          }
        }

      if( IsInsideIdentifier )
        {
        IsInsideIdentifier = ProcessIdentifier(
                             TestChar,
                             SBuilder );

        continue;
        }

      if( IsIdentifierCharacter( TestChar, 0 ))
        {
        IsInsideIdentifier = true;
        SBuilder.Append( Char.ToString( TestChar ));
        }
      else
        {
        AppendCharacterToken( TestChar );
        }
      }

    AppendEndOfLineToken();
    }



  private bool ProcessIdentifier(
                     char TestChar,
                     StringBuilder SBuilder )

    {
    if( IsIdentifierCharacter( TestChar, SBuilder.Length ))
      {
      SBuilder.Append( Char.ToString( TestChar ));
      return true;
      }

    SourceFileToken Token = new SourceFileToken( MForm );
    Token.Text = SBuilder.ToString();
    SBuilder.Clear();
    Token.TokenType = SourceFileToken.
                       EnumTokenType.Identifier;

    AppendToken( Token );
    // ShowStatus( "Identifier: " + Token.Text );

    // This TestChar is not an identifier
    // character.
    AppendCharacterToken( TestChar );

    return false;
    }



  private bool ProcessLiteralString(
                     char TestChar,
                     StringBuilder SBuilder,
                     string ShowLine )
    {
    if( TestChar != '"' )
      {
      SBuilder.Append( Char.ToString( TestChar ));
      return true; // IsInsideString
      }

    SourceFileToken Token = new SourceFileToken( MForm );
    Token.Text = SBuilder.ToString();
    SBuilder.Clear();

    // Put a normal quote character back in.
    Token.Text = Token.Text.Replace( Char.ToString( EscapedQuoteDingbat ), "\"" );
    ShowStatus( " " );
    ShowStatus( "String literal: " + Token.Text );
    ShowStatus( "ShowLine: " + ShowLine );
    Token.TokenType = SourceFileToken.
                        EnumTokenType.StringLiteral;

    AppendToken( Token );
    return false; // IsInsideString
    }




  private void AppendEndOfLineToken()
    {
    // ShowStatus( "EndOfLine token." );

    SourceFileToken Token = new SourceFileToken( MForm );

    Token.Text = "";
    Token.TokenType = SourceFileToken.
                     EnumTokenType.EndOfLine;

    AppendToken( Token );
    }




  private void AppendCharacterToken( char Character )
    {
    // ShowStatus( "Single character token: " + Char.ToString( Character ));
    SourceFileToken Token = new SourceFileToken( MForm );

    Token.Text = Char.ToString( Character );
    Token.TokenType = SourceFileToken.
                     EnumTokenType.Character;

    AppendToken( Token );
    }



  private bool IsPreprocessorLine( string Line )
    {
    // There can only be one preprocessor statement
    // per line.

    // Line = Line.Trim();
    if( Line.StartsWith( "#" ))
      return true;

    return false;
    }



  private bool IsIdentifierCharacter( char ToTest,
                                      int Where )
    {
    if( Where > (1024 + 512))
      throw( new Exception( "This identifier is way too long." ));

    if( Where == 0 )
      {
      if( IsNumeral( ToTest ))
        return false;

      }

    if( (ToTest == '_') ||
         IsLetter( ToTest ) ||
         IsNumeral( ToTest ))
      return true;

    return false;
    }



  private bool IsNumeral( char ToTest )
    {
    if( (ToTest >= '0') && (ToTest <= '9'))
      return true;

    return false;
    }



  private bool IsLetter( char ToTest )
    {
    if( (ToTest >= 'a') && (ToTest <= 'z'))
      return true;

    if( (ToTest >= 'A') && (ToTest <= 'Z'))
      return true;

    // You could add Chinese characters or something
    // like that, so it's not just ASCII letters.

    return false;
    }



 }
}
