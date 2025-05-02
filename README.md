# Name : Chandler Guthrie

This is a spreadsheet application I created in my CPTS_321 (Object Oriented Software Principles)



## Style Cop Removals
### SA1401(All fields must be private): 
- Removed due to requirements of field being an protected access modifier.

### SA1009(Closing parenthesis should be spaced correctly): 
- Formatting Of Code Prevents specific case with null forgiving operator. I check for null before using this operator.
- Ex. 
- Required: this.spreadsheet.GetCell(1, 1) !.Text
- Formatter: this.spreadsheet.GetCell(1, 1)!.Text
- checked for hours to fix this with no solution

### SA1009(A closing square bracket within a C# statement is not spaced correctly): 
- Formatting of code prevents space with a nullable array.
- Ex. 
- Required: private object[] ? cellReferences = null;
- Formatter: private object[]? cellReferences = null;
