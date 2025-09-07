enum CharacterType
{
    Deletion = '*',
    NewCharacter = ' ',
    SentenceEnd = '#',
    EndOfInput = '$', // internal use to indicate end of input stream (exception case)
    Empty = '\0',
    Space = '0'
}