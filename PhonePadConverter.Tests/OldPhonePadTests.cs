namespace PhonePadConverter.Tests;

public class OldPhonePadTests
{
    // Core scenarios + mixed behaviors
    [Theory]
    [InlineData("33#", "e")]
    [InlineData("227*#", "b")]
    [InlineData("4433555 555666#", "hello")]
    [InlineData("8 88777444666*664#", "turing")]
    [InlineData("4433555 55566608 88777444666*664#", "hello turing")]
    [InlineData("2 22#", "ab")]
    [InlineData("2 2 2#", "aaa")]
    [InlineData("20#", "a ")]
    [InlineData("0 0#", "  ")]
    [InlineData("1 2 3#", "&ad")]
    [InlineData("2 #", "a")]
    [InlineData("227*7#", "bp")]
    public void OldPhonePad_CoreScenarios(string input, string expected)
    {
        var actual = OldPhonePadConverter.OldPhonePad(input);
        Assert.Equal(expected, actual);
    }

    // Wrap-around
    [Theory]
    [InlineData("2222#", "a")]
    [InlineData("77777#", "p")]
    [InlineData("9999#", "z")]
    [InlineData("99999#", "w")]
    [InlineData("1#", "&")]
    [InlineData("11#", "'")]
    [InlineData("111#", "(")]
    [InlineData("1111#", "&")]
    [InlineData("11111#", "'")]
    public void OldPhonePad_WrapAround(string input, string expected)
    {
        var actual = OldPhonePadConverter.OldPhonePad(input);
        Assert.Equal(expected, actual);
    }

    // Deletion behavior
    [Theory]
    [InlineData("*#", "")]
    [InlineData("44*#", "")]
    [InlineData("33*#", "")]
    [InlineData("33*2#", "a")]
    [InlineData("*2#", "a")]
    [InlineData("0*#", "")]
    [InlineData("2**#", "")]
    [InlineData("227**7#", "p")]
    public void OldPhonePad_Deletions(string input, string expected)
    {
        var actual = OldPhonePadConverter.OldPhonePad(input);
        Assert.Equal(expected, actual);
    }

    // Validation & edge cases
    [Fact]
    public void OldPhonePad_InvalidCharacter_Throws()
    {
        Assert.Throws<ArgumentException>(() => OldPhonePadConverter.OldPhonePad("A#"));
    }

    [Fact]
    public void OldPhonePad_IncompleteChunk_NoHash_ProducesNoOutput()
    {
        var actual = OldPhonePadConverter.OldPhonePad("33");
        Assert.Equal(string.Empty, actual);
    }

    [Fact]
    public void OldPhonePad_EmptyInput_ReturnsEmpty()
    {
        var actual = OldPhonePadConverter.OldPhonePad(string.Empty);
        Assert.Equal(string.Empty, actual);
    }
}
