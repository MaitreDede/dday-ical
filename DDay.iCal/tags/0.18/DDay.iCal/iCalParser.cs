// $ANTLR 2.7.6 (2005-12-22): "iCal.g" -> "iCalParser.cs"$

    using DDay.iCal.Objects;
    using DDay.iCal.Components;

namespace DDay.iCal
{
	// Generate the header common to all output files.
	using System;
	
	using TokenBuffer              = antlr.TokenBuffer;
	using TokenStreamException     = antlr.TokenStreamException;
	using TokenStreamIOException   = antlr.TokenStreamIOException;
	using ANTLRException           = antlr.ANTLRException;
	using LLkParser = antlr.LLkParser;
	using Token                    = antlr.Token;
	using IToken                   = antlr.IToken;
	using TokenStream              = antlr.TokenStream;
	using RecognitionException     = antlr.RecognitionException;
	using NoViableAltException     = antlr.NoViableAltException;
	using MismatchedTokenException = antlr.MismatchedTokenException;
	using SemanticException        = antlr.SemanticException;
	using ParserSharedInputState   = antlr.ParserSharedInputState;
	using BitSet                   = antlr.collections.impl.BitSet;
	
	public partial class iCalParser : antlr.LLkParser
	{
		public const int EOF = 1;
		public const int NULL_TREE_LOOKAHEAD = 3;
		public const int BEGIN = 4;
		public const int COLON = 5;
		public const int VCALENDAR = 6;
		public const int CRLF = 7;
		public const int END = 8;
		public const int IANA_TOKEN = 9;
		public const int X_NAME = 10;
		public const int PRODID = 11;
		public const int SEMICOLON = 12;
		public const int VERSION = 13;
		public const int CALSCALE = 14;
		public const int METHOD = 15;
		public const int EQUAL = 16;
		public const int COMMA = 17;
		public const int DQUOTE = 18;
		public const int CTL = 19;
		public const int BACKSLASH = 20;
		public const int ESCAPED_CHAR = 21;
		public const int NUMBER = 22;
		public const int DOT = 23;
		public const int CR = 24;
		public const int LF = 25;
		public const int ALPHA = 26;
		public const int DIGIT = 27;
		public const int DASH = 28;
		public const int SPECIAL = 29;
		public const int SPACE = 30;
		public const int HTAB = 31;
		public const int SLASH = 32;
		public const int LINEFOLDER = 33;
		
		
		protected void initialize()
		{
			tokenNames = tokenNames_;
		}
		
		
		protected iCalParser(TokenBuffer tokenBuf, int k) : base(tokenBuf, k)
		{
			initialize();
		}
		
		public iCalParser(TokenBuffer tokenBuf) : this(tokenBuf,3)
		{
		}
		
		protected iCalParser(TokenStream lexer, int k) : base(lexer,k)
		{
			initialize();
		}
		
		public iCalParser(TokenStream lexer) : this(lexer,3)
		{
		}
		
		public iCalParser(ParserSharedInputState state) : base(state,3)
		{
			initialize();
		}
		
