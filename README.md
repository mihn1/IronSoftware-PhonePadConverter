# Old Phone Pad Converter

A .NET 9 / C# console solution that converts old mobile phone keys input into text. It also includes a real-time interactive runner (Program.cs) and an xUnit test suite.  
The original task description lives in: “C# Coding Challenge” (refer to that file for the formal prompt).

## Keypad Mapping
- 1 → & ' (
- 2 → a b c
- 3 → d e f
- 4 → g h i
- 5 → j k l
- 6 → m n o
- 7 → p q r s
- 8 → t u v
- 9 → w x y z
- 0 → space
- * → delete previous committed character
- (space char) → force new character (commits current multi-tap sequence)
- # → end of “sentence” / finalize
- z (only in real-time mode) → quit application

Repeated presses on the same digit cycle the letters (wrap-around).  
Example: 33# → e, 227*# → b (since 227 = "b", then * deletes 'b', leaving nothing, then # finalizes).

## Projects
- PhonePadConverter (console app + real-time runner)
- PhonePadConverter.Tests (xUnit test suite)

## Current Implementations / Future Improvements
- Converter uses a streaming approach to mimic real-time keypad behavior;
- Real-time runner re-renders a single line; when output breaks to multiple lines, visual artifacts occur. Could improve by tracking initial cursor position, clearing only the dirty region, ...