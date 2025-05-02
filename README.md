# CptS321-Chandler_Guthrie-HWs

# Name : Chandler Guthrie
# WSUID: 011801740

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