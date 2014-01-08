using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse
{
    /// <summary>Exposes methods to consume certain tokens from a script file</summary>
    public static class ScriptReader
    {
        #region Read methods
        /// <summary>Reads known white space characters from the reader</summary>
        /// <param name="reader">TextReader to read from</param>
        /// <returns>A collection of the characters read</returns>
        public static IList<Char> ReadWhiteSpace(PeekSeekTextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "The PeekSeekTextReader to read from was unexpectedly null.");

            List<Char> characters = new List<Char>();

            Int32 consumed = -1;
            while ((consumed = reader.Peek()) != -1)
            {
                Char readChar = Convert.ToChar(consumed);

                //iterate through the known whitespace characters
                switch (readChar)
                {
                    //whitespace characters that I have tested and know work as generic whitespace
                    case ' ':
                    case '\r':
                    case '\n':
                    case '\t':
                    case '\v':
                        characters.Add(readChar);
                        consumed = reader.Read();

                        //in case some weirdness happens and can't read the peeked character
                        if (consumed == -1)
                            throw new ApplicationException("Application was able to peek at the next character, but failed when reading the next character.");

                        continue;
                }

                //if logic does not hit the continue, it must not be a whitespace character
                break;
            }

            return characters;
        }
        
        /// <summary>Reads a newline (\n) character from the reader</summary>
        /// <param name="reader">TextReader to read from</param>
        /// <returns>The read character</returns>
        public static Char ReadNewline(PeekSeekTextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "The PeekSeekTextReader to read from was unexpectedly null.");

            Int32 consumed = reader.Read();

            if (consumed == -1)
                throw new InvalidDataException("Expected to read a character of \\n and instead could not read from the TextReader.");

            Char readChar = Convert.ToChar(consumed);
            if (readChar != '\n')
                throw new InvalidDataException(String.Format("Expected to read a character of \\n and instead read a value of {0:X2}.", readChar));

            return readChar;
        }

        /// <summary>Reads two characters from the reader, an expected script token</summary>
        /// <param name="reader">TextReader to read from</param>
        /// <returns>A collection of the characters read</returns>
        public static IList<Char> ReadToken(PeekSeekTextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "The PeekSeekTextReader to read from was unexpectedly null.");

            List<Char> characters = new List<Char>();

            //read the first character
            Int32 consumed = reader.Read();

            if (consumed == -1)
                throw new InvalidDataException("Expected to read the first character from the TextReader and instead could not read from the TextReader.");

            characters.Add(Convert.ToChar(consumed));

            //read the second character
            consumed = reader.Read();

            if (consumed == -1)
                throw new InvalidDataException("Expected to read the second character from the TextReader and instead could not read from the TextReader.");

            characters.Add(Convert.ToChar(consumed));

            return characters;
        }

        /// <summary>Reads an integer from the reader</summary>
        /// <param name="reader">TextReader to read from</param>
        /// <returns>A collection of the characters read</returns>
        /// <remarks>BG:EE does not support hexidecimal notation</remarks>
        public static IList<Char> ReadInteger(PeekSeekTextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "The PeekSeekTextReader to read from was unexpectedly null.");

            List<Char> characters = new List<Char>();

            Int32 consumed = -1;
            while ((consumed = reader.Peek()) != -1)
            {
                Char readChar = Convert.ToChar(consumed);

                //iterate through the known whitespace characters
                switch (readChar)
                {
                    //whitespace characters that I have tested and know work as generic whitespace
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        characters.Add(readChar);
                        consumed = reader.Read();

                        //in case some weirdness happens and can't read the peeked character
                        if (consumed == -1)
                            throw new ApplicationException("Application was able to peek at the next character, but failed when reading the next character.");

                        continue;
                }

                //if logic does not hit the continue, it must not be a whitespace character
                break;
            }

            return characters;
        }

        /// <summary>Reads data from the reader until it hits the specified token</summary>
        /// <param name="reader">PeekSeekTextReader to read from</param>
        /// <param name="token">Two-character token to stop at. Only the first two characters are considered.</param>
        /// <returns>A collection of the characters read</returns>
        public static IList<Char> ReadUntilToken(PeekSeekTextReader reader, ReadableToken token)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "The PeekSeekTextReader to read from was unexpectedly null.");
            else if (token == null)
                throw new ArgumentNullException("token", "The token to compare to was unexpectedly null.");
            else if (token.Token == null)
                throw new ArgumentNullException("token", "The token to compare to has Text that was unexpectedly null.");
            else if (token.Token.Length < 2)
                throw new ArgumentOutOfRangeException(String.Format("The token to compare (\"{0}\") was shorter than two characters.", token), "token");

            //case insensitive?
            String targetedToken = token.Token;
            if (token.AllowLowerCase)
                targetedToken = targetedToken.ToLower();

            List<Char> characters = new List<Char>();

            Int32 peek0 = reader.Peek(), peek1 = reader.Peek(1);
            while (peek0 != -1 && peek1 != -1)
            {
                Char ahead1 = Convert.ToChar(peek0), ahead2 = Convert.ToChar(peek1);

                if (token.AllowLowerCase)
                {
                    ahead1 = Char.ToLower(ahead1);
                    ahead2 = Char.ToLower(ahead2);
                }

                if (ahead1 == targetedToken[0] && ahead2 == targetedToken[1])
                    break;

                Int32 consumed = reader.Read();
                if (consumed == -1)
                    throw new ApplicationException("The read character should not have been -1.");
                
                characters.Add(Convert.ToChar(consumed));

                peek0 = peek1;
                peek1 = reader.Peek(1);
            }

            return characters;
        }

        /// <summary>Reads a set of parameters from the reader until it hits the specified token</summary>
        /// <param name="reader">PeekSeekTextReader to read from</param>
        /// <param name="token">Two-character token to stop at. Only the first two characters are considered.</param>
        /// <returns>The collection of the characters read</returns>
        public static IList<Char> ReadParametersUntilToken(PeekSeekTextReader reader, ReadableToken token)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "The PeekSeekTextReader to read from was unexpectedly null.");
            else if (token == null)
                throw new ArgumentNullException("token", "The token to compare to was unexpectedly null.");
            else if (token.Token == null)
                throw new ArgumentNullException("token", "The token to compare to has Text that was unexpectedly null.");
            else if (token.Token.Length < 2)
                throw new ArgumentOutOfRangeException(String.Format("The token to compare (\"{0}\") was shorter than two characters.", token), "token");

            //case insensitive?
            String targetedToken = token.Token;
            if (token.AllowLowerCase)
                targetedToken = targetedToken.ToLower();

            List<Char> characters = new List<Char>();


            Char? escapeCharacter = null;
            Int32 peek0 = reader.Peek(), peek1 = reader.Peek(1);
            while (peek0 != -1 && peek1 != -1)
            {
                if (escapeCharacter == null)
                {
                    Char ahead1 = Convert.ToChar(peek0), ahead2 = Convert.ToChar(peek1);

                    if (token.AllowLowerCase)
                    {
                        ahead1 = Char.ToLower(ahead1);
                        ahead2 = Char.ToLower(ahead2);
                    }

                    if (ahead1 == targetedToken[0] && ahead2 == targetedToken[1])
                        break;
                }

                Int32 consumed = reader.Read();
                if (consumed == -1)
                    throw new ApplicationException("The read character should not have been -1.");

                Char charRead = Convert.ToChar(consumed);

                characters.Add(charRead);

                //check for quotes, brackets
                if (escapeCharacter == null)
                {
                    if (charRead == '"')
                        escapeCharacter = '"';
                    else if (charRead == '[')
                        escapeCharacter = ']';
                }
                else
                {
                    if (escapeCharacter.Value == charRead)
                        escapeCharacter = null;
                }

                peek0 = peek1;
                peek1 = reader.Peek(1);
            }

            return characters;
        }

        /// <summary>Reads a quoted string from the reader</summary>
        /// <param name="reader">PeekSeekTextReader to read from</param>
        /// <returns>The quoted string</returns>
        public static IList<Char> ReadQuotedText(PeekSeekTextReader reader)
        {
            return ScriptReader.ReadDelimitedText(reader, '"', '"');
        }

        /// <summary>Reads data from the reader until it hits the specified character</summary>
        /// <param name="reader">PeekSeekTextReader to read from</param>
        /// <param name="character">Target character to read until.</param>
        /// <returns>A collection of the characters read</returns>
        public static IList<Char> ReadUntilCharacter(PeekSeekTextReader reader, Char character)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "The PeekSeekTextReader to read from was unexpectedly null.");

            List<Char> characters = new List<Char>();

            Int32 peek = reader.Peek();
            while (peek != -1)
            {
                Char peekedCharacter = Convert.ToChar(peek);

                if (peekedCharacter == character)
                    break;

                Int32 consumed = reader.Read();
                if (consumed == -1)
                    throw new ApplicationException("The read character should not have been -1.");

                characters.Add(Convert.ToChar(consumed));

                peek = reader.Peek();
            }

            return characters;
        }

        /// <summary>Reads a bracketed text block from the reader</summary>
        /// <param name="reader">PeekSeekTextReader to read from</param>
        /// <returns>The read Point</returns>
        public static IList<Char> ReadBracketedText(PeekSeekTextReader reader)
        {
            return ScriptReader.ReadDelimitedText(reader, '[', ']');
        }

        /// <summary>Reads from the current point to the end of the reader's data</summary>
        /// <param name="reader">PeekSeekTextReader to read from</param>
        /// <returns>The remaining characters read</returns>
        public static IList<Char> ReadToEnd(PeekSeekTextReader reader)
        {
            return reader.ReadToEndIList();
        }

        /// <summary>Peeks through the reader and finds the next token that is found and returns it</summary>
        /// <param name="reader">Reader to peek through</param>
        /// <param name="validTokens">Collection of tokens that are being searched for</param>
        /// <returns>The next token found or null if no token found before the end of the reader's stream</returns>
        public static String PeekNextOfTokens(PeekSeekTextReader reader, IList<ReadableToken> validTokens)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "The PeekSeekTextReader to read from was unexpectedly null.");
            else if (validTokens == null)
                throw new ArgumentNullException("validTokens", "The collection of tokens to compare to was unexpectedly null.");

            //validate tokens
            foreach (ReadableToken token in validTokens)
            {   
                if (token == null)
                    throw new ArgumentNullException("validTokens", "One of the tokens being targeted was unexpectedly null.");
                else if (token.Token == null)
                    throw new ArgumentNullException("validTokens", "The token to compare to has Text that was unexpectedly null.");
                else if (token.Token.Length < 2)
                    throw new ArgumentOutOfRangeException(String.Format("The token to compare (\"{0}\") was shorter than two characters.", token), "token");
            }

            //the token to return
            String foundToken = null;

            Char? escapeCharacter = null;
            Int32 peekIndex = 1;
            Int32 peek0 = reader.Peek(), peek1 = reader.Peek(peekIndex);
            while (peek0 != -1 && peek1 != -1)
            {
                Boolean found = false;

                Char ahead0 = Convert.ToChar(peek0), ahead1 = Convert.ToChar(peek1);
                if (escapeCharacter == null)
                {
                    foreach (ReadableToken token in validTokens)
                    {
                        Char evaluate0 = ahead0, evaluate1 = ahead1;

                        String targetedToken = token.Token;
                        if (token.AllowLowerCase)
                        {
                            targetedToken = targetedToken.ToLower();
                            evaluate0 = Char.ToLower(evaluate0);
                            evaluate1 = Char.ToLower(evaluate1);
                        }

                        if (evaluate0 == targetedToken[0] && evaluate1 == targetedToken[1])
                        {
                            foundToken = token.Token;
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                    break;

                //check for quotes, brackets
                if (escapeCharacter == null)
                {
                    if (ahead0 == '"')
                        escapeCharacter = '"';
                    else if (ahead0 == '[')
                        escapeCharacter = ']';
                }
                else
                {
                    if (escapeCharacter.Value == ahead0)
                        escapeCharacter = null;
                }

                ++peekIndex;
                peek0 = peek1;
                peek1 = reader.Peek(peekIndex);

                if (peek1 == -1)
                    throw new ApplicationException("The peeked character should not have been -1.");
            }

            return foundToken;
        }
        #endregion


        #region Helper methods
        /// <summary>Reads the delimited text from the reader</summary>
        /// <param name="reader">PeekSeekTextReader to read from</param>
        /// <param name="begin">Character starting the text block</param>
        /// <param name="end">Character ending the text block</param>
        /// <returns>A collection of the characters read</returns>
        private static IList<Char> ReadDelimitedText(PeekSeekTextReader reader, Char begin, Char end)
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "The PeekSeekTextReader to read from was unexpectedly null.");

            List<Char> delimitedText = new List<Char>();

            //initial read of beginning delimited character
            Int32 consumed = reader.Read();
            if (consumed == -1)
                throw new InvalidDataException(String.Format("Expected to receive a given character ('{0:X2}'), and instead read -1 from reader.", begin));

            Char characterRead = Convert.ToChar(consumed);
            if (characterRead != begin)
                throw new InvalidDataException(String.Format("Expected to receive a given character ('{0:X2}'), and instead read '{1:X2}' from reader.", begin, characterRead));

            //add it
            delimitedText.Add(characterRead);

            while ((consumed = reader.Read()) != -1)
            {
                characterRead = Convert.ToChar(consumed);

                delimitedText.Add(characterRead);

                if (characterRead == end)
                    break;
            }

            //validation
            if (consumed == -1)
                throw new InvalidDataException("Expected to receive a character, and instead read -1 from reader.");

            return delimitedText;
        }
        #endregion
    }
}