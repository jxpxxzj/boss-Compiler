# boss-Compiler

2018 春季学期编译原理 (Compiler Principles) 课程实验.

## 主要任务

- 实现词法分析器
- 实现 LL(1) 语法分析器
- 实现算符优先文法分析器
- 实现递归下降语法分析器
- 实现 LR(1) / SLR(1) / LALR(1) 语法分析器

## 附加功能

- 实现语法树及其可视化
- 实现语义分析
- 支持 JavaScript (ES5) 分析
- 支持解释执行和调试

## 代码说明

### 词法分析器

- `Token.cs`: 词法单元定义
- `Tokens.cs`: 词法单元类型定义
- `Lexer.cs`: 词法分析器 (未完成)

### 语法定义

- `AbstractTerminal.cs`: 抽象终结符
- `NonTerminal.cs`: 非终结符
- `Terminal.cs`: 终结符
- `ProductionExpression.cs`: 单个生成式
- `Production.cs`: 生成式集合
- `Grammar.cs`: 语法

### 语法分析

- `Grammars.cs`: 算术表达式语法定义
- `LL1Parser.cs`: LL(1) 语法分析器
- `Program.cs`: 测试程序
