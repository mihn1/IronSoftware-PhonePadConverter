// Some basic test cases
Console.WriteLine(OldPhonePadConverter.OldPhonePad("33#"));
Console.WriteLine(OldPhonePadConverter.OldPhonePad("227*#"));
Console.WriteLine(OldPhonePadConverter.OldPhonePad("4433555 555666#"));
Console.WriteLine(OldPhonePadConverter.OldPhonePad("8 88777444666*664#"));
Console.WriteLine(OldPhonePadConverter.OldPhonePad("4433555 55566608 88777444666*664#"));

// Real-time interactive input: keep output on the same line, updating as user types.
Console.WriteLine();
Console.WriteLine("Real-time phone pad converter: digits(0-9) space(new char) *(delete) #(end line) z(quit)");
while (true) // Main program loop
{
    Console.WriteLine("---------------------");
    Console.WriteLine("Listening for new input...");
    using var inputStream = new MemoryStream();
    var input = new System.Text.StringBuilder();
    var output = new System.Text.StringBuilder();
    int lastPrintedLen = 0;
    char? tempChar = null;

    while (true) // Sentence loop
    {
        bool isSentenceEnd = false;
        var key = Console.ReadKey(intercept: true);
        char ch = key.KeyChar;
        input.Append(ch);
        if (ch == 'z') return; // quit program

        if (!OldPhonePadConverter.IsPhonePadCharacter(ch))
        {
            Console.Write("\nUnsupported character - please try again\n");
            continue; // ignore unsupported key
        }

        // Append new char to the stream (at the end)
        var originalPos = inputStream.Position;
        inputStream.Seek(0, SeekOrigin.End);
        inputStream.WriteByte((byte)ch);
        inputStream.Seek(originalPos, SeekOrigin.Begin);

        while (true)
        {
            if (OldPhonePadConverter.ReadNextChunk(inputStream, out char outChar))
            {
                // chunk completed
                if (outChar == (char)CharacterType.SentenceEnd)
                {
                    isSentenceEnd = true;
                    break;
                }
                else if (outChar == (char)CharacterType.Deletion)
                {
                    if (tempChar is null && output.Length > 0) output.Length -= 1;
                    tempChar = null; // clear temp char
                }
                else if (outChar != (char)CharacterType.NewCharacter)
                {
                    output.Append(outChar == (char)CharacterType.Space ? ' ' : outChar);

                    if (outChar == tempChar)
                    {
                        tempChar = null; // clear temp char as we have a completed char
                        continue; // same char as temp, wait for next input -> the only case we continue the loop
                    }
                }
            }
            else
            {
                // chunk not completed, set temp char to show in real-time console
                tempChar = outChar;
            }

            break;
        }

        // Re-render output on same line
        string current = output.ToString() + tempChar;
        int pad = lastPrintedLen - current.Length;
        Console.Write('\r');
        Console.Write(current);
        if (pad > 0)
        {
            Console.Write(new string(' ', pad)); // clear leftover chars
            var (Left, Top) = Console.GetCursorPosition();
            Console.SetCursorPosition(Left - pad, Top);
        }
        lastPrintedLen = current.Length;

        if (isSentenceEnd)
        {
            Console.WriteLine(); // move to next line
            Console.WriteLine($"Output Sent: {output}");
            Console.WriteLine($"From Input: {input}");
            break; // end sentence loop
        }
    }
}