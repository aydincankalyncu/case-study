using PromotionCodeGenerator;

using var codeGenerator = new CodeGenerator();
//var code = codeGenerator.GeneratePromotionCode();
//var isCodeValid = codeGenerator.IsPromotionCodeValid(code);
//Console.WriteLine($@"Generated code: {code} is valid: {isCodeValid}");
codeGenerator.GeneratePromotionCodeWithQuantity(50);


