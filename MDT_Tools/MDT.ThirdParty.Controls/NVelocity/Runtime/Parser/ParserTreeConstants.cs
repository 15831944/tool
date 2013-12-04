using System;

namespace NVelocity.Runtime.Parser {

    /* Generated By:JJTree: Do not edit this line. ParserTreeConstants.java */
    public class ParserTreeConstants {

	/// <summary>
	/// private constructor as class is meant to hold constants only.
	/// Class was orginally an interface in Java, but as C# does not support Fields in an interface and
	/// the jjtNodeName field, I converted it to a class with no constructor.
	/// </summary>
	private ParserTreeConstants() { }

	public const int JJTPROCESS = 0;
	public const int JJTVOID = 1;
	public const int JJTESCAPEDDIRECTIVE = 2;
	public const int JJTESCAPE = 3;
	public const int JJTCOMMENT = 4;
	public const int JJTNUMBERLITERAL = 5;
	public const int JJTSTRINGLITERAL = 6;
	public const int JJTIDENTIFIER = 7;
	public const int JJTWORD = 8;
	public const int JJTDIRECTIVE = 9;
	public const int JJTBLOCK = 10;
	public const int JJTOBJECTARRAY = 11;
	public const int JJTINTEGERRANGE = 12;
	public const int JJTMETHOD = 13;
	public const int JJTREFERENCE = 14;
	public const int JJTTRUE = 15;
	public const int JJTFALSE = 16;
	public const int JJTTEXT = 17;
	public const int JJTIFSTATEMENT = 18;
	public const int JJTELSESTATEMENT = 19;
	public const int JJTELSEIFSTATEMENT = 20;
	public const int JJTSETDIRECTIVE = 21;
	public const int JJTEXPRESSION = 22;
	public const int JJTASSIGNMENT = 23;
	public const int JJTORNODE = 24;
	public const int JJTANDNODE = 25;
	public const int JJTEQNODE = 26;
	public const int JJTNENODE = 27;
	public const int JJTLTNODE = 28;
	public const int JJTGTNODE = 29;
	public const int JJTLENODE = 30;
	public const int JJTGENODE = 31;
	public const int JJTADDNODE = 32;
	public const int JJTSUBTRACTNODE = 33;
	public const int JJTMULNODE = 34;
	public const int JJTDIVNODE = 35;
	public const int JJTMODNODE = 36;
	public const int JJTNOTNODE = 37;
	public static readonly System.String[] jjtNodeName = new System.String[]{"process", "void", "EscapedDirective", "Escape", "Comment", "NumberLiteral", "StringLiteral", "Identifier", "Word", "Directive", "Block", "ObjectArray", "IntegerRange", "Method", "Reference", "True", "False", "Text", "IfStatement", "ElseStatement", "ElseIfStatement", "SetDirective", "Expression", "Assignment", "OrNode", "AndNode", "EQNode", "NENode", "LTNode", "GTNode", "LENode", "GENode", "AddNode", "SubtractNode", "MulNode", "DivNode", "ModNode", "NotNode"};
    }
}