	public iCalendar  icalobject() //throws RecognitionException, TokenStreamException
{
		iCalendar iCal = (iCalendar)Activator.CreateInstance(iCalendarType);;
		
		
		try {      // for error handling
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==BEGIN))
					{
						match(BEGIN);
						match(COLON);
						match(VCALENDAR);
						match(CRLF);
						icalbody(iCal);
						match(END);
						match(COLON);
						match(VCALENDAR);
						match(CRLF);
					}
					else
					{
						goto _loop3_breakloop;
					}
					
				}
_loop3_breakloop:				;
			}    // ( ... )*
			iCal.OnLoad(EventArgs.Empty);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_0_);
		}
		return iCal;
	}
	
	public void icalbody(
		iCalendar iCal
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			{
				switch ( LA(1) )
				{
				case IANA_TOKEN:
				case X_NAME:
				case PRODID:
				case VERSION:
				case CALSCALE:
				case METHOD:
				{
					calprops(iCal);
					break;
				}
				case BEGIN:
				case END:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
			{
				switch ( LA(1) )
				{
				case BEGIN:
				{
					component(iCal);
					break;
				}
				case END:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_1_);
		}
	}
	
	public void calprops(
		iCalendar iCal
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			calprop(iCal);
			{ // ( ... )+
				int _cnt18=0;
				for (;;)
				{
					if ((tokenSet_2_.member(LA(1))))
					{
						calprop(iCal);
					}
					else
					{
						if (_cnt18 >= 1) { goto _loop18_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
					}
					
					_cnt18++;
				}
_loop18_breakloop:				;
			}    // ( ... )+
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_3_);
		}
	}
	
	public void component(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			{ // ( ... )+
				int _cnt9=0;
				for (;;)
				{
					if ((LA(1)==BEGIN) && (LA(2)==COLON) && (LA(3)==IANA_TOKEN))
					{
						iana_comp(o);
					}
					else if ((LA(1)==BEGIN) && (LA(2)==COLON) && (LA(3)==X_NAME)) {
						x_comp(o);
					}
					else
					{
						if (_cnt9 >= 1) { goto _loop9_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
					}
					
					_cnt9++;
				}
_loop9_breakloop:				;
			}    // ( ... )+
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_4_);
		}
	}
	
	public void iana_comp(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		IToken  n = null;
		ComponentBase c;
		
		try {      // for error handling
			match(BEGIN);
			match(COLON);
			n = LT(1);
			match(IANA_TOKEN);
			c = o.iCalendar.Create(o, n.getText());
			match(CRLF);
			{ // ( ... )+
				int _cnt12=0;
				for (;;)
				{
					if ((LA(1)==BEGIN||LA(1)==IANA_TOKEN||LA(1)==X_NAME))
					{
						calendarline(c);
					}
					else
					{
						if (_cnt12 >= 1) { goto _loop12_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
					}
					
					_cnt12++;
				}
_loop12_breakloop:				;
			}    // ( ... )+
			match(END);
			match(COLON);
			match(IANA_TOKEN);
			match(CRLF);
			c.OnLoad(EventArgs.Empty);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_4_);
		}
	}
	
	public void x_comp(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		IToken  n = null;
		ComponentBase c;
		
		try {      // for error handling
			match(BEGIN);
			match(COLON);
			n = LT(1);
			match(X_NAME);
			c = o.iCalendar.Create(o, n.getText());
			match(CRLF);
			{ // ( ... )+
				int _cnt15=0;
				for (;;)
				{
					if ((LA(1)==BEGIN||LA(1)==IANA_TOKEN||LA(1)==X_NAME))
					{
						calendarline(c);
					}
					else
					{
						if (_cnt15 >= 1) { goto _loop15_breakloop; } else { throw new NoViableAltException(LT(1), getFilename());; }
					}
					
					_cnt15++;
				}
_loop15_breakloop:				;
			}    // ( ... )+
			match(END);
			match(COLON);
			match(X_NAME);
			match(CRLF);
			c.OnLoad(EventArgs.Empty);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_4_);
		}
	}
	
	public void calendarline(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case IANA_TOKEN:
			case X_NAME:
			{
				contentline(o);
				break;
			}
			case BEGIN:
			{
				component(o);
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_4_);
		}
	}
	
	public void calprop(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case PRODID:
			{
				prodid(o);
				break;
			}
			case VERSION:
			{
				version(o);
				break;
			}
			case CALSCALE:
			{
				calscale(o);
				break;
			}
			case METHOD:
			{
				method(o);
				break;
			}
			case IANA_TOKEN:
			{
				iana_prop(o);
				break;
			}
			case X_NAME:
			{
				x_prop(o);
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_5_);
		}
	}
	
	public void prodid(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		IToken  n = null;
		Property p; string v;
		
		try {      // for error handling
			n = LT(1);
			match(PRODID);
			p = new Property(o);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==SEMICOLON))
					{
						match(SEMICOLON);
						xparam(p);
					}
					else
					{
						goto _loop22_breakloop;
					}
					
				}
