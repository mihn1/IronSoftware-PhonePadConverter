public class OldPhonePadConverter
{
    public static string OldPhonePad(string input)
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input));
        var output = new System.Text.StringBuilder();

        while (ReadNextChunk(stream, out char outChar))
        {
            if (outChar == (char)CharacterType.EndOfInput) break; // Exception case
            else if (outChar == (char)CharacterType.Deletion)
            {
                if (output.Length > 0) output.Remove(output.Length - 1, 1);
            }
            else if (outChar is not ((char)CharacterType.SentenceEnd or (char)CharacterType.NewCharacter))
            {
                // Append to output if outChar is a valid character
                output.Append(outChar == (char)CharacterType.Space ? ' ' : outChar);
            }
        }

        return output.ToString();
    }

    /// <summary>
    /// Read next chunk from the stream and output current character read, returning true if a chunk is completed
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="outChar"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static bool ReadNextChunk(Stream stream, out char outChar)
    {
        int raw = stream.ReadByte();
        if (raw == -1)
        {
            outChar = (char)CharacterType.EndOfInput;
            return false;
        }

        char nextChar = (char)raw;
        char startChar = nextChar;
        outChar = nextChar;

        // check end chunk conditions
        if (IsChunkBreakChar(outChar)) return true;

        int sameCharCount = 0;

        while (true)
        {
            // If we hit a break char, put the position back and return
            // A different digit char also ends the current chunk
            if (nextChar != startChar || IsChunkBreakChar(nextChar))
            {
                stream.Seek(-1, SeekOrigin.Current);
                return true;
            }

            // nextChar should be a valid digit here
            if (!char.IsDigit(nextChar)) throw new ArgumentException("Invalid character in input");
            outChar = GetPhonePadCharacter(startChar, sameCharCount);
            sameCharCount++;

            raw = stream.ReadByte();
            if (raw == -1)
            {
                // chunk has not finished, output the temp char but put the position back to that start of this chunk
                stream.Seek(-sameCharCount, SeekOrigin.Current);
                return false;
            }
            nextChar = (char)raw;
        }
    }

    static bool IsChunkBreakChar(char c) => 
        c is (char)CharacterType.Deletion or 
        (char)CharacterType.SentenceEnd or 
        (char)CharacterType.NewCharacter or 
        (char)CharacterType.Space;

    static char GetPhonePadCharacter(char startChar, int count) => startChar switch
    {
        '1' => "&'("[count % 3],
        '2' => "abc"[count % 3],
        '3' => "def"[count % 3],
        '4' => "ghi"[count % 3],
        '5' => "jkl"[count % 3],
        '6' => "mno"[count % 3],
        '7' => "pqrs"[count % 4],
        '8' => "tuv"[count % 3],
        '9' => "wxyz"[count % 4],
        '0' => ' ', // Use 0 for space
        _ => throw new ArgumentException("Invalid start character"),
    };

    public static bool IsPhonePadCharacter(char c) =>
        c is '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9' or
            (char)CharacterType.Deletion or (char)CharacterType.NewCharacter or (char)CharacterType.SentenceEnd;
}

