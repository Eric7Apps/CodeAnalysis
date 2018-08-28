// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/




using System;
using System.Text;
using System.Collections.Generic;



namespace CodeAnalysis
{
  class IDDictionary
  {
  private MainForm MForm;
  private SortedDictionary<string, string> IdentDictionary;


  private IDDictionary()
    {
    }



  internal IDDictionary( MainForm UseForm )
    {
    MForm = UseForm;
    IdentDictionary = new SortedDictionary<string, string>();
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }



  internal bool AddIdentifier( string ID )
    {
    ID = ID.Trim();
    if( ID.Length == 0 )
      {
      ShowStatus( "Can't add a zero length ID." );
      return false;
      }

    string IDLower = ID.ToLower();
    if( IdentDictionary.ContainsKey( IDLower ))
      {
      string Value = IdentDictionary[IDLower];
      if( Value != ID )
        {
        // This means you can't use identifiers
        // with a different case.

        ShowStatus( "ID case match: " + Value );
        return true; // It is just a warning for now.
        }

      // The same thing is already there.
      return true;
      }

    IdentDictionary[IDLower] = ID;
    return true;
    }



  internal void ShowIDs()
    {
    ShowStatus( " " );
    ShowStatus( " " );
    ShowStatus( "IDs:" );

    foreach( KeyValuePair<string, string> Kvp in IdentDictionary )
      {
      if( !MForm.CheckEvents())
        return;

      if( IsKeyWord( Kvp.Key ))
        continue;

      ShowStatus( Kvp.Key );
      }
    }



  internal static bool IsKeyWord( string ToCheck )
    {
    // These are commonly used key words, in several
    // languages, that should not be used as
    // identifiers.

    if( ToCheck == "abstract" )
      return true;

    // if( ToCheck == "add" )
      // return true;

    if( ToCheck == "as" )
      return true;

    if( ToCheck == "alias" )
      return true;

    if( ToCheck == "auto" )
      return true;

    if( ToCheck == "base" )
      return true;

    if( ToCheck == "bool" )
      return true;

    if( ToCheck == "break" )
      return true;

    if( ToCheck == "byte" )
      return true;

    if( ToCheck == "case" )
      return true;

    if( ToCheck == "catch" )
      return true;

    if( ToCheck == "char" )
      return true;

    if( ToCheck == "checked" )
      return true;

    if( ToCheck == "class" )
      return true;

    if( ToCheck == "const" )
      return true;

    if( ToCheck == "continue" )
      return true;

    if( ToCheck == "decimal" )
      return true;

    if( ToCheck == "default" )
      return true;

    if( ToCheck == "delegate" )
      return true;

    if( ToCheck == "do" )
      return true;

    if( ToCheck == "double" )
      return true;

    if( ToCheck == "else" )
      return true;

    if( ToCheck == "enum" )
      return true;

    if( ToCheck == "event" )
      return true;

    if( ToCheck == "except" )
      return true;

    if( ToCheck == "explicit" )
      return true;

    if( ToCheck == "extern" )
      return true;

    if( ToCheck == "false" )
      return true;

    if( ToCheck == "finally" )
      return true;

    if( ToCheck == "fixed" )
      return true;

    if( ToCheck == "float" )
      return true;

    if( ToCheck == "for" )
      return true;

    if( ToCheck == "foreach" )
      return true;

    if( ToCheck == "get" )
      return true;

    if( ToCheck == "global" )
      return true;

    if( ToCheck == "goto" )
      return true;

    if( ToCheck == "if" )
      return true;

    if( ToCheck == "implicit" )
      return true;

    if( ToCheck == "in" )
      return true;

    if( ToCheck == "int" )
      return true;

    if( ToCheck == "interface" )
      return true;

    if( ToCheck == "internal" )
      return true;

    if( ToCheck == "is" )
      return true;

    if( ToCheck == "lock" )
      return true;

    if( ToCheck == "long" )
      return true;

    if( ToCheck == "namespace" )
      return true;

    if( ToCheck == "new" )
      return true;

    if( ToCheck == "null" )
      return true;

    if( ToCheck == "object" )
      return true;

    if( ToCheck == "operator" )
      return true;

    if( ToCheck == "out" )
      return true;

    if( ToCheck == "override" )
      return true;

    if( ToCheck == "params" )
      return true;

    if( ToCheck == "partial" )
      return true;

    if( ToCheck == "private" )
      return true;

    if( ToCheck == "protected" )
      return true;

    if( ToCheck == "public" )
      return true;

    if( ToCheck == "readonly" )
      return true;

    if( ToCheck == "ref" )
      return true;

    if( ToCheck == "register" )
      return true;

    if( ToCheck == "remove" )
      return true;

    if( ToCheck == "return" )
      return true;

    if( ToCheck == "sbyte" )
      return true;

    if( ToCheck == "sealed" )
      return true;

    if( ToCheck == "set" )
      return true;

    if( ToCheck == "short" )
      return true;

    if( ToCheck == "signed" )
      return true;

    if( ToCheck == "sizeof" )
      return true;

    if( ToCheck == "stackalloc" )
      return true;

    if( ToCheck == "static" )
      return true;

    if( ToCheck == "string" )
      return true;

    if( ToCheck == "struct" )
      return true;

    if( ToCheck == "switch" )
      return true;

    if( ToCheck == "this" )
      return true;

    if( ToCheck == "throw" )
      return true;

    if( ToCheck == "true" )
      return true;

    if( ToCheck == "try" )
      return true;

    if( ToCheck == "typedef" )
      return true;

    if( ToCheck == "typeof" )
      return true;

    if( ToCheck == "uint" )
      return true;

    if( ToCheck == "ulong" )
      return true;

    if( ToCheck == "unchecked" )
      return true;

    if( ToCheck == "union" )
      return true;

    if( ToCheck == "unsafe" )
      return true;

    if( ToCheck == "unsigned" )
      return true;

    if( ToCheck == "ushort" )
      return true;

    if( ToCheck == "using" )
      return true;

    if( ToCheck == "value" )
      return true;

    if( ToCheck == "var" )
      return true;

    if( ToCheck == "virtual" )
      return true;

    if( ToCheck == "void" )
      return true;

    if( ToCheck == "volatile" )
      return true;

    if( ToCheck == "where" )
      return true;

    if( ToCheck == "while" )
      return true;

    if( ToCheck == "yield" )
      return true;

    return false;
    }



  }
}