_loop22_breakloop:				;
			}    // ( ... )*
			match(COLON);
			v=text();
			p.Name = n.getText(); p.Value = v; p.AddToParent();
			match(CRLF);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_5_);
		}
	}
	
	public void version(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		IToken  n = null;
		Property p; string v;
		
		try {      // for error handling
			n = LT(1);
			match(VERSION);
			p = new Property(o);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==SEMICOLON))
					{
						match(SEMICOLON);
						xparam(p);
					}
					else
					{
						goto _loop25_breakloop;
					}
					
				}
_loop25_breakloop:				;
			}    // ( ... )*
			match(COLON);
			v=version_number();
			p.Name = n.getText(); p.Value = v; p.AddToParent();
			match(CRLF);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_5_);
		}
	}
	
	public void calscale(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		IToken  n = null;
		IToken  v = null;
		Property p;
		
		try {      // for error handling
			n = LT(1);
			match(CALSCALE);
			p = new Property(o);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==SEMICOLON))
					{
						match(SEMICOLON);
						xparam(p);
					}
					else
					{
						goto _loop28_breakloop;
					}
					
				}
_loop28_breakloop:				;
			}    // ( ... )*
			match(COLON);
			v = LT(1);
			match(IANA_TOKEN);
			p.Name = n.getText(); p.Value = v.getText(); p.AddToParent();
			match(CRLF);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_5_);
		}
	}
	
	public void method(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		IToken  n = null;
		IToken  v = null;
		Property p;
		
		try {      // for error handling
			n = LT(1);
			match(METHOD);
			p = new Property(o);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==SEMICOLON))
					{
						match(SEMICOLON);
						xparam(p);
					}
					else
					{
						goto _loop31_breakloop;
					}
					
				}
_loop31_breakloop:				;
			}    // ( ... )*
			match(COLON);
			v = LT(1);
			match(IANA_TOKEN);
			p.Name = n.getText(); p.Value = v.getText(); p.AddToParent();
			match(CRLF);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_5_);
		}
	}
	
	public void iana_prop(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		IToken  n = null;
		Property p; string v;
		
		try {      // for error handling
			n = LT(1);
			match(IANA_TOKEN);
			p = new Property(o);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==SEMICOLON))
					{
						match(SEMICOLON);
						xparam(p);
					}
					else
					{
						goto _loop34_breakloop;
					}
					
				}
_loop34_breakloop:				;
			}    // ( ... )*
			match(COLON);
			v=text();
			p.Name = n.getText(); p.Value = v; p.AddToParent();
			match(CRLF);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_5_);
		}
	}
	
	public void x_prop(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		IToken  n = null;
		Property p; string v;
		
		try {      // for error handling
			n = LT(1);
			match(X_NAME);
			p = new Property(o);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==SEMICOLON))
					{
						match(SEMICOLON);
						xparam(p);
					}
					else
					{
						goto _loop37_breakloop;
					}
					
				}
_loop37_breakloop:				;
			}    // ( ... )*
			match(COLON);
			v=text();
			p.Name = n.getText(); p.Value = v; p.AddToParent();
			match(CRLF);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_5_);
		}
	}
	
	public void xparam(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		IToken  n = null;
		Parameter p; string v;
		
		try {      // for error handling
			n = LT(1);
			match(X_NAME);
			p = new Parameter(o, n.getText());
			match(EQUAL);
			v=param_value();
			p.Values.Add(v);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==COMMA))
					{
						match(COMMA);
						v=param_value();
						p.Values.Add(v);
					}
					else
					{
						goto _loop75_breakloop;
					}
					
				}
