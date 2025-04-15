namespace TemplatingEnginesTests;

public class StringBuilderExtensionTests {
	[Theory]
	[InlineData("one two", 5, "one\ntwo  ")]
	[InlineData("one", 5, "one  ")]
	[InlineData("one two three", 5, "one\ntwo\nthree")]
	[InlineData("one two three", 6, "one\ntwo\nthree ")]
	[InlineData("one two three", 7, "one two\nthree  ")]
	[InlineData("aaabbbc", 3, "aaa\nbbb\nc  ")]
	public void StringBuilderAppendWrapPadRightWorksOnSimpleExample(string input, int totalWidth, string output) {
		var sb = new StringBuilder();
		sb.AppendWrapPadRight(input, totalWidth);
		sb.ToString().ShouldBe(output.ReplaceLineEndings());
	}

	[Theory]
	[InlineData("one two three four five", "  ", 15, "one two three\n  four five    ")]
	public void StringBuilderAppendWrapPadRightWorksWithIndent(string input, string indent, int totalWidth, string output) {
		var sb = new StringBuilder();
		sb.AppendWrapPadRight(input, totalWidth, indent);
		sb.ToString().ShouldBe(output.ReplaceLineEndings());
	}
}