_loop75_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_6_);
		}
	}
	
	public string  text() //throws RecognitionException, TokenStreamException
{
		string s = string.Empty;
		
		string t;
		
		try {      // for error handling
			{    // ( ... )*
				for (;;)
				{
					if ((tokenSet_7_.member(LA(1))))
					{
						t=text_char();
						s += t;
					}
					else
					{
						goto _loop68_breakloop;
					}
					
				}
_loop68_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_8_);
		}
		return s;
	}
	
	public string  version_number() //throws RecognitionException, TokenStreamException
{
		string s = string.Empty;
		
		string t;
		
		try {      // for error handling
			t=number();
			s += t;
			{
				switch ( LA(1) )
				{
				case SEMICOLON:
				{
					match(SEMICOLON);
					s += ";";
					t=number();
					s += t;
					break;
				}
				case CRLF:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_8_);
		}
		return s;
	}
	
	public void contentline(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		
		ContentLine c = new ContentLine(o);
		string n;
		string v;
		
		
		try {      // for error handling
			n=name();
			c.Name = n;
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==SEMICOLON))
					{
						match(SEMICOLON);
						param(c);
					}
					else
					{
						goto _loop41_breakloop;
					}
					
				}
_loop41_breakloop:				;
			}    // ( ... )*
			match(COLON);
			v=value();
			c.Value = v; o.SetContentLineValue(c);
			match(CRLF);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_4_);
		}
	}
	
	public string  name() //throws RecognitionException, TokenStreamException
{
		string s = string.Empty;;
		
		IToken  x = null;
		IToken  i = null;
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case X_NAME:
			{
				x = LT(1);
				match(X_NAME);
				s = x.getText();
				break;
			}
			case IANA_TOKEN:
			{
				i = LT(1);
				match(IANA_TOKEN);
				s = i.getText();
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_6_);
		}
		return s;
	}
	
	public void param(
		iCalObject o
	) //throws RecognitionException, TokenStreamException
{
		
		Parameter p; string n; string v;
		
		try {      // for error handling
			n=param_name();
			p = new Parameter(o, n);
			match(EQUAL);
			v=param_value();
			p.Values.Add(v);
			{    // ( ... )*
				for (;;)
				{
					if ((LA(1)==COMMA))
					{
						match(COMMA);
						v=param_value();
						p.Values.Add(v);
					}
					else
					{
						goto _loop45_breakloop;
					}
					
				}
_loop45_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_6_);
		}
	}
	
	public string  value() //throws RecognitionException, TokenStreamException
{
		string v = string.Empty;
		
		string c;
		
		try {      // for error handling
			{    // ( ... )*
				for (;;)
				{
					if ((tokenSet_9_.member(LA(1))))
					{
						c=value_char();
						v += c;
					}
					else
					{
						goto _loop53_breakloop;
					}
					
				}
_loop53_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_8_);
		}
		return v;
	}
	
	public string  param_name() //throws RecognitionException, TokenStreamException
{
		string n = string.Empty;;
		
		IToken  i = null;
		IToken  x = null;
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case IANA_TOKEN:
			{
				i = LT(1);
				match(IANA_TOKEN);
				n = i.getText();
				break;
			}
			case X_NAME:
			{
				x = LT(1);
				match(X_NAME);
				n = x.getText();
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_10_);
		}
		return n;
	}
	
	public string  param_value() //throws RecognitionException, TokenStreamException
{
		string v = string.Empty;;
		
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case BEGIN:
			case COLON:
			case VCALENDAR:
			case END:
			case IANA_TOKEN:
			case X_NAME:
			case PRODID:
			case SEMICOLON:
			case VERSION:
			case CALSCALE:
			case METHOD:
			case EQUAL:
			case COMMA:
			case BACKSLASH:
			case ESCAPED_CHAR:
			case NUMBER:
			case DOT:
			case CR:
			case LF:
			case ALPHA:
			case DIGIT:
			case DASH:
			case SPECIAL:
			case SPACE:
			case HTAB:
			case SLASH:
			case LINEFOLDER:
			{
				v=paramtext();
				break;
			}
			case DQUOTE:
			{
				v=quoted_string();
				break;
			}
			default:
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			 }
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_11_);
		}
		return v;
	}
	
	public string  paramtext() //throws RecognitionException, TokenStreamException
{
		string t = string.Empty;
		
		string c;
		
		try {      // for error handling
			{    // ( ... )*
				for (;;)
				{
					if ((tokenSet_12_.member(LA(1))))
					{
						c=safe_char();
						t += c;
					}
					else
					{
						goto _loop50_breakloop;
					}
					
				}
_loop50_breakloop:				;
			}    // ( ... )*
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_11_);
		}
		return t;
	}
	
	public string  quoted_string() //throws RecognitionException, TokenStreamException
{
		string s = string.Empty;
		
		string c;
		
		try {      // for error handling
			match(DQUOTE);
			{    // ( ... )*
				for (;;)
				{
					if ((tokenSet_13_.member(LA(1))))
					{
						c=qsafe_char();
						s += c;
					}
					else
					{
						goto _loop56_breakloop;
					}
					
				}
_loop56_breakloop:				;
			}    // ( ... )*
			match(DQUOTE);
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_11_);
		}
		return s;
	}
	
	public string  safe_char() //throws RecognitionException, TokenStreamException
{
		string c = string.Empty;
		
		IToken  a = null;
		
		try {      // for error handling
			{
				a = LT(1);
				match(tokenSet_12_);
			}
			c = a.getText();
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_13_);
		}
		return c;
	}
	
	public string  value_char() //throws RecognitionException, TokenStreamException
{
		string c = string.Empty;
		
		IToken  a = null;
		
		try {      // for error handling
			{
				a = LT(1);
				match(tokenSet_9_);
			}
			c = a.getText();
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_14_);
		}
		return c;
	}
	
	public string  qsafe_char() //throws RecognitionException, TokenStreamException
{
		string c = string.Empty;
		
		IToken  a = null;
		
		try {      // for error handling
			{
				a = LT(1);
				match(tokenSet_13_);
			}
			c = a.getText();
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_9_);
		}
		return c;
	}
	
	public string  tsafe_char() //throws RecognitionException, TokenStreamException
{
		string s = string.Empty;
		
		IToken  a = null;
		
		try {      // for error handling
			{
				a = LT(1);
				match(tokenSet_15_);
			}
			s = a.getText();
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_16_);
		}
		return s;
	}
	
	public string  text_char() //throws RecognitionException, TokenStreamException
{
		string s = string.Empty;
		
		IToken  c = null;
		IToken  q = null;
		IToken  e = null;
		
		try {      // for error handling
			switch ( LA(1) )
			{
			case COLON:
			{
				c = LT(1);
				match(COLON);
				s = c.getText();
				break;
			}
			case DQUOTE:
			{
				q = LT(1);
				match(DQUOTE);
				s = q.getText();
				break;
			}
			default:
				if ((tokenSet_15_.member(LA(1))) && (tokenSet_16_.member(LA(2))) && (tokenSet_16_.member(LA(3))))
				{
					s=tsafe_char();
				}
				else if ((LA(1)==ESCAPED_CHAR) && (tokenSet_16_.member(LA(2))) && (tokenSet_16_.member(LA(3)))) {
					e = LT(1);
					match(ESCAPED_CHAR);
					s = e.getText();
				}
			else
			{
				throw new NoViableAltException(LT(1), getFilename());
			}
			break; }
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_16_);
		}
		return s;
	}
	
	public string  number() //throws RecognitionException, TokenStreamException
{
		string s = string.Empty;
		
		IToken  n1 = null;
		IToken  n2 = null;
		
		try {      // for error handling
			n1 = LT(1);
			match(NUMBER);
			s += n1.getText();
			{
				switch ( LA(1) )
				{
				case DOT:
				{
					match(DOT);
					s += ".";
					n2 = LT(1);
					match(NUMBER);
					s += n2.getText();
					break;
				}
				case CRLF:
				case SEMICOLON:
				{
					break;
				}
				default:
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				 }
			}
		}
		catch (RecognitionException ex)
		{
			reportError(ex);
			recover(ex,tokenSet_17_);
		}
		return s;
	}
	
	private void initializeFactory()
	{
	}
	
	public static readonly string[] tokenNames_ = new string[] {
		@"""<0>""",
		@"""EOF""",
		@"""<2>""",
		@"""NULL_TREE_LOOKAHEAD""",
		@"""BEGIN""",
		@"""COLON""",
		@"""VCALENDAR""",
		@"""CRLF""",
		@"""END""",
		@"""IANA_TOKEN""",
		@"""X_NAME""",
		@"""PRODID""",
		@"""SEMICOLON""",
		@"""VERSION""",
		@"""CALSCALE""",
		@"""METHOD""",
		@"""EQUAL""",
		@"""COMMA""",
		@"""DQUOTE""",
		@"""CTL""",
		@"""BACKSLASH""",
		@"""ESCAPED_CHAR""",
		@"""NUMBER""",
		@"""DOT""",
		@"""CR""",
		@"""LF""",
		@"""ALPHA""",
		@"""DIGIT""",
		@"""DASH""",
		@"""SPECIAL""",
		@"""SPACE""",
		@"""HTAB""",
		@"""SLASH""",
		@"""LINEFOLDER"""
	};
	
	private static long[] mk_tokenSet_0_()
	{
		long[] data = { 2L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_0_ = new BitSet(mk_tokenSet_0_());
	private static long[] mk_tokenSet_1_()
	{
		long[] data = { 256L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_1_ = new BitSet(mk_tokenSet_1_());
	private static long[] mk_tokenSet_2_()
	{
		long[] data = { 60928L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_2_ = new BitSet(mk_tokenSet_2_());
	private static long[] mk_tokenSet_3_()
	{
		long[] data = { 272L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_3_ = new BitSet(mk_tokenSet_3_());
	private static long[] mk_tokenSet_4_()
	{
		long[] data = { 1808L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_4_ = new BitSet(mk_tokenSet_4_());
	private static long[] mk_tokenSet_5_()
	{
		long[] data = { 61200L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_5_ = new BitSet(mk_tokenSet_5_());
	private static long[] mk_tokenSet_6_()
	{
		long[] data = { 4128L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_6_ = new BitSet(mk_tokenSet_6_());
	private static long[] mk_tokenSet_7_()
	{
		long[] data = { 17178161008L, 0L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_7_ = new BitSet(mk_tokenSet_7_());
	private static long[] mk_tokenSet_8_()
	{
		long[] data = { 128L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_8_ = new BitSet(mk_tokenSet_8_());
	private static long[] mk_tokenSet_9_()
	{
		long[] data = { 17179344752L, 0L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_9_ = new BitSet(mk_tokenSet_9_());
	private static long[] mk_tokenSet_10_()
	{
		long[] data = { 65536L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_10_ = new BitSet(mk_tokenSet_10_());
	private static long[] mk_tokenSet_11_()
	{
		long[] data = { 135200L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_11_ = new BitSet(mk_tokenSet_11_());
	private static long[] mk_tokenSet_12_()
	{
		long[] data = { 17178947408L, 0L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_12_ = new BitSet(mk_tokenSet_12_());
	private static long[] mk_tokenSet_13_()
	{
		long[] data = { 17179082608L, 0L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_13_ = new BitSet(mk_tokenSet_13_());
	private static long[] mk_tokenSet_14_()
	{
		long[] data = { 17179344880L, 0L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_14_ = new BitSet(mk_tokenSet_14_());
	private static long[] mk_tokenSet_15_()
	{
		long[] data = { 17177898832L, 0L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_15_ = new BitSet(mk_tokenSet_15_());
	private static long[] mk_tokenSet_16_()
	{
		long[] data = { 17178161136L, 0L, 0L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_16_ = new BitSet(mk_tokenSet_16_());
	private static long[] mk_tokenSet_17_()
	{
		long[] data = { 4224L, 0L};
		return data;
	}
	public static readonly BitSet tokenSet_17_ = new BitSet(mk_tokenSet_17_());
	
}
}
